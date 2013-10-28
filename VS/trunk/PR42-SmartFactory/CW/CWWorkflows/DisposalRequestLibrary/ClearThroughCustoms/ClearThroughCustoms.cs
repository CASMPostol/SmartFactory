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

namespace CAS.SmartFactory.CW.Workflows.DisposalRequestLibrary.ClearThroughCustoms
{
  public sealed partial class ClearThroughCustoms: SequentialWorkflowActivity
  {
    public ClearThroughCustoms()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    private void onCreateMessageTemplates( object sender, EventArgs e )
    {
      try
      {
        using ( Entities _entities = new Entities( workflowProperties.WebUrl ) )
        {
          DisposalRequestLib _dr = Element.GetAtIndex<DisposalRequestLib>( _entities.DisposalRequestLibrary, workflowProperties.ItemId );
          IQueryable<IGrouping<String, CustomsWarehouseDisposal>> _clearances = from _cdx in _dr.CustomsWarehouseDisposal group _cdx by _cdx.CWL_CWDisposal2CustomsWarehouseID.Batch;
          foreach ( var item in _clearances )
          {

          }
        }

      }
      catch ( Exception _ex )
      {

      }
    }
  }
}
