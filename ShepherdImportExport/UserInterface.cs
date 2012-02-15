using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CAS.SmartFactory.Shepherd.ImportExport.XML;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

namespace CAS.SmartFactory.Shepherd.ImportExport
{
  using UpdateToolStripEvent = CAS.SmartFactory.Shepherd.Dashboards.GlobalDefinitions.UpdateToolStripEvent;
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
    private void CreateTransportUnts(EntitiesDataContext _EDC, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateTransportUnts starting"));
      for (int i = 0; i < 4; i++)
      {
        string _cn = String.Format("Unit Type {0}", i);
        TransportUnitTypeTranspotUnit _cuntr = new TransportUnitTypeTranspotUnit() { Tytuł = _cn };
        _update(this, new ProgressChangedEventArgs(1, String.Format("Insert {0}", _cn)));
        _EDC.TransportUnitType.InsertOnSubmit(_cuntr);
      }
      _EDC.SubmitChanges();
    }
    private void CreateShippmentTypes(EntitiesDataContext _EDC, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateShippmentTypes starting"));
      for (int i = 0; i < 4; i++)
      {
        string _cn = String.Format("Shippment Type {0}", i);
        ShipmentTypeShipmentType _cuntr = new ShipmentTypeShipmentType() { Tytuł = _cn };
        _update(this, new ProgressChangedEventArgs(1, String.Format("Insert {0}", _cn)));
        _EDC.ShipmentType.InsertOnSubmit(_cuntr);
      }
      _EDC.SubmitChanges();
    }
    private void CreateCountries(EntitiesDataContext _EDC, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateCountries starting"));
      short _ctidx = 0;
      for (int i = 0; i < 10; i++)
      {
        string _cn = String.Format("Country {0}", i);
        CountryClass _cuntr = new CountryClass() { Tytuł = _cn };
        _EDC.Country.InsertOnSubmit(_cuntr);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _cn)));
        _EDC.SubmitChanges();
        CreateCities(_EDC, _cuntr, _update, ref _ctidx);
      }
    }
    private void CreateCities(EntitiesDataContext _EDC, CountryClass _cuntr, UpdateToolStripEvent _update, ref short _ctidx)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateCities starting"));
      for (int i = 0; i < 10; i++)
      {
        string _cn = String.Format("City {0}", _ctidx++);
        CityType _cmm = new CityType() { Tytuł = _cn, CountryName = _cuntr };
        _EDC.City.InsertOnSubmit(_cmm);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _cn)));
        _EDC.SubmitChanges();
      }
    }
    private void CreateCommodity(EntitiesDataContext _EDC, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateCommodity starting"));
      short _spId = 0;
      for (int i = 0; i < 3; i++)
      {
        string _cn = String.Format("Commodity {0}", i);
        CommodityCommodity _cmm = new CommodityCommodity() { Tytuł = _cn };
        _EDC.Commodity.InsertOnSubmit(_cmm);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _cn)));
        _EDC.SubmitChanges();
        CraeteWareHouse(_EDC, _cmm, ref _spId, _update);
      }
    }
    private void CraeteWareHouse(EntitiesDataContext _EDC, CommodityCommodity _cmm, ref short _spId, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteWareHouse starting"));
      Warehouse _wrs = new Warehouse() { Commodity = _cmm, Tytuł = String.Format("Warehouse {0}", _cmm.Tytuł) };
      _EDC.Warehouse.InsertOnSubmit(_wrs);
      _EDC.SubmitChanges();
      CreateShippingPoints(_EDC, _wrs, ref _spId, _update);
    }
    private void CreateShippingPoints(EntitiesDataContext _EDC, Warehouse _wrs, ref short _spId, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateShippingPoints starting"));
      string _frmt = "ShippingPoint {0} at {1}";
      ShippingPoint _sp = new ShippingPoint() { Description = _wrs.Tytuł + " Inbound", Direction = Direction.Inbound, Tytuł = String.Format(_frmt, _spId++, _wrs.Tytuł), Warehouse = _wrs };
      _EDC.ShippingPoint.InsertOnSubmit(_sp);
      CreateTimeSlots(_EDC, _sp, _update);
      _sp = new ShippingPoint() { Description = _wrs.Tytuł + " Outbound", Direction = Direction.Outbound, Tytuł = String.Format(_frmt, _spId++, _wrs.Tytuł), Warehouse = _wrs };
      _EDC.ShippingPoint.InsertOnSubmit(_sp);
      CreateTimeSlots(_EDC, _sp, _update);
      _sp = new ShippingPoint() { Description = _wrs.Tytuł + " BothDirections", Direction = Direction.BothDirections, Tytuł = String.Format(_frmt, _spId++, _wrs.Tytuł), Warehouse = _wrs };
      _EDC.ShippingPoint.InsertOnSubmit(_sp);
      _EDC.SubmitChanges();
      CreateTimeSlots(_EDC, _sp, _update);
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
          _ts.Add(new TimeSlotTimeSlot() { EntryTime = _bgn, EndTime = _end, ExitTime = _end, Occupied = Occupied.Free , ShippingPoint = _sp, StartTime = _bgn });
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
      ServiceType[] _stt = new ServiceType[] { ServiceType.Forwarder, ServiceType.SecurityEscortProvider, ServiceType.Vendor, ServiceType.VendorAndForwarder, };
      string[] _pn = new string[] { "forwarder", "escort", "vendor", "VendorAndForwarder" };
      short _trailerId = 0;
      short _driverId = 0;
      short _truckId = 0;
      for (int i = 0; i <= 3; i++)
      {
        string _nm = String.Format("Partner {0}", _pn[i]);
        Partner _prt = new Partner() { Tytuł = _nm, ServiceType = _stt[i], ShepherdUserTitle = _pn[i] };
        _EDC.JTIPartner.InsertOnSubmit(_prt);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _nm)));
        _EDC.SubmitChanges();
        CraeteTrucks(_EDC, _prt, ref _truckId, _update);
        CraeteDrivers(_EDC, _prt, ref _driverId, _update);
        if (_prt.ServiceType.Value != ServiceType.SecurityEscortProvider)
          CraeteTrailer(_EDC, _prt, ref _trailerId, _update);
        if (_prt.ServiceType.Value == ServiceType.Forwarder)
          CreateRouts(_EDC, _update, _prt);
      }
    }
    private void CreateRouts(EntitiesDataContext _EDC, UpdateToolStripEvent _update, Partner _prt)
    {
      _update(this, new ProgressChangedEventArgs(1, "CreateRouts starting"));
      List<CityType> _ctList = (from _ct in _EDC.City select _ct).ToList<CityType>();
      Random _rdm = new Random(_ctList.Count);
      foreach (CityType _ctItem in _ctList)
        foreach (TransportUnitTypeTranspotUnit _utItem in from _ut in _EDC.TransportUnitType select _ut)
          foreach (ShipmentTypeShipmentType _stItem in from _st in _EDC.ShipmentType select _st)
          {
            Route _rt = new Route()
            {
              Tytuł = String.Format("Forwarder {0} to {1} destination city", _prt.Tytuł, _ctItem.Tytuł),
              CityName = _ctItem,
              TransportUnitType = _utItem,
              ShipmentType = _stItem,
              VendorName = _prt
            };
            _EDC.Route.InsertOnSubmit(_rt);
            _update(this, new ProgressChangedEventArgs(1, String.Format("Added {0}", _rt.Tytuł)));
          }
      _update(this, new ProgressChangedEventArgs(1, "CreateRouts SubmitChanges"));
      _EDC.SubmitChanges();
      _update(this, new ProgressChangedEventArgs(1, "DestinationMarket and MarketMarket starting"));
      for (int _mi = 0; _mi < 3; _mi++)
      {
        string _tm = String.Format("Market {0}", _mi);
        MarketMarket _mrkt = new MarketMarket() { Tytuł = _tm };
        _EDC.Market.InsertOnSubmit(_mrkt);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _mrkt.Tytuł)));
        _EDC.SubmitChanges();
        for (int _dmi = 0; _dmi < 3; _dmi++)
        {
          DestinationMarket _dm = new DestinationMarket()
          {
            Market = _mrkt,
            CityName = _ctList[_rdm.Next(0, _ctList.Count - 1)],
          };
          _dm.Tytuł = String.Format("The {0} market available from the {1} city", _mrkt.Tytuł, _dm.CityName.Tytuł);
          _EDC.DestinationMarket.InsertOnSubmit(_dm);
        }
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _mrkt.Tytuł)));
        _EDC.SubmitChanges();
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
        Truck _trck = new Truck() { Tytuł = _tm, VehicleType = _vt, VendorName = _prt };
        _EDC.Truck.InsertOnSubmit(_trck);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _tm)));
        _EDC.SubmitChanges();
      }
    }
    private void CraeteTrailer(EntitiesDataContext _EDC, Partner _prt, ref short _trailerId, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteTrailer starting"));
      for (int i = 0; i < 2; i++)
      {
        string _tm = String.Format("Trailer {0}", _trailerId++);
        Trailer _trck = new Trailer() { Tytuł = _tm, VendorName = _prt, };
        _EDC.Trailer.InsertOnSubmit(_trck);
        _update(this, new ProgressChangedEventArgs(1, String.Format("SubmitChanges for {0}", _tm)));
        _EDC.SubmitChanges();
      }
    }
    private void CraeteDrivers(EntitiesDataContext _EDC, Partner _prt, ref short _driverId, UpdateToolStripEvent _update)
    {
      _update(this, new ProgressChangedEventArgs(1, "CraeteDrivers starting"));
      for (int i = 0; i < 2; i++)
      {
        string _tm = String.Format("Driver {0}", _driverId++);
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
          CreateShippmentTypes(_EDC, UpdateToolStrip);
          CreateTransportUnts(_EDC, UpdateToolStrip);
          CreateCountries(_EDC, UpdateToolStrip);
          CreateCommodity(_EDC, UpdateToolStrip);
          CreatePartners(_EDC, UpdateToolStrip);
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

    public int _mrkti { get; set; }
  }
}
