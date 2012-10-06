using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.xml.DocumentsFactory
{
  public static class DocumentNames
  {
    public const string FinishedGoodExportFormNumberFormat = "FGEF{0:D9}";
    public const string FinishedGoodExportFormRegularExpression = "";
    public static string xmlStylesheetHref( Type type )
    {
      return String.Format( "href=\"{0}.xslt\"", type.Name );
    }
    internal static SPFile CreateXmlFile<type>( SPFileCollection destinationCollection, string fileName, type object2Serialize )
    {
      SPFile _docFile = default( SPFile );
      XmlSerializer _srlzr = new XmlSerializer( typeof( type ) );
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using ( MemoryStream _docStrm = new MemoryStream() )
      using ( XmlWriter file = XmlWriter.Create( _docStrm, _setting ) )
      {
        file.WriteProcessingInstruction( "xml-stylesheet", "type=\"text/xsl\" href=\"CigaretteExportFormCollection.xslt\"" );
        _srlzr.Serialize( file, object2Serialize );
        _docFile = destinationCollection.Add( fileName + ".xml", _docStrm, true );
      }
      return _docFile;
    }
  }
}


