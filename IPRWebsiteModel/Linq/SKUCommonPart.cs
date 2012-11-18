using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public abstract partial class SKUCommonPart
  {
    #region public
    public static SKUCommonPart Find( Entities edc, string index )
    {
      return ( from idx in edc.SKU
               where idx.SKU.Contains( index )
               orderby idx.Wersja descending
               select idx ).FirstOrDefault();
    }
    /// <summary>
    /// Gets the lookup.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public static SKUCommonPart GetLookup( Entities edc, string index )
    {
      try
      {
        return (
             from idx in edc.SKU
             where idx.SKU.Contains( index )
             orderby idx.Wersja descending
             select idx ).First();
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( m_Source, String.Format( m_Message, index ), ex, "SKU lookup error" );
      }
    }
    /// <summary>
    /// Processes the data.
    /// </summary>
    /// <param name="cigaretteLenght">The cigarette lenght.</param>
    /// <param name="filterLenght">The filter lenght.</param>
    /// <param name="edc">The edc.</param>
    public void ProcessData( string cigaretteLenght, string filterLenght, Entities edc )
    {
      this.FormatIndex = GetFormatLookup( cigaretteLenght, filterLenght, edc );
      this.IPRMaterial = GetIPRMaterial( edc );
    }
    internal protected abstract Format GetFormatLookup( string cigaretteLenght, string filterLenght, Entities edc );
    internal protected abstract bool? GetIPRMaterial( Entities edc );
    #endregion

    #region private
    private const string m_Source = "SKU Processing";
    private const string m_Message = "I cannot find material with SKU: {0}";
    #endregion
  }
}
