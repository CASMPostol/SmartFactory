using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.TimeSlotWebPart
{
  public partial class TimeSlotWebPartUserControl : UserControl
  {
    #region public
    public TimeSlotWebPartUserControl() { }
    internal InterconnectionDataTable<TimeSlotTimeSlot> GetSelectedTimeSlotInterconnectionData()
    {
      return m_InterconnectionDataTable_TimeSlotTimeSlot;
    }
    internal GlobalDefinitions.Roles Role
    {
      set
      {
        switch (value)
        {
          case GlobalDefinitions.Roles.InboundOwner:
          case GlobalDefinitions.Roles.Vendor:
          case GlobalDefinitions.Roles.Operator:
          case GlobalDefinitions.Roles.Escort:
          case GlobalDefinitions.Roles.Guard:
          case GlobalDefinitions.Roles.Forwarder:
            m_DoubleTimeSlotsPanel.Visible = false;
            break;
          case GlobalDefinitions.Roles.OutboundOwner:
          case GlobalDefinitions.Roles.Coordinator:
          case GlobalDefinitions.Roles.Supervisor:
          case GlobalDefinitions.Roles.None:
            m_DoubleTimeSlotsPanel.Visible = true;
            break;
        }
      }
    }
    #endregion

    #region UserControl override
    protected override void OnInit(EventArgs e)
    {
      m_EDC = new EntitiesDataContext(SPContext.Current.Web.Url) { ObjectTrackingEnabled = false };
      base.OnInit(e);
    }
    protected override void OnUnload(EventArgs e)
    {
      m_EDC.Dispose();
      base.OnUnload(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        m_WarehouseDropDownList.DataSource = from _idx in m_EDC.Warehouse
                                             orderby _idx.Tytuł ascending
                                             select _idx;
        m_WarehouseDropDownList.DataBind();
        m_WarehouseDropDownList.SelectedIndex = 0;
        m_Calendar.VisibleDate = DateTime.Today;
        m_Calendar.SelectedDate = DateTime.Today;
      }
      m_InterconnectionDataTable_TimeSlotTimeSlot = new InterconnectionDataTable<TimeSlotTimeSlot>(typeof(TimeSlot).Name);
      m_Calendar.DayRender += new DayRenderEventHandler(m_Calendar_DayRender);
      m_Calendar.SelectionChanged += new EventHandler(m_SelectionChanged);
      m_Calendar.VisibleMonthChanged += new MonthChangedEventHandler(m_Calendar_VisibleMonthChanged);
      m_TimeSlotList.SelectedIndexChanged += new EventHandler(m_TimeSlotList_SelectedIndexChanged);
    }
    protected override void OnPreRender(EventArgs e)
    {
      PreapareCalendar(Direction.Inbound);
      base.OnPreRender(e);
    }
    #endregion

    #region EventHandlers
    private void m_TimeSlotList_SelectedIndexChanged(object sender, EventArgs e)
    {
      m_TimeSlotSelection = true;
      if (m_TimeSlotList.SelectedValue.IsNullOrEmpty())
        return;
      try
      {
        TimeSlotTimeSlot _slctdTmslt = Element.GetAtIndex(m_EDC.TimeSlot, m_TimeSlotList.SelectedValue);
        _slctdTmslt.IsDouble = m_ShowDoubleTimeSlots.Checked;
        m_InterconnectionDataTable_TimeSlotTimeSlot.SetData
          (this, new InterconnectionDataTable<TimeSlotTimeSlot>.InterconnectionEventArgs(_slctdTmslt));
      }
      catch (Exception ex)
      {
        Entities.Anons.WriteEntry(m_EDC, "TimeSlotList_SelectedIndexChanged", ex.Message);
      }
    }
    private void m_Calendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e) { }
    private void m_SelectionChanged(object sender, EventArgs e) { }
    private void m_Calendar_DayRender(object sender, DayRenderEventArgs e)
    {
      if (e.Day.IsOtherMonth)
        e.Cell.BackColor = Color.LightGray; ;
      if (m_AvailableDays.ContainsKey(e.Day.Date))
      {
        e.Cell.BackColor = Color.SeaGreen;
        string _days = String.Format(GlobalDefinitions.NumberOfTimeSLotsFormat, m_AvailableDays[e.Day.Date].ToString());
        e.Cell.Controls.Add(new LiteralControl() { Text = _days });
      }
      else
        e.Day.IsSelectable = false;
    }
    #endregion

    #region private
    private void PreapareCalendar(Direction _direction)
    {
      try
      {
        m_TimeSlotList.Items.Clear();
        DateTime _sd = m_Calendar.SelectedDate.Date;
        DateTime _strt = new DateTime(m_Calendar.VisibleDate.Year, m_Calendar.VisibleDate.Month, 1);
        DateTime _end = _strt.AddMonths(1);
        Warehouse _warehouse = Element.GetAtIndex(m_EDC.Warehouse, m_WarehouseDropDownList.SelectedValue);
        foreach (var _spoint in (from _sp in _warehouse.ShippingPoint
                                 select _sp))
        {
          List<TimeSlot> _avlblTmslts = new List<TimeSlot>();
          if (_spoint.Direction != _direction && _spoint.Direction != Direction.BothDirections)
            continue;
          foreach (var _ts in (from _tsidx in _spoint.TimeSlot
                               where !_tsidx.Occupied.Value && _tsidx.StartTime >= _strt && _tsidx.StartTime < _end
                               orderby _tsidx.StartTime ascending
                               select _tsidx))
          {
            if (!m_AvailableDays.ContainsKey(_ts.StartTime.Value.Date))
              m_AvailableDays.Add(_ts.StartTime.Value.Date, 1);
            else
              m_AvailableDays[_ts.StartTime.Value.Date] += 1;
            if (_ts.StartTime.Value.Date != _sd)
              continue;
            _avlblTmslts.Add(_ts);
          }
          DisplayTimeSlots(_avlblTmslts);
        }
      }
      catch (Exception ex)
      {
        this.Controls.Add(new LiteralControl("Cannot display time slots because; " + ex.Message));
      }
    }
    private void DisplayTimeSlots(List<TimeSlot> _avlblTmslts)
    {
      if (m_TimeSlotSelection)
        return;
      string _dtFormat = "{0:HH:mm}";
      if (m_ShowDoubleTimeSlots.Checked)
      {
        TimeSpan _spn15min = new TimeSpan(0, 15, 0);
        for (int _i = 0; _i < _avlblTmslts.Count - 1; _i++)
        {
          if ((_avlblTmslts[_i + 1].StartTime.Value - _avlblTmslts[_i].EndTime.Value).Duration() > _spn15min)
            continue;
          ListItem _ni = new ListItem(String.Format(_dtFormat, _avlblTmslts[_i].StartTime.Value), _avlblTmslts[_i].Identyfikator.Value.ToString(), true);
          m_TimeSlotList.Items.Add(_ni);
        }
      }
      else
        foreach (TimeSlot _item in _avlblTmslts)
        {
          ListItem _ni = new ListItem(String.Format(_dtFormat, _item.StartTime.Value), _item.Identyfikator.Value.ToString(), true);
          m_TimeSlotList.Items.Add(_ni);
        }
    }
    private bool m_TimeSlotSelection = false;
    private EntitiesDataContext m_EDC = null;
    private SortedDictionary<DateTime, int> m_AvailableDays = new SortedDictionary<DateTime, int>();
    private InterconnectionDataTable<TimeSlotTimeSlot> m_InterconnectionDataTable_TimeSlotTimeSlot;
    #endregion
  }
}
