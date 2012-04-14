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
using CAS.SmartFactory.Shepherd.SendNotification.ImportDictionaries.Schema;
using System.IO;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using System.Collections.Generic;

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
          CreateTemplates(_EDC, _sp);
    }
    private void CreateTemplates(EntitiesDataContext _EDC, ShippingPoint _sp)
    {
      Dictionary<StartHour, EndHour> _hours = new Dictionary<StartHour, EndHour>()
      {
        {StartHour._0, EndHour._1}, {StartHour._1, EndHour._2}, {StartHour._2, EndHour._3}, {StartHour._3, EndHour._4}, {StartHour._4, EndHour._5},
        {StartHour._5, EndHour._6}, {StartHour._6, EndHour._7}, {StartHour._7, EndHour._8}, {StartHour._8, EndHour._9}, {StartHour._9, EndHour._10},
        {StartHour._10, EndHour._11}, {StartHour._11, EndHour._12}, {StartHour._12, EndHour._13}, {StartHour._13, EndHour._14}, {StartHour._14, EndHour._15},
        {StartHour._15, EndHour._16}, {StartHour._16, EndHour._17}, {StartHour._17, EndHour._18}, {StartHour._18, EndHour._19}, {StartHour._19, EndHour._20},
        {StartHour._20, EndHour._21}, {StartHour._21, EndHour._22}, {StartHour._22, EndHour._23}, {StartHour._23, EndHour._0}
      };
      ScheduleTemplate _schedule = new ScheduleTemplate() { ShippingPoint = _sp, Tytuł = _sp.Title() + " All day schedule." };
      _EDC.ScheduleTemplate.InsertOnSubmit(_schedule);
      _EDC.SubmitChanges();
      foreach (Day _day in Enum.GetValues(typeof(Day)))
      {
        List<TimeSlotsTemplate> _ts = new List<TimeSlotsTemplate>();
        foreach (StartHour _sh in _hours.Keys)
          _ts.Add(new TimeSlotsTemplate() { Day = _day, EndHour = _hours[_sh], EndMinute = EndMinute._0, StartHour = _sh, StartMinute = StartMinute._0, ScheduleTemplateShepherd = _schedule });
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
