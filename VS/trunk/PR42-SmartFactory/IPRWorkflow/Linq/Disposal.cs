using System;
using System.Linq;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Disposal
  {
    internal void Export( Entities edc, string documentNo, Clearence clearence, DateTime clearenceDate )
    {
      string _at = "starting";
      try
      {
        _at = "Disposal _lastOne";
        Disposal _lastOne = ( from _dsp in this.Disposal2IPRIndex.Disposal
                              where _dsp.CustomsStatus.Value == Linq.IPR.CustomsStatus.Finished
                              orderby _dsp.No.Value descending
                              select _dsp ).FirstOrDefault<Disposal>();
        if ( _lastOne == null )
          this.No = 1;
        else
          this.No = _lastOne.No++;
        this.SADDocumentNo = documentNo;
        this.SADDate = clearenceDate;
        this.CustomsStatus = Linq.IPR.CustomsStatus.Finished;
        this.Disposal2IPRIndex.AccountBalance -= this.SettledQuantity.Value;
        this.RemainingQuantity = Disposal2IPRIndex.AccountBalance;
        if ( this.RemainingQuantity.Value == 0 )
          this.ClearingType = Linq.IPR.ClearingType.TotalWindingUp;
        this.CustomsProcedure = clearence.ProcedureCode;
        //TODO [pr4-3737] Compensation good must be recognized using the PCN code from customs message http://itrserver/Bugs/BugDetail.aspx?bid=3737
        _at = "PCNCode _tobaccoPCN ";
        PCNCode _tobaccoPCN = ( from _pcnx in edc.PCNCode
                                where _pcnx.IsIPR.GetValueOrDefault( true ) && _pcnx.CompensationGood.Value == Linq.IPR.CompensationGood.Papierosy
                                select _pcnx ).FirstOrDefault();
        //TODO [pr4-3733] Export: Association of the SAD documents: SAD analyses error at Clearence analyses error at started. 
        //this.Disposal2PCNCompensationGood = _tobaccoPCN == null ? null : _tobaccoPCN;
        this.ClearenceIndex = clearence;
      }
      catch ( Exception _ex )
      {
        string _template = "Cannot finish Export of disposal {0} {1} because of internal error: {2} at: {3}";
        throw GenericStateMachineEngine.ActionResult.Exception( _ex, String.Format( _template, this.Title, this.Identyfikator.Value, _ex.Message, _at ) );
      }
    }
  }
}
