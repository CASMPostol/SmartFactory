using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using CAS.SmartFactory.Shepherd.DataModel.ImportDataModel;

namespace CAS.SmartFactory.Shepherd.ImportExport
{
  public delegate void UpdateToolStripEvent(object obj, ProgressChangedEventArgs progres);

  /// <summary>
  /// User Interface
  /// </summary>
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
    //TODO Add to ImportData to monitor import flow.
    private void UpdateToolStrip(object obj, ProgressChangedEventArgs progres)
    {
      m_ToolStripStatusLabel.Text = (string)progres.UserState;
      m_ToolStripProgressBar.Value += progres.ProgressPercentage;
      if (m_Stopwatch.ElapsedMilliseconds >= 250)
      {
        this.Refresh();
        m_Stopwatch.Reset();
        m_Stopwatch.Start();
      }
      if (m_ToolStripProgressBar.Value >= m_ToolStripProgressBar.Maximum)
      {
        m_ToolStripProgressBar.Maximum *= 2;
        this.Refresh();
      }
    }
    private void SetDone(string _label)
    {
      m_ToolStripStatusLabel.Text = _label + " at " + m_ToolStripProgressBar.Value.ToString();
      m_ToolStripProgressBar.Value = m_ToolStripProgressBar.Minimum;
      this.Refresh();
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

    private void CreateTimeSlots(EntitiesDataContext _EDC, ShippingPoint _sp, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateTimeSlots starting"));
      for (int _i = 0; _i < Properties.Settings.Default.NumberOfDays; _i++)
      {
        DateTime _dy = DateTime.Now.Date + TimeSpan.FromDays(_i);
        _update(this, new ProgressChangedEventArgs(1, _dy.ToShortDateString()));
        if (_dy.DayOfWeek == DayOfWeek.Sunday || _dy.DayOfWeek == DayOfWeek.Saturday)
          continue;
        List<TimeSlotTimeSlot> _ts = new List<TimeSlotTimeSlot>();
        short _strtTm = 8;
        for (int _indx = _strtTm; _indx <= _strtTm + Properties.Settings.Default.TimeSlotsPerDay; _indx++)
        {
          DateTime _bgn = _dy + TimeSpan.FromHours(_indx);
          DateTime _end = _bgn + TimeSpan.FromHours(1);
          _ts.Add(new TimeSlotTimeSlot() { EntryTime = _bgn, EndTime = _end, ExitTime = _end, Occupied = Occupied.Free, TimeSlot2ShippingPointLookup = _sp, StartTime = _bgn });
          _update(this, new ProgressChangedEventArgs(1, _bgn.ToShortDateString()));
        }
        _EDC.TimeSlot.InsertAllOnSubmit(_ts);
        _EDC.SubmitChanges();
      }
    }

    private void CreatePartners(EntitiesDataContext _EDC, UpdateToolStripEvent _update, Partner _prt)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreatePartners starting"));
      short _trailerId = 0;
      short _driverId = 0;
      short _truckId = 0;
      for (int i = 0; i <= 3; i++)
      {
        CraeteTrucks(_EDC, _prt, ref _truckId, _update);
        CraeteDrivers(_EDC, _prt, ref _driverId, _update);
        if (_prt.ServiceType.Value != ServiceType.SecurityEscortProvider)
          CraeteTrailer(_EDC, _prt, ref _trailerId, _update);
        _EDC.SubmitChanges();
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _prt.Title)));
      }
    }
    private void CraeteTrucks(EntitiesDataContext _EDC, Partner _prt, ref short _truckId, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteTrucks starting"));
      VehicleType _vt;
      if (_prt.ServiceType.Value == ServiceType.SecurityEscortProvider)
        _vt = VehicleType.SecurityEscortCar;
      else
        _vt = VehicleType.Truck;
      for (int i = 0; i < 2; i++)
      {
        string _tm = String.Format("Truck {0}", _truckId++);
        Truck _trck = new Truck() { Title = _tm, VehicleType = _vt, Truck2PartnerTitle = _prt };
        _EDC.Truck.InsertOnSubmit(_trck);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _tm)));
      }
    }
    private void CraeteTrailer(EntitiesDataContext _EDC, Partner _prt, ref short _trailerId, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteTrailer starting"));
      for (int i = 0; i < 2; i++)
      {
        string _tm = String.Format("Trailer {0}", _trailerId++);
        Trailer _trck = new Trailer() { Title = _tm, Trailer2PartnerTitle = _prt, };
        _EDC.Trailer.InsertOnSubmit(_trck);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _tm)));
      }
    }
    private void CraeteDrivers(EntitiesDataContext _EDC, Partner _prt, ref short _driverId, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteDrivers starting"));
      for (int i = 0; i < 2; i++)
      {
        string _tm = String.Format("Driver {0}", _driverId++);
        Driver _drv = new Driver() { Title= _tm, Driver2PartnerTitle = _prt, IdentityDocumentNumber = "IdentityDocumentNumber" };
        _EDC.Driver.InsertOnSubmit(_drv);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _tm)));
      }
    }
    #endregion

    #region EventHandlers
    private void CreateDictionaries_Click(object sender, EventArgs e)
    {
      try
      {
        m_Stopwatch.Start();
        using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URLTextBox.Text.Trim()))
        {
          foreach (Partner _prt in _EDC.Partner)
            CreatePartners(_EDC, UpdateToolStrip, _prt);
        }
        SetDone("Done");
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        SetDone(ex.Message);
      }
    }
    private void AddTimeSlots_Click(object sender, EventArgs e)
    {
      m_Stopwatch.Start();
      try
      {
        UpdateToolStrip(this, new ProgressChangedEventArgs(1, "AddTimeSlots_Click"));
        using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URLTextBox.Text.Trim()))
        {
          foreach (ShippingPoint _sp in from _ei in _EDC.ShippingPoint select _ei)
            CreateTimeSlots(_EDC, _sp, UpdateToolStrip);
        }
        SetDone("Done");
      }
      catch (Exception ex)
      {
        SetDone(ex.Message);
      }
    }
    private void ImportDictionaries_Click(object sender, EventArgs e)
    {
      Stream strm = OpenFile();
      if (strm == null)
        return;
      m_Stopwatch.Start();
      try
      {
        using (strm)
        {
          UpdateToolStrip(this, new ProgressChangedEventArgs(1, "Reading xml file"));
          PreliminaryDataRoute _cnfg = PreliminaryDataRoute.ImportDocument(strm);
          UpdateToolStrip(this, new ProgressChangedEventArgs(1, "Importing Data"));
          _cnfg.ImportData(m_URLTextBox.Text.Trim());
          SetDone("Done");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
        SetDone(ex.Message);
      }
    }
    #endregion
  }
}
