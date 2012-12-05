using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.AccountClearance;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  /// <summary>
  /// SPDocumentFactory
  /// </summary>
  internal static class SPDocumentFactory
  {
    internal static int Prepare( SPWeb site, RequestContent _consignment, string fileName )
    {
      string _stt = "Starting";
      try
      {
        _stt = "SPDocumentLibrary";
        SPDocumentLibrary _lib = (SPDocumentLibrary)site.Lists[ Entities.IPRLibraryName ];
        _stt = "AddDocument2Collection";
        SPFile _docFile = _consignment.AddDocument2Collection( _lib.RootFolder.Files, fileName );
        return _docFile.Item.ID;
      }
      catch ( Exception ex )
      {
        throw new ApplicationError( "CAS.SmartFactory.IPR.DocumentsFactory.SPDocumentFactory", _stt, String.Format( "Cannot finish the operation because of error {0}", ex.Message ), ex );
      }
    }
  }
}
