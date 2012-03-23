using System;
using System.IO;
using System.Linq;
using System.Workflow.Activities;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.SendNotification.CreatePO
{
  public sealed partial class CreatePO : SequentialWorkflowActivity
  {
    public CreatePO()
    {
      InitializeComponent();
    }

    #region WorkflowActivated
    internal static Guid WorkflowId = new Guid("54732fdd-0178-406a-aae1-7fdfb11ed7e7");
    internal static WorkflowDescription WorkflowDescription { get { return new WorkflowDescription(WorkflowId, "New Freight PO", "Create Freight Purchae Order"); } }
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties m_WorkflowProperties = new SPWorkflowActivationProperties();
    private void m_OnWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
    {
      using (SPSite _st = m_WorkflowProperties.Site)
      {
        _url = _st.Url;
      }
      m_LogAfterCreateToHistoryList_UserId1 = m_WorkflowProperties.OriginatorUser.ID;
      m_LogAfterCreateToHistoryList_HistoryOutcome1 = "Activation";
      m_LogAfterCreateToHistoryList_HistoryDescription1 = "OnWorkflowActivated finished";
    }
    #endregion
    private string _url = default(string);
    private void CreatePOItem(object sender, EventArgs e)
    {
      string _spTitle = default(string);
      string _newFileName = default(string);
      string _stt = default(string);
      try
      {
        using (EntitiesDataContext _EDC = new EntitiesDataContext(_url))
        {
          _stt = "using";
          Shipping _sp = (from idx in _EDC.Shipping
                          where idx.Identyfikator == m_WorkflowProperties.ItemId
                          select idx).First();
          _stt = "Shipping";
          _spTitle = _sp.Tytuł;
          SPDocumentLibrary _lib = (SPDocumentLibrary)m_WorkflowProperties.Web.Lists[CommonDefinition.FreightPOLibraryTitle];
          _stt = "SPDocumentLibrary";
          SPFile _teml = m_WorkflowProperties.Web.GetFile(_lib.DocumentTemplateUrl);
          string _fname = String.Format("FREIGHT PO No {0}.docx", _sp.Identyfikator.ToString());
          SPFile _docFile = OpenXMLHelpers.AddDocument2Collection(_teml, _lib.RootFolder.Files, _fname);
          _newFileName = _docFile.Name;
          _stt = "_doc";
          int _docId = (int)_docFile.Item.ID;
          FreightPO _fpo = (from idx in _EDC.FreightPOLibrary
                            where idx.Identyfikator == _docId
                            select idx).First();
          if (_sp.Route != null)
          {
            Route _rt = _sp.Route;
            if (_rt.FreightPayer != null)
            {
              _fpo.PayerAddress = _rt.FreightPayer != null ? _rt.FreightPayer.Address : String.Empty;
              _fpo.PayerNIP = _rt.FreightPayer.NIPVATNo;
              _fpo.PayerName = _rt.FreightPayer.Tytuł;
              _fpo.SendInvoiceTo = _rt.FreightPayer.SendInvoiceTo;
              _fpo.PayerZipCode = _rt.FreightPayer.KodPocztowy;
              _fpo.PayerCity = _rt.FreightPayer.Miasto;
            }
            _fpo.TransportCosts = _sp.Route.TransportCosts.GetValueOrDefault(0.0);
            _fpo.TransportUnit = _sp.TransportUnit.Title();

            _fpo.Commodity = _rt.Commodity.Title();
            _fpo.Country = _rt.CityName == null ? String.Empty : _rt.CityName.CountryName.Title();
            _fpo.Currency = _rt.Currency.Title();
            _fpo.FreightPO0 = _rt.FreightPO;
          }
          _stt = "Route";
          _stt = "_fpo";
          _fpo.Forwarder = _sp.VendorName.Tytuł;
          _fpo.City = _sp.City.Title();
          _fpo.DispatchDate = _sp.EndTime;
          _fpo.EMail = _sp.VendorName == null ? "oferty@cas.eu" : _sp.VendorName.EMail;
          _fpo.LoadingDate = _sp.StartTime;
          _fpo.Tytuł = "FREIGHT PURCHASE ORDER # {0}";
          _fpo.WarehouseAddress = _sp.Warehouse;
          _stt = "FreightPO";
          _fpo.Tytuł = String.Format(_fpo.Tytuł, _fpo.Identyfikator);
          _stt = "_fpo.Tytuł";
          _EDC.SubmitChanges();
        };
        _stt = "SubmitChanges";
        m_LogAfterCreateToHistoryList_HistoryOutcome1 = "Item Created";
        m_LogAfterCreateToHistoryList_HistoryDescription1 = String.Format("File {0} containing purchase order for shipping {1} successfully created.", _newFileName, _spTitle);
      }
      catch (Exception _ex)
      {
        m_LogAfterCreateToHistoryList_HistoryOutcome1 = "Exception";
        string _frmt = "Creation of the PO failed in the \"{0}\" state because of the error {1}";
        m_LogAfterCreateToHistoryList_HistoryDescription1 = string.Format(_frmt, _stt, _ex.Message);
        m_LogAfterCreateToHistoryList_OtherData1 = _spTitle;
      }
    }
    public String m_LogAfterCreateToHistoryList_HistoryDescription1 = default(System.String);
    public String m_LogAfterCreateToHistoryList_HistoryOutcome1 = default(System.String);
    public String m_LogAfterCreateToHistoryList_OtherData1 = default(System.String);
    public Int32 m_LogAfterCreateToHistoryList_UserId1 = default(System.Int32);
  }
}
