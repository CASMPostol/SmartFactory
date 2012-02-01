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
    public TimeSlotWebPartUserControl()
    {
      SimpleTimeSlotList = true;
    }
    internal InterconnectionDataTable<TimeSlotTimeSlot> GetSelectedTimeSlotInterconnectionData()
    {
      string _tn = typeof(TimeSlot).GetType().Name;
      int _sts = -1;
      if (!String.IsNullOrEmpty(m_TimeSlotList.SelectedValue))
        _sts = int.Parse(m_TimeSlotList.SelectedValue);
      try
      {
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url) { ObjectTrackingEnabled = false })
          return new InterconnectionDataTable<TimeSlotTimeSlot>(TimeSlotTimeSlot.GetAtIndex(edc, _sts, true), typeof(TimeSlot).Name);
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
    EntitiesDataContext m_EDC = null;
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
        m_Calendar.VisibleDate = DateTime.Today;
        m_Calendar.SelectedDate = DateTime.Today;
        m_WarehouseDropDownList.DataSource = from _idx in m_EDC.Warehouse
                                             orderby _idx.Tytuł ascending
                                             select _idx;
        m_WarehouseDropDownList.DataBind();
        m_WarehouseDropDownList.SelectedIndex = 0;
      }
      m_UpdateTimeSlotListMethod(m_EDC, m_Calendar.SelectedDate.Date, m_WarehouseDropDownList.SelectedValue, Direction.Inbound);
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
          m_UpdateTimeSlotListMethod(edc, _sd, _wrhs, Direction.Inbound);
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
        e.Cell.BackColor = Color.SeaGreen;
        string _days = String.Format("<font size=\"1\" color=\"#ff0000\"> [{0}]</font>", m_AvailableDays[e.Day.Date].ToString());
        e.Cell.Controls.Add(new LiteralControl() { Text = _days });
      }
      else
        e.Day.IsSelectable = false;
    }
    protected override void OnPreRender(EventArgs e)
    {
      DateTime _sd = m_Calendar.SelectedDate;
      string _wrhs = m_WarehouseDropDownList.SelectedValue;
      if ((_sd != null) && (!String.IsNullOrEmpty(_wrhs)))
        foreach (var item in TimeSlotTimeSlot.GetFreeForSelectedMonth(m_EDC, _sd, _wrhs))
        {
          if (!m_AvailableDays.ContainsKey(item.Date))
            m_AvailableDays.Add(item.Date, 1);
          else
            m_AvailableDays[item.Date] += 1;
        }
      base.OnPreRender(e);
    }
    private delegate void UpdateTimeSlotListEventHandler(EntitiesDataContext edc, DateTime _sd, string _wrhs, Direction _direction);
    private UpdateTimeSlotListEventHandler m_UpdateTimeSlotListMethod;
    private void UpdateTimeSlotListSimple(EntitiesDataContext edc, DateTime _sd, string _warehoise, Direction _direction)
    {
      m_TimeSlotList.Items.Clear();
      Warehouse _warehouse = Element.GetAtIndex(edc.Warehouse, _warehoise);
      foreach (var _spoint in (from _sp in edc.ShippingPoint 
                               where _sp.Warehouse.Identyfikator == _warehouse.Identyfikator 
                               select _sp).ToList<ShippingPoint>())
      {
        if (_spoint.Direction != _direction && _spoint.Direction != Direction.BothDirections)
          continue;
        DateTime _strt = new DateTime(_sd.Year, _sd.Month, 1);
        DateTime _end = _strt.AddMonths(1);
        foreach (var _ts in (from _tsidx in _spoint.TimeSlot
                             where !_tsidx.Occupied.Value && _tsidx.StartTime >= _strt && _tsidx.StartTime < _end 
                             orderby _tsidx.StartTime ascending
                             select _tsidx)) //.ToList<TimeSlot>().OrderBy( _ts => _ts.StartTime ))
        {
          if (_ts.StartTime.Value.Date != _sd.Date)
            continue;
          ListItem _ni = new ListItem(String.Format("{0:HH:mm}", _ts.StartTime), _ts.Identyfikator.Value.ToString(), true);
          m_TimeSlotList.Items.Add(_ni);
        }
      }
    }
    private void UpdateTimeSlotListExtended(EntitiesDataContext edc, DateTime _sd, string _wrhs, Direction _direction)
    {
      int _intWhr = -1;
      if (!int.TryParse(_wrhs, out _intWhr) || _intWhr <= 0)
        return;
      int _hr = 0;
      bool _first = false;
      foreach (var _cts in TimeSlotTimeSlot.GetForSelectedDay(edc, _sd, _intWhr))
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
