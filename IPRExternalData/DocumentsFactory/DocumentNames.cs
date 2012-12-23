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
    internal static void WriteXmlFile<type>( SPFile docFile, type object2Serialize, string stylesheetName )
    {
      XmlSerializer _srlzr = new XmlSerializer( typeof( type ) );
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using ( Stream _docStrm = new MemoryStream( 30000 ) )
      using ( XmlWriter _file = XmlWriter.Create( _docStrm, _setting ) )
      {
        _file.WriteProcessingInstruction( "xml-stylesheet", "type=\"text/xsl\" " + String.Format( "href=\"{0}.xslt\"", stylesheetName ) );
        _srlzr.Serialize( _file, object2Serialize );
      }
      docFile.SaveBinary( _docStrm );
      docFile.Update();
    }
    /// <summary>
    /// Dust form stylesheet name
    /// </summary>
    public const string DustFormStylesheetName = "DustFormStylesheet";
    /// <summary>
    /// The cartons form stylesheet name
    /// </summary>
    public const string CartonsFormStylesheetName = "CartonsFormStylesheet";
    /// <summary>
    /// Waste form stylesheet name
    /// </summary>
    public const string WasteFormStylesheetName = "WasteFormStylesheet";
    /// <summary>
    /// Tobacco form stylesheet name
    /// </summary>
    public const string TobaccoFormStylesheetName = "TobaccoFormStylesheet";
    /// <summary>
    /// Cigarette export form name
    /// </summary>
    public const string CigaretteExportFormName = "CigaretteExportFormCollection";
    /// <summary>
    /// The request for account clearence name
    /// </summary>
    public static string RequestForAccountClearenceName = "RequestForAccountClearence";
    /// <summary>
    /// The balance sheet content name
    /// </summary>
    public static string BalanceSheetContentName = "BalanceSheetCollection";
  }
}


