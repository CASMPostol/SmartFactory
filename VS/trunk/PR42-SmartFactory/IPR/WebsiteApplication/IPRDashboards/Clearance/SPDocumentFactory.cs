using System;
using CAS.SharePoint;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using CAS.SmartFactory.xml.DocumentsFactory.Disposals;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.IPR.Dashboards.Clearance
{
  /// <summary>
  /// Dokument Extension
  /// </summary>
  internal static class SPDocumentFactory
  {
    internal static int Prepare( SPWeb site, CigaretteExportFormCollection _consignment, string fileName )
    {
      string _stt = "AddDocument2Collection";
      try
      {
        SPFile _docFile = _consignment.AddDocument2Collection( site, fileName, CommonDefinitions.IPRSADConsignmentLibraryTitle );
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
      string _stt = "AddDocument2Collection";
      try
      {
        SPFile _docFile = document.AddDocument2Collection( site, fileName, CommonDefinitions.IPRSADConsignmentLibraryTitle, compensatiionGood );
        return _docFile.Item.ID;
      }
      catch ( Exception ex )
      {
        throw new ApplicationError( "Dokument.PrepareConsignment", _stt, String.Format( "Cannot finish the operation because of error {0}", ex.Message ), ex );
      }
    }
  }
}
