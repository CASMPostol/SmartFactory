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
    /// <summary>
    /// Finds the specified <see cref="SKUCommonPart"/>.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public static SKUCommonPart Find( Entities edc, string index )
    {
      return ( from idx in edc.SKU
               where idx.SKU.Contains( index )
               orderby idx.Wersja descending
               select idx ).FirstOrDefault();
    }
    /// <summary>
    /// Processes the data.
    /// </summary>
    /// <param name="cigaretteLenght">The cigarette lenght.</param>
    /// <param name="filterLenght">The filter lenght.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="warnings">The warnings.</param>
    public void ProcessData( string cigaretteLenght, string filterLenght, Entities edc, List<String> warnings )
    {
      this.FormatIndex = GetFormatLookup( cigaretteLenght, filterLenght, edc, warnings );
      this.IPRMaterial = GetIPRMaterial( edc );
    }
    /// <summary>
    /// Gets the format lookup.
    /// </summary>
    /// <param name="cigaretteLenght">The cigarette lenght.</param>
    /// <param name="filterLenght">The filter lenght.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="warnings">The warnings.</param>
    /// <returns></returns>
    internal protected abstract Format GetFormatLookup( string cigaretteLenght, string filterLenght, Entities edc, List<String> warnings );
    /// <summary>
    /// Gets the IPR material.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    internal protected abstract bool? GetIPRMaterial( Entities edc );
    #endregion

    #region private
    private const string m_Source = "SKU Processing";
    private const string m_Message = "I cannot find material with SKU: {0}";
    #endregion
  }
}
