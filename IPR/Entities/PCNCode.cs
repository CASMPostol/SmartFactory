﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class PCNCode
  {
    internal static void ImportData(ConfigurationPCNCodeItem[] configuration, EntitiesDataContext edc)
    {
      List<PCNCode> list = new List<PCNCode>();
      foreach (ConfigurationPCNCodeItem item in configuration)
      {
        PCNCode pcn = new PCNCode
        {
          CompensationGood = item.CompensationGood.ParseCompensationGood(),
          Disposal = null, //TODO to be remove - it is actualy removed from the model but it is still here ?
          ProductCodeNumber = item.ProductCodeNumber,
          Tytuł = item.Title
        };
        list.Add(pcn);
      };
      edc.PCNCode.InsertAllOnSubmit(list);
    }
  }
}
