using System.Collections.Generic;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  internal static class FormatExtension
  {
    internal static void ImportData(ConfigurationFormatItem[] configurationFormatItem, Entities edc)
    {
      List<Format> list = new List<Format>();
      foreach (ConfigurationFormatItem item in configurationFormatItem)
      {
        Format frmt = new Format
        {
          CigaretteLenght = item.CigaretteLenght,
          FilterLenght = item.FilterLenght,
          Title = item.Title
        };
        list.Add(frmt);
      };
      edc.Format.InsertAllOnSubmit(list);
    }
  }
}
