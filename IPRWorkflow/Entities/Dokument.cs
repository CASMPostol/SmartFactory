using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Dokument
  {
    internal static Dokument GetEntity(int id, EntityList<Dokument> list)
    {
      try
      {
        return (from enr in list where enr.Identyfikator == id select enr).First<Dokument>();
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
