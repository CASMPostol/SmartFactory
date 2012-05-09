using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Workflow.Activities;
using System.Workflow.ComponentModel;
using CAS.SmartFactory.Shepherd.Entities;
using CAS.SmartFactory.Shepherd.ImportDataModel;
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
      using (EntitiesDataContext _EDC = new EntitiesDataContext(workflowProperties.WebUrl))
        foreach (ShippingPoint _sp in from _ei in _EDC.ShippingPoint select _ei)
          CreateTimeSlotTemplates(_EDC, _sp);
    }
    private void CreateTimeSlotTemplates(EntitiesDataContext _EDC, ShippingPoint _sp)
    {
      Dictionary<StartHour, EndHour> _hours = new Dictionary<StartHour, EndHour>()
      {
        {StartHour._0, EndHour._1}, {StartHour._1, EndHour._2}, {StartHour._2, EndHour._3}, {StartHour._3, EndHour._4}, {StartHour._4, EndHour._5},
        {StartHour._5, EndHour._6}, {StartHour._6, EndHour._7}, {StartHour._7, EndHour._8}, {StartHour._8, EndHour._9}, {StartHour._9, EndHour._10},
        {StartHour._10, EndHour._11}, {StartHour._11, EndHour._12}, {StartHour._12, EndHour._13}, {StartHour._13, EndHour._14}, {StartHour._14, EndHour._15},
        {StartHour._15, EndHour._16}, {StartHour._16, EndHour._17}, {StartHour._17, EndHour._18}, {StartHour._18, EndHour._19}, {StartHour._19, EndHour._20},
        {StartHour._20, EndHour._21}, {StartHour._21, EndHour._22}, {StartHour._22, EndHour._23}, {StartHour._23, EndHour._0}
      };
      ScheduleTemplate _schedule = new ScheduleTemplate() { ShippingPointLookupTitle = _sp, Tytuł = _sp.Title() + " All day schedule." };
      _EDC.ScheduleTemplate.InsertOnSubmit(_schedule);
      _EDC.SubmitChanges();
      List<Day> _days = new List<Day>() { { Day.Friday }, { Day.Monday }, { Day.Saturday }, { Day.Sunday }, { Day.Thursday }, { Day.Tuesday }, { Day.Wednesday } }; 
      foreach (Day _day in _days)
      {
        List<TimeSlotsTemplate> _ts = new List<TimeSlotsTemplate>();
        foreach (StartHour _sh in _hours.Keys)
            _ts.Add(new TimeSlotsTemplate() { TimeSlotsTemplateDay = _day, TimeSlotsTemplateEndHour = _hours[_sh], TimeSlotsTemplateEndMinute = EndMinute._0, TimeSlotsTemplateStartHour = _sh, TimeSlotsTemplateStartMinute = StartMinute._0, ScheduleTemplateTitle = _schedule });
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
