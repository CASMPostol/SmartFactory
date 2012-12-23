﻿using System;
using System.Workflow.Activities;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.BalanceSheet;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.IPR.Workflows.JSOXReport
{
  public sealed partial class JSOXReport: SequentialWorkflowActivity
  {
    public JSOXReport()
    {
      InitializeComponent();
    }

    public Guid workflowId = default( System.Guid );
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void CreateReport( object sender, EventArgs e )
    {
      try
      {
        using ( Entities edc = new Entities( workflowProperties.WebUrl ) )
        {
          JSOXLib _list = Element.GetAtIndex<JSOXLib>( edc.JSOXLibrary, workflowProperties.ItemId );
          _list.UpdateJSOXReport( edc );
          string _documentName = xml.XMLResources.RequestForBalanceSheetDocumentName( _list.Identyfikator.Value );
          BalanceSheetContent _content = DocumentsFactory.BalanceSheetContentFactory.CreateRequestContent( _list, _list.Identyfikator.Value, _documentName );
          _content.UpdateDocument( workflowProperties.Item.File );
          edc.SubmitChanges();
        }
        workflowProperties.List.Update();
        EndLogToHistory_HistoryDescription = "Report updated successfully";
      }
      catch ( Exception ex )
      {
        EndLogToHistory_HistoryOutcome = "Closing fatal error";
        string _patt = "Cannot create JSOX report sheet because of fata error {0} at {1}";
        EndLogToHistory_HistoryDescription = String.Format( _patt, ex.Message, ex.StackTrace );
      }
    }
    public String EndLogToHistory_HistoryOutcome = "Finisched";
    public String EndLogToHistory_HistoryDescription = default( System.String );
  }
}
