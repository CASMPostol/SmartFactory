using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Activities;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

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
      m_AfterCreationLogToHistoryList_UserId1 = workflowProperties.OriginatorUser.ID;
    }
    #endregion

    #region CreatePO
    private enum TeamMembers
    {
      _1stDriver,
      _2ndDriver,
      _1stEscort,
      _2ndEscort,
      DriverSPhone,
      EscortCarNo,
      EscortPhone,
      TrailerNo,
      TruckNo
    }
    private void m_CreatePO_ExecuteCode(object sender, EventArgs e)
    {
      string _spTitle = default(string);
      string _newFileName = default(string);
      string _stt = "Starting";
      try
      {
        _stt = "using";
        using (EntitiesDataContext _EDC = new EntitiesDataContext(workflowProperties.SiteUrl))
        {
          Shipping _sp = (from idx in _EDC.Shipping
                          where idx.Id == workflowProperties.ItemId
                          select idx).First();
          if (!_sp.IsOutbound.Value)
          {
            m_AfterCreationLogToHistoryList_HistoryDescription1 = "Document has not been created because it is not outbound shipment";
            return;
          }
          _stt = "Shipping";
          _spTitle = _sp.Title;
          SPDocumentLibrary _lib = (SPDocumentLibrary)workflowProperties.Web.Lists[CommonDefinition.SealProtocolLibraryTitle];
          _stt = "SPDocumentLibrary";
          SPFile _teml = workflowProperties.Web.GetFile(_lib.DocumentTemplateUrl);
          string _fname = String.Format("SealProtocolFileName".GetLocalizedString(), _sp.Id.ToString());
          SPFile _docFile = OpenXMLHelpers.AddDocument2Collection(_teml, _lib.RootFolder.Files, _fname);
          _newFileName = _docFile.Name;
          _stt = "_doc";
          int _docId = (int)_docFile.Item.ID;
          SealProtocol _sprt = (from idx in _EDC.SealProtocolLibrary
                                where idx.Id == _docId
                                select idx).First();
          Dictionary<TeamMembers, string> _team = GetTeamData(_EDC, _sp);
          _sprt.SealProtocol1stDriver = _team[TeamMembers._1stDriver];
          _sprt.SealProtocol2ndDriver = _team[TeamMembers._2ndDriver];
          _sprt.SealProtocol1stEscort = _team[TeamMembers._1stEscort];
          _sprt.SealProtocol2ndEscort = _team[TeamMembers._2ndEscort];
          _sprt.SealProtocolDispatchDateActual = _sp.StartTime;
          _sprt.SealProtocolCity = _sp.Shipping2City.Title();
          _sprt.SealProtocolContainersNo = _sp.ContainerNo.NotAvailable();
          _sprt.SealProtocolCountry = _sp.Shipping2City == null ? String.Empty : _sp.Shipping2City.CountryTitle.Title();
          _sprt.SealProtocolDispatchDate = _sp.StartTime;
          _sprt.SealProtocolDriverPhone = _team[TeamMembers.DriverSPhone];
          _sprt.SealProtocolEscortCarNo = _team[TeamMembers.EscortCarNo];
          _sprt.SealProtocolEscortPhone = _team[TeamMembers.EscortPhone];
          _sprt.SealProtocolForwarder = _sp.PartnerTitle.Title();
          _sprt.SealProtocolSecurityEscortProvider = _sp.Shipping2PartnerTitle.Title();
          _sp.SecuritySealProtocolIndex = _sprt;
          _sprt.SealProtocolTrailerNo = _team[TeamMembers.TrailerNo];
          _sprt.SealProtocolTruckNo = _team[TeamMembers.TruckNo];
          _sprt.Title = String.Format("Security Seal & Signature Protocol SSP-3{0, 5}", _sprt.Id);
          _sprt.SealProtocolWarehouse = _sp.Shipping2WarehouseTitle == null ? String.Empty.NotAvailable() : _sp.Shipping2WarehouseTitle.WarehouseAddress.NotAvailable();
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
    private Dictionary<TeamMembers, string> GetTeamData(EntitiesDataContext edc, Shipping _sp)
    {
      Dictionary<TeamMembers, string> _ret = new Dictionary<TeamMembers, string>();
      _ret.Add(TeamMembers._1stDriver, String.Empty.NotAvailable());
      _ret.Add(TeamMembers._1stEscort, String.Empty.NotAvailable());
      _ret.Add(TeamMembers._2ndDriver, String.Empty.NotAvailable());
      _ret.Add(TeamMembers._2ndEscort, String.Empty.NotAvailable());
      _ret.Add(TeamMembers.DriverSPhone, String.Empty.NotAvailable());
      _ret.Add(TeamMembers.EscortPhone, String.Empty.NotAvailable());
      _ret.Add(TeamMembers.EscortCarNo, _sp.Shipping2TruckTitle.Title());
      _ret.Add(TeamMembers.TrailerNo, _sp.TrailerTitle.Title());
      _ret.Add(TeamMembers.TruckNo, _sp.TruckTitle.Title());
      TeamMembers _cd = TeamMembers._1stDriver;
      TeamMembers _cse = TeamMembers._1stEscort;
      foreach (ShippingDriversTeam _std in _sp.ShippingDriversTeams(edc))
      {
        if (_std.DriverTitle == null)
          continue;
        if (_std.DriverTitle.Driver2PartnerTitle.ServiceType.Value != ServiceType.SecurityEscortProvider)
        {
          _ret[_cd] = _std.DriverTitle.Title();
          if (_cd == TeamMembers._1stDriver)
          {
            _cd = TeamMembers._2ndDriver;
            _ret[TeamMembers.DriverSPhone] = _std.DriverTitle != null ? _std.DriverTitle.CellPhone.NotAvailable() : String.Empty.NotAvailable();
          }
        }
        else if (_std.DriverTitle.Driver2PartnerTitle.ServiceType.Value == ServiceType.SecurityEscortProvider)
        {
          _ret[_cse] = _std.DriverTitle.Title();
          if (_cse == TeamMembers._1stEscort)
          {
            _cse = TeamMembers._2ndEscort;
            _ret[TeamMembers.EscortPhone] = _std.DriverTitle != null ? _std.DriverTitle.CellPhone.NotAvailable() : String.Empty.NotAvailable();
          }
        }
        else
          new ApplicationException(String.Format("Wrong ServiceType = {0}", _std.DriverTitle.Driver2PartnerTitle.ServiceType.Value));
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
