using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SharePoint.Web;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Disposal
  /// </summary>
  public partial class Disposal
  {

    #region ctor
    internal Disposal( IPR ipr, Linq.DisposalStatus _typeOfDisposal, decimal _toDispose )
      : this()
    {
      ClearingType = Linq.ClearingType.PartialWindingUp;
      CustomsProcedure = String.Empty.NotAvailable();
      CustomsStatus = Linq.CustomsStatus.NotStarted;
      Disposal2IPRIndex = ipr;
      DisposalStatus = _typeOfDisposal;
      Disposal2PCNID = null; //will be assigned during claring through custom
      DutyAndVAT = new Nullable<double>();  // calculated in SetUpCalculatedColumns,
      DutyPerSettledAmount = new Nullable<double>();  // calculated in SetUpCalculatedColumns,
      InvoiceNo = String.Empty.NotAvailable(); //To be assigned during finished goods export.
      IPRDocumentNo = String.Empty.NotAvailable();
      JSOXCustomsSummaryIndex = null;
      No = new Nullable<double>();
      RemainingQuantity = new Nullable<double>(); //To be set during sad processing
      SADDate = Extensions.SPMinimum;
      SADDocumentNo = String.Empty.NotAvailable();
      SettledQuantityDec = _toDispose;
      Title = String.Empty; // calculated in SetUpCalculatedColumns,
      VATPerSettledAmount = new Nullable<double>(); //calculated in SetUpCalculatedColumns,
      TobaccoValue = new Nullable<double>(); //calculated in SetUpCalculatedColumns, 
      string _titleTmplt = "Disposal: {0} of material {1}";
      Title = String.Format( _titleTmplt, this.DisposalStatus.Value.ToString(), this.Disposal2IPRIndex.Batch );
    }
    #endregion

    #region public
    internal Material Material
    {
      set
      {
        if ( value == null )
          throw new ArgumentNullException( "Material", "Material cannot be null" );
        Disposal2BatchIndex = value.Material2BatchIndex;
        Disposal2MaterialIndex = value;
      }
    }
    internal Clearence Clearance
    {
      set
      {
        if ( value == null )
          throw new ArgumentNullException( "Clearance", "Clearence cannot be null" );
        if ( this.CustomsStatus.Value != Linq.CustomsStatus.NotStarted )
          throw new ArgumentException( "Clearance can be assigned ony to NoStarted disposals - internal fatal error." );
        this.CustomsStatus = Linq.CustomsStatus.Started;
        this.CustomsProcedure = Entities.ToString( value.ClearenceProcedure.Value );
        this.Disposal2ClearenceIndex = value;
        this.SADDocumentNo = value.DocumentNo;
        this.SADDate = value.CustomsDebtDate;
        this.CustomsProcedure = Entities.ToString( value.ClearenceProcedure.Value );
        CalculateDutyAndVat();
      }
    }
    internal InvoiceContent InvoicEContent
    {
      set
      {
        if ( value == null )
          throw new ArgumentNullException( "InvoicEContent", "InvoicEContent cannot be null" );
        this.Disposal2InvoiceContentIndex = value;
        this.Clearance = value.InvoiceIndex.ClearenceIndex;
        this.InvoiceNo = value.InvoiceIndex.BillDoc;
      }
    }
    /// <summary>
    /// Exports the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="closingBatch">if set to <c>true</c> the batch is to be closed.</param>
    /// <param name="invoiceContent">Content of the invoice.</param>
    /// <exception cref="ApplicationError">if any internal exception has to be catched.</exception>
    public void Export( Entities entities, ref decimal quantity, bool closingBatch, InvoiceContent invoiceContent )
    {
      string _at = "Startting";
      try
      {
        if ( !closingBatch )
          if ( quantity < this.SettledQuantityDec )
          {
            _at = "_newDisposal";
            Disposal _newDisposal = new Disposal( Disposal2IPRIndex, DisposalStatus.Value, this.SettledQuantityDec - quantity );
            _newDisposal.Material = this.Disposal2MaterialIndex;
            this.SettledQuantityDec = quantity;
            quantity = 0;
            _at = "InsertOnSubmit";
            entities.Disposal.InsertOnSubmit( _newDisposal );
          }
          else
            quantity -= this.SettledQuantityDec;
        _at = "InvoicEContent";
        this.InvoicEContent = invoiceContent;
        this.CalculateDutyAndVat();
      }
      catch ( Exception _ex )
      {
        throw new ApplicationError( @"CAS.SmartFactory.IPR.WebsiteModel.Linq.Disposal.Export", _at, _ex.Message, _ex );
      }
    }
    internal void ClearThroughCustom()
    {
      CustomsStatus = Linq.CustomsStatus.Started;
      CalculateDutyAndVat();
    }
    internal void FinishClearingThroughCustoms( Entities edc, SADGood sadGood )
    {
      string _at = "starting";
      try
      {
        _at = "Disposal _lastOne";
        IQueryable<Disposal> _lastOne = from _dsp in this.Disposal2IPRIndex.Disposal
                                        where _dsp.CustomsStatus.Value == Linq.CustomsStatus.Finished
                                        select _dsp;
        if ( _lastOne.Count<Disposal>() == 0 )
          this.No = 1;
        else
          this.No = _lastOne.Max<Disposal>( dspsl => dspsl.No.Value ) + 1;
        AssignSADGood( edc, sadGood );
        decimal _balance = 0;
        if ( this.DisposalStatus.Value == Linq.DisposalStatus.Cartons )
          _balance = Convert.ToDecimal( this.Disposal2IPRIndex.AccountBalance.Value );
        else
          _balance = Convert.ToDecimal( this.Disposal2IPRIndex.AccountBalance.Value ) - this.SettledQuantityDec;
        this.Disposal2IPRIndex.AccountBalance = this.RemainingQuantity = Convert.ToDouble( _balance );
        if ( _balance == 0 )
          this.ClearingType = Linq.ClearingType.TotalWindingUp;
        this.CustomsStatus = Linq.CustomsStatus.Finished;
        this.ClearingType = Disposal2IPRIndex.GetClearingType();
        CalculateDutyAndVat();
      }
      catch ( Exception _ex )
      {
        string _template = "Cannot finish Export of disposal {0} {1} because of internal error: {2} at: {3}";
        throw GenericStateMachineEngine.ActionResult.Exception( _ex, String.Format( _template, this.Title, this.Identyfikator.Value, _ex.Message, _at ) );
      }
    }
    internal static IQueryable<Disposal> Disposals( EntitySet<Disposal> disposlas, DisposalEnum _kind )
    {
      return disposlas.Where<Disposal>( x => x.DisposalStatus.Value == Entities.GetDisposalStatus( _kind ) );
    }
    internal void Adjust( ref decimal _toDispose )
    {
      this.SettledQuantityDec += this.Disposal2IPRIndex.Withdraw( ref _toDispose, this.SettledQuantityDec );
    }
    internal decimal SettledQuantityDec
    {
      get { return Convert.ToDecimal( this.SettledQuantity ); }
      set { this.SettledQuantity = Convert.ToDouble( value ); }
    }
    #endregion

    #region private
    /// <summary>
    /// Calculate values of columns: Title, DutyPerSettledAmount, VATPerSettledAmount, TobaccoValue, DutyAndVAT.
    /// </summary>
    /// <param name="clearingType">Type of the clearing.</param>
    private void CalculateDutyAndVat()
    {
      try
      {
        if ( this.DisposalStatus.Value == Linq.DisposalStatus.Cartons )
          return;
        double _portion = SettledQuantity.Value / Disposal2IPRIndex.NetMass.Value;
        if ( this.ClearingType.Value == Linq.ClearingType.PartialWindingUp )
        {
          DutyPerSettledAmount = ( Disposal2IPRIndex.Duty.Value * _portion ).RoundCurrency();
          VATPerSettledAmount = ( Disposal2IPRIndex.VAT.Value * _portion ).RoundCurrency();
          TobaccoValue = ( Disposal2IPRIndex.Value.Value * _portion ).RoundCurrency();
        }
        else
        {
          DutyPerSettledAmount = GetDutyNotCleared();
          VATPerSettledAmount = GetVATNotCleared();
          TobaccoValue = GetPriceNotCleared();
        }
        DutyAndVAT = DutyPerSettledAmount.Value + VATPerSettledAmount.Value;
      }
      catch ( Exception ex )
      {
        throw GenericStateMachineEngine.ActionResult.Exception( ex, @"CAS.SmartFactory.IPR.WebsiteModel.Linq\SetUpCalculatedColumns" );
      }
    }
    private void AssignSADGood( Entities edc, SADGood sadGood )
    {
      string productCodeNumber = sadGood.PCNTariffCode;
      PCNCode _pcn = PCNCode.Find( edc, IsDisposal, productCodeNumber );
      this.Disposal2PCNID = _pcn;
      this.SADDocumentNo = sadGood.SADDocumentIndex.DocumentNumber;
      this.SADDate = sadGood.SADDocumentIndex.CustomsDebtDate;
      this.CustomsProcedure = sadGood.Procedure;
    }
    private double GetDutyNotCleared()
    {
      return Disposal2IPRIndex.Duty.Value - ( from _dec in Disposal2IPRIndex.Disposal where _dec.DutyPerSettledAmount.HasValue select new { val = _dec.DutyPerSettledAmount.Value } ).Sum( itm => itm.val );
    }
    private double GetPriceNotCleared()
    {
      return Disposal2IPRIndex.IPRUnitPrice.Value - ( from _dec in Disposal2IPRIndex.Disposal where _dec.TobaccoValue.HasValue select new { val = _dec.TobaccoValue.Value } ).Sum( itm => itm.val );
    }
    private double GetVATNotCleared()
    {
      return Disposal2IPRIndex.VAT.Value - ( from _dec in Disposal2IPRIndex.Disposal where _dec.VATPerSettledAmount.HasValue select new { val = _dec.VATPerSettledAmount.Value } ).Sum( itm => itm.val );
    }
    private bool IsDisposal
    {
      get
      {
        bool _ret = false;
        switch ( this.DisposalStatus.Value )
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
