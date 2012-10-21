using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.xml.DocumentsFactory
{
  /// <summary>
  /// Document Names
  /// </summary>
  public static class DocumentNames
  {
    internal static SPFile CreateXmlFile<type>( SPFileCollection destinationCollection, string fileName, type object2Serialize, string stylesheetName )
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
        file.WriteProcessingInstruction( "xml-stylesheet", "type=\"text/xsl\" " + String.Format( "href=\"{0}.xslt\"", stylesheetName ) );
        _srlzr.Serialize( file, object2Serialize );
        _docFile = destinationCollection.Add( fileName + ".xml", _docStrm, true );
      }
      return _docFile;
    }
    /// <summary>
    /// Dust form stylesheet name
    /// </summary>
    public const string DustFormStylesheetName = "DustFormStylesheet.xslt";
    /// <summary>
    /// The cartons form stylesheet name
    /// </summary>
    public const string CartonsFormStylesheetName = "CartonsFormStylesheet.xslt";
    /// <summary>
    /// Waste form stylesheet name
    /// </summary>
    public const string WasteFormStylesheetName = "WasteFormStylesheet.xslt";
    /// <summary>
    /// Tobacco form stylesheet name
    /// </summary>
    public const string TobaccoFormStylesheetName = "TobaccoFormStylesheet.xslt";
    /// <summary>
    /// Cigarette export form name
    /// </summary>
    public const string CigaretteExportFormName = "CigaretteExportFormCollection.xslt";
  }
}


