using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using CAS.SmartFactory.Shepherd.DataModel.ImportDataModel;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.SendNotification.ImportDictionaries
{
  public sealed partial class ImportDictionaries : SequentialWorkflowActivity
  {

    #region Activation
    public ImportDictionaries()
    {
      InitializeComponent();
    }
    public Guid workflowId = default(System.Guid);
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    private void OnWorkflowActivated_Invoked(object sender, ExternalDataEventArgs e)
    { }
    #endregion

    #region ImportDictionary
    private void ImportDictionaryCodeActivity_ExecuteCode(object sender, EventArgs e)
    {
      using (Stream _stream = workflowProperties.Item.File.OpenBinaryStream())
      {
        PreliminaryDataRoute _cnfg = PreliminaryDataRoute.ImportDocument(_stream);
        _cnfg.ImportData(workflowProperties.WebUrl);
        FillUpTimeSlots = _cnfg.FillUpTimeSlots;
        FillUpTimeSlotTemplates = _cnfg.FillUpTimeSlotTemplates;
      }
    }
    public bool FillUpTimeSlots { get; set; }
    public bool FillUpTimeSlotTemplates { get; set; }
    #endregion

    #region TemplateCreation
    private void TemplateCreationCondition(object sender, ConditionalEventArgs e)
    {
      e.Result = FillUpTimeSlotTemplates;
    }
    private void CreateTemplates_ExecuteCode(object sender, EventArgs e)
    {
      using (EntitiesDataContext _EDC = new EntitiesDataContext(workflowProperties.SiteUrl))
        foreach (ShippingPoint _sp in from _ei in _EDC.ShippingPoint select _ei)
          CreateTimeSlotTemplates(_EDC, _sp);
    }
    private void CreateTimeSlotTemplates(EntitiesDataContext _EDC, ShippingPoint _sp)
    {
      Dictionary<TimeSlotsTemplateStartHour, TimeSlotsTemplateEndHour> _hours = new Dictionary<TimeSlotsTemplateStartHour, TimeSlotsTemplateEndHour>()
      {
        {TimeSlotsTemplateStartHour._0, TimeSlotsTemplateEndHour._1}, {TimeSlotsTemplateStartHour._1, TimeSlotsTemplateEndHour._2}, {TimeSlotsTemplateStartHour._2, TimeSlotsTemplateEndHour._3}, {TimeSlotsTemplateStartHour._3, TimeSlotsTemplateEndHour._4}, {TimeSlotsTemplateStartHour._4, TimeSlotsTemplateEndHour._5},
        {TimeSlotsTemplateStartHour._5, TimeSlotsTemplateEndHour._6}, {TimeSlotsTemplateStartHour._6, TimeSlotsTemplateEndHour._7}, {TimeSlotsTemplateStartHour._7, TimeSlotsTemplateEndHour._8}, {TimeSlotsTemplateStartHour._8, TimeSlotsTemplateEndHour._9}, {TimeSlotsTemplateStartHour._9, TimeSlotsTemplateEndHour._10},
        {TimeSlotsTemplateStartHour._10, TimeSlotsTemplateEndHour._11}, {TimeSlotsTemplateStartHour._11, TimeSlotsTemplateEndHour._12}, {TimeSlotsTemplateStartHour._12, TimeSlotsTemplateEndHour._13}, {TimeSlotsTemplateStartHour._13, TimeSlotsTemplateEndHour._14}, {TimeSlotsTemplateStartHour._14, TimeSlotsTemplateEndHour._15},
        {TimeSlotsTemplateStartHour._15, TimeSlotsTemplateEndHour._16}, {TimeSlotsTemplateStartHour._16, TimeSlotsTemplateEndHour._17}, {TimeSlotsTemplateStartHour._17, TimeSlotsTemplateEndHour._18}, {TimeSlotsTemplateStartHour._18, TimeSlotsTemplateEndHour._19}, {TimeSlotsTemplateStartHour._19, TimeSlotsTemplateEndHour._20},
        {TimeSlotsTemplateStartHour._20, TimeSlotsTemplateEndHour._21}, {TimeSlotsTemplateStartHour._21, TimeSlotsTemplateEndHour._22}, {TimeSlotsTemplateStartHour._22, TimeSlotsTemplateEndHour._23}, {TimeSlotsTemplateStartHour._23, TimeSlotsTemplateEndHour._0}
      };
      ScheduleTemplate _schedule = new ScheduleTemplate() { ShippingPointLookupTitle = _sp, Tytuł = _sp.Title() + " All day schedule." };
      _EDC.ScheduleTemplate.InsertOnSubmit(_schedule);
      _EDC.SubmitChanges();
      List<TimeSlotsTemplateDay > _days = new List<TimeSlotsTemplateDay>() { { TimeSlotsTemplateDay.Friday }, { TimeSlotsTemplateDay.Monday }, { TimeSlotsTemplateDay.Saturday }, { TimeSlotsTemplateDay.Sunday }, { TimeSlotsTemplateDay.Thursday }, { TimeSlotsTemplateDay.Tuesday }, { TimeSlotsTemplateDay.Wednesday } }; 
      foreach (TimeSlotsTemplateDay _day in _days)
      {
        List<TimeSlotsTemplate> _ts = new List<TimeSlotsTemplate>();
        foreach (TimeSlotsTemplateStartHour _sh in _hours.Keys)
            _ts.Add(new TimeSlotsTemplate() { TimeSlotsTemplateDay = _day, TimeSlotsTemplateEndHour = _hours[_sh], TimeSlotsTemplateEndMinute = TimeSlotsTemplateEndMinute._0, TimeSlotsTemplateStartHour = _sh, TimeSlotsTemplateStartMinute = TimeSlotsTemplateStartMinute._0, ScheduleTemplateTitle = _schedule });
        _EDC.TimeSlotsTemplate.InsertAllOnSubmit(_ts);
        _EDC.SubmitChanges();
      }
    }
    #endregion

    #region FaultHandlerLog
    public String FaultHandlerLog_HistoryDescription = default(System.String);
    private void FaultHandlerLog_MethodInvoking(object sender, EventArgs e)
    {
      FaultHandlerLog_HistoryDescription = "Operation have been interrupted by the error: " + FaultHandlerActivity.Fault.Message;
    }
    #endregion
  }
}
