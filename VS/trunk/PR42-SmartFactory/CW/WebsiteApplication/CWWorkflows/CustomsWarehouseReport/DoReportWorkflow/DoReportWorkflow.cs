//<summary>
//  Title   : partial class DoReportWorkflow
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

using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint.Workflow;
using System;
using System.Linq;
using System.Workflow.Activities;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseReport.DoReportWorkflow
{
  /// <summary>
  /// partial class DoReportWorkflow : SequentialWorkflowActivity
  /// </summary>
  public sealed partial class DoReportWorkflow : SequentialWorkflowActivity
  {

    #region public
    public DoReportWorkflow()
    {
      InitializeComponent();
    }
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    public String StartingLogToHistory_HistoryDescription = default(System.String);
    public String StartingLogToHistory_HistoryOutcome = "Preparation report started.";
    public String CompletedLogToHistory_HistoryDescription = "Preparation report completed.";
    public String CompletedLogToHistory_HistoryOutcome = "Success";
    #endregion

    #region private
    private void DoReport_ExecuteCode(object sender, EventArgs e)
    {
      try
      {
        using (Entities _entities = new Entities(workflowProperties.WebUrl))
        {
          if ((from _cwl in _entities.CustomsWarehouseReportLibrary where _cwl.Id.Value > workflowProperties.ItemId select new { }).Any())
          {
            this.CompletedLogToHistory_HistoryDescription = "Aborted";
            this.CompletedLogToHistory_HistoryOutcome = "A next rapport.";
            return;
          }
          string _documentName = Settings.CustomsWarehouseReportFileName(_entities, workflowProperties.ItemId + 1);
          AccountsReportContentWithStylesheet _newRequestContent = AccountsReportContentWithStylesheet.CreateReportContent(_entities, _documentName);
          //if (_update)
          //  File.WriteXmlFile<AccountsReportContentWithStylesheet>((SPDocumentLibrary)workflowProperties.Web.Lists[Entities.CustomsWarehouseReportLibName], workflowProperties.ItemId, _newRequestContent);
          //else
          File.CreateXmlFile<AccountsReportContentWithStylesheet>(workflowProperties.Web, _newRequestContent, _documentName, Entities.CustomsWarehouseReportLibName);
        }
      }
      catch (CAS.SharePoint.ApplicationError _ap)
      {
        CompletedLogToHistory_HistoryOutcome = "ApplicationError";
        CompletedLogToHistory_HistoryDescription = _ap.Message;
      }
      catch (Exception _ex)
      {
        CompletedLogToHistory_HistoryOutcome = "Exception";
        CompletedLogToHistory_HistoryDescription = _ex.Message;
      }
    }
    #endregion

  }
}
