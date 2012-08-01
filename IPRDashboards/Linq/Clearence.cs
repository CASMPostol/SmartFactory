using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Clearence
  {
    internal static Clearence CreataClearence( EntitiesDataContext edc, string title, string code )
    {
      Clearence _newClearence = new Clearence()
      {
        DocumentNo = "N/A",
        ProcedureCode = code,
        ReferenceNumber = "N/A",
        Status = false,
        Tytuł = "Creating"
      };
      edc.Clearence.InsertOnSubmit( _newClearence );
      edc.SubmitChanges();
      _newClearence.Tytuł = String.Format( title, _newClearence.Identyfikator.Value );
      edc.SubmitChanges();
      return _newClearence;
    }
  }
}
