using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Dokument
  {
    internal static int PrepareConsignment
      ( SPWeb site, List<CigaretteExportForm> _consignment, string fileName, string invoiceNo )
    {
      string _stt = "Starting";
      try
      {
        _stt = "CigaretteExportFormCollection";
        CigaretteExportFormCollection _cefc = CigaretteExportFormCollectionFactory.CigaretteExportFormCollection( _consignment, fileName, invoiceNo );
        _stt = "SPDocumentLibrary";
        SPDocumentLibrary _lib = (SPDocumentLibrary)site.Lists[ CommonDefinitions.IPRSADConsignmentLibraryTitle ];
        _stt = "AddDocument2Collection";
        SPFile _docFile = _cefc.AddDocument2Collection( _lib.RootFolder.Files, fileName );
        return _docFile.Item.ID;
      }
      catch ( Exception ex )
      {
        throw new ApplicationError( "Dokument.PrepareConsignment", _stt, String.Format( "Cannot finish the operation because of error {0}", ex.Message ), ex );
      }
    }
    internal static int PrepareConsignment
      ( SPWeb site, xml.DocumentsFactory.DustWasteForm.DocumentContent document, string fileName, xml.DocumentsFactory.DustWasteForm.CompensatiionGood compensatiionGood )
    {
      string _stt = "Starting";
      try
      {
        _stt = "SPDocumentLibrary";
        SPDocumentLibrary _lib = (SPDocumentLibrary)site.Lists[ CommonDefinitions.IPRSADConsignmentLibraryTitle ];
        _stt = "AddDocument2Collection";
        SPFile _docFile = document.AddDocument2Collection( _lib.RootFolder.Files, fileName, compensatiionGood );
        return _docFile.Item.ID;
      }
      catch ( Exception ex )
      {
        throw new ApplicationError( "Dokument.PrepareConsignment", _stt, String.Format( "Cannot finish the operation because of error {0}", ex.Message ), ex );
      }
    }
  }
}
