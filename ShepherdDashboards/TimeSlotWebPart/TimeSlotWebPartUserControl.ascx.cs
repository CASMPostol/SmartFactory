using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.TimeSlotWebPart
{
  public partial class TimeSlotWebPartUserControl : UserControl
  {
    #region public
    public TimeSlotWebPartUserControl()
    {
      SimpleTimeSlotList = true;
    }
    internal InterconnectionDataTable<TimeSlotTimeSlot> GetSelectedTimeSlotInterconnectionData()
    {
      string _tn = typeof(TimeSlotTimeSlot).GetType().Name;
      int _sts = -1;
      if (!String.IsNullOrEmpty(m_TimeSlotList.SelectedValue))
        _sts = int.Parse(m_TimeSlotList.SelectedValue);
      try
      {
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url) { ObjectTrackingEnabled = false })
          return new InterconnectionDataTable<TimeSlotTimeSlot>(TimeSlotTimeSlot.GetAtIndex(edc, _sts, true), typeof(TimeSlotTimeSlot).Name);
      }
      catch (Exception)
      {
        return new InterconnectionDataTable<TimeSlotTimeSlot>();
      }
    }
    #endregion

    #region private
    internal bool SimpleTimeSlotList
    {
      set
      {
        if (value)
          m_UpdateTimeSlotListMethod = UpdateTimeSlotListSimple;
        else
          m_UpdateTimeSlotListMethod = UpdateTimeSlotListExtended;
      }
    }
    protected override void OnInit(EventArgs e)
    {
      base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        m_Calendar.VisibleDate = DateTime.Today;
        m_Calendar.SelectedDate = DateTime.Today;
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url) { ObjectTrackingEnabled = false })
        {
          m_WarehouseDropDownList.DataSource = Warehouse.GatAll(edc);
          m_WarehouseDropDownList.DataBind();
          m_WarehouseDropDownList.SelectedIndex = 0;
          m_UpdateTimeSlotListMethod(edc, m_Calendar.SelectedDate, m_WarehouseDropDownList.SelectedValue);
        }
      }
      m_Calendar.DayRender += new DayRenderEventHandler(m_Calendar_DayRender);
      m_WarehouseDropDownList.SelectedIndexChanged += new EventHandler(m_SelectionChanged);
      m_Calendar.SelectionChanged += new EventHandler(m_SelectionChanged);
    }
    private void m_SelectionChanged(object sender, EventArgs e)
    {
      try
      {
        DateTime _sd = m_Calendar.SelectedDate;
        string _wrhs = m_WarehouseDropDownList.SelectedValue;
        if ((_sd == null) || (String.IsNullOrEmpty(_wrhs)))
          return;
        m_TimeSlotList.Items.Clear();
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url) { ObjectTrackingEnabled = false })
          m_UpdateTimeSlotListMethod(edc, _sd, _wrhs);
      }
      catch (Exception ex)
      {
        this.Controls.Add(new LiteralControl("Cannot display time slots because; " + ex.Message));
      }
    }
    private void m_Calendar_DayRender(object sender, DayRenderEventArgs e)
    {
      if (e.Day.IsOtherMonth)
        e.Cell.BackColor = Color.LightGray; ;
      if (m_AvailableDays.ContainsKey(e.Day.Date))
      {
        e.Cell.BackColor = Color.LightGreen;
        e.Cell.Controls.Add(new Literal() { Text = m_AvailableDays[e.Day.Date].ToString() });
      }
      else
        e.Day.IsSelectable = false;
    }
    protected override void OnPreRender(EventArgs e)
    {
      DateTime _sd = m_Calendar.SelectedDate;
      string _wrhs = m_WarehouseDropDownList.SelectedValue;
      if ((_sd != null) && (!String.IsNullOrEmpty(_wrhs)))
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url) { ObjectTrackingEnabled = false })
          foreach (var item in TimeSlotTimeSlot.GetFreeForSelectedMonth(edc, _sd, _wrhs))
          {
            if (!m_AvailableDays.ContainsKey(item.Date))
              m_AvailableDays.Add(item.Date, 1);
            else
              m_AvailableDays[item.Date] += 1;
          }
      base.OnPreRender(e);
    }
    private delegate void UpdateTimeSlotListEventHandler(EntitiesDataContext edc, DateTime _sd, string _wrhs);
    private UpdateTimeSlotListEventHandler m_UpdateTimeSlotListMethod;
    private void UpdateTimeSlotListSimple(EntitiesDataContext edc, DateTime _sd, string _wrhs)
    {
      bool _first = false;
      foreach (var _cts in TimeSlotTimeSlot.GetForSelectedDay(edc, _sd, _wrhs))
      {
        ListItem _ni = new ListItem(String.Format("{0:HH:mm}", _cts.StartTime), _cts.Identyfikator.ToString(), true);
        if (_first)
        {
          _ni.Selected = true;
          _first = false;
        }
        m_TimeSlotList.Items.Add(_ni);
      }
    }
    private void UpdateTimeSlotListExtended(EntitiesDataContext edc, DateTime _sd, string _wrhs)
    {
      int _hr = 0;
      bool _first = false;
      foreach (var _cts in TimeSlotTimeSlot.GetForSelectedDay(edc, _sd, _wrhs))
      {
        while (_hr++ < _cts.StartTime.Value.Hour)
          m_TimeSlotList.Items.Add(new ListItem(m_EmptyTimeSlot, "-1"));
        _hr++;
        ListItem _ni = new ListItem(String.Format("{0:HH:mm}", _cts.StartTime), _cts.Identyfikator.ToString(), true);
        if (_first)
        {
          _ni.Selected = true;
          _first = false;
        }
        m_TimeSlotList.Items.Add(_ni);
      }
      while (_hr++ <= 24)
        m_TimeSlotList.Items.Add(new ListItem(m_EmptyTimeSlot, "-1"));
    }
    private const string m_EmptyTimeSlot = "_______";
    private SortedDictionary<DateTime, int> m_AvailableDays = new SortedDictionary<DateTime, int>();
    #endregion
  }
}
