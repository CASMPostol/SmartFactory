using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Format
  {
    internal static Format GetCutfillerFormatLookup( Entities edc )
    {
      return GetFormatLookup( m_CutfillerLength, m_CutfillerLength, edc );
    }
    public static Format GetFormatLookup( string cigaretteLength, string filterSegmentLength, Entities edc )
    {
      try
      {
        return ( from idx in edc.Format where idx.Match( cigaretteLength, filterSegmentLength ) orderby idx.Wersja descending select idx ).First();
      }
      catch ( Exception ex )
      {
        string message = String.Format( "Cannot find the format cigarette length: {0}/filter segment length: {1}", cigaretteLength, filterSegmentLength );
        throw new IPRDataConsistencyException( m_Souece, message, ex, "GetFormatLookup failed" );
      }
    }
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
    private const string m_Souece = "Format processing";
    private const string m_CutfillerLength = "0.00 mm";
  }
}
