//<summary>
//  Title   : DisposalRequest List Item Events
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
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint;
using DisposalRequestXml = CAS.SmartFactory.CW.Interoperability.ERP.DisposalRequest;

namespace CAS.SmartFactory.CW.Workflows.DisposalRequest
{
  /// <summary>
  /// DisposalRequest List Item Events
  /// </summary>
  public class DisposalRequest : SPItemEventReceiver
  {
    #region public override
    /// <summary>
    /// An item was added
    /// </summary>
    /// <param name="properties">An object of <see cref="SPItemEventProperties"/></param>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      try
      {
        if (!properties.ListTitle.Contains("Disposal Request Library"))
        {
          //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
          base.ItemAdded(properties);
          return;
        }
        this.EventFiringEnabled = false;
        using (Entities _edc = new Entities(properties.WebUrl))
        {
          ActivityLogCT.WriteEntry(_edc, m_Title, String.Format("Import of the Disposal Request {0} XML message started", properties.ListItem.File.Name));
          At = "ImportDocument";
          DisposalRequestXml _xml = default(DisposalRequestXml);
          using (Stream _stream = properties.ListItem.File.OpenBinaryStream())
            _xml = DisposalRequestXml.ImportDocument(_stream);
          At = "GetAtIndex - DisposalRequestLib";
          DisposalRequestLib _entry = Element.GetAtIndex<DisposalRequestLib>(_edc.DisposalRequestLibrary, properties.ListItemId);
          _entry.Archival = true;
          _entry.ClearenceProcedure = Covert2ClearenceProcedure(_xml.ClearenceProcedure);
          At = "GetXmlContent";
          GetXmlContent(_edc, _xml, _entry, ProgressChange);
          At = "SubmitChanges";
          _edc.SubmitChanges();
          foreach (CAS.SmartFactory.Customs.Warnning _wrnngx in m_Warnings)
            ActivityLogCT.WriteEntry(_edc, m_Title, String.Format("Import warnning: {0}", _wrnngx.Message));
          ActivityLogCT.WriteEntry(_edc, m_Title, String.Format("Import of the Disposal Request {0} XML message message finished - document imported", properties.ListItem.File.Name));
        }
      }
      //catch (InputDataValidationException _idve)
      //{
      //  _idve.ReportActionResult(_properties.WebUrl, _properties.ListItem.File.Name);
      //}
      //catch (IPRDataConsistencyException _ex)
      //{
      //  _ex.Source += " at " + At;
      //  using (Entities _edc = new Entities(_properties.WebUrl))
      //  {
      //    _ex.Add2Log(_edc);
      //    BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>(_edc.BatchLibrary, _properties.ListItemId);
      //    _entry.BatchLibraryOK = false;
      //    _entry.BatchLibraryComments = _ex.Comments;
      //    _edc.SubmitChanges();
      //  }
      //}
      catch (Exception _ex)
      {
        using (Entities _edc = new Entities(properties.WebUrl))
        {
          ActivityLogCT.WriteEntry(_edc, "BatchEventReceiver.ItemAdded" + " at " + At, _ex.Message);
          DisposalRequestLib _entry = Element.GetAtIndex<DisposalRequestLib>(_edc.DisposalRequestLibrary, properties.ListItemId);
          _entry.ClearenceProcedure = new Nullable<ClearenceProcedure>();
          _edc.SubmitChanges();
        }
      }
      finally
      {
        this.EventFiringEnabled = true;
      }
      base.ItemAdded(properties);
    }
    #endregion

    #region private
    private ClearenceProcedure Covert2ClearenceProcedure(Interoperability.ERP.ClearenceProcedure clearanceProcedure)
    {
      ClearenceProcedure _ret = default(ClearenceProcedure);
      switch (clearanceProcedure)
      {
        case Interoperability.ERP.ClearenceProcedure.Item4071:
          _ret = ClearenceProcedure._4071;
          break;
        case Interoperability.ERP.ClearenceProcedure.Item5171:
          _ret = ClearenceProcedure._5171;
          break;
        case Interoperability.ERP.ClearenceProcedure.Item3171:
          _ret = ClearenceProcedure._3171;
          break;
        case Interoperability.ERP.ClearenceProcedure.Item7171:
          _ret = ClearenceProcedure._7171;
          break;
      }
      return _ret;
    }
    private void ProgressChange(object sender, ProgressChangedEventArgs progres)
    {
      if (progres.UserState is String)
        At = (string)progres.UserState;
      else if (progres.UserState is Warnning)
        m_Warnings.Add(progres.UserState as Warnning);
      else
        throw new ArgumentException("Wrong state reported", "UserState");
    }
    /// <summary>
    /// Gets the content of the XML.
    /// </summary>
    /// <param name="xml">The document.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="parent">The entry.</param>
    /// <param name="progressChanged">The progress changed.</param>
    private static void GetXmlContent(Entities entities, DisposalRequestXml xml, DisposalRequestLib parent, ProgressChangedEventHandler progressChanged)
    {
      progressChanged(null, new ProgressChangedEventArgs(1, "GetXmlContent: starting"));
      foreach (var _xmli in xml.DisposalRequestContent)
      {
        CustomsWarehouseDisposal.XmlData _xmlData = new CustomsWarehouseDisposal.XmlData()
        {
          AdditionalQuantity = _xmli.AddedKg,
          DeclaredQuantity = _xmli.QtyToClear,
          SKUDescription = _xmli.Description
        };
        CustomsWarehouse.Dispose(entities, _xmli.BatchNo.Trim(), parent, _xmlData);
      }
      progressChanged(null, new ProgressChangedEventArgs(1, "GetXmlContent: finished"));
    }
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    private string At { get; set; }
    private const string m_Title = "Batch Message Import";
    private const string m_Message = "Import of the disposal request message {0} starting.";
    private List<Warnning> m_Warnings = new List<Warnning>();
    #endregion

  }
}