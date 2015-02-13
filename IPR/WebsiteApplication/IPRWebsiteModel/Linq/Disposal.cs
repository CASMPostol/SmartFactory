//<summary>
//  Title   : Disposal Entity partial class.
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint;
using CAS.SharePoint.Logging;
using CAS.SharePoint.Web;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Disposal
  /// </summary>
  public sealed partial class Disposal
  {

    #region creator
    internal Disposal(IPR ipr, Linq.DisposalStatus _typeOfDisposal, decimal _toDispose)
      : this()
    {
      Archival = false;
      ClearingType = Linq.ClearingType.PartialWindingUp;
      CustomsProcedure = String.Empty.NotAvailable();
      CustomsStatus = Linq.CustomsStatus.NotStarted;
      Disposal2IPRIndex = ipr;
      DisposalStatus = _typeOfDisposal;
      Disposal2PCNID = null; //will be assigned during clearing through custom
      DutyPerSettledAmount = new Nullable<double>();  // calculated in SetUpCalculatedColumns,
      InvoiceNo = String.Empty.NotAvailable(); //To be assigned during finished goods export.
      IPRDocumentNo = String.Empty.NotAvailable();
      JSOXCustomsSummaryIndex = null;
      SPNo = new Nullable<double>();
      RemainingQuantity = new Nullable<double>(); //To be set during sad processing
      SADDate = Extensions.SPMinimum;
      SADDocumentNo = String.Empty.NotAvailable();
      SettledQuantityDec = _toDispose;
      string _titleTmplt = "Disposal: {0} of material {1}";
      Title = String.Format(_titleTmplt, this.DisposalStatus.Value.ToString(), this.Disposal2IPRIndex.Batch);
    }
    #endregion

    #region public
    /// <summary>
    /// Gets or sets the settled quantity dec.
    /// </summary>
    /// <value>
    /// The settled quantity as decimal.
    /// </value>
    public decimal SettledQuantityDec
    {
      get { return Convert.ToDecimal(this.SettledQuantity).Round2Decimals(); }
      set
      {
        this.SettledQuantity = Convert.ToDouble(value).Round2Decimals();
        CalculateDutyAndVat();
      }
    }

    #region internal
    internal Material Material
    {
      set
      {
        if (value == null)
          throw new ArgumentNullException("Material", "Material cannot be null");
        Disposal2BatchIndex = value.Material2BatchIndex;
        Disposal2MaterialIndex = value;
      }
    }
    internal Clearence Clearance
    {
      set
      {
        if (value == null)
          throw new ArgumentNullException("Clearance", "Clearance cannot be null");
        if (this.CustomsStatus.Value != Linq.CustomsStatus.NotStarted)
          throw new ArgumentException("Clearance can be assigned only to NoStarted disposals - internal fatal error.");
        this.CustomsProcedure = Entities.ToString(value.ClearenceProcedure.Value);
        this.Disposal2ClearenceIndex = value;
        this.SADDocumentNo = value.DocumentNo;
        this.SADDate = value.CustomsDebtDate;
        this.CustomsProcedure = Entities.ToString(value.ClearenceProcedure.Value);
      }
    }
    internal void ClearThroughCustom(InvoiceContent value, Action<Disposal> reCalculate)
    {
      if (value == null)
        throw new ArgumentNullException("value", "At ClearThroughCustom value cannot be null");
      this.Disposal2InvoiceContentIndex = value;
      this.Clearance = value.InvoiceIndex.ClearenceIndex;
      this.CustomsStatus = Linq.CustomsStatus.Started;
      this.InvoiceNo = value.InvoiceIndex.BillDoc;
      reCalculate(this);
    }
    internal void ClearThroughCustom(Entities entities, ClearenceProcedure procedure, int sadConsignmentNumber, Action<Disposal> reCalculate)
    {
      CustomsStatus = Linq.CustomsStatus.Started;
      CustomsProcedure = Entities.ToString(procedure);
      SadConsignmentNo = SADConsignment.DocumentNumber(entities, sadConsignmentNumber);
      reCalculate(this);
    }
    /// <summary>
    /// Exports the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="closingBatch">if set to <c>true</c> the batch is to be closed.</param>
    /// <param name="invoiceContent">Content of the invoice.</param>
    /// <param name="sadConsignmentNumber">The sad consignment number.</param>
    /// <param name="reCalculate">The recalculate delegate is used to adjust VAT, duty and value for last started disposal.</param>
    /// <exception cref="ApplicationError">if any internal exception has to be cough.</exception>
    internal void Export(Entities entities, ref decimal quantity, bool closingBatch, InvoiceContent invoiceContent, int sadConsignmentNumber, Action<Disposal> reCalculate)
    {
      string _at = "Starting";
      try
      {
        if (!closingBatch)
          if (quantity < this.SettledQuantityDec)
          {
            _at = "_newDisposal";
            Disposal _newDisposal = new Disposal(Disposal2IPRIndex, DisposalStatus.Value, this.SettledQuantityDec - quantity);
            _newDisposal.Material = this.Disposal2MaterialIndex;
            this.SettledQuantityDec = quantity;
            quantity = 0;
            _at = "InsertOnSubmit";
            entities.Disposal.InsertOnSubmit(_newDisposal);
          }
          else
            quantity -= this.SettledQuantityDec;
        _at = "ClearThroughCustom";
        this.ClearThroughCustom(invoiceContent, reCalculate);
        this.SadConsignmentNo = SADConsignment.DocumentNumber(entities, sadConsignmentNumber);
      }
      catch (Exception _ex)
      {
        throw new ApplicationError(@"CAS.SmartFactory.IPR.WebsiteModel.Linq.Disposal.Export", _at, _ex.Message, _ex);
      }
    }
    internal void FinishClearingThroughCustoms(Entities edc, SADGood sadGood, NamedTraceLogger.TraceAction trace)
    {
      string _at = "starting";
      if (this.CustomsStatus.Value == Linq.CustomsStatus.Finished)
        return;
      try
      {
        _at = "Disposal _lastOne";
        IEnumerable<Disposal> _lastOne = from _dsp in this.Disposal2IPRIndex.Disposals(edc, trace)
                                         where _dsp.CustomsStatus.Value == Linq.CustomsStatus.Finished
                                         select _dsp;
        if (_lastOne.Count<Disposal>() == 0)
          this.SPNo = 1;
        else
          this.SPNo = _lastOne.Max<Disposal>(dspsl => dspsl.SPNo.Value) + 1;
        decimal _balance = CalculateRemainingQuantity();
        if (_balance == 0)
          this.ClearingType = Linq.ClearingType.TotalWindingUp;
        else
          this.ClearingType = Linq.ClearingType.PartialWindingUp;
        AssignSADGood(edc, sadGood);
        this.CustomsStatus = Linq.CustomsStatus.Finished;
      }
      catch (Exception _ex)
      {
        string _template = "Cannot finish Export of disposal {0} {1} because of internal error: {2} at: {3}";
        _template = String.Format(_template, this.Title, this.Id.Value, _ex.Message, _at);
        trace("Exception at Disposal.FinishClearingThroughCustoms", 181, TraceSeverity.High);
        throw GenericStateMachineEngine.ActionResult.Exception(_ex, _template);
      }
    }
    internal decimal CalculateRemainingQuantity()
    {
      decimal _balance = 0;
      if (this.DisposalStatus.Value == Linq.DisposalStatus.Cartons)
        _balance = Convert.ToDecimal(this.Disposal2IPRIndex.AccountBalance.Value);
      else
        _balance = Convert.ToDecimal(this.Disposal2IPRIndex.AccountBalance.Value) - this.SettledQuantityDec;
      this.Disposal2IPRIndex.AccountBalance = this.RemainingQuantity = Convert.ToDouble(_balance);
      return _balance;
    }
    internal void Adjust(Entities edc, ref decimal _toDispose, SharePoint.Logging.NamedTraceLogger.TraceAction trace)
    {
      trace("Entering Disposal.Adjust", 193, TraceSeverity.Verbose);
      this.SettledQuantityDec += this.Disposal2IPRIndex.Withdraw(ref _toDispose, this.SettledQuantityDec);
      if (this.CustomsStatus.Value == Linq.CustomsStatus.Finished)
        this.Disposal2IPRIndex.RecalculateClearedRecords(edc, trace);
    }
    #endregion

    #region static
    /// <summary>
    /// Gets the entries4 JSOX.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <param name="parent">The parent.</param>
    /// <returns>The collection of <see cref="Disposal"/> that must be added to JSOX report.</returns>
    public static IQueryable<Disposal> GetEntries4JSOX(Entities edc, JSOXLib parent)
    {
      return from _dspx in edc.Disposal
             where (_dspx.DisposalStatus.Value != Linq.DisposalStatus.Cartons) &&
                   (!_dspx.JSOXReportID.HasValue || _dspx.JSOXReportID.Value == parent.Id.Value) &&
                   (_dspx.CustomsStatus.Value == Linq.CustomsStatus.Finished)
             orderby _dspx.Disposal2IPRIndex.Title
             select _dspx;
    }
    #endregion

    #endregion

    #region private
    /// <summary>
    /// Calculate values of columns: Title, DutyPerSettledAmount, VATPerSettledAmount, TobaccoValue, DutyAndVAT.
    /// </summary>
    private void CalculateDutyAndVat()
    {
      try
      {
        if (this.DisposalStatus.Value == Linq.DisposalStatus.Cartons)
          return;
        double _portion = SettledQuantity.Value / Disposal2IPRIndex.NetMass.Value;
        DutyPerSettledAmount = (Disposal2IPRIndex.Duty.Value * _portion).Round2Decimals();
        VATPerSettledAmount = (Disposal2IPRIndex.VAT.Value * _portion).Round2Decimals();
        TobaccoValue = (Disposal2IPRIndex.Value.Value * _portion).Round2Decimals();
        DutyAndVAT = (DutyPerSettledAmount.Value + VATPerSettledAmount.Value).Round2Decimals();
      }
      catch (Exception ex)
      {
        throw GenericStateMachineEngine.ActionResult.Exception(ex, @"CAS.SmartFactory.IPR.WebsiteModel.Linq\SetUpCalculatedColumns");
      }
    }
    private void AssignSADGood(Entities edc, SADGood sadGood)
    {
      switch (this.DisposalStatus.Value)
      {
        case Linq.DisposalStatus.SHMenthol:
        case Linq.DisposalStatus.Tobacco:
        case Linq.DisposalStatus.Overuse:
          this.Disposal2PCNID = Disposal2IPRIndex.IPR2PCNPCN;
          break;
        case Linq.DisposalStatus.Dust:
        case Linq.DisposalStatus.Waste:
        case Linq.DisposalStatus.Cartons:
        case Linq.DisposalStatus.TobaccoInCigaretes:
        case Linq.DisposalStatus.TobaccoInCigaretesDestinationEU:
        case Linq.DisposalStatus.TobaccoInCigaretesProduction:
        case Linq.DisposalStatus.TobaccoInCutfiller:
        case Linq.DisposalStatus.TobaccoInCutfillerDestinationEU:
        case Linq.DisposalStatus.TobaccoInCutfillerProduction:
          string _productCodeNumber = sadGood.PCNTariffCode;
          PCNCode _pcn = PCNCode.Find(edc, IsDisposal, _productCodeNumber);
          if (_pcn == null)
          {
            string _mtmp = "Cannot find pcn code: {0} for the good: {1} with IsDisposal set to {2}";
            throw new InputDataValidationException(
                "wrong PCN code in customs message", "PCN Code",
                string.Format(_mtmp, _productCodeNumber, sadGood.GoodsDescription, IsDisposal),
                true);
          }
          this.Disposal2PCNID = _pcn;
          break;
        case Linq.DisposalStatus.Invalid:
        case Linq.DisposalStatus.None:
        default:
          break;
      }
      this.SADDate = sadGood.SADDocumentIndex.CustomsDebtDate;
      this.SADDocumentNo = sadGood.SADDocumentIndex.DocumentNumber;
      this.CustomsProcedure = sadGood.SPProcedure;
      //TODO remove it
      if (this.JSOXCustomsSummaryIndex == null)
        return;
      this.JSOXCustomsSummaryIndex.SADDate = sadGood.SADDocumentIndex.CustomsDebtDate;
      this.JSOXCustomsSummaryIndex.ExportOrFreeCirculationSAD = sadGood.SADDocumentIndex.DocumentNumber;
      this.JSOXCustomsSummaryIndex.CustomsProcedure = sadGood.SPProcedure;
      this.JSOXCustomsSummaryIndex.CompensationGood = this.Disposal2PCNID.CompensationGood;
      this.JSOXCustomsSummaryIndex.RemainingQuantity = this.RemainingQuantity;
    }
    private bool IsDisposal
    {
      get
      {
        bool _ret = false;
        switch (this.DisposalStatus.Value)
        {
          case Linq.DisposalStatus.SHMenthol:
          case Linq.DisposalStatus.Overuse:
          case Linq.DisposalStatus.Tobacco:
            _ret = false;
            break;
          case Linq.DisposalStatus.Dust:
          case Linq.DisposalStatus.Waste:
          case Linq.DisposalStatus.Cartons:
          case Linq.DisposalStatus.TobaccoInCigaretes:
          case Linq.DisposalStatus.TobaccoInCigaretesDestinationEU:
          case Linq.DisposalStatus.TobaccoInCigaretesProduction:
          case Linq.DisposalStatus.TobaccoInCutfiller:
          case Linq.DisposalStatus.TobaccoInCutfillerDestinationEU:
          case Linq.DisposalStatus.TobaccoInCutfillerProduction:
            _ret = true;
            break;
        }
        return _ret;
      }
    }
    #endregion

  }
}
