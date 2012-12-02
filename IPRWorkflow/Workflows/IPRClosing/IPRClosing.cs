using System;
using System.Linq;
using System.Workflow.Activities;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.IPR.Workflows.IPRClosing
{
  public sealed partial class IPRClosing: SequentialWorkflowActivity
  {
    public IPRClosing()
    {
      InitializeComponent();
    }
    public Guid workflowId = new System.Guid( "{B16A9665-EFEA-4677-954B-6A1FE2A633FC}" );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void RecordValidation_ExecuteCode( object sender, EventArgs e )
    {
      using ( Entities _edc = new Entities( workflowProperties.WebUrl ) )
      {
        WebsiteModel.Linq.IPR _record = Element.GetAtIndex<WebsiteModel.Linq.IPR>( _edc.IPR, workflowProperties.ItemId );
        if ( _record.AccountBalance != 0 )
        {
          LogWarningMessageToHistory_HistoryDescription = String.Format( LogWarningMessageToHistory_HistoryDescription, "AccountBalance must be equal 0" );
          return;
        }
        bool _notFinished = _record.Disposal.Where<Disposal>( v => v.CustomsStatus.Value != CustomsStatus.Finished ).Any<Disposal>();
        if ( _notFinished )
        {
          LogWarningMessageToHistory_HistoryDescription = String.Format( LogWarningMessageToHistory_HistoryDescription, "All disposals must be cleared through customs." );
          return;
        }
        Valid = true;
      }
    }
    private bool Valid = false;
    private void ProcessIfValid( object sender, ConditionalEventArgs e )
    {
      e.Result = Valid;
    }
    private void Closeing_ExecuteCode( object sender, EventArgs e )
    {
      using ( Entities _edc = new Entities( workflowProperties.WebUrl ) )
      {
        WebsiteModel.Linq.IPR _record = Element.GetAtIndex<WebsiteModel.Linq.IPR>( _edc.IPR, workflowProperties.ItemId );
        _record.AccountClosed = true;
      }
    }
    public String LogWarningMessageToHistory_HistoryDescription = "Cannot close the IPR account because {0}, corect the content and try again.";
  }
}
