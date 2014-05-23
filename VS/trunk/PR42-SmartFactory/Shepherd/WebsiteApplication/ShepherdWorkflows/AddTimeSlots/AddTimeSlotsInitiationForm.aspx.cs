//<summary>
//  Title   : AddTimeSlotsInitiationForm for workflow AddTimeSlots
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
using System.Web;
using System.Web.UI;
using CAS.SmartFactory.Shepherd.Workflows.WorkflowData;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.Workflows.AddTimeSlots
{
  /// <summary>
  /// AddTimeSlotsInitiationForm
  /// </summary>
  public partial class AddTimeSlotsInitiationForm: LayoutsPageBase
  {
    protected void Page_Load( object sender, EventArgs e )
    {
      InitializeParams();
      for ( int i = 1; i <= 31; i++ )
        m_Day.AddTextAndValue( i );
      for ( int i = 1; i <= 12; i++ )
        m_Month.AddTextAndValue( i );
      int _yer = DateTime.Now.Year;
      for ( int i = _yer; i < _yer + 3; i++ )
        m_Year.AddTextAndValue( i );
      for ( int i = 1; i <= 25; i++ )
        m_Duration.AddTextAndValue( i );
    }
    /// <summary>
    /// This method is called when the user clicks the button to start the workflow.
    /// </summary>
    /// <returns></returns>
    private string GetInitiationData()
    {
      TimeSlotsInitiationData _data = new WorkflowData.TimeSlotsInitiationData()
      {
        Duration = m_Duration.SelectedItem.Text.String2IntOrDefault( 30 ),
        StartDate = new DateTime( m_Year.SelectedValue.String2IntOrDefault( DateTime.Now.Year ), m_Month.SelectedValue.String2IntOrDefault( 1 ), m_Day.SelectedValue.String2IntOrDefault( 1 ) )
      };
      return _data.Serialize();
    }
    protected void StartWorkflow_Click( object sender, EventArgs e )
    {
      // Optionally, add code here to perform additional steps before starting your workflow
      try
      {
        HandleStartWorkflow();
      }
      catch ( Exception )
      {
        SPUtility.TransferToErrorPage( SPHttpUtility.UrlKeyValueEncode( "Failed to Start Workflow" ) );
      }
    }
    protected void Cancel_Click( object sender, EventArgs e )
    {
      SPUtility.Redirect( "Workflow.aspx", SPRedirectFlags.RelativeToLayoutsPage, HttpContext.Current, Page.ClientQueryString );
    }

    #region Workflow Initiation Code - Typically, the following code should not be changed

    private string associationGuid;
    private SPList workflowList;
    private SPListItem workflowListItem;

    private void InitializeParams()
    {
      try
      {
        this.associationGuid = Request.Params[ "TemplateID" ];

        // Parameters 'List' and 'ID' will be null for site workflows.
        if ( !String.IsNullOrEmpty( Request.Params[ "List" ] ) && !String.IsNullOrEmpty( Request.Params[ "ID" ] ) )
        {
          this.workflowList = this.Web.Lists[ new Guid( Request.Params[ "List" ] ) ];
          this.workflowListItem = this.workflowList.GetItemById( Convert.ToInt32( Request.Params[ "ID" ] ) );
        }
      }
      catch ( Exception )
      {
        SPUtility.TransferToErrorPage( "Failed to read Request Parameters" );
      }
    }

    private void HandleStartWorkflow()
    {
      if ( this.workflowList != null && this.workflowListItem != null )
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
      SPWorkflowAssociation association = this.Web.WorkflowAssociations[ new Guid( this.associationGuid ) ];
      this.Web.Site.WorkflowManager.StartWorkflow( (object)null, association, GetInitiationData(), SPWorkflowRunOptions.Synchronous );
      SPUtility.Redirect( this.Web.Url, SPRedirectFlags.UseSource, HttpContext.Current );
    }

    private void StartListWorkflow()
    {
      SPWorkflowAssociation association = this.workflowList.WorkflowAssociations[ new Guid( this.associationGuid ) ];
      this.Web.Site.WorkflowManager.StartWorkflow( workflowListItem, association, GetInitiationData() );
      SPUtility.Redirect( this.workflowList.DefaultViewUrl, SPRedirectFlags.UseSource, HttpContext.Current );
    }
    #endregion
  }
}