using System;
using System.Linq;
using System.Workflow.Activities;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

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
          if (!_sp.IsOutbound.Value)
          {
            m_AfterCreateLogToHistoryList_HistoryDescription = "Document has not been created because it is not outbound shipment";
            return;
          }
          _stt = "Shipping";
          _spTitle = _sp.Tytuł;
          SPDocumentLibrary _lib = (SPDocumentLibrary)workflowProperties.Web.Lists[CommonDefinition.EscortPOLibraryTitle];
          _stt = "SPDocumentLibrary";
          SPFile _teml = workflowProperties.Web.GetFile(_lib.DocumentTemplateUrl);
          string _fname = String.Format("ESCORTPONFileName".GetLocalizedString(), _sp.Identyfikator.ToString());
          SPFile _docFile = OpenXMLHelpers.AddDocument2Collection(_teml, _lib.RootFolder.Files, _fname);
          _newFileName = _docFile.Name;
          _stt = "_doc";
          int _docId = (int)_docFile.Item.ID;
          EscortPO _epo = (from idx in _EDC.EscortPOLibrary
                           where idx.Identyfikator == _docId
                           select idx).First();
          _stt = "EscortPO";
          if (_sp.Shipping2RouteTitle != null)
          {
              _epo.SecurityPOCommodity = _sp.Shipping2RouteTitle.Route2Commodity.Title();
              if (_sp.Shipping2RouteTitle.FreightPayerTitle != null)
            {
                _epo.SecurityPOEscortPayerAddress = _sp.Shipping2RouteTitle.FreightPayerTitle != null ? _sp.Shipping2RouteTitle.FreightPayerTitle.CompanyAddress : String.Empty.NotAvailable();
                _epo.SecurityPOEscortPayerCity = _sp.Shipping2RouteTitle.FreightPayerTitle.WorkCity.NotAvailable();
                _epo.SecurityPOEscortPayerName = _sp.Shipping2RouteTitle.FreightPayerTitle.PayerName.NotAvailable();
                _epo.SecurityPOEscortPayerNIP = _sp.Shipping2RouteTitle.FreightPayerTitle.NIP.NotAvailable();
                _epo.SecurityPOEscortPayerZip = _sp.Shipping2RouteTitle.FreightPayerTitle.WorkZip.NotAvailable();
                _epo.SecurityPOSentInvoiceToMultiline = _sp.Shipping2RouteTitle.FreightPayerTitle.SendInvoiceToMultiline.NotAvailable();
            }
          }
          _stt = "SendInvoiceTo";
          if (_sp.Shipping2PartnerTitle != null)
          {
              _epo.SecurityPOEscortCurrency = _sp.SecurityEscortCatalogTitle.CurrencyTitle.Title();
              _epo.SecurityPOEscortCosts = _sp.SecurityEscortCatalogTitle.SecurityCost;
              _epo.SPOFreightPO = _sp.SecurityEscortCatalogTitle.SecurityEscrotPO;
              _epo.SecurityPOEscortProvider = _sp.Shipping2PartnerTitle.Title();
          }
          _stt = "SecurityEscortProvider";
          _epo.SecurityPOCity = _sp.Shipping2City.Title();
          _epo.SecurityPOCountry = _sp.Shipping2City == null ? String.Empty.NotAvailable() : _sp.Shipping2City.CountryTitle.Title();
          _epo.SPODispatchDate = _sp.StartTime;
          _epo.EmailAddress = _sp.Shipping2PartnerTitle == null ? String.Empty.NotAvailable() : _sp.Shipping2PartnerTitle.EmailAddress.NotAvailable();
          _epo.Tytuł = String.Format("SECURITY ESCORT PURCHASE ORDER EPO-2{0, 5}", _epo.Identyfikator);
          _epo.FPOWarehouseAddress = _sp.Shipping2WarehouseTitle == null ? String.Empty.NotAvailable() : _sp.Shipping2WarehouseTitle.WarehouseAddress.NotAvailable();
          _stt = "WarehouseAddress";
          _sp.Shipping2EscortPOIndex = _epo;
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
