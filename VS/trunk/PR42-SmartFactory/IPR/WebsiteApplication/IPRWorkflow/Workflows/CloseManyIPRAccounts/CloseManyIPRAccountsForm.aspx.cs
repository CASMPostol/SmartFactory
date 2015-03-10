//_______________________________________________________________
//  Title   : Name of Application
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using CAS.SharePoint;
using CAS.SharePoint.Linq;
using CAS.SharePoint.Serialization;
using CAS.SmartFactory.IPR.WebsiteModel;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IPRLinq = CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Workflows.CloseManyIPRAccounts
{

  /// <summary>
  /// Class CloseManyIPRAccountsForm - UI to get list of IPR accounts to be closed.
  /// </summary>
  public partial class CloseManyIPRAccountsForm : LayoutsPageBase, IPreRender
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CloseManyIPRAccountsForm"/> class.
    /// </summary>
    public CloseManyIPRAccountsForm()
    {
      m_DataContextManagement = new DataContextManagement<IPRLinq.Entities>(this);
    }
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      TraceEvent("Entering CloseManyIPRAccountsForm.Page_Load", 55, TraceSeverity.Monitorable);
      InitializeParams();
      try
      {
        m_DataSource = m_DataContextManagement.DataContext.IPR.
          Where<IPRLinq.IPR>(x => !x.AccountClosed.Value && x.AccountBalance == 0).
          ToList<IPRLinq.IPR>().
          Where<IPRLinq.IPR>(x => x.AllEntriesClosed(m_DataContextManagement.DataContext, (a, b, c) => TraceEvent(a, b, c))).
          Select<IPRLinq.IPR, IPRAccountDataSource>(y => new IPRAccountDataSource
          {
            Batch = y.Batch,
            ClosingDate = y.ClosingDate.GetValueOrDefault(CAS.SharePoint.Extensions.SPMinimum),
            CustomsDebtDate = y.CustomsDebtDate.GetValueOrDefault(CAS.SharePoint.Extensions.SPMinimum),
            DocumentNo = y.DocumentNo,
            Grade = y.Grade,
            Title = y.Title,
            SKU = y.SKU,
            NetMass = y.NetMass.GetValueOrDefault(-1),
            AccountBalance = y.AccountBalance.GetValueOrDefault(-1),
            ValidToDate = y.ValidToDate.GetValueOrDefault(CAS.SharePoint.Extensions.SPMinimum),
            Id = y.Id.Value,
            IsSelected = true,
            Cartons = y.Cartons.GetValueOrDefault(-1),
            OGLValidTo = y.OGLValidTo.GetValueOrDefault(Extensions.SPMinimum)
          }).ToList<IPRAccountDataSource>();
        if (this.IsPostBack)
        {
          TraceEvent("CloseManyIPRAccountsForm.Page_Load - IsPostBack do nothing.", 82, TraceSeverity.Monitorable);
          return;
        }
        TraceEvent(
          String.Format("CloseManyIPRAccountsForm: found {0} accounts ready to be closed", String.Join(",", m_DataSource.Select<IPRAccountDataSource, string>(x => x.Title).ToArray<string>())),
          83,
          TraceSeverity.Verbose);
        m_AvailableGridView.DataSource = m_DataSource;
        m_AvailableGridView.DataBind();
        TraceEvent("Finished CloseManyIPRAccountsForm.Page_Load", 91, TraceSeverity.Monitorable);
      }
      catch (Exception _ex)
      {
        this.Controls.Add(new CAS.SharePoint.Web.ExceptionMessage(_ex));
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyIPRAccountsForm.Page_Load"), 96, TraceSeverity.High);
      }
    }
    /// <summary>
    /// Gets the initiation data. This method is called when the user clicks the button to start the workflow.
    /// </summary>
    /// <returns>System.String.</returns>
    private string GetInitiationData()
    {
      List<int> _selected = new List<int>();
      for (int i = 0; i < m_AvailableGridView.Rows.Count; i++)
      {
        GridViewRow _row = m_AvailableGridView.Rows[i];
        CheckBox _cb = FindControlRecursive(_row, "x_IsSelected") as CheckBox;
        if (_cb == null)
          throw new ArgumentException("Cannot find CheckBox on the page");
        if (_cb.Checked)
          _selected.Add(m_DataSource[i].Id);
      }
      InitializationFormData _initializationFormData = new InitializationFormData() { AccountsArray = _selected.ToArray() };
      return JsonSerializer.Serialize<InitializationFormData>(_initializationFormData);
    }
    private List<IPRAccountDataSource> m_DataSource = null;
    private Control FindControlRecursive(Control rootControl, string controlID)
    {
      if (rootControl.ID == controlID)
        return rootControl;
      foreach (Control controlToSearch in rootControl.Controls)
      {
        Control controlToReturn = FindControlRecursive(controlToSearch, controlID);
        if (controlToReturn != null)
          return controlToReturn;
      }
      return null;
    }
    /// <summary>
    /// Handles the Click event of the StartWorkflow control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void StartWorkflow_Click(object sender, EventArgs e)
    {
      // Optionally, add code here to perform additional steps before starting your workflow
      try
      {
        HandleStartWorkflow();
      }
      catch (Exception _ex)
      {
        this.Controls.Add(new CAS.SharePoint.Web.ExceptionMessage(_ex));
        TraceEvent(_ex.ExceptionDiagnosticMessage("CloseManyIPRAccountsForm.StartWorkflow_Click"), 142, TraceSeverity.High);
        throw;
      }
    }
    /// <summary>
    /// Handles the Click event of the Cancel control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected void Cancel_Click(object sender, EventArgs e)
    {
      SPUtility.Redirect("Workflow.aspx", SPRedirectFlags.RelativeToLayoutsPage, HttpContext.Current, Page.ClientQueryString);
    }
    private DataContextManagement<IPRLinq.Entities> m_DataContextManagement = null;
    private void TraceEvent(string message, int eventId, TraceSeverity severity)
    {
      WebsiteModelExtensions.TraceEvent(message, eventId, severity, WebsiteModelExtensions.LoggingCategories.CloseManyAccounts);
    }

    #region Workflow Initiation Code - Typically, the following code should not be changed

    private string associationGuid;
    private SPList workflowList;
    private SPListItem workflowListItem;

    private void InitializeParams()
    {
      try
      {
        this.associationGuid = Request.Params["TemplateID"];

        // Parameters 'List' and 'ID' will be null for site workflows.
        if (!String.IsNullOrEmpty(Request.Params["List"]) && !String.IsNullOrEmpty(Request.Params["ID"]))
        {
          this.workflowList = this.Web.Lists[new Guid(Request.Params["List"])];
          this.workflowListItem = this.workflowList.GetItemById(Convert.ToInt32(Request.Params["ID"]));
        }
      }
      catch (Exception)
      {
        SPUtility.TransferToErrorPage(SPHttpUtility.UrlKeyValueEncode("Failed to read Request Parameters"));
      }
    }

    private void HandleStartWorkflow()
    {
      if (this.workflowList != null && this.workflowListItem != null)
      {
        StartListWorkflow();
      }
      else
      {
        StartSiteWorkflow();
      }
    }

    private void StartSiteWorkflow()
    {
      SPWorkflowAssociation association = this.Web.WorkflowAssociations[new Guid(this.associationGuid)];
      this.Web.Site.WorkflowManager.StartWorkflow((object)null, association, GetInitiationData(), SPWorkflowRunOptions.Synchronous);
      SPUtility.Redirect(this.Web.Url, SPRedirectFlags.UseSource, HttpContext.Current);
    }

    private void StartListWorkflow()
    {
      TraceEvent("Entering CloseManyIPRAccountsForm.StartListWorkflow", 207, TraceSeverity.Monitorable);
      SPWorkflowAssociation association = this.workflowList.WorkflowAssociations[new Guid(this.associationGuid)];
      this.Web.Site.WorkflowManager.StartWorkflow(workflowListItem, association, GetInitiationData());
      TraceEvent(" CloseManyIPRAccountsForm.StartListWorkflow Redirect to: " + this.workflowList.DefaultViewUrl, 210, TraceSeverity.Monitorable);
      bool _redirectResult = SPUtility.Redirect(this.workflowList.DefaultViewUrl, SPRedirectFlags.UseSource, HttpContext.Current);
      TraceEvent(String.Format("CloseManyIPRAccountsForm.StartListWorkflow Redirect result: {0}", _redirectResult), 212, TraceSeverity.Monitorable);
    }
    #endregion

  }
}