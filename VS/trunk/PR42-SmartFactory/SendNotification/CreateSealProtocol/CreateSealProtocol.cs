using System;
using System.Linq;
using System.Workflow.Activities;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;
using System.Collections.Generic;

namespace CAS.SmartFactory.Shepherd.SendNotification.CreateSealProtocol
{
  public sealed partial class CreateSealProtocol : SequentialWorkflowActivity
  {
    #region public
    public CreateSealProtocol()
    {
      InitializeComponent();
    }
    internal static Guid WorkflowId = new Guid("dddc274f-d1ff-4613-834a-548546f527aa");
    internal static WorkflowDescription WorkflowDescription { get { return new WorkflowDescription(WorkflowId, "New Seal Protocol", "Create Seal Protocol"); } }
    public Guid workflowId = default(System.Guid);
    #endregion

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
    private enum TeamMembers
    {
      _1stDriver, _2ndDriver, _1stEscort, _2ndEscort, DriverSPhone, EscortCarNo, EscortPhone, TrailerNo, TruckNo
    }
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
          ShippingShipping _sp = (from idx in _EDC.Shipping
                                  where idx.Identyfikator == workflowProperties.ItemId
                                  select idx).First();
          if (!_sp.IsOutbound.Value)
          {
            m_AfterCreationLogToHistoryList_HistoryDescription1 = "Document has not been created because it is not outbound shipment";
            return;
          }
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
          SealProtocol _sprt = (from idx in _EDC.SealProtocolLibrary
                                where idx.Identyfikator == _docId
                                select idx).First();
          Dictionary<TeamMembers, string> _team = GetTeamData(_sp);
          _sprt._1stDriver = _team[TeamMembers._1stDriver];
          _sprt._2ndDriver = _team[TeamMembers._2ndDriver];
          _sprt._1stEscort = _team[TeamMembers._1stEscort];
          _sprt._2ndEscort = _team[TeamMembers._2ndEscort];
          _sprt.ActualDispatchDate = _sp.StartTime;
          _sprt.City = _sp.City.Title();
          _sprt.ConainersNo = _sp.ContainerNo.NotAvailable();
          _sprt.Country = _sp.City == null ? String.Empty : _sp.City.CountryName.Title();
          _sprt.LoadingDate = _sp.StartTime;
          _sprt.DriverSPhone = _team[TeamMembers.DriverSPhone];
          _sprt.EscortCarNo = _team[TeamMembers.EscortCarNo];
          _sprt.EscortPhone = _team[TeamMembers.EscortPhone];
          _sprt.Forwarder = _sp.VendorName.Title();
          _sprt.SecurityEscortProvider = _sp.SecurityEscortProvider.Title();
          _sp.SecuritySealProtocol = _sprt;
          _sprt.TrailerNo = _team[TeamMembers.TrailerNo];
          _sprt.TruckNo = _team[TeamMembers.TruckNo];
          _sprt.Tytuł = String.Format("Security Seal & Signature Protocol SSP-3{0, 5}", _sprt.Identyfikator);
          _sprt.Warehouse = _sp.Warehouse == null ? String.Empty.NotAvailable() : _sp.Warehouse.WarehouseAddress.NotAvailable();
          _stt = "WarehouseAddress ";
          _EDC.SubmitChanges();
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
    private Dictionary<TeamMembers, string> GetTeamData(ShippingShipping _sp)
    {
      Dictionary<TeamMembers, string> _ret = new Dictionary<TeamMembers, string>();
      _ret.Add(TeamMembers._1stDriver, String.Empty.NotAvailable());
      _ret.Add(TeamMembers._1stEscort, String.Empty.NotAvailable());
      _ret.Add(TeamMembers._2ndDriver, String.Empty.NotAvailable());
      _ret.Add(TeamMembers._2ndEscort, String.Empty.NotAvailable());
      _ret.Add(TeamMembers.DriverSPhone, String.Empty.NotAvailable());
      _ret.Add(TeamMembers.EscortPhone, String.Empty.NotAvailable());
      _ret.Add(TeamMembers.EscortCarNo, _sp.SecurityEscortCarRegistrationNumber.Title());
      _ret.Add(TeamMembers.TrailerNo, _sp.TrailerRegistrationNumber.Title());
      _ret.Add(TeamMembers.TruckNo, _sp.TruckCarRegistrationNumber.Title());
      TeamMembers _cd = TeamMembers._1stDriver;
      TeamMembers _cse = TeamMembers._1stEscort;
      foreach (ShippingDriversTeam _std in _sp.ShippingDriversTeam)
      {
        if (_std.Driver == null)
          continue;
        if (_std.Driver.VendorName.ServiceType.Value != ServiceType.SecurityEscortProvider)
        {
          _ret[_cd] = _std.Driver.Title();
          if (_cd == TeamMembers._1stDriver)
          {
            _cd = TeamMembers._2ndDriver;
            _ret[TeamMembers.DriverSPhone] = _std.Driver != null ? _std.Driver.NumerTelefonuKomórkowego.NotAvailable() : String.Empty.NotAvailable();
          }
        }
        else if (_std.Driver.VendorName.ServiceType.Value == ServiceType.SecurityEscortProvider)
        {
          _ret[_cse] = _std.Driver.Title();
          if (_cse == TeamMembers._1stEscort)
          {
            _cse = TeamMembers._2ndEscort;
            _ret[TeamMembers.EscortPhone] = _std.Driver != null ? _std.Driver.NumerTelefonuKomórkowego.NotAvailable() : String.Empty.NotAvailable();
          }
        }
        else
          new ApplicationException(String.Format("Wrong ServiceType = {0}", _std.Driver.VendorName.ServiceType.Value));
      }
      return _ret;
    }
    #endregion

    #region AfterCreationLog
    public String m_AfterCreationLogToHistoryList_HistoryDescription1 = default(System.String);
    public String m_AfterCreationLogToHistoryList_HistoryOutcome1 = default(System.String);
    public Int32 m_AfterCreationLogToHistoryList_UserId1 = default(System.Int32);
    #endregion
  }
}
