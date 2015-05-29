//<summary>
//  Title   : BatchEventReceiver
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SmartFactory.Customs;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using BatchMaterialXml = CAS.SmartFactory.xml.erp.BatchMaterial;
using BatchXml = CAS.SmartFactory.xml.erp.Batch;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers
{
  /// <summary>
  /// Batch List Item Events
  /// </summary>
  public class BatchEventReceiver : SPItemEventReceiver
  {

    #region public
    /// <summary>
    /// An item was added.
    /// </summary>
    /// <param name="properties">Contains properties for asynchronous list item event handlers.</param>
    public override void ItemAdded(SPItemEventProperties properties)
    {
      TraceEvent("Entering BatchEventReceiver.ItemAdded", 43, TraceSeverity.Monitorable);
      base.ItemAdded(properties);
      try
      {
        if (!properties.ListTitle.Contains("Batch Library"))
        {
          //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
          TraceEvent("BatchEventReceiver.ItemAdded ", 50, TraceSeverity.Verbose);
          TraceEvent(String.Format("Exiting BatchEventReceiver.ItemAdded - event called for wrong list, list name {0}.", properties.ListTitle), 51, TraceSeverity.Monitorable);
          base.ItemAdded(properties);
          return;
          //throw new IPRDataConsistencyException(m_Title, "Wrong library name", null, "Wrong library name");
        }
        this.EventFiringEnabled = false;
        using (Entities _edc = new Entities(properties.WebUrl))
        {
          BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>(_edc.BatchLibrary, properties.ListItemId);
          At = "ImportBatchFromXml";
          BatchXml _xml = default(BatchXml);
          using (Stream _stream = properties.ListItem.File.OpenBinaryStream())
            _xml = ImportBatchFromXml(_edc, _stream, properties.ListItem.File.Name, ProgressChange);
          At = "Getting Data";
          GetXmlContent(_xml, _edc, _entry, ProgressChange);
          At = "ListItem assign";
          _entry.BatchLibraryOK = true;
          _entry.BatchLibraryComments = "Batch message import succeeded.";
          At = "SubmitChanges";
          TraceEvent("BatchEventReceiver.ItemAdded at SubmitChanges", 70, TraceSeverity.Verbose);
          _edc.SubmitChanges();
          foreach (Warnning _wrnngx in m_Warnings)
            ActivityLogCT.WriteEntry(_edc, m_Title, String.Format("Import of the batch warning: {0}", _wrnngx.Message));
          ActivityLogCT.WriteEntry(_edc, m_Title, String.Format("Import of the batch {0} message finished", properties.ListItem.File.Name));
        }
      }
      catch (InputDataValidationException _idve)
      {
        _idve.ReportActionResult(properties.WebUrl, properties.ListItem.File.Name);
      }
      catch (IPRDataConsistencyException _ex)
      {
        _ex.Source += " at " + At;
        using (Entities _edc = new Entities(properties.WebUrl))
        {
          _ex.Add2Log(_edc);
          BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>(_edc.BatchLibrary, properties.ListItemId);
          _entry.BatchLibraryOK = false;
          _entry.BatchLibraryComments = _ex.Comments;
          _edc.SubmitChanges();
        }
      }
      catch (Exception _ex)
      {
        using (Entities _edc = new Entities(properties.WebUrl))
        {
          ActivityLogCT.WriteEntry(_edc, "BatchEventReceiver.ItemAdded" + " at " + At, _ex.Message);
          BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>(_edc.BatchLibrary, properties.ListItemId);
          _entry.BatchLibraryComments = "Batch message import error";
          _entry.BatchLibraryOK = false;
          _edc.SubmitChanges();
        }
      }
      finally
      {
        this.EventFiringEnabled = true;
      }
      TraceEvent("Finishing BatchEventReceiver.ItemAdded", 107, TraceSeverity.Verbose);
    }
    /// <summary>
    /// Imports the batch from XML.
    /// </summary>
    /// <param name="edc">The _edc.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="progressChanged">The progress changed delegate <see cref="ProgressChangedEventHandler" />.</param>
    /// <returns></returns>
    /// <exception cref="CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException">Batch XML message validation failed;XML sysntax validation</exception>
    /// <exception cref="IPRDataConsistencyException"></exception>
    public static BatchXml ImportBatchFromXml(Entities edc, Stream stream, string fileName, ProgressChangedEventHandler progressChanged)
    {
      progressChanged(null, new ProgressChangedEventArgs(1, "ImportBatchFromXml.starting"));
      string _Message = String.Format(m_Message, fileName);
      ActivityLogCT.WriteEntry(edc, m_Title, _Message);
      BatchXml _xml = BatchXml.ImportDocument(stream);
      progressChanged(null, new ProgressChangedEventArgs(1, "ImportBatchFromXml.Validate"));
      List<string> _validationErrors = new List<string>();
      _xml.Validate(Settings.GetParameter(edc, SettingsEntry.BatchNumberPattern), _validationErrors);
      if (_validationErrors.Count > 0)
      {
        ErrorsList _el = new ErrorsList(_validationErrors, true);
        throw new InputDataValidationException("Batch XML message validation failed", "XML sysntax validation", _el);
      }
      return _xml;
    }
    #endregion

    #region private
    private void ProgressChange(object sender, ProgressChangedEventArgs progress)
    {
      if (progress.UserState is String)
        At = (string)progress.UserState;
      else if (progress.UserState is Warnning)
        m_Warnings.Add(progress.UserState as Warnning);
      else
        throw new ArgumentException("Wrong state reported", "UserState");
    }
    /// <summary>
    /// Gets the content of the XML.
    /// </summary>
    /// <param name="xml">The document.</param>
    /// <param name="edc">The <see cref="Entities"/> instance.</param>
    /// <param name="parent">The entry.</param>
    /// <param name="progressChanged">The progress changed.</param>
    private static void GetXmlContent(BatchXml xml, Entities edc, BatchLib parent, ProgressChangedEventHandler progressChanged)
    {
      progressChanged(null, new ProgressChangedEventArgs(1, "GetXmlContent: starting"));
      Content _contentInfo = new Content(edc, xml.Material, xml.Status, progressChanged);
      progressChanged(null, new ProgressChangedEventArgs(1, "GetXmlContent: FindLookup"));
      Batch _batch = Batch.FindLookup(edc, _contentInfo.Product.Batch);
      List<string> _warnings = new List<string>();
      bool _newBatch = false;
      progressChanged(null, new ProgressChangedEventArgs(1, "GetXmlContent: switch"));
      switch (_contentInfo.BatchStatus)
      {
        case BatchStatus.Progress:
          if (_batch != null)
            throw new InputDataValidationException("wrong status of the input batch", "Get XML Content", "The status of Progress is not allowed if any batch has been imported previously", true);
          _batch = new Batch();
          _newBatch = true;
          break;
        case BatchStatus.Intermediate:
        case BatchStatus.Final:
          if (_batch != null)
          {
            if (_batch.BatchStatus.Value != BatchStatus.Intermediate)
            {
              string _ptrn = "The final batch {0} has been analyzed already.";
              throw new InputDataValidationException("wrong status of the input batch", "Get XML Content", String.Format(_ptrn, _contentInfo.Product.Batch), true);
            }
          }
          else
          {
            _batch = new Batch();
            _newBatch = true;
          }
          break;
      }
      progressChanged(null, new ProgressChangedEventArgs(1, "GetXmlContent: Validate"));
      _contentInfo.Validate(edc, _batch.Disposal(edc, _newBatch));
      if (_newBatch)
        edc.Batch.InsertOnSubmit(_batch);
      progressChanged(null, new ProgressChangedEventArgs(1, "GetXmlContent: BatchProcessing"));
      _batch.BatchProcessing(edc, _contentInfo, parent, _newBatch, TraceEvent);
      progressChanged(null, new ProgressChangedEventArgs(1, "GetXmlContent: SubmitChanges"));
      edc.SubmitChanges();
    }
    private static BatchStatus GetBatchStatus(xml.erp.BatchStatus batchStatus)
    {
      BatchStatus _status = default(BatchStatus);
      switch (batchStatus)
      {
        case xml.erp.BatchStatus.Final:
          _status = BatchStatus.Final;
          break;
        case xml.erp.BatchStatus.Intermediate:
          _status = BatchStatus.Intermediate;
          break;
        case xml.erp.BatchStatus.Progress:
          _status = BatchStatus.Progress;
          break;
      }
      return _status;
    }
    private class Content : SummaryContentInfo
    {

      internal Content(Entities entities, BatchMaterialXml[] xml, xml.erp.BatchStatus batchStatus, ProgressChangedEventHandler progressChanged)
        : base(GetBatchStatus(batchStatus))
      {
        foreach (BatchMaterialXml item in xml)
        {
          Entities.ProductDescription _product = entities.GetProductType(item.Material, item.Stor__Loc, item.IsFinishedGood);
          Material _newMaterial = new Material(entities, _product, item.Batch, item.Material, item.Stor__Loc, item.Material_description, item.Unit, item.Quantity, item.Quantity_calculated, item.material_group);
          _newMaterial.GetListOfIPRAccounts(entities);
          progressChanged(this, new ProgressChangedEventArgs(1, String.Format("SKU={0}", _newMaterial.SKU)));
          Add(_newMaterial);
        }
        if (Product == null)
          throw new InputDataValidationException("Unrecognized finished good", "Product", "Wrong Batch XML message", true);
      }
    }
    private ErrorsList m_Warnings = new ErrorsList();
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    private string At { get; set; }
    private const string m_Title = "Batch Message Import";
    private const string m_Message = "Import of the batch message {0} starting.";
    private static void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.BatchProcessing);
    }
    #endregion

  }
}
