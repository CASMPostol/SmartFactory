using System.Collections.Generic;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  internal static class ConsentExtension
  {
    internal static void ImportData(ConfigurationConsentItem[] configuration, Entities edc)
    {
      List<Consent> list = new List<Consent>();
      foreach (ConfigurationConsentItem item in configuration)
      {
        Consent cns = new Consent
        {
          ProductivityRateMax = item.ProductivityRateMax,
          ProductivityRateMin = item.ProductivityRateMin,
          ValidFromDate = item.ValidFromDate,
          ValidToDate = item.ValidToDate,
          ConsentPeriod = item.ConsentPeriod,
          Title = item.ConsentNo
        };
        list.Add(cns);
      };
      edc.Consent.InsertAllOnSubmit(list);
    }
  }
}
