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
      try
      {
        this.EventFiringEnabled = false;
        CustomsDocument document = CustomsDocument.ImportDocument(properties.ListItem.File.OpenBinaryStream());
        using (EntitiesDataContext edc = new EntitiesDataContext("http://casmp/sites/IPR/"))
        {
          Anons mess = new Anons()
          {
            Tytuł = String.Format("Import of the {0} starting.", document.MessageRootName()),
            Treść = document.GetNrWlasny()
          };
          edc.ActivityLog.InsertOnSubmit(mess);
          edc.SubmitChanges();
          SADDocumentType entry =
            (from enr in edc.SADDocument
             where enr.Identyfikator == properties.ListItem.ID
             select enr).First<SADDocumentType>();
          entry.ReferenceNumber = document.GetNrWlasny();
          entry.Tytuł = document.MessageRootName();
          GetSADGood(document, edc, entry);
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
        properties.ListItem["Name"] = properties.ListItem["Name"] + "Import Error !!";
        properties.ListItem.UpdateOverwriteVersion();
      }
      finally
      {
        this.EventFiringEnabled = true;
      }
      base.ItemAdded(properties);
    }

    private static void GetSADGood(CustomsDocument document, EntitiesDataContext edc, SADDocumentType entry)
    {
      List<SADGood> cmdts = new List<SADGood>();
      for (int i = 0; i < document.GoodsTableLength(); i++)
      {
        SADGood newRow = new SADGood()
        {
          SADDocumentLookup = entry,
          Tytuł = String.Format("{0}: {1}", document.MessageRootName(), document.GetNrWlasny()),
          GoodsDescription = document[i].GetDescription(),
          PCNTariffCode = document[i].GetPCNTariffCode(),
          GrossMass = document[i].GetGrossMass(),
          Procedure = document[i].GetProcedure(),
          TotalAmountInvoiced = document[i].GetTotalAmountInvoiced(),
          ItemNo = document[i].GetItemNo()
        };
        cmdts.Add(newRow);
        GetSADDuties(document[i], edc, newRow);
        GetSADPackage(document[i], edc, newRow);
      }
      edc.SADGood.InsertAllOnSubmit(cmdts);
      edc.SubmitChanges();
    }
    private static void GetSADDuties(GoodDescription document, EntitiesDataContext edc, SADGood entry)
    {
      List<SADDuties> cmdts = new List<SADDuties>();
      foreach (DutiesDescription duty in document.GetSADDuties() )
      {
        SADDuties newRow = new SADDuties()
        {
          SADGoodLookup = entry,
          Tytuł = String.Format("{0}: {1}", duty.GetType(), duty.GetAmount()),
          Amount = duty.GetAmount(),
          Type = duty.GetDutyType(),
        };
        cmdts.Add(newRow);
      }
      edc.SADDuties.InsertAllOnSubmit(cmdts);
      edc.SubmitChanges();
    }
    private static void GetSADPackage(GoodDescription document, EntitiesDataContext edc, SADGood entry)
    {
      List<SADPackage> cmdts = new List<SADPackage>();
      foreach (PackageDescription package in document.GetSADPackage())
      {
        SADPackage newRow = new SADPackage()
        {
          SADGoodLookup = entry,
          Tytuł = String.Format("{0}: {1}", package.GetItemNo(), document.GetPackage()),
          ItemNo = package.GetItemNo(),
          Package = package.GetPackage()
        };
        cmdts.Add(newRow);
      }
      edc.SADPackage.InsertAllOnSubmit(cmdts);
      edc.SubmitChanges();
    }
    private static void GetSADQuantity(GoodDescription document, EntitiesDataContext edc, SADGood entry)
    {
      List<SADQuantity> cmdts = new List<SADQuantity>();
      foreach (QuantityDescription quantity in document.GetSADQuantity())
      {
        SADQuantity newRow = new SADQuantity()
        {
          SADGoodLookup = entry,
          Tytuł = String.Format("{0}: {1}", quantity.GetNetMass(), quantity.GetUnits()),
          ItemNo = quantity.GetItemNo(),
          NetMass = quantity.GetNetMass(),
          Units = quantity.GetUnits()
        };
        cmdts.Add(newRow);
      }
      edc.SADQuantity.InsertAllOnSubmit(cmdts);
      edc.SubmitChanges();
    }
  }
}
