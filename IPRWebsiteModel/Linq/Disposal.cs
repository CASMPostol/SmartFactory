using System;
using System.Linq;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Disposal
  {
    public void Export( Entities edc, string documentNo, Clearence clearence, DateTime clearenceDate )
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
        this.SADDocumentNo = documentNo;
        this.SADDate = clearenceDate;
        this.CustomsStatus = Linq.CustomsStatus.Finished;
        this.Disposal2IPRIndex.AccountBalance -= this.SettledQuantity.Value;
        this.RemainingQuantity = Disposal2IPRIndex.AccountBalance;
        if ( this.RemainingQuantity.Value == 0 )
          this.ClearingType = Linq.ClearingType.TotalWindingUp;
        this.CustomsProcedure = clearence.ProcedureCode;
        //TODO [pr4-3737] Compensation good must be recognized using the PCN code from customs message http://itrserver/Bugs/BugDetail.aspx?bid=3737
        _at = "PCNCode _tobaccoPCN ";
        PCNCode _tobaccoPCN = ( from _pcnx in edc.PCNCode
                                where _pcnx.IsIPR.GetValueOrDefault( true ) && _pcnx.CompensationGood.Value == Linq.CompensationGood.Papierosy
                                select _pcnx ).FirstOrDefault();
        //TODO [pr4-3733] Export: Association of the SAD documents: SAD analyses error at Clearence analyses error at started. 
        //this.Disposal2PCNCompensationGood = _tobaccoPCN == null ? null : _tobaccoPCN;
        this.Disposal2ClearenceIndex = clearence;
      }
      catch ( Exception _ex )
      {
        string _template = "Cannot finish Export of disposal {0} {1} because of internal error: {2} at: {3}";
        throw GenericStateMachineEngine.ActionResult.Exception( _ex, String.Format( _template, this.Title, this.Identyfikator.Value, _ex.Message, _at ) );
      }
    }
    public void StartClearance( ClearingType clearingType, string invoiceNoumber, string procedure, Clearence clearence )
    {
      this.Disposal2ClearenceIndex = clearence;
      this.CustomsStatus = Linq.CustomsStatus.Started;
      this.ClearingType = clearingType;
      this.CustomsProcedure = procedure;
      this.InvoiceNo = invoiceNoumber;
      this.SetUpCalculatedColumns( ClearingType.Value );
    }
  }
}
