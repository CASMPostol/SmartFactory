﻿using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Waste
  {
    #region public
    internal static Waste GetLookup( ProductType type, Entities edc )
    {
      try
      {
        return ( from idx in edc.Waste where idx.ProductType == type orderby idx.Wersja descending select idx ).First();
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( m_Source, String.Format( m_Message, type ), ex, "Waste lookup error" );
      }
    }
    #endregion

    #region private
    private const string m_Source = "Waste";
    private const string m_Message = "I cannot find waste coefficient for the product type: {0}";
    #endregion
  }
}
