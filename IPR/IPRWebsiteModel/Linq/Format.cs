using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Format
  {
    /// <summary>
    /// Gets the default format lookup.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    internal static Format FindFormatLookup( Entities edc )
    {
      return FindFormatLookup( m_CutfillerLength, m_CutfillerLength, edc );
    }
    /// <summary>
    /// Gets the format lookup according to the finisched good description.
    /// </summary>
    /// <param name="cigaretteLength">Length of the cigarette.</param>
    /// <param name="filterSegmentLength">Length of the filter segment.</param>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <returns></returns>
    public static Format FindFormatLookup( string cigaretteLength, string filterSegmentLength, Entities edc )
    {
      return ( from idx in edc.Format where idx.Match( cigaretteLength, filterSegmentLength ) orderby idx.Wersja descending select idx ).FirstOrDefault();
    }
    /// <summary>
    /// Gets the format lookup.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="edc">The edc.</param>
    /// <returns></returns>
    public static Format GetFormatLookup( string name, Entities edc )
    {
      Format frmt = ( from idx in edc.Format where idx.Title.StartsWith( name ) orderby idx.Identyfikator descending, idx.Wersja descending select idx ).First();
      return frmt;
    }
    private bool Match( string cigaretteLength, string filterSegmentLength )
    {
      const string frmt = "{0}:{1}";
      return String.Format( frmt, this.CigaretteLenght, this.FilterLenght ).CompareTo( String.Format( frmt, cigaretteLength, filterSegmentLength ) ) == 0;
    }
    private const string m_CutfillerLength = "0.00 mm";
  }
}
