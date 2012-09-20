﻿using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Consent
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
    internal static Consent Lookup(Entities _edc, string _consentNo)
    {
      return (from _cidx in _edc.Consent where _cidx.Title.Trim().Equals(_consentNo.Trim()) orderby _cidx.Wersja descending select _cidx).First();
    }
  }
}
