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
      m_LogAfterCreateToHistoryList_OtherData1 = default(string);
    }
    #endregion
    private string _url = default(string);
    private void CreatePOItem(object sender, EventArgs e)
    {
      string _spTitle = default(string);
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
          byte[] _buff = null;
          using (Stream _tmpStrm = _teml.OpenBinaryStream())
          {
            _buff = new byte[_tmpStrm.Length + 200000]; //must be expandable
            _tmpStrm.Read(_buff, 0, (int)_tmpStrm.Length);
          }
          SPFile _docFile = null;
          _stt = "_buff";
          using (MemoryStream _docStrm = new MemoryStream(_buff))
          {
            WordprocessingDocument _doc = WordprocessingDocument.Open(_docStrm, true);
            _stt = "Open";
            _doc.ChangeDocumentType(WordprocessingDocumentType.Document);
            _doc.Close();
            _docFile = _lib.RootFolder.Files.Add(String.Format("FREIGHT PO No {0}.docx", _sp.Identyfikator.ToString()), _docStrm, true);
            _docStrm.Flush();
            _docStrm.Close();
          }
          _stt = "_doc";
          int _docId = (int)_docFile.Item.ID;
          FreightPayer _FreightPayer = null;
          CommodityCommodity _Commodity = null;
          CountryClass _Country = null;
          Currency _Currency = null;
          string _FreightPO0 = String.Empty;
          if (_sp.Route != null)
          {
            Route _rt = _sp.Route;
            _FreightPayer = _rt.FreightPayer;
            _Commodity = _rt.Commodity;
            _Country = _rt.CityName == null ? null : _rt.CityName.CountryName;
            _Currency = _rt.Currency;
            _FreightPO0 = _rt.FreightPO;
          }
          _stt = "Route";
          FreightPO _fpo = (from idx in _EDC.FreightPOLibrary
                            where idx.Identyfikator == _docId
                            select idx).First();
          _stt = "_fpo";
          _fpo.Carrier = _sp.VendorName;
          _fpo.City = _sp.City;
          _fpo.Commodity = _Commodity;
          _fpo.CompanyAddress = _FreightPayer;
          _fpo.Country = _Country;
          _fpo.Currency = _Currency;
          _fpo.DispatchDate = _sp.EndTime;
          _fpo.EMail = _sp.VendorName == null ? "oferty@cas.eu" : _sp.VendorName.EMail;
          _fpo.FreightPO0 = _FreightPO0;
          _fpo.LoadingDate = _sp.StartTime;
          _fpo.NIP = _FreightPayer;
          _fpo.PayerName = _FreightPayer;
          _fpo.SendInvoiceTo = _FreightPayer;
          _fpo.TransportCosts = _sp.Route;
          _fpo.TransportUnitType = _sp.TransportUnit;
          _fpo.Tytuł = "FREIGHT PURCHASE ORDER # {0}";
          _fpo.WarehouseAddress = _sp.Warehouse;
          _fpo.WorkZip = _FreightPayer;
          _fpo.WorkCity = _FreightPayer;
          _stt = "FreightPO";
          _fpo.Tytuł = String.Format(_fpo.Tytuł, _fpo.Identyfikator);
          _stt = "_fpo.Tytuł";
          _EDC.SubmitChanges();
        };
        _stt = "SubmitChanges";
        m_LogAfterCreateToHistoryList_HistoryOutcome1 = "Item Created";
        m_LogAfterCreateToHistoryList_HistoryDescription1 = "Shipping: " + _spTitle;
        m_LogAfterCreateToHistoryList_OtherData1 = default(string);
      }
      catch (Exception _ex)
      {
        m_LogAfterCreateToHistoryList_HistoryOutcome1 = "Exception";
        string _frmt = "Create PO failed in the \"{0}\" state because of the error {1}";
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
