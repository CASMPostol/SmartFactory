using System;
using System.Xml;
using System.Xml.Serialization;
using CAS.SmartFactory.xml.CELINA.SAD;
using Microsoft.SharePoint;
using CAS.SmartFactory.xml;
using System.Collections.Generic;
using CAS.SmartFactory.IPR.Entities;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.SADImportXML
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
    public override void ItemAdded(SPItemEventProperties properties)
    {
      try
      {
        this.EventFiringEnabled = false;
        CustomsDocument document = CustomsDocument.ImportDocument(properties.ListItem.File.OpenBinaryStream());
        using (EntitiesDataContext edc = new EntitiesDataContext("http://casmp/sites/IPR/"))
        {
          Anons mess = new Anons() { Tytuł = "Import SAD starting", Treść = document.GetNrWlasny() };
          edc.ActivityLog.InsertOnSubmit(mess);
          edc.SubmitChanges();
          //List<string> columns = new List<string>();
          //foreach (SPField li in properties.ListItem.Fields)
          //  columns.Add(li.InternalName);
          //SPList goods = properties.Web.Lists.TryGetList("SADCommodity");
          //if (goods == null)
          //  throw new ApplicationException("Cannot find the SADCommodity list");
          List<SADCommodity> cmdts = new List<SADCommodity>();
          for (int i = 0; i < document.GoodsTableLength(); i++)
          {
            SADCommodity newRow = new SADCommodity()
            {
              ParentListIdentyfikator = properties.ListItem.ID,
              Tytuł = document.GetNrWlasny()
            };
            cmdts.Add(newRow);

            //SPListItem entry = goods.AddItem();
            //entry["Title"] = document.GetNrWlasny();
            //entry["Type"] = document.GetNrWlasny();
            //entry["SKU"] = document.GetNrWlasny();
            //entry["Batch"] = document.GetNrWlasny();
            //entry["Net_x0020_mass"] = document.GetNrWlasny();
            //entry["PCN_x0020_tariff_x0020_code"] = document.GetNrWlasny();
            //entry["Gross_x0020_mass"] = document.GetNrWlasny();
            //entry["Procedure"] = document.GetNrWlasny();
            //entry["Package"] = document.GetNrWlasny();
            //entry["Total_x0020_amount_x0020_invoice"] = document.GetNrWlasny();
            //entry["Duty"] = document.GetNrWlasny();
            //entry["VAT"] = document.GetNrWlasny();
            //entry["Units"] = document.GetNrWlasny();
            //entry["Item_x0020_No"] = document.GetItemNo(i);
            //entry.UpdateOverwriteVersion();
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

        SPListItem item = log.AddItem();
        item["Title"] = "Customs document import error";
        item["Body"] = ex.Message;
        item.UpdateOverwriteVersion();

        properties.ListItem["Name"] = "Import Error";
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
