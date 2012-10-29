using System;
using System.Linq;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Disposal
  {
    public void Export( Entities edc, string documentNo, Clearence clearence, DateTime clearanceDate, string productCodeNumber )
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
                         where _pcnx.Procedure.Contains( RequestedProcedure( clearence.ClearenceProcedure ) ) && _pcnx.ProductCodeNumber.Contains( productCodeNumber )
                         // && this.DisposalStatus == _pcnx.CompensationGood 
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
    //TODO [pr4-3737] Compensation good must be recognized using the PCN code from customs message http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=1744
    private string RequestedProcedure( ClearenceProcedure? nullable )
    {
      throw new NotImplementedException();
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
