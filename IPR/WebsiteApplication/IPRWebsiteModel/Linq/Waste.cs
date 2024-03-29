﻿using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Waste
  {
    #region public
    /// <summary>
    /// Gets the lookup to the <see cref="Waste"/>.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    /// <exception cref="IPRDataConsistencyException">Waste lookup error</exception>
    public static Waste GetLookup( ProductType type, Entities edc )
    {
      try
      {
        return ( from idx in edc.Waste where idx.ProductType == type orderby idx.Version descending select idx ).First();
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
