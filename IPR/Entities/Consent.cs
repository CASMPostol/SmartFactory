using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Consent
  {
    internal static void ImportData(ConfigurationConsentItem[] configuration, EntitiesDataContext edc)
    {
      List<Consent> list = new List<Consent>();
      foreach (ConfigurationConsentItem item in configuration)
      {
        Consent cns = new Consent
        {
          ConsentNo = item.ConsentNo,
          IPR = null, //TODO to be removed
          ProductivityRateMax = item.ProductivityRateMax,
          ProductivityRateMin = item.ProductivityRateMin,
          Tytuł = item.Title,
          ValidFromDate = item.ValidFromDate,
          ValidToDate = item.ValidToDate
        };
        list.Add(cns);
      };
      edc.Consent.InsertAllOnSubmit(list);
    }
  }
}
