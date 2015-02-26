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

using CAS.SharePoint.Linq;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseManyAccounts
{
  public partial class CloseManyAccountsForm : LayoutsPageBase, CAS.SharePoint.Linq.IPreRender
  {
    public CloseManyAccountsForm()
    {
      m_DataContextManagement = new DataContextManagement<Entities>(this);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      InitializeParams();
      m_DataSource = m_DataContextManagement.DataContext.CustomsWarehouse.
        Where<CustomsWarehouse>(x => !x.AccountClosed.Value && x.AccountBalance == 0).
        Select(y => new CustomsWarehouseDataSource
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
          IsSelected = true
        }).ToList<CustomsWarehouseDataSource>();
      m_AvailableGridView.DataSource = m_DataSource;
      m_AvailableGridView.DataBind();
    }
    /// <summary>
    /// Gets the initiation data. This method is called when the user clicks the button to start the workflow.
    /// </summary>
    /// <returns>System.String.</returns>
    private string GetInitiationData()
    {
      InitializationFormData _initializationFormData = new InitializationFormData() 
        { AccountsArray = m_DataSource.Where<CustomsWarehouseDataSource>(x => x.IsSelected).
                                       Select<CustomsWarehouseDataSource, int>(y=> y.Id).
                                       ToArray<int>() };
      return CAS.SharePoint.Serialization.JsonSerializer.Serialize<InitializationFormData>(_initializationFormData);
    }
    private List<CustomsWarehouseDataSource> m_DataSource = null;
    protected void StartWorkflow_Click(object sender, EventArgs e)
    {
      // Optionally, add code here to perform additional steps before starting your workflow
      try
      {
        HandleStartWorkflow();
      }
      catch (Exception)
      {
        SPUtility.TransferToErrorPage(SPHttpUtility.UrlKeyValueEncode("Failed to Start Workflow"));
      }
    }
    protected void Cancel_Click(object sender, EventArgs e)
    {
      SPUtility.Redirect("Workflow.aspx", SPRedirectFlags.RelativeToLayoutsPage, HttpContext.Current, Page.ClientQueryString);
    }
    private DataContextManagement<Entities> m_DataContextManagement = null;

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
      SPWorkflowAssociation association = this.workflowList.WorkflowAssociations[new Guid(this.associationGuid)];
      this.Web.Site.WorkflowManager.StartWorkflow(workflowListItem, association, GetInitiationData());
      SPUtility.Redirect(this.workflowList.DefaultViewUrl, SPRedirectFlags.UseSource, HttpContext.Current);
    }
    #endregion
  }
}