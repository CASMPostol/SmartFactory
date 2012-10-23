using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Format
  {
    internal static Format GetFormatLookup(string name, Entities edc)
    {
      Format frmt = (from idx in edc.Format where idx.Title.StartsWith(name) orderby idx.Identyfikator descending, idx.Wersja descending select idx).First();
      return frmt;
    }
    private bool Match(string cigaretteLength, string filterSegmentLength)
    {
      const string frmt = "{0}:{1}";
      return String.Format(frmt, this.CigaretteLenght, this.FilterLenght).CompareTo(String.Format(frmt, cigaretteLength, filterSegmentLength)) == 0;
    }
  }
}
