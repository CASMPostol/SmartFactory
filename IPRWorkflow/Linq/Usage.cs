using System.Collections.Generic;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  internal partial class UsageExtension
  {
    internal static void ImportData( ConfigurationUsageItem[] configuration, Entities edc )
    {
      List<Usage> list = new List<Usage>();
      foreach ( ConfigurationUsageItem item in configuration )
      {
        Usage usg = new Usage
        {
          Batch = null,
          FormatIndex = Format.GetFormatLookup( item.Format_lookup, edc ),
          UsageMax = item.UsageMax,
          UsageMin = item.UsageMin,
          //TODO  [pr4-3560] Usage List - add data and display CFT... column http://itrserver/Bugs/BugDetail.aspx?bid=3560
          //Schema does not contain definition of this columns.
          CTFUsageMax = int.MaxValue,
          CTFUsageMin = int.MinValue
        };
        list.Add( usg );
      };
      edc.Usage.InsertAllOnSubmit( list );
    }
  }
}
