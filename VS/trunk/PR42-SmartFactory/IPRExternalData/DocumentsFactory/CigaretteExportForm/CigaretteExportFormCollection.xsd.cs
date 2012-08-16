using System.IO;
using System.Xml.Serialization;
using Microsoft.SharePoint;
using System.Xml;

namespace CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm
{
  public partial class CigaretteExportFormCollection
  {
    /// <summary>
    /// Adds the document to collection.
    /// </summary>
    /// <param name="destinationCollection">The destination collection.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <returns>An object of <see cref="SPFile"/> containing the serialized <paramref name="destinationCollection"/></returns>
    public SPFile AddDocument2Collection( SPFileCollection destinationCollection, string fileName )
    {
      SPFile _docFile = default( SPFile );
      XmlSerializer _srlzr = new XmlSerializer( typeof( CigaretteExportFormCollection ) );
      XmlWriterSettings _setting = new XmlWriterSettings()
      {
        Indent = true,
        IndentChars = "  ",
        NewLineChars = "\r\n"
      };
      using ( MemoryStream _docStrm = new MemoryStream() )
      using (XmlWriter file = XmlWriter.Create( _docStrm, _setting ))
      {
        _srlzr.Serialize( _docStrm, this );
        _docFile = destinationCollection.Add( fileName + ".xml", _docStrm, true );
      }
      return _docFile;
    }
  }
}
