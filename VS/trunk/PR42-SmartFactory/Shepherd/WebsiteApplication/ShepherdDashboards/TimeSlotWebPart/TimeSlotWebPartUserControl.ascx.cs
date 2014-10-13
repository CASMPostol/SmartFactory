//<summary>
//  Title   : TimeSlotWebPartUserControl UserControl
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
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Globalization;

namespace CAS.SmartFactory.Shepherd.Dashboards.TimeSlotWebPart
{
  /// <summary>
  /// TimeSlotWebPartUserControl UserControl
  /// </summary>
  public partial class TimeSlotWebPartUserControl : UserControl
  {
    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeSlotWebPartUserControl"/> class.
    /// </summary>
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
            m_RoleDirection = Direction.Inbound;
            break;
          case GlobalDefinitions.Roles.OutboundOwner:
          case GlobalDefinitions.Roles.Coordinator:
          case GlobalDefinitions.Roles.Supervisor:
          case GlobalDefinitions.Roles.None:
            m_DoubleTimeSlotsPanel.Visible = true;
            m_RoleDirection = Direction.Outbound;
            break;
        }
      }
    }
    #endregion

    #region UserControl override
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    /// <exception cref="System.ApplicationException">Page_Load exception:  + ex.Message</exception>
    protected void Page_Load(object sender, EventArgs e)
    {
      try
      {
        if (!IsPostBack)
        {
          m_WarehouseDropDownList.DataSource = from _idx in EDC.Warehouse
                                               orderby _idx.Title ascending
                                               select new { Title = _idx.Title, ID = _idx.Id.Value };
          m_WarehouseDropDownList.DataTextField = "Title";
          m_WarehouseDropDownList.DataValueField = "ID";
          m_WarehouseDropDownList.DataBind();
          //This best practice addresses the issue identified by the SharePoint Dispose Checker Tool as SPDisposeCheckID_210
          SPWeb _currentWeb = SPControl.GetContextWeb(this.Context);
          Partner _Partner = Partner.FindForUser(EDC, _currentWeb.CurrentUser);
          if (_Partner != null)
            m_WarehouseDropDownList.Select(_Partner.Partner2WarehouseTitle);
          else
            m_WarehouseDropDownList.SelectedIndex = 0;
          m_Calendar.VisibleDate = DateTime.Today;
          m_Calendar.SelectedDate = DateTime.Today;
        }
        m_InterconnectionDataTable_TimeSlotTimeSlot = new InterconnectionDataTable<TimeSlotTimeSlot>(typeof(TimeSlot).Name);
        m_Calendar.DayRender += new DayRenderEventHandler(m_Calendar_DayRender);
        m_Calendar.SelectionChanged += new EventHandler(m_Calendar_SelectionChanged);
        m_Calendar.VisibleMonthChanged += new MonthChangedEventHandler(m_Calendar_VisibleMonthChanged);
        m_TimeSlotList.SelectedIndexChanged += new EventHandler(m_TimeSlotList_SelectedIndexChanged);
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Page_Load exception: " + ex.Message, ex);
      }
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender" /> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      PrepareCalendar(m_RoleDirection);
      base.OnPreRender(e);
    }
    #endregion

    #region EventHandlers
    private void m_TimeSlotList_SelectedIndexChanged(object sender, EventArgs e)
    {
      string _at = "Starting";
      m_TimeSlotSelection = true;
      if (m_TimeSlotList.SelectedValue.IsNullOrEmpty())
        return;
      try
      {
        _at = "TimeSlotTimeSlot";
        TimeSlotTimeSlot _slctdTmslt = Element.GetAtIndex(EDC.TimeSlot, m_TimeSlotList.SelectedValue);
        _slctdTmslt.IsDouble = m_ShowDoubleTimeSlots.Checked;
        _at = "m_InterconnectionDataTable_TimeSlotTimeSlot";
        m_InterconnectionDataTable_TimeSlotTimeSlot.SetData
          (this, new InterconnectionDataTable<TimeSlotTimeSlot>.InterconnectionEventArgs(_slctdTmslt));
      }
      catch (Exception ex)
      {
        string _msg = String.Format("TimeSlotList_SelectedIndexChanged at: {0}", _at);
        Anons.WriteEntry(EDC, _msg, ex.Message);
      }
    }
    private void m_Calendar_VisibleMonthChanged(object sender, MonthChangedEventArgs e) { }
    private void m_Calendar_SelectionChanged(object sender, EventArgs e) { }
    private void m_Calendar_DayRender(object sender, DayRenderEventArgs e)
    {
      if (e.Day.IsOtherMonth)
        e.Cell.BackColor = Color.LightGray;
      ;
      if (m_AvailableDays.ContainsKey(e.Day.Date))
      {
        e.Cell.BackColor = Color.LightBlue;
        string _days = String.Format(GlobalDefinitions.NumberOfTimeSLotsFormat, m_AvailableDays[e.Day.Date].ToString());
        e.Cell.Controls.Add(new LiteralControl() { Text = _days });
      }
      else
        e.Day.IsSelectable = false;
    }
    #endregion

    #region private
    /// <summary>
    /// Prepares the calendar.
    /// </summary>
    /// <param name="direction">The _direction.</param>
    private void PrepareCalendar(Direction direction)
    {
      try
      {
        if (m_WarehouseDropDownList.SelectedValue.IsNullOrEmpty())
          return;
        DateTime _sd = m_Calendar.SelectedDate.Date;
        DateTime _strt = new DateTime(m_Calendar.VisibleDate.Year, m_Calendar.VisibleDate.Month, 1);
        DateTime _end = _strt.AddMonths(1);
        Warehouse _warehouse = Element.GetAtIndex(EDC.Warehouse, m_WarehouseDropDownList.SelectedValue);
        List<TimeSlot> _2Expose = new List<TimeSlot>();
        List<TimeSlotTimeSlot> _all4Date = (from _tsidx in EDC.TimeSlot
                                            where _tsidx.Occupied.Value == Occupied.Free && _tsidx.StartTime >= _strt && _tsidx.StartTime < _end
                                            orderby _tsidx.StartTime ascending
                                            select _tsidx).ToList<TimeSlotTimeSlot>();
        DateTime _now4User = SPContext.Current.Web.CurrentUser.RegionalSettings.TimeZone.UTCToLocalTime(System.DateTime.UtcNow);
        m_UserLocalTime.Value = _now4User.ToString(CultureInfo.GetCultureInfo((int)SPContext.Current.Web.CurrentUser.RegionalSettings.LocaleId));
        if (_strt < _now4User)
          _strt = _now4User;
        foreach (var _spoint in (from _sp in _warehouse.ShippingPoint select _sp))
        {
          if (_spoint.Direction != direction && _spoint.Direction != Direction.BothDirections)
            continue;
          List<TimeSlotTimeSlot> _avlblTmslts = _all4Date.Where(x => x.TimeSlot2ShippingPointLookup == _spoint && x.StartTime >= _strt && x.StartTime < _end).ToList<TimeSlotTimeSlot>();
          if (m_ShowDoubleTimeSlots.Checked)
          {
            TimeSpan _spn15min = new TimeSpan(0, 15, 0);
            for (int _i = 0; _i < _avlblTmslts.Count - 1; _i++)
            {
              TimeSlot _cts = _avlblTmslts[_i];
              if ((_avlblTmslts[_i + 1].StartTime.Value - _cts.EndTime.Value).Duration() > TimeSlotTimeSlot.Span15min)
                continue;
              AddToAvailable(_cts);
              if (_cts.StartTime.Value.Date != _sd)
                continue;
              _2Expose.Add(_cts);
            }
          }
          else
            foreach (TimeSlot _cts in _avlblTmslts)
            {
              AddToAvailable(_cts);
              if (_cts.StartTime.Value.Date != _sd)
                continue;
              _2Expose.Add(_cts);
            }
        }
        ExposeTimeSlots(_2Expose);
      }
      catch (Exception ex)
      {
        this.Controls.Add(new LiteralControl("CannotDisplayTimeSlots" + ex.Message));
      }
    }
    private void AddToAvailable(TimeSlot _cts)
    {
      if (!m_AvailableDays.ContainsKey(_cts.StartTime.Value.Date))
        m_AvailableDays.Add(_cts.StartTime.Value.Date, 1);
      else
        m_AvailableDays[_cts.StartTime.Value.Date] += 1;
    }
    private void ExposeTimeSlots(List<TimeSlot> availableList)
    {
      if (m_TimeSlotSelection)
        return;
      m_TimeSlotList.Items.Clear();
      HashSet<string> _labels2Display = new HashSet<string>();
      foreach (TimeSlot _item in availableList.OrderBy<TimeSlot, DateTime>(x => x.StartTime.Value))
      {
        string _label = String.Format("FormatHourMinutes".GetShepherdLocalizedString(), _item.StartTime.Value);
        if (_labels2Display.Contains(_label))
          continue;
        _labels2Display.Add(_label);
        ListItem _nli = new ListItem(_label, _item.Id.Value.ToString(), true);
        m_TimeSlotList.Items.Add(_nli);
      }
    }
    private bool m_TimeSlotSelection = false;
    private DataContextManagementAutoDispose<EntitiesDataContext> myDataContextManagement = null;
    private EntitiesDataContext EDC
    {
      get
      {
        if (myDataContextManagement == null)
          myDataContextManagement = DataContextManagementAutoDispose<EntitiesDataContext>.GetDataContextManagement(this);
        return myDataContextManagement.DataContext;
      }
    }
    private SortedDictionary<DateTime, int> m_AvailableDays = new SortedDictionary<DateTime, int>();
    private InterconnectionDataTable<TimeSlotTimeSlot> m_InterconnectionDataTable_TimeSlotTimeSlot;
    private Direction m_RoleDirection = Direction.Inbound;
    #endregion
  }
}
