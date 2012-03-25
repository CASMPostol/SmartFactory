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

namespace CAS.SmartFactory.Shepherd.SendNotification.CreateSecurityPO1
{
  public sealed partial class CreateSecurityPO1 : SequentialWorkflowActivity
  {
    public CreateSecurityPO1()
    {
      InitializeComponent();
    }
    internal static Guid WorkflowId = new Guid("668b6423-a3a9-47eb-a8f4-5c11bf29474e");
    internal static WorkflowDescription WorkflowDescription { get { return new WorkflowDescription(WorkflowId, "New Security PO", "Create Security Escort Purchae Order"); } }
    public Guid workflowId = default(System.Guid);

    #region OnWorkflowActivated
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void m_OnWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
    {
      using (SPSite _st = workflowProperties.Site)
        _url = _st.Url;
      m_AfterCreateLogToHistoryList_UserId = workflowProperties.OriginatorUser.ID;
    }
    private string _url = default(string);
    #endregion

    #region CreatePO
    private void m_CreatePOMethod_Run(object sender, EventArgs e)
    {
      string _spTitle = default(string);
      string _newFileName = default(string);
      string _stt = "Starting";
      try
      {
        using (EntitiesDataContext _EDC = new EntitiesDataContext(_url))
        {
          _stt = "using";
          Shipping _sp = (from idx in _EDC.Shipping
                          where idx.Identyfikator == workflowProperties.ItemId
                          select idx).First();
          _stt = "Shipping";
          _spTitle = _sp.Tytuł;
          SPDocumentLibrary _lib = (SPDocumentLibrary)workflowProperties.Web.Lists[CommonDefinition.EscortPOLibraryTitle];
          _stt = "SPDocumentLibrary";
          SPFile _teml = workflowProperties.Web.GetFile(_lib.DocumentTemplateUrl);
          string _fname = String.Format("Seal Protocol No {0}.docx", _sp.Identyfikator.ToString());
          SPFile _docFile = OpenXMLHelpers.AddDocument2Collection(_teml, _lib.RootFolder.Files, _fname);
          _newFileName = _docFile.Name;
          _stt = "_doc";
          int _docId = (int)_docFile.Item.ID;
          EscortPO _epo = (from idx in _EDC.EscortPOLibrary
                           where idx.Identyfikator == _docId
                           select idx).First();
          _stt = "EscortPO";
          if (_sp.Route != null)
          {
            _epo.Commodity = _sp.Route.Commodity.Title();
            if (_sp.Route.FreightPayer != null)
            {
              _epo.PayerAddress = _sp.Route.FreightPayer != null ? _sp.Route.FreightPayer.Address : String.Empty;
              _epo.PayerCty = _sp.Route.FreightPayer.Miasto;
              _epo.PayerName = _sp.Route.FreightPayer.Tytuł;
              _epo.PayerNIP = _sp.Route.FreightPayer.NIPVATNo;
              _epo.PayerZipCode = _sp.Route.FreightPayer.NIPVATNo;
              _epo.SendInvoiceTo = _sp.Route.FreightPayer.SendInvoiceTo;
            }
          }
          _stt = "SendInvoiceTo";
          if (_sp.SecurityEscortProvider != null)
          {
            _epo.City = _sp.SecurityEscort.FreightPayer.Miasto;
            _epo.Country = _sp.SecurityEscort.EscortDestination;
            _epo.Currency = _sp.SecurityEscort.Currency.Title();
            _epo.EscortCosts = _sp.SecurityEscort.SecurityCost;
            _epo.FreightPO = _sp.SecurityEscort.SecurityEscortPO;
            _epo.SecurityEscortProvider = _sp.SecurityEscortProvider.Title();
          }
          _stt = "SecurityEscortProvider";
          _epo.DispatchDate = _sp.EndTime;
          _epo.EMail = _sp.VendorName == null ? "oferty@cas.eu" : _sp.VendorName.EMail;
          _epo.Tytuł = String.Format("SECURITY ESCORT PURCHASE ORDER EPO-2{0, 5}", _epo.Identyfikator);
          _epo.WarehouseAddress = _sp.Warehouse == null ? String.Empty : _sp.Warehouse.WarehouseAddress;
          _stt = "WarehouseAddress";
          _sp.EscortPO = _epo;
          _EDC.SubmitChanges();
        }
        _stt = "SubmitChanges";
        m_AfterCreateLogToHistoryList_HistoryOutcome = "Item Created";
        m_AfterCreateLogToHistoryList_HistoryDescription = String.Format("File {0} containing purchase order for shipping {1} successfully created.", _newFileName, _spTitle);
      }
      catch (Exception _ex)
      {
        m_AfterCreateLogToHistoryList_HistoryOutcome = "Exception";
        string _frmt = "Creation of the Escort PO failed in the \"{0}\" state because of the error {1}";
        m_AfterCreateLogToHistoryList_HistoryDescription = string.Format(_frmt, _stt, _ex.Message);
      }
    }

    #endregion
    #region AfterCreateLogToHistoryList
    public String m_AfterCreateLogToHistoryList_HistoryDescription = default(System.String);
    public String m_AfterCreateLogToHistoryList_HistoryOutcome = default(System.String);
    public Int32 m_AfterCreateLogToHistoryList_UserId = default(System.Int32);
    #endregion
  }
}
