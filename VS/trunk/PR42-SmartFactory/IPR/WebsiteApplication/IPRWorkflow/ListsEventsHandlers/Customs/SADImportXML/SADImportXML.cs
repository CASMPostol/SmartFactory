//<summary>
//  Title   :  SAD List Item Events
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CAS.SharePoint;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.IPR.ListsEventsHandlers.Customs.SADImportXML;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace CAS.SmartFactory.IPR.Customs
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class SADImportXML : SPItemEventReceiver
  {
    /// <summary>
    /// An item was added
    /// </summary>
    /// <param name="properties"> Contains properties for asynchronous list item event handlers, and serves as a base class for 
    /// event handlers.
    /// </param>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      TraceEvent("Entering SADImportXML ItemAdded", 46, TraceSeverity.Monitorable);
      ErrorsList m_Warnings = new ErrorsList();
      string _at = "beginning";
      if (!properties.ListTitle.Contains(CommonDefinition.SADDocumentLibrary))
      {
        //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
        TraceEvent(String.Format("Exiting SADImportXML.ItemAdded - event called for wrong lis list name {0}.", properties.ListTitle), 52, TraceSeverity.Monitorable);
        base.ItemAdded(properties);
        return;
      }
      bool _entrySADDocumentLibraryOK = false;
      string _entrySADDocumentLibraryComments = "Item adding error";
      try
      {
        this.EventFiringEnabled = false;
        if (properties.ListItem.File == null)
        {
          TraceEvent("Exiting SADImportXML.ItemAdded - file is empty", 63, TraceSeverity.High);
          base.ItemAdded(properties);
          return;
          //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
          //throw new IPRDataConsistencyException("ItemAdded", "Import of a SAD declaration message failed because the file is empty.", null, "There is no file");
        }
        try
        {
          ActivityLogCT.WriteEntry(m_Title, String.Format("Import of the SAD declaration {0} starting.", properties.ListItem.File.Name), properties.WebUrl);
          CustomsDocument _message = null;
          using (Stream _str = properties.ListItem.File.OpenBinaryStream())
            _message = CustomsDocument.ImportDocument(_str);
          int _sadIDValue;
          using (Entities edc = new Entities(properties.WebUrl))
          {
            _at = "GetAtIndex<SADDocumentLib>";
            SADDocumentLib _SADLibEntry = Element.GetAtIndex<SADDocumentLib>(edc.SADDocumentLibrary, properties.ListItem.ID);
            _at = "GetSADDocument";
            SADDocumentType _SADEntity = GetSADDocument(_message, edc, _SADLibEntry);
            _at = "SubmitChanges";
            edc.SubmitChanges();
            _sadIDValue = _SADEntity.Id.Value;
          }
          _at = "DeclarationProcessing";
          _entrySADDocumentLibraryComments = "OK";
          ClearenceHelpers.DeclarationProcessing(properties.WebUrl, _sadIDValue, _message.MessageRootName(), ref _entrySADDocumentLibraryComments, m_Warnings, TraceEvent);
        }
        catch (InputDataValidationException _ie)
        {
          TraceEvent(String.Format("Exception {0} at SADImportXML.ItemAdded file={1}", _ie.GetType().Name, properties.ListItem.File.Name), 92, TraceSeverity.Monitorable);
          _ie.ReportActionResult(properties.WebUrl, properties.ListItem.File.Name);
        }
        catch (Exception _ex)
        {
          string _pattern = "XML import error at {0}.";
          if (_ex is CustomsDataException)
          {
            _pattern = "XML import error at {0}.";
            _at = ((CustomsDataException)_ex).Source;
          }
          else if (_ex is IPRDataConsistencyException)
          {
            IPRDataConsistencyException _iprex = _ex as IPRDataConsistencyException;
            _pattern = "SAD analyses error at {0}.";
            _at = _iprex.Source;
          }
          else if (_ex is GenericStateMachineEngine.ActionResult)
          {
            GenericStateMachineEngine.ActionResult _ar = _ex as GenericStateMachineEngine.ActionResult;
            if (_ar.LastActionResult == GenericStateMachineEngine.ActionResult.Result.NotValidated)
              _pattern = "SAD content validation error at {0}.";
            else
              _pattern = "SAD analyses internal error at {0}.";
            _at = _ar.Source;
          }
          else
            _pattern = "ItemAdded error at {0}.";
          string _innerMsg = String.Empty;
          if (_ex.InnerException != null)
            _innerMsg = String.Format(" as the result of {0}.", _ex.InnerException.Message);
          string _msg = String.Format("Message= {0}; Inner: {1}", _ex.Message, _innerMsg);
          string _where = String.Format(_pattern, _at);
          ActivityLogCT.WriteEntry(_where, _msg, properties.WebUrl);
          TraceEvent(String.Format("Exception {0} at SADImportXML.ItemAdded/{1}, Stack: {2}", _ex.GetType().Name, _where, _ex.StackTrace), 126, TraceSeverity.High);
        }
        try
        {
          using (Entities edc = new Entities(properties.WebUrl))
          {
            SADDocumentLib _entry = Element.GetAtIndex<SADDocumentLib>(edc.SADDocumentLibrary, properties.ListItem.ID);
            _entry.SADDocumentLibraryOK = _entrySADDocumentLibraryOK;
            _entry.SADDocumentLibraryComments = _entrySADDocumentLibraryComments;
            _entrySADDocumentLibraryOK = true;
            _at = "m_Warnings";
            foreach (Warnning _wrnngx in m_Warnings)
              ActivityLogCT.WriteEntry(edc, m_Title, String.Format("Import of the SAD declaration wanning: {0}", _wrnngx.Message));
            if (m_Warnings.Count == 0)
              ActivityLogCT.WriteEntry(edc, m_Title, String.Format("Import of the SAD declaration {0} finished.", properties.ListItem.File.Name));
            else
              ActivityLogCT.WriteEntry(edc, m_Title, String.Format("Import of the SAD declaration {0} finished. {1} warnings have been reported.", properties.ListItem.File.Name, m_Warnings.Count));
            _at = "SubmitChanges";
            TraceEvent("SADImportXMLItemAdded SubmitChanges", 144, TraceSeverity.Verbose);
            edc.SubmitChanges();
          }
        }
        catch (Exception _ex)
        {
          string _pattern = "Unexpected SADDocumentLib SubmitChanges error: {0}.";
          ActivityLogCT.WriteEntry(m_Title, String.Format(_pattern, _ex.Message), properties.WebUrl);
          TraceEvent(String.Format("Exception {0} at SADImportXML.ItemAdded/{1}, Stack: {2}", _ex.GetType().Name, _at, _ex.StackTrace), 152, TraceSeverity.High);
        }
      }
      finally
      {
        this.EventFiringEnabled = true;
      }
      base.ItemAdded(properties);
      TraceEvent("Finished SADImportXML ItemAdded", 160, TraceSeverity.Monitorable);
    }

    #region private

    #region private get xml
    private static SADDocumentType GetSADDocument(CustomsDocument document, Entities edc, SADDocumentLib lookup)
    {
      try
      {
        SADDocumentType newRow = new SADDocumentType()
          {
            SADDocumenLibrarytIndex = lookup,
            Title = String.Format("{0}: {1} / {2}", document.MessageRootName(), document.GetDocumentNumber(), document.GetReferenceNumber()),
            Currency = document.GetCurrency(),
            CustomsDebtDate = document.GetCustomsDebtDate(),
            DocumentNumber = document.GetDocumentNumber(),
            ExchangeRate = document.GetExchangeRate(),
            GrossMass = document.GetGrossMass(),
            NetMass = 0, //TODO remove column is useless
            ReferenceNumber = document.GetReferenceNumber()
          };
        edc.SADDocument.InsertOnSubmit(newRow);
        GetSADGood(document.GetSADGood(), edc, newRow);
        return newRow;
      }
      catch (IPRDataConsistencyException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("SADDocumentType", ex.Message, ex, "SAD main part analysis problem");
      }
    }
    private static void GetSADGood(GoodDescription[] document, Entities edc, SADDocumentType lookup)
    {
      if (document.NullOrEmpty<GoodDescription>())
        return;
      try
      {
        foreach (GoodDescription _doc in document)
        {
          string _description = _doc.GetDescription().SPValidSubstring();
          SADGood newRow = new SADGood()
          {
            SADDocumentIndex = lookup,
            Title = String.Format("{0}: {1}", _doc.GetDescription().SPValidSubstring(), _doc.GetPCNTariffCode()),
            GoodsDescription = _description,
            PCNTariffCode = _doc.GetPCNTariffCode(),
            GrossMass = _doc.GetGrossMass(),
            NetMass = _doc.GetNetMass(),
            SPProcedure = _doc.GetProcedure(),
            TotalAmountInvoiced = _doc.GetTotalAmountInvoiced(),
            ItemNo = _doc.GetItemNo()
          };
          edc.SADGood.InsertOnSubmit(newRow);
          edc.SubmitChanges();
          GetSADDuties(_doc.GetSADDuties(), edc, newRow);
          GetSADPackage(_doc.GetSADPackage(), edc, newRow);
          GetSADQuantity(_doc.GetSADQuantity(), edc, newRow);
          GetSADRequiredDocuments(_doc.GetSADRequiredDocuments(), edc, newRow);
        }
      }
      catch (IPRDataConsistencyException)
      {
        throw;
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADGood", ex.Message, ex, "Goods analysis problem");
      }
    }
    private static void GetSADDuties(DutiesDescription[] document, Entities edc, SADGood lookup)
    {
      if (document.NullOrEmpty<DutiesDescription>())
        return;
      List<SADDuties> rows = new List<SADDuties>();
      try
      {
        foreach (DutiesDescription duty in document)
        {
          SADDuties newRow = new SADDuties()
          {
            SADDuties2SADGoodID = lookup,
            Title = String.Format("{0}: {1}", duty.GetDutyType(), duty.GetAmount()),
            Amount = duty.GetAmount(),
            DutyType = duty.GetDutyType()
          };
          rows.Add(newRow);
        }
        if (rows.Count == 0)
          return;
        edc.SADDuties.InsertAllOnSubmit(rows);
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADDuties", ex.Message, ex, "Duties analysis problem");
      }
    }
    private static void GetSADPackage(PackageDescription[] document, Entities edc, SADGood entry)
    {
      try
      {
        if (document.NullOrEmpty<PackageDescription>())
          return;
        List<SADPackage> rows = new List<SADPackage>();
        foreach (PackageDescription package in document)
        {
          SADPackage newRow = new SADPackage()
          {
            SADPackage2SADGoodID = entry,
            Title = String.Format("{0}: {1}", package.GetItemNo(), package.GetPackage()),
            ItemNo = package.GetItemNo(),
            Package = package.GetPackage()
          };
          rows.Add(newRow);
        }
        if (rows.Count == 0)
          return;
        edc.SADPackage.InsertAllOnSubmit(rows);
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADPackage", ex.Message, ex, "Packages analysis problem");
      }
    }
    private static void GetSADQuantity(QuantityDescription[] document, Entities edc, SADGood entry)
    {
      if (document.NullOrEmpty<QuantityDescription>())
        return;
      List<SADQuantity> rows = new List<SADQuantity>();
      try
      {
        foreach (QuantityDescription quantity in document)
        {
          SADQuantity newRow = new SADQuantity()
          {
            SADQuantity2SADGoodID = entry,
            Title = String.Format("{0}: {1}", quantity.GetNetMass(), quantity.GetUnits()),
            ItemNo = quantity.GetItemNo(),
            NetMass = quantity.GetNetMass(),
            Units = quantity.GetUnits()
          };
          rows.Add(newRow);
        }
        if (rows.Count == 0)
          return;
        edc.SADQuantity.InsertAllOnSubmit(rows);
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADQuantity", ex.Message, ex, "Quantity analysis problem");
      }
    }
    private static void GetSADRequiredDocuments(RequiredDocumentsDescription[] document, Entities edc, SADGood entry)
    {
      try
      {
        if (document.NullOrEmpty<RequiredDocumentsDescription>())
          return;
        List<SADRequiredDocuments> rows = new List<SADRequiredDocuments>();
        foreach (RequiredDocumentsDescription requiredDocument in document)
        {
          SADRequiredDocuments newRow = new SADRequiredDocuments()
          {
            SADRequiredDoc2SADGoodID = entry,
            Title = String.Format("{0}: {1}", requiredDocument.GetCode(), requiredDocument.GetNumber()),
            Code = requiredDocument.GetCode(),
            Number = requiredDocument.GetNumber()
          };
          rows.Add(newRow);
        }
        if (rows.Count == 0)
          return;
        edc.SADRequiredDocuments.InsertAllOnSubmit(rows);
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADRequiredDocuments", ex.Message, ex, "Required documents analysis problem");
      }
    }
    #endregion

    private const string m_Title = "SAD Document Import";
    private static void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.SADProcessing);
    }
    #endregion

  }
}
