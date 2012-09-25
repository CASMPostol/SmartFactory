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
        //TODO move this code to the disposal creation procedure - must be get from message
        _at = "PCNCode _tobaccoPCN ";
        PCNCode _tobaccoPCN = ( from _pcnx in edc.PCNCode
                                where _pcnx.IsIPR.GetValueOrDefault( true ) && _pcnx.CompensationGood.Value == CompensationGood.Papierosy
                                select _pcnx ).FirstOrDefault();
        //TODO PCNCode - model error - this proble has been reported already.
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
