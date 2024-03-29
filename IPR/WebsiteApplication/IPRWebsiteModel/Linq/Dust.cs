﻿using System;
using System.Linq;
using System.Collections.Generic;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Dust
  {
    /// <summary>
    /// Gets the lookup to the <see cref="Dust"/> entity.
    /// </summary>
    /// <param name="type">The type.</param>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    /// <exception cref="IPRDataConsistencyException">Cannot find Dust</exception>
    public static Dust GetLookup( ProductType type, Entities edc )
    {
      try
      {
        return (from idx in edc.Dust where idx.ProductType == type orderby idx.Version select idx).First();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, m_Message, ex, "Cannot find Dust");
      }
    }
    #region private
    private const string m_Source = "Dust";
    private const string m_Message = "I cannot find any dust coefficient";
    #endregion
  }
}
