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
      ( SPWeb site, List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm> _consignment, string fileName )
    {
      string _stt = "Starting";
      try
      {
        CigaretteExportFormCollection _cefc = CigaretteExportFormCollectionFactory.CigaretteExportFormCollection( _consignment, fileName );
        SPDocumentLibrary _lib = (SPDocumentLibrary)site.Lists[ CommonDefinitions.IPRSADConsignmentLibraryTitle ];
        _stt = "AddDocument2Collection";
        SPFile _docFile = _cefc.AddDocument2Collection( _lib.RootFolder.Files, fileName );
        return _docFile.Item.ID;
      }
      catch ( Exception ex )
      {
        throw new ApplicationError( "InvoiceLib.PrepareConsignment", _stt, String.Format( "Cannot finish the operation because of error {0}", ex.Message ), ex );
      }
    }
  }
}
