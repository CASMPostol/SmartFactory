//_______________________________________________________________
//  Title   : SPDocumentFactory
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.SharePoint;
using CAS.SharePoint.Logging;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using CAS.SmartFactory.xml.DocumentsFactory.Disposals;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;

namespace CAS.SmartFactory.IPR.Dashboards.Clearance
{
  /// <summary>
  /// Document Extension
  /// </summary>
  internal static class SPDocumentFactory
  {
    internal static int Prepare(SPWeb site, CigaretteExportFormCollection _consignment, string fileName, NamedTraceLogger.TraceAction trace)
    {
      string _stt = "AddDocument2Collection";
      try
      {
        SPFile _docFile = _consignment.AddDocument2Collection(site, fileName, CommonDefinitions.IPRSADConsignmentLibraryTitle);
        return _docFile.Item.ID;
      }
      catch (Exception ex)
      {
        string _ms = String.Format("Cannot finish the operation because of error {0}", ex.Message);
        trace("ApplicationError at SPDocumentFactory.Prepare: " + _ms + "Stack: " + ex.StackTrace, 41, TraceSeverity.High);
        throw new ApplicationError("SPDocumentFactory.Prepare", _stt, _ms, ex);
      }
    }
    internal static int Prepare
      (SPWeb site, DocumentContent document, string fileName, CompensatiionGood compensatiionGood)
    {
      string _stt = "AddDocument2Collection";
      try
      {
        SPFile _docFile = document.AddDocument2Collection(site, fileName, CommonDefinitions.IPRSADConsignmentLibraryTitle, compensatiionGood);
        return _docFile.Item.ID;
      }
      catch (Exception ex)
      {
        throw new ApplicationError("Dokument.PrepareConsignment", _stt, String.Format("Cannot finish the operation because of error {0}", ex.Message), ex);
      }
    }
  }
}
