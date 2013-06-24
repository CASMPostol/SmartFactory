using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class SKUCutfiller
  {
    /// <summary>
    /// Gets the IPR material.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    internal protected override bool? GetIPRMaterial( Entities edc )
    {
      return ( !String.IsNullOrEmpty( BlendPurpose ) ) && BlendPurpose.Contains( "NEU" );
    }
    /// <summary>
    /// Gets the format lookup fo finished good.
    /// </summary>
    /// <param name="cigaretteLenght">The cigarette lenght.</param>
    /// <param name="filterLenght">The filter lenght.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="warnings">The warnings.</param>
    /// <returns></returns>
    internal protected override Format GetFormatLookup( string cigaretteLenght, string filterLenght, Entities edc, List<String> warnings )
    {
      Format _ret = Format.FindFormatLookup( edc );
      if ( _ret == null )
        warnings.Add( "Cannot find the format of cutfiller in the dictionary - Format list must be updated" );
      return _ret;
    }
  }
}
