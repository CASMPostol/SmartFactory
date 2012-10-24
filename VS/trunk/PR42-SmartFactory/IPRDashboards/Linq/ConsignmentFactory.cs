using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using CAS.SmartFactory.xml.DocumentsFactory.Disposals;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.IPR.Dashboards.Clearance
{
  /// <summary>
  /// Dokument Extension
  /// </summary>
  internal static class ConsignmentFactory
  {
    internal static int Prepare
      ( SPWeb site, CigaretteExportFormCollection _consignment, string fileName )
    {
      string _stt = "Starting";
      try
      {
        _stt = "SPDocumentLibrary";
        SPDocumentLibrary _lib = (SPDocumentLibrary)site.Lists[ CommonDefinitions.IPRSADConsignmentLibraryTitle ];
        _stt = "AddDocument2Collection";
        SPFile _docFile = _consignment.AddDocument2Collection( _lib.RootFolder.Files, fileName );
        return _docFile.Item.ID;
      }
      catch ( Exception ex )
      {
        throw new ApplicationError( "Dokument.PrepareConsignment", _stt, String.Format( "Cannot finish the operation because of error {0}", ex.Message ), ex );
      }
    }
    internal static int Prepare
      ( SPWeb site, DocumentContent document, string fileName, CompensatiionGood compensatiionGood )
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
