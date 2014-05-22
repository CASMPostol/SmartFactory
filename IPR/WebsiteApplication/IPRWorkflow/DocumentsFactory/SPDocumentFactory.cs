//<summary>
//  Title   : class SPDocumentFactory
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory;
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
    internal static SPFile Prepare(SPWeb site, BalanceSheetContent content, string fileName)
    {
      string _stt = "Starting";
      try
      {
        _stt = "AddDocument2Collection";
        return content.AddDocument2Collection(site, fileName, Entities.JSOXLibraryName);
      }
      catch (Exception ex)
      {
        throw GetApplicationError(ex, _stt);
      }
    }
    private static ApplicationError GetApplicationError(Exception ex, string _stt)
    {
      return new ApplicationError("CAS.SmartFactory.IPR.DocumentsFactory.SPDocumentFactory", _stt, String.Format("Cannot finish the operation because of error {0}", ex.Message), ex);
    }
  }
}
