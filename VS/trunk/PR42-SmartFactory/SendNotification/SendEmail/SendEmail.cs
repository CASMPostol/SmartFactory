using System;
using System.IO;
using System.Linq;
using System.Workflow.Activities;
using System.Xml;
using System.Xml.Serialization;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using CAS.SmartFactory.Shepherd.SendNotification.WorkflowData;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.SendNotification.SendEmail
{
  public sealed partial class SendEmail : SequentialWorkflowActivity
  {
    public SendEmail()
    {
      InitializeComponent();
    }
    #region Activation
    private FreightPurchaseOrderTemplate _emailBodyObject = new FreightPurchaseOrderTemplate();
    private void m_onWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
    {
      POLibraryWorkflowAssociationData _activationData = default(POLibraryWorkflowAssociationData);
      try
      {
        // deserialize initiation data; 
        XmlSerializer serializer = new XmlSerializer(typeof(POLibraryWorkflowAssociationData));
        XmlTextReader reader = new XmlTextReader(new StringReader(m_WorkflowProperties.InitiationData));
        _activationData = (POLibraryWorkflowAssociationData)serializer.Deserialize(reader);
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in WorkflowActivated because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
      _emailBodyObject = CreateEmailMessage(_activationData);
    }
    private FreightPurchaseOrderTemplate CreateEmailMessage(POLibraryWorkflowAssociationData _activationData)
    {
      try
      {
        m_sendEmail1_CC = m_WorkflowProperties.OriginatorEmail;
        m_sendEmail1_From = m_WorkflowProperties.OriginatorEmail;
        m_sendEmail1_Subject = _activationData.Title;
        using (SPSite _st = m_WorkflowProperties.Site)
        {
          using (EntitiesDataContext _EDC = new EntitiesDataContext(_st.Url))
          {
            FreightPO _fpo = (from idx in _EDC.FreightPOLibrary
                              where idx.Identyfikator == m_WorkflowProperties.ItemId
                              select idx).First();
            m_sendEmail1_To = String.IsNullOrEmpty(_fpo.EMail) ? CommonDefinition.PartnerSentToBackupEmail : _fpo.EMail;
            return new FreightPurchaseOrderTemplate()
            {
              Encodedabsurl = new Uri((string)m_WorkflowProperties.Item["EncodedAbsUrl"]),
              Modified = (DateTime)m_WorkflowProperties.Item["Modified"],
              ModifiedBy = m_WorkflowProperties.OriginatorUser.Name,
              DocumentName = m_WorkflowProperties.Item.File.Name,
              FPO2CityTitle = _fpo.City,
              FPO2CommodityTitle = _fpo.Commodity,
              FPO2CountryTitle = _fpo.Country,
              FPO2RouteGoodsHandlingPO = _fpo.FreightPO0,
              FPO2TransportUnitTypeTitle = _fpo.TransportUnit,
              FPOLoadingDate = _fpo.LoadingDate.GetValueOrDefault(DateTime.MaxValue),
              FPO2WarehouseAddress = _fpo.WarehouseAddress,
            };
          }
        }
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in CreateEmailMessage because of error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    internal static Guid WorkflowId = new Guid("1bb10ba9-70da-4064-95b3-cd048ce4c3cd");
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties m_WorkflowProperties = default(SPWorkflowActivationProperties);
    #endregion

    #region SentEmail
    private void m_sendEmail1_MethodInvoking(object sender, EventArgs e)
    {
      try
      {
        m_sendEmail1_Body = _emailBodyObject.TransformText();
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in TransformText because of the error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    public String m_sendEmail1_Body = default(System.String);
    public String m_sendEmail1_CC = default(System.String);
    public String m_sendEmail1_From = default(System.String);
    public String m_sendEmail1_Subject = default(System.String);
    public String m_sendEmail1_To = default(System.String);
    #endregion

    #region LogToHistory
    private void m_logToHistoryListActivity1_MethodInvoking(object sender, EventArgs e)
    {
      m_logToHistoryListActivity1_HistoryDescription = String.Format("Sending notification to: {0}", m_WorkflowProperties.Item["Carrier"]);
      m_logToHistoryListActivity1_HistoryOutcome = m_WorkflowProperties.Workflow.ParentAssociation.Name;
      m_logToHistoryListActivity1_UserId = m_WorkflowProperties.OriginatorUser.ID;
    }
    public Int32 m_logToHistoryListActivity1_UserId = default(System.Int32);
    public String m_logToHistoryListActivity1_HistoryDescription = default(System.String);
    public String m_logToHistoryListActivity1_HistoryOutcome = default(System.String);
    public String m_logToHistoryListActivity1_OtherData = default(System.String);
    #endregion

    #region OnFaultLog
    public String _OnFaultLogToHistoryListActivity_HistoryDescription1 = default(System.String);
    public String _OnFaultLogToHistoryListActivity_HistoryOutcome1 = default(System.String);
    private void _OnFaultLogToHistoryListActivity_MethodInvoking(object sender, EventArgs e)
    {
      _OnFaultLogToHistoryListActivity_HistoryOutcome1 = "Error";
      _OnFaultLogToHistoryListActivity_HistoryDescription1 = m_FaultHandlerActivity.Fault.Message;
    }
    #endregion
  }
}
