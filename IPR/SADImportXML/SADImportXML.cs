using System;
using System.Collections.Generic;
using CAS.SmartFactory.IPR.Entities;
using CAS.SmartFactory.xml;
using Microsoft.SharePoint;
using System.Diagnostics;

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
          List<SADCommodity> cmdts = new List<SADCommodity>();
          for (int i = 0; i < document.GoodsTableLength(); i++)
          {
            SADCommodity newRow = new SADCommodity()
            {
              ParentListIdentyfikator = properties.ListItem.ID,
              Tytuł = String.Format("{0}: {1}", document.MessageRootName(),  document.GetNrWlasny()),
              GoodsName = document[i].GetDescription(),
              NetMass = document[i].GetNetMass(),
              Units = document[i].GetUnits(),
              PCNTariffCode = document[i].GetPCNTariffCode(),
              GrossMass = document[i].GetGrossMass(),
              Procedure = document[i].GetProcedure(),
              Package = document[i].GetPackage(),
              TotalAmountInvoiced = document[i].GetTotalAmountInvoiced(),
              CartonsInKg = document[i].GetCartonsInKg(),
              ItemNo = document[i].GetItemNo()
            };
            cmdts.Add(newRow);
          }
          edc.SADCommodity.InsertAllOnSubmit(cmdts);
          edc.SubmitChanges();
        }
      }
      catch (Exception ex)
      {
        SPWeb web = properties.Web;
        SPList log = web.Lists.TryGetList("Activity Log");
        if (log == null)
          return;
        EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Activity Log\" list", EventLogEntryType.Error, 114);
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
  }
}
