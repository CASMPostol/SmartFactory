//<summary>
//  Title   : AddTimeSlots workflow
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Activities;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using CAS.SmartFactory.Shepherd.Workflows.WorkflowData;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.Workflows.AddTimeSlots
{
  /// <summary>
  /// AddTimeSlots workflow
  /// </summary>
  public sealed partial class AddTimeSlots : SequentialWorkflowActivity
  {
    public AddTimeSlots()
    {
      InitializeComponent();
    }
    public Guid workflowId = default(System.Guid);

    #region StartLog
    public SPWorkflowActivationProperties workflowProperties = new SPWorkflowActivationProperties();
    public String StartLogToHistory_HistoryDescription = default(System.String);
    private void StartLogToHistory_MethodInvoking(object sender, EventArgs e)
    {
      try
      {
        TimeSlotsInitiationData _data = TimeSlotsInitiationData.Deserialize(workflowProperties.InitiationData);
        StartLogToHistory_HistoryDescription = String.Format("Starting applying the template. From: {0}, Weeks: {1}", _data.StartDate, _data.Duration); ;
      }
      catch (Exception ex)
      {
        string _frmt = "Worflow aborted in StartLogToHistory because of the error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    #endregion

    #region AddTimeslots
    private void AddTimeslotsActivity_ExecuteCode(object sender, EventArgs e)
    {
      try
      {
        using (EntitiesDataContext _EDC = new EntitiesDataContext(workflowProperties.SiteUrl))
        {
          TimeSlotTimeSlot.DeleteExpired( _EDC );
          TimeSlotsInitiationData _data = TimeSlotsInitiationData.Deserialize(workflowProperties.InitiationData);
          ScheduleTemplate _tmpl = Element.GetAtIndex<ScheduleTemplate>(_EDC.ScheduleTemplate, workflowProperties.ItemId.ToString());
          if (_tmpl.ShippingPointLookupTitle == null)
            throw new ApplicationException("Template does not have Shipingpoint assigned");
          var _src = from _tst in _tmpl.TimeSlotsTemplate
                     group _tst by _tst.TimeSlotsTemplateDay.Value;
          Dictionary<TimeSlotsTemplateDay, IGrouping<TimeSlotsTemplateDay, TimeSlotsTemplate>> _dys = _src.ToDictionary(x => x.Key);
          Dictionary<DayOfWeek, TimeSlotsTemplateDay> _tt = new Dictionary<DayOfWeek, TimeSlotsTemplateDay>() { { DayOfWeek.Friday, TimeSlotsTemplateDay.Friday}, 
                                                                              { DayOfWeek.Monday, TimeSlotsTemplateDay.Monday },
                                                                              { DayOfWeek.Saturday, TimeSlotsTemplateDay.Saturday},
                                                                              { DayOfWeek.Sunday, TimeSlotsTemplateDay.Sunday},
                                                                              { DayOfWeek.Thursday, TimeSlotsTemplateDay.Thursday},
                                                                              { DayOfWeek.Tuesday, TimeSlotsTemplateDay.Tuesday},
                                                                              { DayOfWeek.Wednesday, TimeSlotsTemplateDay.Wednesday},
                                                                            };
          for (int _week = 0; _week < _data.Duration; _week++)
          {
            for (int _dayIdx = 0; _dayIdx < 7; _dayIdx++)
            {
              TimeSlotsTemplateDay _dayOfWeek = _tt[_data.StartDate.DayOfWeek];
              if (_dys.ContainsKey(_dayOfWeek))
              {
                foreach (var _item in _dys[_dayOfWeek])
                {
                  DateTime _strt = CreateDateTime(_data, _item.TimeSlotsTemplateStartHour.Value.Hour2Int(), _item.TimeSlotsTemplateStartMinute.Value.Minute2Int());
                  DateTime _end = CreateDateTime(_data, _item.TimeSlotsTemplateEndHour.Value.Hour2Int(), _item.TimeSlotsTemplateStartMinute.Value.Minute2Int());
                  TimeSlotTimeSlot _nts = new TimeSlotTimeSlot()
                  {
                    StartTime = _strt,
                    EndTime = _end,
                    TimeSlot2ShippingPointLookup = _tmpl.ShippingPointLookupTitle,
                    EntryTime = _strt,
                    ExitTime = _end,
                    Occupied = Occupied.Free,
                    IsDouble = false,
                    TimeSpan = (_end - _strt).TotalMinutes
                  };
                  _EDC.TimeSlot.InsertOnSubmit(_nts);
                  m_TimeSlotsCounter++;
                }
              } //!_tt.ContainsKey(_
              _data.StartDate += TimeSpan.FromDays(1);
            }
            _EDC.SubmitChanges();
          }
        }
      }
      catch (Exception ex)
      {
        string _frmt = "Workflow aborted in AddTimeslotsActivity because of the error: {0}";
        throw new ApplicationException(String.Format(_frmt, ex.Message));
      }
    }
    private int m_TimeSlotsCounter = 0;
    private static DateTime CreateDateTime(TimeSlotsInitiationData _data, int _hour, int _minute)
    {
      return new DateTime(_data.StartDate.Year, _data.StartDate.Month, _data.StartDate.Day, _hour, _minute, 0);
    }
    #endregion

    #region FinischLog
    public String FinischLogToHistory_HistoryDescription2 = default(System.String);
    private void FinischLogToHistory_MethodInvoking(object sender, EventArgs e)
    {
      FinischLogToHistory_HistoryDescription2 = String.Format("The template has been applied and {0} timeslots created", m_TimeSlotsCounter);
    }
    #endregion

    #region FaultHandlerLog
    public String FaultHandlerLogToHistory_HistoryDescription = default(System.String);
    private void FaultHandlerLogToHistory_MethodInvoking(object sender, EventArgs e)
    {
      FaultHandlerLogToHistory_HistoryDescription = FaultHandler.Fault.Message;
    }
    #endregion
  }
}
