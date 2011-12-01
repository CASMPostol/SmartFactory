using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Format
  {
    private bool Match(string cigaretteLength, string filterSegmentLength)
    {
      const string frmt = "{0:d}:{1:d}";
      return String.Format(frmt, this.CigaretteLenght, this.FilterLenght).CompareTo(String.Format(frmt, cigaretteLength, filterSegmentLength)) == 0;
    }
    internal static void ImportData(ConfigurationFormatItem[] configurationFormatItem, EntitiesDataContext edc)
    {
      List<Format> list = new List<Format>();
      foreach (ConfigurationFormatItem item in configurationFormatItem)
      {
        Format frmt = new Format
        {
          CigaretteLenght = item.CigaretteLenght,
          FilterLenght = item.FilterLenght,
          Tytuł = item.Title
        };
        list.Add(frmt);
      };
      edc.Format.InsertAllOnSubmit(list);
    }
    internal static Format GetCutfillerFormatLookup(EntitiesDataContext edc)
    {
      return GetFormatLookup(String.Empty, String.Empty, edc);
    }
    internal static Format GetFormatLookup(string cigaretteLength, string filterSegmentLength, EntitiesDataContext edc)
    {
      Format frmt = (from idx in edc.Format where idx.Match(cigaretteLength, filterSegmentLength) orderby idx.Wersja descending select idx).First();
      return frmt;
    }
    internal static Format GetFormatLookup(string name, EntitiesDataContext edc)
    {
      Format frmt = (from idx in edc.Format where idx.Tytuł.StartsWith(name) orderby idx.Identyfikator descending, idx.Wersja descending select idx).First();
      return frmt;
    }
  }
}
