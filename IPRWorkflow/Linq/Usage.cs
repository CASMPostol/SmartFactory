using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Usage
  {

    internal static Usage GetLookup(Format format, Entities edc)
    {
      try
      {
        return (from idx in edc.Usage where idx.FormatIndex.Identyfikator == format.Identyfikator select idx).Aggregate((x, y) => (x.Wersja < y.Wersja ? y : x));
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, String.Format(m_Message, format), ex, "Usage lookup error");
      }
    }
    internal static void ImportData(ConfigurationUsageItem[] configuration, Entities edc)
    {
      List<Usage> list = new List<Usage>();
      foreach (ConfigurationUsageItem item in configuration)
      {
        Usage usg = new Usage
        {
          Batch = null,
          FormatIndex = Format.GetFormatLookup(item.Format_lookup, edc),
          UsageMax = item.UsageMax,
          UsageMin = item.UsageMin,
          //TODO  [pr4-3560] Usage List - add data and display CFT... column http://itrserver/Bugs/BugDetail.aspx?bid=3560
          //Schema does not contain definition of this columns.
          CTFUsageMax = int.MaxValue, 
          CTFUsageMin = int.MinValue
        };
        list.Add(usg);
      };
      edc.Usage.InsertAllOnSubmit(list);
    }

    #region private
    private const string m_Source = "Usage";
    private const string m_Message = "I cannot find usage coefficient for the format: {0}";
    #endregion
  }
}
