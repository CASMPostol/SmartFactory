﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using CAS.SmartFactory.IPR.Entities;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.IPR.Customs
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class SADImportXML : SPItemEventReceiver
  {
    /// <summary>
    /// An item is being added
    /// </summary>
    public override void ItemAdding(SPItemEventProperties properties)
    {
      base.ItemAdding(properties);
    }
    /// <summary>
    /// An item was added
    /// </summary>
    /// <param name="properties"> Contains properties for asynchronous list item event handlers, and serves as a base class for 
    /// event handlers.
    /// </param>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      if (properties.List.Title.Contains(CommonDefinition.SADDocumentLibrary))
      {
        string _at = "beginning";
        try
        {
          this.EventFiringEnabled = false;
          using (EntitiesDataContext edc = new EntitiesDataContext(properties.WebUrl))
          {
            if (properties.ListItem.File == null)
            {
              Anons log = new Anons()
              {
                Tytuł = m_Title,
                Treść = "Import of a SAD declaration message failed because the file is empty."
              };
              edc.ActivityLog.InsertOnSubmit(log);
              edc.SubmitChanges();
              return;
            }
            Anons mess = new Anons()
            {
              Tytuł = m_Title,
              Treść = String.Format("Import of the SAD declaration {0} starting.", properties.ListItem.File.ToString())
            };
            edc.ActivityLog.InsertOnSubmit(mess);
            edc.SubmitChanges();
            _at = "ImportDocument";
            CustomsDocument _message = CustomsDocument.ImportDocument(properties.ListItem.File.OpenBinaryStream());
            _at = "GetAtIndex";
            SADDocumentLib entry = Element.GetAtIndex<SADDocumentLib>(edc.SADDocumentLibrary, properties.ListItem.ID);
            _at = "GetSADDocument";
            SADDocumentType _sad = GetSADDocument(_message, edc, entry);
            _at = "SubmitChanges #1";
            edc.SubmitChanges();
            _at = "Clearence.Associate";
            string _comments = "OK";
            Clearence _clrnc = Clearence.Associate(edc, _message.MessageRootName(), _sad, out _comments);
            _sad.ClearenceID = _clrnc;
            entry.OK = true;
            entry.Comments = _comments;
            _at = "SubmitChanges #2";
            edc.SubmitChanges();
          }
        }
        catch (Exception ex)
        {
          SPWeb web = properties.Web;
          SPList log = web.Lists.TryGetList("Activity Log");
          string _comments = String.Empty;
          if (log == null)
          {
            EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Activity Log\" list", EventLogEntryType.Error, 114);
            return;
          }
          SPListItem item = log.AddItem();
          string _pattern = "XML import error at {0}.";
          if (ex is CustomsDataException)
          {
            _pattern = "XML import error at {0}.";
            _at = ((CustomsDataException)ex).Source;
            _comments = "XML import error";
          }
          if (ex is IPRDataConsistencyException)
          {
            IPRDataConsistencyException _iprex = ex as IPRDataConsistencyException;
            _pattern = "SAD analyses error at {0}.";
            _at = _iprex.Source;
            _comments = _iprex.Comments;
          }
          else
          {
            _pattern = "ItemAdded error at {0}.";
            _comments = "Item adding error";
          }
          item["Title"] = String.Format(_pattern, _at);
          item["Body"] = String.Format("Source= {0}; Message={1}", ex.Source, ex.Message);
          item.UpdateOverwriteVersion();
          //properties.AfterProperties["Name"] = properties.AfterProperties["Name"] + ": Import Error !!";
          //properties.AfterProperties["SADDocumentLibraryComments"] = _comments;
          //properties.AfterProperties["SADDocumentLibraryOK"] = false;
          //properties.ListItem.UpdateOverwriteVersion();
        }
        finally
        {
          this.EventFiringEnabled = true;
        }
      }
      base.ItemAdded(properties);
    }
    private static SADDocumentType GetSADDocument(CustomsDocument document, EntitiesDataContext edc, SADDocumentLib lookup)
    {
      try
      {
        SADDocumentType newRow = new SADDocumentType()
          {
            SADDocumentLibraryLookup = lookup,
            Tytuł = String.Format("{0}: {1} / {2}", document.MessageRootName(), document.GetDocumentNumber(), document.GetReferenceNumber()),
            Currency = document.GetCurrency(),
            CustomsDebtDate = document.GetCustomsDebtDate(),
            DocumentNumber = document.GetDocumentNumber(),
            ExchangeRate = document.GetExchangeRate(),
            GrossMass = document.GetGrossMass(),
            ReferenceNumber = document.GetReferenceNumber()
          };
        edc.SADDocument.InsertOnSubmit(newRow);
        GetSADGood(document.GetSADGood(), edc, newRow);
        return newRow;
      }
      catch (IPRDataConsistencyException _ex)
      {
        throw _ex;
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("SADDocumentType", ex.Message, ex, "SAD main part analysis problem");
      }
    }
    private static void GetSADGood(GoodDescription[] document, EntitiesDataContext edc, SADDocumentType lookup)
    {
      if (document.NullOrEmpty<GoodDescription>())
        return;
      try
      {
        foreach (GoodDescription _doc in document)
        {
          SADGood newRow = new SADGood()
          {
            SADDocumentLookup = lookup,
            Tytuł = String.Format("{0}: {1}", "PCNCode"/*i.GetDescription()*/, _doc.GetPCNTariffCode()),
            GoodsDescription = _doc.GetDescription().SPValidSubstring(),
            PCNTariffCode = _doc.GetPCNTariffCode(),
            GrossMass = _doc.GetGrossMass(),
            Procedure = _doc.GetProcedure(),
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
      catch (IPRDataConsistencyException _ex)
      {
        throw _ex;
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADGood", ex.Message, ex, "Goods analysis problem");
      }
    }
    private static void GetSADDuties(DutiesDescription[] document, EntitiesDataContext edc, SADGood lookup)
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
            SADGoodID = lookup,
            Tytuł = String.Format("{0}: {1}", duty.GetType(), duty.GetAmount()),
            Amount = duty.GetAmount(),
            Type = duty.GetDutyType()
          };
          rows.Add(newRow);
        }
        if (rows.Count == 0) return;
        edc.SADDuties.InsertAllOnSubmit(rows);
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADDuties", ex.Message, ex, "Duties analysis problem");
      }
    }
    private static void GetSADPackage(PackageDescription[] document, EntitiesDataContext edc, SADGood entry)
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
            SADGoodID = entry,
            Tytuł = String.Format("{0}: {1}", package.GetItemNo(), package.GetPackage()),
            ItemNo = package.GetItemNo(),
            Package = package.GetPackage()
          };
          rows.Add(newRow);
        }
        if (rows.Count == 0) return;
        edc.SADPackage.InsertAllOnSubmit(rows);
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADPackage", ex.Message, ex, "Packages analysis problem");
      }
    }
    private static void GetSADQuantity(QuantityDescription[] document, EntitiesDataContext edc, SADGood entry)
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
            SADGoodID = entry,
            Tytuł = String.Format("{0}: {1}", quantity.GetNetMass(), quantity.GetUnits()),
            ItemNo = quantity.GetItemNo(),
            NetMass = quantity.GetNetMass(),
            Units = quantity.GetUnits()
          };
          rows.Add(newRow);
        }
        if (rows.Count == 0) return;
        edc.SADQuantity.InsertAllOnSubmit(rows);
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADQuantity", ex.Message, ex, "Quantity analysis problem");
      }
    }
    private static void GetSADRequiredDocuments(RequiredDocumentsDescription[] document, EntitiesDataContext edc, SADGood entry)
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
            SADGoodID = entry,
            Tytuł = String.Format("{0}: {1}", requiredDocument.GetCode(), requiredDocument.GetNumber()),
            Code = requiredDocument.GetCode(),
            Number = requiredDocument.GetNumber()
          };
          rows.Add(newRow);
        }
        if (rows.Count == 0) return;
        edc.SADRequiredDocuments.InsertAllOnSubmit(rows);
        edc.SubmitChanges();
      }
      catch (Exception ex)
      {
        throw new IPRDataConsistencyException("GetSADRequiredDocuments", ex.Message, ex, "Required documents analysis problem");
      }
    }
    private const string m_Title = "SAD Document Import";
  }
}
