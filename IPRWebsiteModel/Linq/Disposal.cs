using System;
using System.Linq;
using CAS.SharePoint;
using CAS.SharePoint.Web;

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
    /// <exception cref="ApplicationError">if any internal exception has to be catched.</exception>
    public void Export( Entities entities, Clearence clearence, ref double quantity, bool closingBatch, string invoiceNoumber )
    {
      string _at = "Startting";
      try
      {
        ClearingType _clearingType = Linq.ClearingType.PartialWindingUp;
        if ( !closingBatch && quantity < SettledQuantity )
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
            SettledQuantity = SettledQuantity - quantity
          };
          _at = "SetUpCalculatedColumns";
          _newDisposal.SetUpCalculatedColumns( CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp );
          SettledQuantity = quantity;
          quantity = 0;
          _at = "InsertOnSubmit";
          entities.Disposal.InsertOnSubmit( _newDisposal );
          _at = "SubmitChanges #1";
          entities.SubmitChanges();
        }
        else
        {
          _clearingType = Disposal2IPRIndex.GetClearingType();
          quantity -= SettledQuantity.Value;
        }
        _at = "StartClearance";
        StartClearance( _clearingType, invoiceNoumber, clearence );
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
        Disposal _lastOne = ( from _dsp in this.Disposal2IPRIndex.Disposal
                              where _dsp.CustomsStatus.Value == Linq.CustomsStatus.Finished
                              orderby _dsp.No.Value descending
                              select _dsp ).FirstOrDefault<Disposal>();
        if ( _lastOne == null )
          this.No = 1;
        else
          this.No = _lastOne.No++;
        PCNCode _pcn = PCNCode.Find( edc, clearence.ClearenceProcedure.Value, productCodeNumber );
        this.SADDocumentNo = documentNo;
        this.SADDate = clearanceDate;
        this.CustomsStatus = Linq.CustomsStatus.Finished;
        this.Disposal2IPRIndex.AccountBalance -= this.SettledQuantity.Value;
        this.RemainingQuantity = Disposal2IPRIndex.AccountBalance;
        if ( this.RemainingQuantity.Value == 0 )
          this.ClearingType = Linq.ClearingType.TotalWindingUp;
        this.CustomsProcedure = clearence.ProcedureCode;
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
    #endregion

    #region private
    /// <summary>
    /// Starts the clearance.
    /// </summary>
    /// <param name="clearingType">Type of the clearing.</param>
    /// <param name="invoiceNoumber">The invoice noumber.</param>
    /// <param name="clearence">The clearence.</param>
    private void StartClearance( ClearingType clearingType, string invoiceNoumber, Clearence clearence )
    {
      this.Disposal2ClearenceIndex = clearence;
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
    #endregion

  }
}
