﻿using System;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.AccountClearance;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.IPR.DocumentsFactory
{
  /// <summary>
  /// SPDocumentFactory
  /// </summary>
  internal static class SPDocumentFactory
  {
    internal static int Prepare( SPWeb site, RequestContent content, string fileName )
    {
      string _stt = "Starting";
      try
      {
        _stt = "SPDocumentLibrary";
        SPDocumentLibrary _lib = (SPDocumentLibrary)site.Lists[ Entities.IPRLibraryName ];
        _stt = "AddDocument2Collection";
        SPFile _docFile = content.AddDocument2Collection( _lib.RootFolder.Files, fileName );
        return _docFile.Item.ID;
      }
      catch ( Exception ex )
      {
        throw GetApplicationError( ex, _stt );
      }
    }
    internal static int Prepare( SPWeb site, BalanceSheetContent content, string fileName )
    {
      string _stt = "Starting";
      try
      {
        _stt = "SPDocumentLibrary";
        SPDocumentLibrary _lib = (SPDocumentLibrary)site.Lists[ Entities.JSOXLibraryName ];
        _stt = "AddDocument2Collection";
        SPFile _docFile = content.AddDocument2Collection( _lib.RootFolder.Files, fileName );
        return _docFile.Item.ID;
      }
      catch ( Exception ex )
      {
        throw GetApplicationError( ex, _stt );
      }
    }
    private static ApplicationError GetApplicationError( Exception ex, string _stt )
    {
      return new ApplicationError( "CAS.SmartFactory.IPR.DocumentsFactory.SPDocumentFactory", _stt, String.Format( "Cannot finish the operation because of error {0}", ex.Message ), ex );
    }
  }
}
