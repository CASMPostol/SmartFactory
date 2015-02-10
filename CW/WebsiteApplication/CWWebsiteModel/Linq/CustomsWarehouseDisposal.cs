//<summary>
//  Title   : Name of Application
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

using CAS.SharePoint.Logging;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// Entity partial class of CustomsWarehouseDisposal.
  /// </summary>
  public partial class CustomsWarehouseDisposal
  {
    #region public
    /// <summary>
    /// CustomsWarehouseDisposal data obtained from xml message.
    /// </summary>
    public struct XmlData
    {
      /// <summary>
      /// Additional quantity declared to dispose
      /// </summary>
      public decimal AdditionalQuantity;
      /// <summary>
      /// The declared quantity
      /// </summary>
      public decimal DeclaredQuantity;
      /// <summary>
      /// The SKU description
      /// </summary>
      public string SKUDescription;
    }
    /// <summary>
    /// Goods the name.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public string GoodsName(Entities entities)
    {
      CustomsWarehouse _cw = CWL_CWDisposal2CustomsWarehouseID;
      return Settings.FormatGoodsName(entities, _cw.TobaccoName, _cw.Grade, _cw.SKU, _cw.Batch, this.ClearingType.Value, _cw.DocumentNo);
    }
    /// <summary>
    /// Gets the goods code.
    /// </summary>
    /// <value>
    /// The goods code.
    /// </value>
    public string ProductCode
    {
      get
      {
        string _code = this.CWL_CWDisposal2PCNTID.ProductCodeNumber;
        return _code.Substring(0, _code.Length - 2);
      }
    }
    /// <summary>
    /// Gets the TARIC.
    /// </summary>
    /// <value>
    /// The TARIC.
    /// </value>
    public string ProductCodeTaric
    {
      get
      {
        string _code = this.CWL_CWDisposal2PCNTID.ProductCodeNumber;
        return _code.Substring(_code.Length - 2, 2);
      }
    }
    /// <summary>
    /// Calculated and assigns values to CW_SettledNetMass, CW_SettledGrossMass, CW_AddedKg, TobaccoValue
    /// </summary>
    /// <param name="value">The net mass.</param>
    internal void CalculateMassValu(double value)
    {
      this.CW_SettledNetMass = value.RoundValue();
      double _Portion = value / CWL_CWDisposal2CustomsWarehouseID.CW_Quantity.Value;
      TobaccoValue = (_Portion * CWL_CWDisposal2CustomsWarehouseID.Value.Value).RoundValue();
      CW_SettledGrossMass = (CW_PackageToClear.Value * CWL_CWDisposal2CustomsWarehouseID.PackageWeight() + value).RoundValue();
      this.CW_AddedKg = (value - this.CW_DeclaredNetMass.Value).RoundValue();
    }
    internal void FinishClearThroughCustoms(Entities edc, SADGood sadGood, NamedTraceLogger.TraceAction traceEvent)
    {
      traceEvent("Starting CustomsWarehouseDisposal.FinishClearThroughCustoms for sadGood:" + sadGood.Title, 100, TraceSeverity.Verbose);
      if (this.CustomsStatus.Value == Linq.CustomsStatus.Finished)
        return;
      try
      {
        List<CustomsWarehouseDisposal> _Finished = (from _dsp in this.CWL_CWDisposal2CustomsWarehouseID.CustomsWarehouseDisposal(edc, false)
                                                    where _dsp.CustomsStatus.Value == Linq.CustomsStatus.Finished
                                                    select _dsp).ToList<CustomsWarehouseDisposal>();
        if (_Finished.Count<CustomsWarehouseDisposal>() == 0)
          this.SPNo = 1;
        else
          this.SPNo = _Finished.Max<CustomsWarehouseDisposal>(dspsl => dspsl.SPNo.Value) + 1;
        AssignSADGood(edc, sadGood, traceEvent);
        decimal _balance = CalculateRemainingQuantity();
        if (_balance == 0)
        {
          this.CW_RemainingPackage = 0;
          this.CW_RemainingTobaccoValue = 0;
          this.ClearingType = Linq.ClearingType.TotalWindingUp;
        }
        else
        {
          double _value = _Finished.Sum<CustomsWarehouseDisposal>(x => x.TobaccoValue.Value);
          double _pckgs = _Finished.Sum<CustomsWarehouseDisposal>(x => x.CW_PackageToClear.Value);
          this.CW_RemainingPackage = this.CWL_CWDisposal2CustomsWarehouseID.CW_PackageUnits - _pckgs - this.CW_PackageToClear;
          this.CW_RemainingTobaccoValue = this.CWL_CWDisposal2CustomsWarehouseID.Value - _value - this.TobaccoValue;
          this.ClearingType = Linq.ClearingType.PartialWindingUp;
        }
        CheckCNConsistency(traceEvent);
        this.CustomsStatus = Linq.CustomsStatus.Finished;
      }
      catch (Exception ex)
      {
        traceEvent("Exception at CustomsWarehouseDisposal.FinishClearThroughCustoms for sadGood:" + ex.Message, 133, TraceSeverity.High);
        throw;
      }
      traceEvent("Finished CustomsWarehouseDisposal.FinishClearThroughCustoms", 136, TraceSeverity.Verbose);
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    internal void UpdateTitle()
    {
      try
      {
        string _numTxt = this.Id.HasValue ? String.Format("{0:D6}", this.Id.Value) : "XXXXXX";
        Title = String.Format("CW-{0:D4}-{1}", DateTime.Today.Year, _numTxt);
      }
      catch (Exception) { }
    }
    #endregion

    #region private
    partial void OnCreated()
    {
      this.PropertyChanging += CustomsWarehouseDisposal_PropertyChanging;
    }
    private void CustomsWarehouseDisposal_PropertyChanging(object sender, System.ComponentModel.PropertyChangingEventArgs e)
    {
      if (e.PropertyName == "Title") return;
      UpdateTitle();
    }
    private void CheckCNConsistency(NamedTraceLogger.TraceAction traceEvent)
    {
      traceEvent("Starting CustomsWarehouseDisposal.CheckCNConsistency but it is not implemented.", 163, TraceSeverity.Verbose);
      //TODO CheckCNCosistency NotImplementedException();
    }
    private void AssignSADGood(Entities edc, SADGood sadGood, NamedTraceLogger.TraceAction traceEvent)
    {
      traceEvent("Starting CustomsWarehouseDisposal.AssignSADGood", 167, TraceSeverity.Verbose);
      this.SADDate = sadGood.SADDocumentIndex.CustomsDebtDate;
      this.SADDocumentNo = sadGood.SADDocumentIndex.DocumentNumber;
      //TODO check consistency and generate warning.
      this.CustomsProcedure = sadGood.SPProcedure;
      if (this.TobaccoValue != sadGood.TotalAmountInvoiced)
      {
        string _msg = "Total Amount Invoiced value is not equal as requested to clear through customs";
        traceEvent("Finishing CustomsWarehouseDisposal.AssignSADGood: " + _msg, 167, TraceSeverity.High);
        throw new ArgumentOutOfRangeException("TotalAmountInvoiced", _msg);
      }
      decimal _vat = 0;
      decimal _duties = 0;
      foreach (SADDuties _sdc in sadGood.SADDuties(edc, false))
      {
        switch (Settings.DutyKind(_sdc.DutyType))
        {
          case Settings.DutyKindEnum.VAT:
            _vat += _sdc.Amount.DecimalValue();
            break;
          case Settings.DutyKindEnum.Duty:
            _duties += _sdc.Amount.DecimalValue();
            break;
          case Settings.DutyKindEnum.ExciseDuty:
            throw new NotImplementedException();
        }
      }
      this.DutyPerSettledAmount = _duties.DoubleValue();
      this.VATPerSettledAmount = _vat.DoubleValue();
      this.DutyAndVAT = (_vat + _duties).DoubleValue();
      traceEvent("Finishing CustomsWarehouseDisposal.AssignSADGood", 167, TraceSeverity.Verbose);
    }
    private decimal CalculateRemainingQuantity()
    {
      decimal _balance = this.CWL_CWDisposal2CustomsWarehouseID.AccountBalance.DecimalValue() - this.CW_SettledNetMass.DecimalValue();
      this.CWL_CWDisposal2CustomsWarehouseID.AccountBalance = this.RemainingQuantity = _balance.DoubleValue();
      return _balance;
    }
    #endregion
  }
}
