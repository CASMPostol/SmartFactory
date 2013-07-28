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
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.BinCard;
using CAS.SharePoint.DocumentsFactory;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.BinCard
{
  public sealed partial class BinCard: SequentialWorkflowActivity
  {
    public BinCard()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void CreateBinCard( object sender, EventArgs e )
    {
      using ( Entities _entities = new Entities( workflowProperties.WebUrl ) )
      {
        CustomsWarehouse _cw = Element.GetAtIndex<CustomsWarehouse>( _entities.CustomsWarehouse, workflowProperties.ItemId );
        if ( _cw.CWL_CW2BinCardTitle == null )
          CreateNewBinCard( _entities, workflowProperties.Web, workflowProperties.WebUrl, workflowProperties.ItemId );
        else
          UpdateBinCard();

      }
      logToHistoryListActivity_HistoryOutcome = "Success";
      logToHistoryListActivity_HistoryDescription = "Document created successfully";
    }
    private static void UpdateBinCard()
    {
      throw new NotImplementedException();
    }
    private static void CreateNewBinCard( Entities entities, SPWeb web, string webUrl, int itemId )
    {
      SPFile _newFile = default( SPFile );
      BinCardContentType _empty = BinCardContentType.CreateEmptyContent();
      string _documentName = Settings.BinCardDocumentName( entities, itemId + 1 );
      _newFile = File.Create<BinCardContentType>( web, _empty, _documentName, BinCardLib.Name, BinCardContentType.StylesheetNmane );

    }
    public String logToHistoryListActivity_HistoryOutcome = default( System.String );
    public String logToHistoryListActivity_HistoryDescription = default( System.String );
    public String terminateActivity_Error1 = default( System.String );
  }
}
