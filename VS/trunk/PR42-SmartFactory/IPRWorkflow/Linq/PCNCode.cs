﻿using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class PCNCode
  {
    internal static void ImportData(ConfigurationPCNCodeItem[] configuration, Entities edc)
    {
      List<PCNCode> list = new List<PCNCode>();
      foreach (ConfigurationPCNCodeItem item in configuration)
      {
        PCNCode pcn = new PCNCode
        {
          CompensationGood = item.CompensationGood.ParseCompensationGood(),
          ProductCodeNumber = item.ProductCodeNumber,
          Title = item.Title
        };
        list.Add(pcn);
      };
      edc.PCNCode.InsertAllOnSubmit(list);
    }
    internal static PCNCode AddOrGet(Entities _edc, string _code)
    {
      PCNCode _pcncode = (from _pcnx in _edc.PCNCode where _code.Trim().Contains(_pcnx.ProductCodeNumber) select _pcnx).FirstOrDefault();
      if (_pcncode == null)
      {
        _pcncode = new PCNCode()
          {
            CompensationGood = Linq.IPR.CompensationGood.Tytoń,
            ProductCodeNumber = _code,
            Title = String.Format( "{0} {1}", Linq.IPR.CompensationGood.Tytoń, _code )
          };
        _edc.PCNCode.InsertOnSubmit(_pcncode);
      }
      return _pcncode;
    }
  }
}
