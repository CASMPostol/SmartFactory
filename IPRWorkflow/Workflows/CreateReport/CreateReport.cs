using System;
using System.Workflow.Activities;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.IPR.Workflows.CreateReport
{
  /// <summary>
  /// Create Report
  /// </summary>
  public sealed partial class CreateReport: SequentialWorkflowActivity
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CreateReport" /> class.
    /// </summary>
    public CreateReport()
    {
      InitializeComponent();
    }

    public Guid workflowId = new System.Guid( "{0B735E12-FEE5-48E7-9727-D11CEE7866C6}" );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();

    #region CreateListsItems
    private void CreateRecorts( object sender, EventArgs e )
    {

    }
    #endregion

    #region ValidationSequence
    private void AssociateWithBatches( object sender, EventArgs e )
    {
      try
      {
        using ( Entities _edc = new Entities( this.workflowProperties.WebUrl ) )
        {
          StockLib _stock = Element.GetAtIndex<StockLib>( _edc.StockLibrary, workflowProperties.ItemId );
          ValidationPaased = _stock.Validate( _edc );
          if ( !ValidationPaased )
          {
            string _mtmplt = "Data validation failed. There are problems reported that must be resolved to start calculation procedure. Details you can find on the application log.";
            ValidationFailedLog_HistoryDescription = _mtmplt;
          }
        }
      }
      catch ( Exception _ex )
      {
        string _patt = "Cannot validate data because of a fata error {0} at: {1}";
        ValidationFailedLog_HistoryDescription = String.Format( _patt, _ex.Message, _ex.StackTrace );
      }
    }

    public String ValidationFailedLog_HistoryDescription = default( System.String );
    private bool ValidationPaased = false;
    private void Validated( object sender, ConditionalEventArgs e )
    {
      e.Result = ValidationPaased;
    }
    #endregion

    #region MainReportCreationSequence
    private void CalculateReports( object sender, EventArgs e )
    {
      CalculationPassed = true;
    }
    private bool CalculationPassed = false;
    private void Calculated( object sender, ConditionalEventArgs e )
    {
      e.Result = CalculationPassed;
    }
    private void GenerateReport( object sender, EventArgs e )
    {

    }
    #endregion


  }
}
