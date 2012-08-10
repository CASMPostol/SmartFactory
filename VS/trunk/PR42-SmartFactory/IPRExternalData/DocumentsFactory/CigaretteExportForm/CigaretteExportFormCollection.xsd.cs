using System.IO;
using System.Xml.Serialization;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm
{
  public partial class CigaretteExportFormCollection
  {
    public SPFile AddDocument2Collection( SPFileCollection _dstCollection, string fileName )
    {
      SPFile _docFile = default( SPFile );
      XmlSerializer _srlzr = new XmlSerializer( typeof( CigaretteExportFormCollection ) );
      using ( MemoryStream _docStrm = new MemoryStream() )
      {
        _srlzr.Serialize( _docStrm, this );
        _docFile = _dstCollection.Add( fileName + ".xml", _docStrm, true );
      }
      return _docFile;
    }
  }
}
