using System;
using System.Workflow.Activities;
using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.BinCard;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

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
      using ( Entities _entities = new Entities( workflowProperties.WebUrl ) { ObjectTrackingEnabled = false } )
      {
        CustomsWarehouse _cw = Element.GetAtIndex<CustomsWarehouse>( _entities.CustomsWarehouse, workflowProperties.ItemId );
        BinCardContentType _newBinCard = Factory.CreateContent( _cw );
        if ( _cw.CWL_CW2BinCardTitle == null )
        {
          string _documentName = Settings.BinCardDocumentName( _entities, workflowProperties.ItemId + 1 );
          SPFile _newFile =  File.CreateXmlFile<BinCardContentType>( workflowProperties.Web, _newBinCard, _documentName, BinCardLib.Name, BinCardContentType.StylesheetNmane );
          BinCardLib _BinCardLibRntry = Element.GetAtIndex<BinCardLib>( _entities.BinCardLibrary, _newFile.Item.ID );
          _cw.CWL_CW2BinCardTitle = _BinCardLibRntry;
          _entities.SubmitChanges();
        }
        else
          File.WriteXmlFile<BinCardContentType>( workflowProperties.Item.File, _newBinCard, BinCardContentType.StylesheetNmane );
      }
      logToHistoryListActivity_HistoryOutcome = "Success";
      logToHistoryListActivity_HistoryDescription = "Document created successfully";
    }
    public String logToHistoryListActivity_HistoryOutcome = default( System.String );
    public String logToHistoryListActivity_HistoryDescription = default( System.String );
    public String terminateActivity_Error1 = default( System.String );
  }
}
