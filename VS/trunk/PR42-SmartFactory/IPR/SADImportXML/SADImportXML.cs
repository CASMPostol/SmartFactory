using System;
using System.Collections.Generic;
using System.Diagnostics;
using CAS.SmartFactory.IPR.Entities;
using CAS.SmartFactory.xml;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint.Linq.Provider;
using System.Linq;

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
      if (properties.List.Title.Contains("SAD"))
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
            CustomsDocument document = CustomsDocument.ImportDocument(properties.ListItem.File.OpenBinaryStream());
            Dokument entry =
              (from enr in edc.SADDocumentLibrary
               where enr.Identyfikator == properties.ListItem.ID
               select enr).First<Dokument>();
            GetSADDocument(document, edc, entry);
            edc.SubmitChanges();
          }
        }
        catch (Exception ex)
        {
          SPWeb web = properties.Web;
          SPList log = web.Lists.TryGetList("Activity Log");
          if (log == null)
          {
            EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Activity Log\" list", EventLogEntryType.Error, 114);
            return;
          }
          SPListItem item = log.AddItem();
          item["Title"] = "Customs document import error";
          item["Body"] = ex.Message;
          item.UpdateOverwriteVersion();
          properties.ListItem["Name"] = properties.ListItem["Name"] + ": Import Error !!";
          properties.ListItem.UpdateOverwriteVersion();
        }
        finally
        {
          this.EventFiringEnabled = true;
        }
      base.ItemAdded(properties);
    }
    private static void GetSADDocument(CustomsDocument document, EntitiesDataContext edc, Dokument lookup)
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
      GetSADGood(document.GetSADGood(), edc, newRow);
      edc.SADDocument.InsertOnSubmit(newRow);
    }
    private static void GetSADGood(GoodDescription[] document, EntitiesDataContext edc, SADDocumentType lookup)
    {
      if (document.NullOrEmpty<GoodDescription>())
        return;
      List<SADGood> rows = new List<SADGood>();
      foreach (GoodDescription i in document)
      {
        SADGood newRow = new SADGood()
        {
          SADDocumentLookup = lookup,
          Tytuł = String.Format("{0}: {1}", i.GetDescription(), i.GetPCNTariffCode()),
          GoodsDescription = i.GetDescription(),
          PCNTariffCode = i.GetPCNTariffCode(),
          GrossMass = i.GetGrossMass(),
          Procedure = i.GetProcedure(),
          TotalAmountInvoiced = i.GetTotalAmountInvoiced(),
          ItemNo = i.GetItemNo()
        };
        rows.Add(newRow);
        GetSADDuties(i.GetSADDuties(), edc, newRow);
        GetSADPackage(i.GetSADPackage(), edc, newRow);
        GetSADQuantity(i.GetSADQuantity(), edc, newRow);
        GetSADRequiredDocuments(i.GetSADRequiredDocuments(), edc, newRow);
      }
      if (rows.Count == 0) return;
      edc.SADGood.InsertAllOnSubmit(rows);
    }
    private static void GetSADDuties(DutiesDescription[] document, EntitiesDataContext edc, SADGood lookup)
    {
      if (document.NullOrEmpty<DutiesDescription>())
        return;
      List<SADDuties> rows = new List<SADDuties>();
      foreach (DutiesDescription duty in document)
      {
        SADDuties newRow = new SADDuties()
        {
          SADGoodLookup = lookup,
          Tytuł = String.Format("{0}: {1}", duty.GetType(), duty.GetAmount()),
          Amount = duty.GetAmount(),
          Type = duty.GetDutyType()
        };
        rows.Add(newRow);
      }
      if (rows.Count == 0)
        return;
      edc.SADDuties.InsertAllOnSubmit(rows);
    }
    private static void GetSADPackage(PackageDescription[] document, EntitiesDataContext edc, SADGood entry)
    {
      if (document.NullOrEmpty<PackageDescription>())
        return;
      List<SADPackage> rows = new List<SADPackage>();
      foreach (PackageDescription package in document)
      {
        SADPackage newRow = new SADPackage()
        {
          SADGoodLookup = entry,
          Tytuł = String.Format("{0}: {1}", package.GetItemNo(), package.GetPackage()),
          ItemNo = package.GetItemNo(),
          Package = package.GetPackage()
        };
        rows.Add(newRow);
      }
      if (rows.Count == 0)
        return;
      edc.SADPackage.InsertAllOnSubmit(rows);
    }
    private static void GetSADQuantity(QuantityDescription[] document, EntitiesDataContext edc, SADGood entry)
    {
      if (document.NullOrEmpty<QuantityDescription>())
        return;
      List<SADQuantity> rows = new List<SADQuantity>();
      foreach (QuantityDescription quantity in document)
      {
        SADQuantity newRow = new SADQuantity()
        {
          SADGoodLookup = entry,
          Tytuł = String.Format("{0}: {1}", quantity.GetNetMass(), quantity.GetUnits()),
          ItemNo = quantity.GetItemNo(),
          NetMass = quantity.GetNetMass(),
          Units = quantity.GetUnits()
        };
        rows.Add(newRow);
      }
      if (rows.Count == 0)
        return;
      edc.SADQuantity.InsertAllOnSubmit(rows);
    }
    private static void GetSADRequiredDocuments(RequiredDocumentsDescription[] document, EntitiesDataContext edc, SADGood entry)
    {
      if (document.NullOrEmpty<RequiredDocumentsDescription>())
        return;
      List<SADRequiredDocuments> rows = new List<SADRequiredDocuments>();
      foreach (RequiredDocumentsDescription requiredDocument in document)
      {
        SADRequiredDocuments newRow = new SADRequiredDocuments()
        {
          SADGoodLookup = entry,
          Tytuł = String.Format("{0}: {1}", requiredDocument.GetCode(), requiredDocument.GetNumber()),
          Code = requiredDocument.GetCode(),
          Number = requiredDocument.GetNumber()
        };
        rows.Add(newRow);
      }
      if (rows.Count == 0)
        return;
      edc.SADRequiredDocuments.InsertAllOnSubmit(rows);
    }
    private const string m_Title = "SAD Document Import";
  }
}
