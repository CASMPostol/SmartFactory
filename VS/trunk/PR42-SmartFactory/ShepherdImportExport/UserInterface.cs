using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CAS.SmartFactory.Shepherd.ImportExport.XML;
using CAS.SmartFactory.Shepherd.ImportExport.Entities;

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
    private void CreateCountries(EntitiesDataContext _EDC, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateCountries starting"));
      for (int i = 0; i < 10; i++)
      {
        string _cn = String.Format("Country {0}", i);
        CountryClass _cuntr = new CountryClass() { Tytuł = _cn };
        _EDC.Country.InsertOnSubmit(_cuntr);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _cn)));
        _EDC.SubmitChanges();
        CreateCities(_EDC, _cuntr, _update);
      }
    }
    private void CreateCities(EntitiesDataContext _EDC, CountryClass _cuntr, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateCities starting"));
      for (int i = 0; i < 10; i++)
      {
        string _cn = String.Format("City {0}", i);
        CityType _cmm = new CityType() { Tytuł = _cn, CountryName = _cuntr };
        _EDC.City.InsertOnSubmit(_cmm);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _cn)));
        _EDC.SubmitChanges();
      }
    }
    private void CreateCommodity(EntitiesDataContext _EDC, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateCommodity starting"));
      for (int i = 0; i < 3; i++)
      {
        string _cn = String.Format("Commodity {0}", i);
        CommodityCommodity _cmm = new CommodityCommodity() { Tytuł = _cn };
        _EDC.Commodity.InsertOnSubmit(_cmm);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _cn)));
        _EDC.SubmitChanges();
        CraeteWareHouse(_EDC, _cmm, _update);
      }
    }
    private void CraeteWareHouse(EntitiesDataContext _EDC, CommodityCommodity _cmm, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteWareHouse starting"));
      Warehouse _wrs = new Warehouse() { Commodity = _cmm, Tytuł = String.Format("Warehouse {0}", _cmm.Tytuł) };
      _EDC.Warehouse.InsertOnSubmit(_wrs);
      _EDC.SubmitChanges();
      CreateShippingPoints(_EDC, _wrs, _update);
    }
    private void CreateShippingPoints(EntitiesDataContext _EDC, Warehouse _wrs, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateShippingPoints starting"));
      string _frmt = "ShippingPoint {0}";
      ShippingPoint _sp = new ShippingPoint() { Description = _wrs.Tytuł + " Inbound", Direction = Direction.Inbound, Tytuł = String.Format(_frmt, _wrs.Tytuł), Warehouse = _wrs };
      _EDC.ShippingPoint.InsertOnSubmit(_sp);
      _sp = new ShippingPoint() { Description = _wrs.Tytuł + " Outbound", Direction = Direction.Outbound, Tytuł = String.Format(_frmt, _wrs.Tytuł), Warehouse = _wrs };
      _EDC.ShippingPoint.InsertOnSubmit(_sp);
      _sp = new ShippingPoint() { Description = _wrs.Tytuł + " BothDirections", Direction = Direction.BothDirections, Tytuł = String.Format(_frmt, _wrs.Tytuł), Warehouse = _wrs };
      _EDC.ShippingPoint.InsertOnSubmit(_sp);
      _EDC.SubmitChanges();
      CreateTimeSlots(_EDC, _sp, UpdateToolStrip);
    }
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
          _ts.Add(new TimeSlotTimeSlot() { EntryTime = _bgn, EndTime = _end, ExitTime = _end, Occupied = false, ShippingPoint = _sp, StartTime = _bgn });
          _update(this, new ProgressChangedEventArgs(1, _bgn.ToShortDateString()));
        }
        _EDC.TimeSlot.InsertAllOnSubmit(_ts);
        _EDC.SubmitChanges();
      }
    }
    private void ImportData(PreliminaryData cnfg, string _URL, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "ImportData starting"));
      using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URLTextBox.Text.Trim()))
      {
      }
      SetDone("Done");
    }
    private void CreatePartners(EntitiesDataContext _EDC, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreatePartners starting"));
      ServiceType[] _stt = new ServiceType[] { ServiceType.Forwarder, ServiceType.SecurityEscortProvider, ServiceType.VendorAndForwarder };
      string[] _pn = new string[] { "forwarder", "escort", "vendor" };
      for (int i = 0; i <= 2; i++)
      {
        string _nm = String.Format("Partner {0}", i);
        Partner _prt = new Partner() { Tytuł = _nm, ServiceType = _stt[i], ShepherdUserTitle = _pn[i] };
        _EDC.JTIPartner.InsertOnSubmit(_prt);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _nm)));
        _EDC.SubmitChanges();
        CraeteTrucks(_EDC, _prt, _update);
        CraeteDrivers(_EDC, _prt, _update);
        if (_prt.ServiceType.Value != ServiceType.SecurityEscortProvider)
          CraeteTrailer(_EDC, _prt, _update);
      }
    }
    private void CraeteTrucks(EntitiesDataContext _EDC, Partner _prt, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteTrucks starting"));
      VehicleType _vt;
      if (_prt.ServiceType.Value == ServiceType.SecurityEscortProvider)
        _vt = VehicleType.SecurityEscortCar;
      else
        _vt = VehicleType.Truck;
      for (int i = 0; i < 2; i++)
      {
        string _tm = String.Format("Truck {0}", i);
        Truck _trck = new Truck() { Tytuł = _tm, VehicleType = _vt, VendorName = _prt };
        _EDC.Truck.InsertOnSubmit(_trck);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _tm)));
        _EDC.SubmitChanges();
      }
    }
    private void CraeteTrailer(EntitiesDataContext _EDC, Partner _prt, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteTrailer starting"));
      for (int i = 0; i < 2; i++)
      {
        string _tm = String.Format("Trailer {0}", i);
        Trailer _trck = new Trailer() { Tytuł = _tm, VendorName = _prt, };
        _EDC.Trailer.InsertOnSubmit(_trck);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _tm)));
        _EDC.SubmitChanges();
      }
    }
    private void CraeteDrivers(EntitiesDataContext _EDC, Partner _prt, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteDrivers starting"));
      for (int i = 0; i < 2; i++)
      {
        string _tm = String.Format("Driver {0}", i);
        Driver _drv = new Driver() { Tytuł = _tm, VendorName = _prt, IdentityDocumentNumber = "IdentityDocumentNumber" };
        _EDC.Driver.InsertOnSubmit(_drv);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _tm)));
        _EDC.SubmitChanges();
      }
    }
    #endregion
    #region EventHandlers
    private void TimeSlotsCreateButton_Click(object sender, EventArgs e)
    {
      try
      {
        m_Stopwatch.Start();
        using (EntitiesDataContext _EDC = new EntitiesDataContext(m_URLTextBox.Text.Trim()))
        {
          UpdateToolStrip(this, new ProgressChangedEventArgs(1, "CreateCities Starting"));
          CreateCountries(_EDC, UpdateToolStrip);
          UpdateToolStrip(this, new ProgressChangedEventArgs(1, "CreateCommodity Starting"));
          CreateCommodity(_EDC, UpdateToolStrip);
          UpdateToolStrip(this, new ProgressChangedEventArgs(1, "CreatePartners Starting"));
          CreatePartners(_EDC, UpdateToolStrip);
        }
        SetDone("Done");
      }
      catch (Exception ex)
      {
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
          PreliminaryData cnfg = PreliminaryData.ImportDocument(strm);
          UpdateToolStrip(this, new ProgressChangedEventArgs(10, "Importing Data"));
          ImportData(cnfg, m_URLTextBox.Text.Trim(), UpdateToolStrip);
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
