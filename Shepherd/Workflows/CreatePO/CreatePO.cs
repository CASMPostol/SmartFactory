using System;
using System.Linq;
using System.Workflow.Activities;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.SendNotification.CreatePO
{
  public sealed partial class CreatePO : SequentialWorkflowActivity
  {
    #region public
    public CreatePO()
    {
      InitializeComponent();
    }
    internal static Guid WorkflowId = new Guid("54732fdd-0178-406a-aae1-7fdfb11ed7e7");
    internal static WorkflowDescription WorkflowDescription { get { return new WorkflowDescription(WorkflowId, "New Freight PO", "Create Freight Purchae Order"); } }
    public Guid workflowId = default(System.Guid);
    #endregion

    #region WorkflowActivated
    public SPWorkflowActivationProperties m_WorkflowProperties = new SPWorkflowActivationProperties();
    private void m_OnWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
    {
      _url = m_WorkflowProperties.Site.Url;
      m_LogAfterCreateToHistoryList_UserId1 = m_WorkflowProperties.OriginatorUser.ID;
    }
    private string _url = default(string);
    #endregion

    #region CreatePOItem
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
          if (!_sp.IsOutbound.Value)
          {
            m_LogAfterCreateToHistoryList_HistoryDescription1 = "Document has not been created because it is not outbound shipment";
            return;
          }
          _stt = "Shipping";
          _spTitle = _sp.Tytuł;
          SPDocumentLibrary _lib = (SPDocumentLibrary)m_WorkflowProperties.Web.Lists[CommonDefinition.FreightPOLibraryTitle];
          _stt = "SPDocumentLibrary";
          SPFile _teml = m_WorkflowProperties.Web.GetFile(_lib.DocumentTemplateUrl);
          string _fname = String.Format("FREIGHTPOFileName".GetLocalizedString(), _sp.Identyfikator.ToString());
          SPFile _docFile = OpenXMLHelpers.AddDocument2Collection(_teml, _lib.RootFolder.Files, _fname);
          _newFileName = _docFile.Name;
          _stt = "_doc";
          int _docId = (int)_docFile.Item.ID;
          FreightPO _fpo = (from idx in _EDC.FreightPOLibrary
                            where idx.Identyfikator == _docId
                            select idx).First();
          _stt = "FreightPO";
          if (_sp.Shipping2RouteTitle != null)
          {
              Route _rt = _sp.Shipping2RouteTitle;
            _fpo.FreightPOTransportCosts = _rt.TransportCosts.GetValueOrDefault(0.0);
            _fpo.FreightPOTransportUnitType = _sp.Shipping2TransportUnitType.Title();
            _fpo.FreightPOCommodity = _rt.Route2Commodity.Title();
            _fpo.FreightPOCurrency = _rt.CurrencyTitle.Title();
            _fpo.FPOFreightPO = _rt.GoodsHandlingPO.NotAvailable();
            _stt = "FreightPO0";
            if (_rt.FreightPayerTitle != null)
            {
                _fpo.FreightPOPayerAddress = _rt.FreightPayerTitle != null ? _rt.FreightPayerTitle.CompanyAddress : String.Empty.NotAvailable();
                _fpo.FreightPOPayerNIP = _rt.FreightPayerTitle.NIP.NotAvailable();
                _fpo.FreightPOPayerName = _rt.FreightPayerTitle.PayerName.NotAvailable();
                _fpo.FreightPOSendInvoiceToMultiline = _rt.FreightPayerTitle.SendInvoiceToMultiline.NotAvailable();
                _fpo.FreightPOPayerZip = _rt.FreightPayerTitle.WorkZip.NotAvailable();
                _fpo.FreightPOPayerCity = _rt.FreightPayerTitle.WorkCity.NotAvailable();
            }
          }
          _stt = "Route";
          _fpo.FreightPOForwarder = _sp.PartnerTitle.Title();
          _fpo.FreightPOCity = _sp.Shipping2City.Title();
          _fpo.FreightPOCountry = _sp.Shipping2City == null ? String.Empty.NotAvailable() : _sp.Shipping2City.CountryTitle.Title();
          _fpo.FPODispatchDate = _sp.EndTime;
          _fpo.EmailAddress = _sp.PartnerTitle == null ? String.Empty.NotAvailable() : _sp.PartnerTitle.EmailAddress.NotAvailable();
          _fpo.FPOLoadingDate = _sp.StartTime;
          _fpo.Tytuł = String.Format("FREIGHT PURCHASE ORDER FPO-1{0, 5}", _fpo.Identyfikator.Value);
          _fpo.FPOWarehouseAddress = _sp.Shipping2WarehouseTitle == null ? String.Empty.NotAvailable() : _sp.Shipping2WarehouseTitle.WarehouseAddress.NotAvailable();
          _stt = "WarehouseAddress ";
          _sp.Shipping2FreightPOIndex = _fpo;
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
      }
    }
    #endregion

    #region LogAfterCreate
    public String m_LogAfterCreateToHistoryList_HistoryDescription1 = default(System.String);
    public String m_LogAfterCreateToHistoryList_HistoryOutcome1 = default(System.String);
    public Int32 m_LogAfterCreateToHistoryList_UserId1 = default(System.Int32);
    public String m_LogAfterCreateToHistoryList_OtherData1 = default(System.String);
    #endregion
  }
}
