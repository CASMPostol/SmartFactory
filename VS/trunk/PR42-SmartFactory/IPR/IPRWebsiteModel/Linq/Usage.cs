﻿using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Usage
  {

    public static Usage GetLookup( Format format, Entities edc )
    {
      try
      {
        return (from idx in edc.Usage where idx.FormatIndex.Id == format.Id select idx).Aggregate((x, y) => (x.Version < y.Version ? y : x));
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, String.Format(m_Message, format), ex, "Usage lookup error");
      }
    }

    #region private
    private const string m_Source = "Usage";
    private const string m_Message = "I cannot find usage coefficient for the format: {0}";
    #endregion
  }
}
