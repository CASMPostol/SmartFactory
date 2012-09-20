using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Clearence
  {
    internal static Clearence CreataClearence( Entities edc, string title, string code, ClearenceProcedure procedure )
    {
      Clearence _newClearence = new Clearence()
      {
        DocumentNo = "N/A",
        ProcedureCode = code,
        ReferenceNumber = "N/A",
        Status = false,
        Title = "Creating",
        ClearenceProcedure = procedure
      };
      edc.Clearence.InsertOnSubmit( _newClearence );
      edc.SubmitChanges();
      _newClearence.Title = String.Format( title, _newClearence.Identyfikator.Value );
      edc.SubmitChanges();
      return _newClearence;
    }
  }
}
