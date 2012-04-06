﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Workflow.Activities;
using CAS.SmartFactory.Shepherd.SendNotification.Entities;
using CAS.SmartFactory.Shepherd.SendNotification.WorkflowData;
using Microsoft.SharePoint.Workflow;

namespace CAS.SmartFactory.Shepherd.SendNotification.AddTimeSlots
{
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
        StartLogToHistory_HistoryDescription = String.Format("Starting applayin the template. From: {0}, Weeks: {1}", _data.StartDate, _data.Duration); ;
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
        using (EntitiesDataContext _EDC = new EntitiesDataContext(workflowProperties.Site.Url))
        {
          TimeSlotsInitiationData _data = TimeSlotsInitiationData.Deserialize(workflowProperties.InitiationData);
          StartLogToHistory_HistoryDescription = String.Format("Starting applayin the template. From: {0}, Weeks: {1}", _data.StartDate, _data.Duration);
          ScheduleTemplateScheduleTemplate _tmpl = Element.GetAtIndex<ScheduleTemplateScheduleTemplate>(_EDC.ScheduleTemplate, workflowProperties.ItemId);
          if (_tmpl.ShippingPoint == null)
            throw new ApplicationException("Template does not have Shipingpoint assigned");
          var _src = from _tst in _tmpl.TimeSlotsTemplate
                     group _tst by _tst.Day.Value;
          Dictionary<Day, IGrouping<Day, TimeSlotsTemplate>> _dys = _src.ToDictionary(x => x.Key);
          Dictionary<DayOfWeek, Day> _tt = new Dictionary<DayOfWeek, Day>() { { DayOfWeek.Friday, Day.Friday}, 
                                                                              { DayOfWeek.Monday, Day.Monday },
                                                                              { DayOfWeek.Saturday, Day.Saturday},
                                                                              { DayOfWeek.Sunday, Day.Sunday},
                                                                              { DayOfWeek.Thursday, Day.Thursday},
                                                                              { DayOfWeek.Tuesday, Day.Tuesday},
                                                                              { DayOfWeek.Wednesday, Day.Wednesday},
                                                                            };
          for (int _week = 0; _week < _data.Duration; _week++)
          {
            for (int _dayIdx = 0; _dayIdx < 7; _dayIdx++)
            {
              Day _dayOfWeek = _tt[_data.StartDate.DayOfWeek];
              if (_dys.ContainsKey(_dayOfWeek))
              {
                foreach (var _item in _dys[_dayOfWeek])
                {
                  DateTime _strt = CreateDateTime(_data, _item.StartHour.Value.Hour2Int(), _item.StartMinute.Value.Minute2Int());
                  DateTime _end = CreateDateTime(_data, _item.EndHour.Value.Hour2Int(), _item.StartMinute.Value.Minute2Int());
                  TimeSlotTimeSlot _nts = new TimeSlotTimeSlot()
                  {
                    StartTime = _strt,
                    EndTime = _end,
                    ShippingPoint = _tmpl.ShippingPoint,
                    EntryTime = _strt,
                    ExitTime = _end,
                    Occupied = Occupied.Free,
                    IsDouble = false,
                    ActualTimeSpan = (_end - _strt).TotalMinutes
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
        string _frmt = "Worflow aborted in AddTimeslotsActivity because of the error: {0}";
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
