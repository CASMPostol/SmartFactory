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
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

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
    private void Validate( object sender, EventArgs e )
    {
      try
      {
        using ( Entities _edc = new Entities( this.workflowProperties.WebUrl ) )
        {
          Stock _stock = Element.GetAtIndex<Stock>( _edc.Stock, workflowProperties.ItemId );
          int _problems = 0;
          foreach ( StockEntry _sex in _stock.FinishedGoodsNotProcessed( _edc ) )
          {
            Batch _batchLookup = Batch.FindStockToBatchLookup( _edc, _sex.Batch );
            if ( _batchLookup != null )
            {
              _sex.BatchIndex = _batchLookup;
              continue;
            }
            ActivityLogCT.WriteEntry( _edc, "CreateReport", _sex.NoMachingBatcgWarningMessage );
            _problems++;
          }
          foreach ( StockEntry _senbx in _stock.FinishedGoodsNotBalanced( _edc ) )
          {
            ActivityLogCT.WriteEntry( _edc, "CreateReport", _senbx.NoMachingQuantityWarningMessage );
            _problems++;
          }
          if ( !ValidationPaased )
          {
            ValidationPaased = false;
            string _mtmplt = "Data validation failed. There are {0} reported problems that must be resolved to start report calculation procedure. Details you can find on the application log.";
            ValidationFailedLog_HistoryDescription = String.Format( _mtmplt, _problems );
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
