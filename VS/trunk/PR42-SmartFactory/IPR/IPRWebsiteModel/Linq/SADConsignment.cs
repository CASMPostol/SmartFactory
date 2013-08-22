using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class SADConsignment
  {
    internal static string DocumentNumber( Entities entities, int sadConsignmentNumber )
    {
      return Settings.DocumentNumber( entities, sadConsignmentNumber );
    }
  }
}
