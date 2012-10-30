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
    /// Starts the clearance.
    /// </summary>
    /// <param name="clearingType">Type of the clearing.</param>
    /// <param name="invoiceNoumber">The invoice noumber.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="clearence">The clearence.</param>
    public void StartClearance( ClearingType clearingType, string invoiceNoumber, string procedure, Clearence clearence )
    {
      this.Disposal2ClearenceIndex = clearence;
      this.CustomsStatus = Linq.CustomsStatus.Started;
      this.ClearingType = clearingType;
      this.CustomsProcedure = procedure;
      this.InvoiceNo = invoiceNoumber;
      this.SetUpCalculatedColumns( ClearingType.Value );
    }
    /// <summary>
    /// Calculate values of columns: Title, DutyPerSettledAmount, VATPerSettledAmount, TobaccoValue, DutyAndVAT.
    /// </summary>
    /// <param name="clearingType">Type of the clearing.</param>
    public void SetUpCalculatedColumns( ClearingType clearingType )
    {
      string _titleTmplt = "Disposal: {0} of material {1}";
      Title = String.Format( _titleTmplt, this.DisposalStatus.Value.ToString(), this.Disposal2IPRIndex.Batch );
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
        //TODO [pr4-3737] Compensation good must be recognized using the PCN code from customs message http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=1744
        PCNCode _pcn = ( from _pcnx in edc.PCNCode
                         where _pcnx.Procedure.Contains( Entities.RequestedProcedure( clearence.ClearenceProcedure ) ) && _pcnx.ProductCodeNumber.Contains( productCodeNumber )
                         select _pcnx ).FirstOrDefault();
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
