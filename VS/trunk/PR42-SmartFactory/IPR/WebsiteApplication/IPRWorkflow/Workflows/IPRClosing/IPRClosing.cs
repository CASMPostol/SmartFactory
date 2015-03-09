//<summary>
//  Title   : class IPRClosing
//  System  : Microsoft Visual C# .NET 2012
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

using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.AccountClearance;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Activities;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.Workflows.IPRClosing
{
  /// <summary>
  /// IPR Closing Workflow
  /// </summary>
  public sealed partial class IPRClosing : SequentialWorkflowActivity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="IPRClosing" /> class.
    /// </summary>
    public IPRClosing()
    {
      InitializeComponent();
    }
    /// <summary>
    /// The workflow id
    /// </summary>
    public Guid workflowId = new System.Guid("{B16A9665-EFEA-4677-954B-6A1FE2A633FC}");
    /// <summary>
    /// The workflow properties
    /// </summary>
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void Closeing_ExecuteCode(object sender, EventArgs e)
    {
      TraceEvent("Entering IPRClosing.Closeing_ExecuteCode", 54, TraceSeverity.Monitorable);
      string _at = "Starting";
      try
      {
        bool _Closing = true;
        using (Entities _edc = new Entities(workflowProperties.WebUrl))
        {
          IPRClass _iprItem = Element.GetAtIndex<WebsiteModel.Linq.IPR>(_edc.IPR, workflowProperties.ItemId);
          _at = "if (_record.AccountBalance != 0)";
          if (_iprItem.AccountBalance != 0)
          {
            LogFinalMessageToHistory_HistoryOutcome = "Closing error";
            LogFinalMessageToHistory_HistoryOutcome = String.Format(LogWarningTemplate, "AccountBalance must be equal 0");
            _Closing = false;
          }
          _at = "bool _notFinished";
          if (_iprItem.AllEntriesClosed(_edc, (x, y, z) => TraceEvent(x, y, z)))
          {
            LogFinalMessageToHistory_HistoryOutcome = "Closing error";
            LogFinalMessageToHistory_HistoryOutcome = String.Format(LogWarningTemplate, "All disposals must be cleared through customs before closing account.");
            _Closing = false;
          }
          string _documentName = Settings.RequestForAccountClearenceDocumentName(_edc, _iprItem.Id.Value);
          _at = "CreateRequestContent";
          List<Disposal> _Disposals = _iprItem.Disposals(_edc, (x, y, z) => TraceEvent(x, y, z)).Where<Disposal>(x => x.CustomsStatus == CustomsStatus.Finished).ToList<Disposal>();
          RequestContent _content = DocumentsFactory.AccountClearanceFactory.CreateRequestContent(_Disposals, _iprItem, _documentName);
          if (_iprItem.IPRLibraryIndex != null)
          {
            _at = "File.WriteXmlFile";
            File.WriteXmlFile<RequestContent>(this.workflowProperties.Web, _iprItem.IPRLibraryIndex.Id.Value, Entities.IPRLibraryName, _content, DocumentNames.RequestForAccountClearenceName);
          }
          else
          {
            _at = "File.CreateXmlFile";
            SPFile _docFile = File.CreateXmlFile<RequestContent>(this.workflowProperties.Web, _content, _documentName, Entities.IPRLibraryName, DocumentNames.RequestForAccountClearenceName);
            WebsiteModel.Linq.IPRLib _document = Element.GetAtIndex<WebsiteModel.Linq.IPRLib>(_edc.IPRLibrary, _docFile.Item.ID);
            _document.DocumentNo = _iprItem.Title;
            _iprItem.IPRLibraryIndex = _document;
          }
          if (_Closing)
          {
            _iprItem.AccountClosed = true;
            _iprItem.ClosingDate = DateTime.Today.Date;
          }
          _at = "SubmitChanges";
          _edc.SubmitChanges();
        }
      }
      catch (Exception ex)
      {
        LogFinalMessageToHistory_HistoryOutcome = "Closing fatal error";
        string _pat = "Cannot close the IPR account because of fatal error {0} at {1}";
        LogFinalMessageToHistory_HistoryDescription = String.Format(_pat, ex.Message, _at);
        TraceEvent("Exception at IPRClosing.Closeing_ExecuteCode:: " + LogFinalMessageToHistory_HistoryDescription + " Stack: " + ex.StackTrace, 54, TraceSeverity.High);
      }
      TraceEvent("Finished IPRClosing.Closeing_ExecuteCode", 54, TraceSeverity.Monitorable);
    }
    /// <summary>
    /// The log warning message to history_ history description
    /// </summary>
    public String LogWarningTemplate = "Cannot close the IPR account because {0}, correct the content and try again.";
    /// <summary>
    /// The log final message to history_ history description
    /// </summary>
    public String LogFinalMessageToHistory_HistoryDescription = "The IPR record has been successfully closed.";
    /// <summary>
    /// The log final message to history_ history outcome
    /// </summary>
    public String LogFinalMessageToHistory_HistoryOutcome = "Finished successfully";

    private static void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.IPRClosing);
    }

  }
}
