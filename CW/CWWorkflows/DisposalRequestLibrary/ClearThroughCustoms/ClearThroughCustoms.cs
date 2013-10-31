//<summary>
//  Title   : public sealed partial class ClearThroughCustoms
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

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
using CAS.SmartFactory.Customs.Messages.CELINA.SAD;
using CAS.SmartFactory.Customs;

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
          foreach ( var item in _dr.CustomsWarehouseDisposal )
          {
            SAD _sad = CraeteSAD( _entities, item );
          }
        }
      }
      catch ( Exception _ex )
      {

      }
    }
    private static SAD CraeteSAD( Entities _entities, CustomsWarehouseDisposal item )
    {
      SADZgloszenieTowarDokumentWymagany[] _dcs = new SADZgloszenieTowarDokumentWymagany[]
      {
        SADZgloszenieTowarDokumentWymagany.Create(1, "9DK8", "9DK8_123456789", "03.10.2013" ),
        SADZgloszenieTowarDokumentWymagany.Create(2, "N935", "82131901", "" ),
        SADZgloszenieTowarDokumentWymagany.Create(3, "5DK5", "12PL360000G000259", "PL8280001819-1111" )
      };
      decimal value = item.CWL_CWDisposal2CustomsWarehouseID.Value.ConvertToDecimal();
      string reference = item.CWL_CWDisposal2CustomsWarehouseID.DocumentNo;
      SADZgloszenieTowar[] _good = new SADZgloszenieTowar[] 
      {
        SADZgloszenieTowar.Create( item.CW_SettledNetMass.ConvertToDecimal(), item.CW_RemainingPackage.ConvertToDecimal(), reference, _dcs, value )
      };
      SADZgloszenieUC customsOffice = null;
      SADZgloszenie _application = SADZgloszenie.Create( SADZgloszenieWartoscTowarow.Create( item.TobaccoValue.ConvertToDecimal() ), _good, customsOffice );
      return SAD.Create( Settings.GetParameter( _entities, SettingsEntry.OrganizationEmail ), _application );
    }
  }
}
