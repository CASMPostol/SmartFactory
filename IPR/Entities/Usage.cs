using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Usage
  {

    internal static Usage GetLookup(Format format, EntitiesDataContext edc)
    {
      Usage value = null;
      try
      {
        value = (from idx in edc.Usage where idx.FormatLookup.Identyfikator == format.Identyfikator select idx).Aggregate((x, y) => (x.Wersja < y.Wersja ? y : x));
        return value;
      }
      catch (ArgumentNullException)
      {
        //value = new Usage() {   = 0, ProductType = type, Tytuł = type.ToString() }; //TODO remove in final version and replace by throwin an exception
      }
      return value;
    }
    internal static void ImportData(ConfigurationUsageItem[] configuration, EntitiesDataContext edc)
    {
      List<Usage> list = new List<Usage>();
      foreach (ConfigurationUsageItem item in configuration)
      {
        Usage usg = new Usage
        {
          Batch = null,
          FormatLookup = Format.GetFormatLookup(item.Format_lookup, edc),
          UsageMax = item.UsageMax,
          UsageMin = item.UsageMin
        };
        list.Add(usg);
      };
      edc.Usage.InsertAllOnSubmit(list);
    }
  }
}
