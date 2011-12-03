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
using System.ComponentModel;

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
      //TODo remove or implement
      //if (properties.ListItem.File == null)
      //{
      //  Anons.WriteEntry(edc, m_Title, "Import of a SKU xml message failed because the file is empty.");
      //  return;
      //}
      this.EventFiringEnabled = false;
      SKUEvetReceiher(
        properties.ListItem.File.OpenBinaryStream(),
        properties.WebUrl,
        properties.ListItem.ID,
        (object obj, ProgressChangedEventArgs progres) =>
        {
          return;
        });
      this.EventFiringEnabled = true;
      base.ItemAdded(properties);
    }
    public static void SKUEvetReceiher(System.IO.Stream stream, string url, int listIndex, ProgressChangedEventHandler progressChanged)
    {
      EntitiesDataContext edc = null;
      try
      {
        edc = new EntitiesDataContext(url);
        String message = String.Format("Import of the SKU message {0} starting.", listIndex);
        Anons.WriteEntry(edc, m_Title, message);
        SKUXml xml = SKUXml.ImportDocument(stream);
        Dokument entry = Dokument.GetEntity(listIndex, edc.SKULibrary);
        SKUCommonPart.GetXmlContent(xml, edc, entry, progressChanged);
        progressChanged(null, new ProgressChangedEventArgs(1, "Submiting Changes"));
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
      }
    }
    private const string m_Title = "SKU Message Import";
  }
}



