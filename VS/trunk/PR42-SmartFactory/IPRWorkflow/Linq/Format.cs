﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Format
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
    internal static Format GetCutfillerFormatLookup(Entities edc)
    {
      return GetFormatLookup(m_CutfillerLength, m_CutfillerLength, edc);
    }
    internal static Format GetFormatLookup(string cigaretteLength, string filterSegmentLength, Entities edc)
    {
      try
      {
        return (from idx in edc.Format where idx.Match(cigaretteLength, filterSegmentLength) orderby idx.Wersja descending select idx).First();
      }
      catch (Exception ex)
      {
        string message = String.Format("Cannot find the format cigarette length: {0}/filter segment length: {1}", cigaretteLength, filterSegmentLength);
        throw new CAS.SmartFactory.xml.ImputDataErrorException(m_Souece, message, ex);
      }
    }
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
    private const string m_Souece = "Format processing";
    private const string m_CutfillerLength = "0.00 mm";
  }
}
