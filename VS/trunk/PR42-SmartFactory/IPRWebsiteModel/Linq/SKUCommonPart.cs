using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public abstract partial class SKUCommonPart
  {
    #region public
    internal static SKUCommonPart Find(Entities edc, string index)
    {
      SKUCommonPart newSKU = null;
      try
      {
        newSKU = (
            from idx in edc.SKU
            where idx.SKU.Contains(index)
            orderby idx.Wersja descending
            select idx).First();
      }
      catch (Exception) { }
      return newSKU;
    }
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    internal static SKUCommonPart GetLookup(Entities edc, string index)
    {
      try
      {
       return (
            from idx in edc.SKU
            where idx.SKU.Contains(index)
            orderby idx.Wersja descending
            select idx).First();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException(m_Source, String.Format(m_Message, index), ex, "SKU lookup error");
      }
    }
    #endregion

    #region private
    protected abstract bool? GetIPRMaterial( Entities edc );
    private const string m_Source = "SKU Processing";
    private const string m_Message = "I cannot find material with SKU: {0}";
    #endregion
  }
}
