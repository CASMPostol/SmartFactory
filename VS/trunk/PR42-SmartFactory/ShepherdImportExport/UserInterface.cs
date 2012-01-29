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
using System.IO;
using System.Diagnostics;
using CAS.SmartFactory.Shepherd.ImportExport.XML;

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
    private Stopwatch m_Stopwatch = new Stopwatch();
    private delegate void UpdateToolStripEvent(object obj, ProgressChangedEventArgs progres);
    private void UpdateToolStrip(object obj, ProgressChangedEventArgs progres)
    {
      m_ToolStripStatusLabel.Text = (string)progres.UserState;
      m_ToolStripProgressBar.Value += progres.ProgressPercentage;
      if (m_Stopwatch.ElapsedMilliseconds >= 250)
        this.Refresh();
      if (m_ToolStripProgressBar.Value >= m_ToolStripProgressBar.Maximum)
      {
        m_ToolStripProgressBar.Maximum *= 2;
        this.Refresh();
      }
      m_Stopwatch.Reset();
      m_Stopwatch.Start();
    }
    private void SetDone(string _label)
    {
      m_ToolStripStatusLabel.Text = _label;
      m_ToolStripProgressBar.Value = m_ToolStripProgressBar.Minimum;
    }
    private Stream OpenFile()
    {
      UpdateToolStrip(this, new ProgressChangedEventArgs(1, "Openning the file"));
      if (m_FileManagementComonent.m_OpenFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
      {
        SetDone("Aborted by user");
        m_FileNameStatusLabel.Text = String.Empty;
        return null;
      }
      m_FileNameStatusLabel.Text = m_FileManagementComonent.m_OpenFileDialog.FileName;
      return m_FileManagementComonent.m_OpenFileDialog.OpenFile();
    }
    #region importing data
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
        CreateTimeSlots(_EDC, _sp, UpdateToolStrip);
      }
    }
    private void CreateTimeSlots(EntitiesDataContext _EDC, ShippingPoint _sp, UpdateToolStripEvent _update)
    {
      for (DateTime _dy = DateTime.Now; _dy < DateTime.Now + TimeSpan.FromDays(25); _dy.AddDays(1))
      {
        if (_dy.DayOfWeek == DayOfWeek.Sunday || _dy.DayOfWeek == DayOfWeek.Saturday)
          continue;
        List<TimeSlotTimeSlot> _ts = null;
        for (DateTime _hr = _dy; _hr.Hour <= 16; _hr.AddHours(1))
        {
          _ts.Add(new TimeSlotTimeSlot() { EntryTime = _hr, EndTime = _hr.AddHours(1), ExitTime = _hr.AddHours(1), Occupied = false, ShippingPoint = _sp, StartTime = _hr });
          _update(this, new ProgressChangedEventArgs(1, _hr.ToShortDateString()));
        }
        _EDC.TimeSlot.InsertAllOnSubmit(_ts);
        _EDC.SubmitChanges();
      }
    }
    private void ImportData(PreliminaryData cnfg, string _URL, UpdateToolStripEvent _update)
    {
      using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URLTextBox.Text.Trim()))
      {
      }
      SetDone("Done");
    }
    #endregion
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
    private void button1_Click(object sender, EventArgs e)
    {
      using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URLTextBox.Text.Trim()))
      {
        foreach (ShippingPoint _sp in from _ei in _EDC.ShippingPoint select _ei)
          CreateTimeSlots(_EDC, _sp, UpdateToolStrip);
      }
      SetDone("Done");
    }
    private void m_ImportDictionaries_Click(object sender, EventArgs e)
    {
      Stream strm = OpenFile();
      if (strm == null)
        return;
      try
      {
        using (strm)
        {
          UpdateToolStrip(this, new ProgressChangedEventArgs(1, "Reading xml file"));
          PreliminaryData cnfg = PreliminaryData.ImportDocument(strm);
          UpdateToolStrip(this, new ProgressChangedEventArgs(10, "Importing Data"));
          ImportData(cnfg, m_URLTextBox.Text.Trim(), UpdateToolStrip);
          SetDone("Done");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}
