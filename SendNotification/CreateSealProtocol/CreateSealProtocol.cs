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
using CAS.SmartFactory.Shepherd.SendNotification.Entities;

namespace CAS.SmartFactory.Shepherd.SendNotification.CreateSealProtocol
{
  public sealed partial class CreateSealProtocol : SequentialWorkflowActivity
  {
    public CreateSealProtocol()
    {
      InitializeComponent();
    }
    internal static Guid WorkflowId = new Guid("dddc274f-d1ff-4613-834a-548546f527aa");
    internal static WorkflowDescription WorkflowDescription { get { return new WorkflowDescription(WorkflowId, "New Seal Protocol", "Create Seal Protocol"); } }
    public Guid workflowId = default(System.Guid);
    #region OnWorkflowActivated
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void m_OnWorkflowActivated1_Invoked(object sender, ExternalDataEventArgs e)
    {
      using (SPSite _st = workflowProperties.Site)
        _url = _st.Url;
      m_AfterCreationLogToHistoryList_UserId1 = workflowProperties.OriginatorUser.ID;
    }
    private string _url = default(string);
    #endregion

    #region CreatePO
    private void m_CreatePO_ExecuteCode(object sender, EventArgs e)
    {
      string _spTitle = default(string);
      string _newFileName = default(string);
      string _stt = "Starting";
      try
      {
        _stt = "using";
        using (EntitiesDataContext _EDC = new EntitiesDataContext(_url))
        {
          Shipping _sp = (from idx in _EDC.Shipping
                          where idx.Identyfikator == workflowProperties.ItemId
                          select idx).First();
          _stt = "Shipping";
          _spTitle = _sp.Tytuł;
          SPDocumentLibrary _lib = (SPDocumentLibrary)workflowProperties.Web.Lists[CommonDefinition.SealProtocolLibraryTitle];
          _stt = "SPDocumentLibrary";
          SPFile _teml = workflowProperties.Web.GetFile(_lib.DocumentTemplateUrl);
          string _fname = String.Format("Seal Protocol No {0}.docx", _sp.Identyfikator.ToString());
          SPFile _docFile = OpenXMLHelpers.AddDocument2Collection(_teml, _lib.RootFolder.Files, _fname);
          _newFileName = _docFile.Name;
          _stt = "_doc";
          int _docId = (int)_docFile.Item.ID;
          SealProtocol _fpo = (from idx in _EDC.SealProtocolLibrary
                               where idx.Identyfikator == _docId
                               select idx).First();
        }
        m_AfterCreationLogToHistoryList_HistoryOutcome1 = "Item Created";
        m_AfterCreationLogToHistoryList_HistoryDescription1 = String.Format("File {0} containing purchase order for shipping {1} successfully created.", _newFileName, _spTitle);
      }
      catch (Exception _ex)
      {
        m_AfterCreationLogToHistoryList_HistoryOutcome1 = "Exception";
        string _frmt = "Creation of the Seal Protocol failed in the \"{0}\" state because of the error {1}";
        m_AfterCreationLogToHistoryList_HistoryDescription1 = string.Format(_frmt, _stt, _ex.Message);
      }
    }
    #endregion

    #region AfterCreationLog
    public String m_AfterCreationLogToHistoryList_HistoryDescription1 = default(System.String);
    public String m_AfterCreationLogToHistoryList_HistoryOutcome1 = default(System.String);
    public Int32 m_AfterCreationLogToHistoryList_UserId1 = default(System.Int32);
    #endregion
  }
}
