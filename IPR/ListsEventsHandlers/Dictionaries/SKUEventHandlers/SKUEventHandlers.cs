﻿using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using CAS.SmartFactory.IPR.Entities;
using SKUXml = CAS.SmartFactory.xml.erp.SKU;
using CigarettesXml = CAS.SmartFactory.xml.erp.Cigarettes;
using CutfillerXml = CAS.SmartFactory.xml.erp.Cutfiller;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Dictionaries
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class SKUEventHandlers : SPItemEventReceiver
  {
    /// <summary>
    /// An item was added.
    /// </summary>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      if (!properties.List.Title.Contains("SKU"))
        return;
      EntitiesDataContext edc = null;
      try
      {
        this.EventFiringEnabled = false;
        if (properties.ListItem.File == null)
        {
          Anons.WriteEntry(edc, m_Title, "Import of a SKU xml message failed because the file is empty.");
          return;
        }
        edc = new EntitiesDataContext(properties.WebUrl);
        String message = String.Format("Import of the SKU message {0} starting.", properties.ListItem.File.ToString());
        Anons.WriteEntry(edc, m_Title, message);
        SKUXml xml = SKUXml.ImportDocument(properties.ListItem.File.OpenBinaryStream());
        Dokument entry = Dokument.GetEntity(properties.ListItem.ID, edc.SKULibrary);
        SKUCommonPart.GetXmlContent(xml, edc, entry);
        edc.SubmitChangesSilently(RefreshMode.OverwriteCurrentValues);
      }
      catch (Exception ex)
      {
        if (edc != null)
          Anons.WriteEntry(edc, "SKU message import error", ex.Message);
      }
      finally
      {
        if (edc != null)
        {
          Anons.WriteEntry(edc, m_Title, "Import of the message finished");
          edc.SubmitChangesSilently(RefreshMode.KeepCurrentValues);
          edc.Dispose();
        }
        this.EventFiringEnabled = true;
        base.ItemAdded(properties);
      }
    }
    private const string m_Title = "SKU Message Import";
  }
}



