using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Workflow.ComponentModel.Compiler;
using System.Workflow.ComponentModel.Serialization;
using System.Workflow.ComponentModel;
using System.Workflow.ComponentModel.Design;
using System.Workflow.Runtime;
using System.Workflow.Activities;
using System.Workflow.Activities.Rules;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.WorkflowActions;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.AccountsReport;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.CW.WebsiteModel;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseReport.DoReportWorkflow
{
  public sealed partial class DoReportWorkflow : SequentialWorkflowActivity
  {
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

    private void DoReport_ExecuteCode(object sender, EventArgs e)
    {
      try
      {
        bool _update = true;
        using (Entities _entities = new Entities(workflowProperties.WebUrl))
        {
          if ((from _cwl in _entities.CustomsWarehouseReportLibrary where _cwl.Id.Value > workflowProperties.ItemId select new { }).Any())
          {
            this.CompletedLogToHistory_HistoryDescription = "Aborted";
            this.CompletedLogToHistory_HistoryOutcome = "A next raport.";
            return;
          }
          CustomsWarehouseReportLib _cwr = Element.GetAtIndex<CustomsWarehouseReportLib>(_entities.CustomsWarehouseReportLibrary, workflowProperties.ItemId);
          string _documentName = Settings.CustomsWarehouseReportFileName(_entities, workflowProperties.ItemId);
          AccountsReportContentWithStylesheet _newRequestContent = CreateContent(_entities, _documentName);
          if (_update)
            File.WriteXmlFile<AccountsReportContentWithStylesheet>((SPDocumentLibrary)workflowProperties.Web.Lists[Entities.CustomsWarehouseReportLibName], workflowProperties.ItemId, _newRequestContent);
          else
            File.CreateXmlFile<AccountsReportContentWithStylesheet>(workflowProperties.Web, _newRequestContent, _documentName, Entities.CustomsWarehouseLibName);
        }
      }
      catch (CAS.SharePoint.ApplicationError _ap)
      {
        CompletedLogToHistory_HistoryOutcome = "ApplicationError";
        CompletedLogToHistory_HistoryDescription = _ap.Message;
      }
      catch (Exception _ex)
      {
        CompletedLogToHistory_HistoryOutcome = "Exeption";
        CompletedLogToHistory_HistoryDescription = _ex.Message;
      }

    }
    private class AccountsReportContentWithStylesheet : AccountsReportContent, IStylesheetNameProvider { }
    private AccountsReportContentWithStylesheet CreateContent(Entities entities, string documentName)
    {

      Dictionary<string, Consent> _ConsentsList = new Dictionary<string, Consent>();
      IQueryable<IGrouping<string, CustomsWarehouse>> _groups = from _grx in entities.CustomsWarehouse
                                                                where !_grx.Archival.Value && !_grx.AccountClosed.Value && _grx.AccountBalance.Value > 0
                                                                let _curr = _grx.Currency.Trim().ToUpper()
                                                                group _grx by _curr;
      List<ArrayOfAccountsAccountsArray> _AccountsColection = CreateArrayOfAccountsAccountsArray(_groups);

      AccountsReportContentWithStylesheet _ret = new AccountsReportContentWithStylesheet()
      {
        AccountsColection = _AccountsColection.ToArray(),
        ConsentNo = String.Join(",", _ConsentsList.Select(x => String.Format("{0}(1:d)", x.Value.Title, x.Value.ConsentDate.Value)).ToArray<string>()),
        ConsentDate = DateTime.MinValue,
        DocumentDate = DateTime.Today,
        DocumentName = documentName,
        ReportDate = DateTime.Today
      };
      return _ret;
    }

    private List<ArrayOfAccountsAccountsArray> CreateArrayOfAccountsAccountsArray(IQueryable<IGrouping<string, CustomsWarehouse>> _groups)
    {
      List<ArrayOfAccountsAccountsArray> _ret = new List<ArrayOfAccountsAccountsArray>();
      foreach (IGrouping<string, CustomsWarehouse> _grx in _groups)
      {
        double _TotalNetMass;
        double _TotalValue;
        List<ArrayOfAccountsDetailsDetailsOfOneAccount> _AccountsDetails = CreateDetails(_grx, out _TotalNetMass, out _TotalValue);
        ArrayOfAccountsAccountsArray _newItem = new ArrayOfAccountsAccountsArray()
        {
          AccountsDetails = _AccountsDetails.ToArray(),
          TotalCurrency = _grx.Key,
          TotalNetMass = _TotalNetMass.RountMass(),
          TotalValue = _TotalValue
        };
        _ret.Add(_newItem);
      }
      return _ret;
    }
    private List<ArrayOfAccountsDetailsDetailsOfOneAccount> CreateDetails(IGrouping<string, CustomsWarehouse> _grx, out double _TotalNetMass, out double _TotalValue)
    {
      _TotalNetMass = 0;
      _TotalValue = 0;
      List<ArrayOfAccountsDetailsDetailsOfOneAccount> _ret = new List<ArrayOfAccountsDetailsDetailsOfOneAccount>();

      return _ret;
    }

  }
}
