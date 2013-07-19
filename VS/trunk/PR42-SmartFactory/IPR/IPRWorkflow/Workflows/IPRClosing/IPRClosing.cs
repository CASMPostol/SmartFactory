using System;
using System.Linq;
using System.Workflow.Activities;
using CAS.SmartFactory.IPR.DocumentsFactory;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.AccountClearance;
using Microsoft.SharePoint.Workflow;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.Workflows.IPRClosing
{
  /// <summary>
  /// IPR Closing Workflow
  /// </summary>
  public sealed partial class IPRClosing: SequentialWorkflowActivity
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
    public Guid workflowId = new System.Guid( "{B16A9665-EFEA-4677-954B-6A1FE2A633FC}" );
    /// <summary>
    /// The workflow properties
    /// </summary>
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void RecordValidation_ExecuteCode( object sender, EventArgs e )
    {
      try
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
      catch ( Exception ex )
      {
        LogWarningMessageToHistory_HistoryOutcome = "Validation fatal error";
        string _patt = "Cannot validate the IPR account because of fata error {0} at {1}";
        LogWarningMessageToHistory_HistoryDescription = String.Format( _patt, ex.Message, ex.StackTrace );
      }
    }
    private bool Valid = false;
    private void ProcessIfValid( object sender, ConditionalEventArgs e )
    {
      e.Result = Valid;
    }
    private void Closeing_ExecuteCode( object sender, EventArgs e )
    {
      try
      {
        using ( Entities _edc = new Entities( workflowProperties.WebUrl ) )
        {
          IPRClass _record = Element.GetAtIndex<WebsiteModel.Linq.IPR>( _edc.IPR, workflowProperties.ItemId );
          string _documentName = Settings.RequestForAccountClearenceDocumentName( _edc, _record.Id.Value );
          RequestContent _content = DocumentsFactory.AccountClearanceFactory.CreateRequestContent( _record, _record.Id.Value, _documentName );
          int _id = SPDocumentFactory.Prepare( this.workflowProperties.Web, _content, _documentName );
          WebsiteModel.Linq.IPRLib _document = Element.GetAtIndex<WebsiteModel.Linq.IPRLib>( _edc.IPRLibrary, _id );
          _record.IPRLibraryIndex = _document;
          _record.AccountClosed = true;
          _record.ClosingDate = DateTime.Today.Date;
          _document.DocumentNo = _record.Title;
          _edc.SubmitChanges();
        }
      }
      catch ( Exception ex )
      {
        LogFinalMessageToHistory_HistoryOutcome = "Closing fatal error";
        string _patt = "Cannot close the IPR account because of fata error {0} at {1}";
        LogFinalMessageToHistory_HistoryDescription = String.Format( _patt, ex.Message, ex.StackTrace );
      }
    }
    /// <summary>
    /// The log warning message to history_ history description
    /// </summary>
    public String LogWarningMessageToHistory_HistoryDescription = "Cannot close the IPR account because {0}, corect the content and try again.";
    /// <summary>
    /// The log warning message to history_ history outcome
    /// </summary>
    public String LogWarningMessageToHistory_HistoryOutcome = "Closing failed";
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
