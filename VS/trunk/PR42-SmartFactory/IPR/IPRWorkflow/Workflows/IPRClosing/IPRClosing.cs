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

using System;
using System.Linq;
using System.Workflow.Activities;
using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.AccountClearance;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
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
      try
      {
        bool Valid = true;
        using (Entities _edc = new Entities(workflowProperties.WebUrl))
        {
          IPRClass _record = Element.GetAtIndex<WebsiteModel.Linq.IPR>(_edc.IPR, workflowProperties.ItemId);
          if (_record.AccountBalance != 0)
          {
            LogFinalMessageToHistory_HistoryOutcome = "Closing error";
            LogFinalMessageToHistory_HistoryOutcome = String.Format(LogWarningTemplate, "AccountBalance must be equal 0");
            Valid = false;
          }
          bool _notFinished = _record.Disposals(_edc).Where<Disposal>(v => v.SettledQuantityDec > 0 && v.CustomsStatus.Value != CustomsStatus.Finished).Any<Disposal>();
          if (_notFinished)
          {
            LogFinalMessageToHistory_HistoryOutcome = "Closing error";
            LogFinalMessageToHistory_HistoryOutcome = String.Format(LogWarningTemplate, "All disposals must be cleared through customs.");
            Valid = false;
          }
          string _documentName = Settings.RequestForAccountClearenceDocumentName(_edc, _record.Id.Value);
          RequestContent _content = DocumentsFactory.AccountClearanceFactory.CreateRequestContent(_edc, _record, _record.Id.Value, _documentName);
          if (_record.IPRLibraryIndex != null)
            File.WriteXmlFile<RequestContent>(this.workflowProperties.Web, _record.IPRLibraryIndex.Id.Value, Entities.IPRLibraryName, _content, DocumentNames.RequestForAccountClearenceName);
          else
          {
            SPFile _docFile = CAS.SharePoint.DocumentsFactory.File.CreateXmlFile<RequestContent>(this.workflowProperties.Web, _content, _documentName, Entities.IPRLibraryName, DocumentNames.RequestForAccountClearenceName);
            WebsiteModel.Linq.IPRLib _document = Element.GetAtIndex<WebsiteModel.Linq.IPRLib>(_edc.IPRLibrary, _docFile.Item.ID);
            _document.DocumentNo = _record.Title;
            _record.IPRLibraryIndex = _document;
          }
          if (Valid)
          {
            _record.AccountClosed = true;
            _record.ClosingDate = DateTime.Today.Date;
          }
          _edc.SubmitChanges();
        }
      }
      catch (Exception ex)
      {
        LogFinalMessageToHistory_HistoryOutcome = "Closing fatal error";
        string _patt = "Cannot close the IPR account because of fatal error {0} at {1}";
        LogFinalMessageToHistory_HistoryDescription = String.Format(_patt, ex.Message, ex.StackTrace);
      }
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

  }
}
