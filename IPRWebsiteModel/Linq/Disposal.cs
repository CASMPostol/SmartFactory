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
    #region public

    /// <summary>
    /// Exports the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="quantity">The quantity.</param>
    /// <param name="closingBatch">if set to <c>true</c> the batch is to be closed.</param>
    /// <param name="invoiceNoumber">The invoice noumber.</param>
    /// <param name="invoiceContent">Content of the invoice.</param>
    /// <exception cref="ApplicationError">if any internal exception has to be catched.</exception>
    public void Export( Entities entities, Clearence clearence, ref decimal quantity, bool closingBatch, string invoiceNoumber, InvoiceContent invoiceContent )
    {
      string _at = "Startting";
      try
      {
        ClearingType _clearingType = Linq.ClearingType.PartialWindingUp;
        decimal _settledQuantity = Convert.ToDecimal( SettledQuantity.Value );
        if ( closingBatch )
          _clearingType = Disposal2IPRIndex.GetClearingType();
        else if ( quantity < _settledQuantity )
        {
          _at = "_newDisposal";
          Disposal _newDisposal = new Disposal()
          {
            Disposal2BatchIndex = Disposal2BatchIndex,
            Disposal2ClearenceIndex = null,
            ClearingType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp,
            CustomsStatus = Linq.CustomsStatus.NotStarted,
            CustomsProcedure = "N/A",
            DisposalStatus = DisposalStatus,
            InvoiceNo = "N/A",
            IPRDocumentNo = "N/A",
            Disposal2IPRIndex = Disposal2IPRIndex,
            Disposal2PCNID = Disposal2PCNID,
            JSOXCustomsSummaryIndex = null,
            Disposal2MaterialIndex = Disposal2MaterialIndex,
            No = int.MaxValue,
            SADDate = CAS.SharePoint.Extensions.SPMinimum,
            SADDocumentNo = "N/A",
            SettledQuantity = Convert.ToDouble( _settledQuantity - quantity )
          };
          _at = "SetUpCalculatedColumns";
          _newDisposal.SetUpCalculatedColumns( CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp );
          SettledQuantity = Convert.ToDouble( quantity );
          quantity = 0;
          _at = "InsertOnSubmit";
          entities.Disposal.InsertOnSubmit( _newDisposal );
          _at = "SubmitChanges #1";
          entities.SubmitChanges();
        }
        else
        {
          _clearingType = Disposal2IPRIndex.GetClearingType();
          quantity -= _settledQuantity;
        }
        _at = "StartClearance";
        StartClearance( _clearingType, invoiceNoumber, clearence, invoiceContent );
      }
      catch ( Exception _ex )
      {
        throw new ApplicationError( @"CAS.SmartFactory.IPR.WebsiteModel.Linq.Export", _at, _ex.Message, _ex );
      }
    }
    /// <summary>
    /// Calculate values of columns: Title, DutyPerSettledAmount, VATPerSettledAmount, TobaccoValue, DutyAndVAT.
    /// </summary>
    /// <param name="clearingType">Type of the clearing.</param>
    internal void SetUpCalculatedColumns( ClearingType clearingType )
    {
      try
      {
        string _titleTmplt = "Disposal: {0} of material {1}";
        Title = String.Format( _titleTmplt, this.DisposalStatus.Value.ToString(), this.Disposal2IPRIndex.Batch );
        if ( this.DisposalStatus.Value == Linq.DisposalStatus.Cartons )
          return;
        double _portion = SettledQuantity.Value / Disposal2IPRIndex.NetMass.Value;
        if ( clearingType == Linq.ClearingType.PartialWindingUp )
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
    internal void ClearThroughCustoms( Entities edc, string documentNo, Clearence clearence, DateTime clearanceDate, string productCodeNumber )
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
        PCNCode _pcn = PCNCode.Find( edc, IsDisposal, productCodeNumber );
        this.SADDocumentNo = documentNo;
        this.SADDate = clearanceDate;
        this.CustomsStatus = Linq.CustomsStatus.Finished;
        double _balance = Convert.ToDouble( Convert.ToDecimal( this.Disposal2IPRIndex.AccountBalance.Value ) - Convert.ToDecimal( this.SettledQuantity.Value ) );
        this.Disposal2IPRIndex.AccountBalance = _balance;
        this.RemainingQuantity = _balance;
        if ( this.RemainingQuantity.Value == 0 )
          this.ClearingType = Linq.ClearingType.TotalWindingUp;
        this.CustomsProcedure = Entities.ToString( clearence.ClearenceProcedure.Value );
        _at = "PCNCode _tobaccoPCN ";
        this.Disposal2PCNID = _pcn;
        this.Disposal2ClearenceIndex = clearence;
      }
      catch ( Exception _ex )
      {
        string _template = "Cannot finish Export of disposal {0} {1} because of internal error: {2} at: {3}";
        throw GenericStateMachineEngine.ActionResult.Exception( _ex, String.Format( _template, this.Title, this.Identyfikator.Value, _ex.Message, _at ) );
      }
    }
    internal static List<Disposal> Disposals( EntitySet<Disposal> disposlas, DisposalEnum _kind )
    {
      return disposlas.Where<Disposal>( x => x.DisposalStatus.Value == Entities.GetDisposalStatus( _kind ) ).ToList<Disposal>();
    }
    internal void Adjust( ref decimal _toDispose )
    {
      if ( this.CustomsStatus.Value == Linq.CustomsStatus.NotStarted )
      {
        decimal _2Add = this.Disposal2IPRIndex.TobaccoNotAllocatedDec - _toDispose;
        if ( _2Add <= 0 )
          return;
        this.SettledQuantity = Convert.ToDouble( this.SettledQuantityDec + _2Add );
        _toDispose -= _2Add;
      }
      else
        ;
    }
    #endregion

    #region private
    /// <summary>
    /// Starts the clearance.
    /// </summary>
    /// <param name="clearingType">Type of the clearing.</param>
    /// <param name="invoiceNoumber">The invoice noumber.</param>
    /// <param name="clearence">The clearence.</param>
    private void StartClearance( ClearingType clearingType, string invoiceNoumber, Clearence clearence, InvoiceContent invoiceContent )
    {
      this.Disposal2ClearenceIndex = clearence;
      this.Disposal2InvoiceContentIndex = invoiceContent;
      this.CustomsStatus = Linq.CustomsStatus.Started;
      this.ClearingType = clearingType;
      this.CustomsProcedure = Entities.ToString( clearence.ClearenceProcedure.Value );
      this.InvoiceNo = invoiceNoumber;
      this.SetUpCalculatedColumns( ClearingType.Value );
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


    private decimal SettledQuantityDec { get { return Convert.ToDecimal( this.SettledQuantity ); } }
  }
}
