using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace CAS.SmartFactory.Shepherd.ImportExport
{
  public partial class UserInterface : Form
  {
    public UserInterface()
    {
      InitializeComponent();
    }
    protected override void OnLoad(EventArgs e)
    {
      m_URLTextBox.Text = Properties.Settings.Default.URL.Trim();
      base.OnLoad(e);
    }
    protected override void OnClosed(EventArgs e)
    {
      Properties.Settings.Default.URL = m_URLTextBox.Text;
      base.OnClosed(e);
    }
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
          components.Dispose();
      }
      base.Dispose(disposing);
    }
    private void m_TimeSlotsCreateButton_Click(object sender, EventArgs e)
    {
      //SPSecurity.RunWithElevatedPrivileges(delegate()
      //  {
      //  });
      using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URLTextBox.Text.Trim()))
      {
        CityType.CreateCities(_EDC);
        CreateCommodity(_EDC);
      }
    }
    private void CreateCities(EntitiesDataContext _EDC)
    {
      for (int i = 0; i < 10; i++)
      {
        CityType _cmm = new CityType() { Tytuł = String.Format("City {0}", i) };
        _EDC.City.InsertOnSubmit(_cmm);
        _EDC.SubmitChanges();
      }
    }
    private void CreateCommodity(EntitiesDataContext _EDC)
    {
      for (int i = 0; i < 10; i++)
      {
        CommodityCommodity _cmm = new CommodityCommodity() { Tytuł = String.Format("Commodity {0}", i) };
        _EDC.Commodity.InsertOnSubmit(_cmm);
        _EDC.SubmitChanges();
        CraeteWareHouse(_EDC, _cmm);
      }
    }
    private void CraeteWareHouse(EntitiesDataContext _EDC, CommodityCommodity _cmm)
    {
      Warehouse _wrs = new Warehouse() { Commodity = _cmm, Tytuł = String.Format("Warehouse {0}", _cmm.Tytuł) };
      _EDC.Warehouse.InsertOnSubmit(_wrs);
      _EDC.SubmitChanges();
      CreateShippingPoints(_EDC, _wrs);
    }
    private void CreateShippingPoints(EntitiesDataContext _EDC, Warehouse _wrs)
    {
      foreach (Direction _tx in Enum.GetValues(typeof(Direction)))
      {
        ShippingPoint _sp = new ShippingPoint() { Description = _wrs.Tytuł, Direction = _tx, Tytuł = String.Format("ShippingPoint {0}", _wrs.Tytuł), Warehouse = _wrs };
        _EDC.ShippingPoint.InsertOnSubmit(_sp);
        _EDC.SubmitChanges();
        CreateTimeSlots(_EDC, _sp);
      }
    }
    private void CreateTimeSlots(EntitiesDataContext _EDC, ShippingPoint _sp)
    {
      for (DateTime _dy = DateTime.Now; _dy < DateTime.Now + TimeSpan.FromDays(25); _dy.AddDays(1))
      {
        if (_dy.DayOfWeek == DayOfWeek.Sunday || _dy.DayOfWeek == DayOfWeek.Saturday)
          continue;
        List<TimeSlotTimeSlot> _ts = null;
        for (DateTime _hr = _dy; _hr.Hour <= 16; _hr.AddHours(1))
          _ts.Add(new TimeSlotTimeSlot() { EntryTime = _hr, EndTime = _hr.AddHours(1), ExitTime = _hr.AddHours(1), Occupied = false, ShippingPoint = _sp, StartTime = _hr });
        _EDC.TimeSlot.InsertAllOnSubmit(_ts);
        _EDC.SubmitChanges();
      }
    }
  }
}
