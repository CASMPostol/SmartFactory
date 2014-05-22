using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class SKUCigarette
  {
    #region public
    /// <summary>
    /// Gets the format lookup.
    /// </summary>
    /// <param name="cigaretteLenght">The cigarette lenght.</param>
    /// <param name="filterLenght">The filter lenght.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="warnings">The warnings.</param>
    /// <returns></returns>
    internal protected override Format GetFormatLookup( string cigaretteLenght, string filterLenght, Entities edc, List<String> warnings )
    {
      this.CigaretteLenght = cigaretteLenght;
      this.FilterLenght = filterLenght;
      Format frmt = Format.FindFormatLookup( cigaretteLenght, filterLenght, edc );
      if ( frmt == null )
        warnings.Add(String.Format( "Cannot find the format cigarette length: {0}/filter segment length: {1}", cigaretteLenght, filterLenght ));
      return frmt;
    }
    /// <summary>
    /// Gets the IPR material.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    internal protected override bool? GetIPRMaterial( Entities edc )
    {

      if ( String.IsNullOrEmpty( this.PrimeMarket ) )
      {
        ActivityLogCT.WriteEntry( edc, m_Source, string.Format( m_PMTemplate, this.Title ) );
        return new Nullable<bool>();
      }
      return !CustomsUnion.CheckIfUnion( this.PrimeMarket, edc );
    }
    private const string m_FrmtTemplate = "I cannot recognize the format for {0}";
    private const string m_Source = "Cigarettes SKU processing";
    private const string m_PMTemplate = "I cannot analize the market for {0} because the name is epty";
    #endregion
  }
}
