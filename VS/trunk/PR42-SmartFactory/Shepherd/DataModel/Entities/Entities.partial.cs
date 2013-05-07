using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using System.Globalization;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities
{
  public class ActionResult: List<string>
  {
    #region public
    internal bool Valid { get { return this.Count == 0; } }
    public void AddException( string _src, Exception _excptn )
    {
      string _msg = String.Format("The operation interrupted at {0} by the error: {1}.", _src, _excptn.Message);
      base.Add(_msg);
    }
    public void ReportActionResult( string _url )
    {
      if (this.Count == 0)
        return;
      try
      {
        using (EntitiesDataContext EDC = new EntitiesDataContext(_url))
        {
          foreach (string _msg in this)
          {
            Anons _entry = new Anons() { Tytuł = "ReportActionResult", Treść = _msg, Wygasa = DateTime.Now + new TimeSpan(2, 0, 0, 0) };
            EDC.EventLogList.InsertOnSubmit(_entry);
          }
          EDC.SubmitChanges();
        }
      }
      catch (Exception) { }
    }
    public void AddMessage(string _src, string _message)
    {
      string _msg = String.Format("The operation reports at {0} the problem: {1}.", _src, _message);
      base.Add(_message);
    }
    #endregion
  }
  /// <summary>
  /// Extends the EntitiesDataContext class
  /// </summary>
  public partial class EntitiesDataContext
  {
    /// <summary>
    /// Persists to the content database changes made by the current user to one or more lists using the specified failure mode;
    /// or, if a concurrency conflict is found, populates the <see cref="P:Microsoft.SharePoint.Linq.DataContext.ChangeConflicts"/> property.
    /// </summary>
    /// <param name="mode">Specifies how the list item changing system of the LINQ to SharePoint provider will respond when it 
    /// finds that a list item has been changed by another process since it was retrieved.
    /// </param>
    public void SubmitChangesSilently(RefreshMode mode)
    {
      try
      {
        SubmitChanges();
      }
      catch (ChangeConflictException)
      {
        foreach (ObjectChangeConflict changedListItem in this.ChangeConflicts)
        {
          changedListItem.Resolve(mode);
        }
        this.SubmitChanges();
      }
    }
    public void ResolveChangeConflicts( ActionResult _rsult )
    {
      string _cp = "Starting";
      try
      {
        foreach (ObjectChangeConflict _itx in this.ChangeConflicts)
        {
          _cp = "ObjectChangeConflict";
          string _tmp = String.Format("Object: {0}", _itx.Object == null ? "null" : _itx.Object.ToString());
          if (_itx.MemberConflicts != null)
          {
            string _ft = ", Conflicts: Member.Name={0}; CurrentValue={1}; DatabaseValue={2}; OriginalValue={3}";
            String _chnges = String.Empty;
            foreach (MemberChangeConflict _mid in _itx.MemberConflicts)
            {
              _chnges += String.Format(_ft,
                _mid.Member == null ? "null" : _mid.Member.Name,
                _mid.CurrentValue == null ? "null" : _mid.CurrentValue.ToString(),
                _mid.DatabaseValue == null ? "null" : _mid.DatabaseValue.ToString(),
                _mid.OriginalValue == null ? "null" : _mid.OriginalValue.ToString());
            }
            _tmp += _chnges;
          }
          else
            _tmp += "; No member details";
          _rsult.AddMessage("ResolveChangeConflicts at: " + _cp, _tmp);
          _cp = "AddMessage";
          _itx.Resolve(RefreshMode.KeepCurrentValues);
        } //foreach (ObjectChangeConflict
      }
      catch (Exception ex)
      {
        string _frmt = "The current operation has been interrupted in ResolveChangeConflicts at {0} by error {1}.";
        throw new ApplicationException(String.Format(_frmt, _cp, ex.Message));
      }
    }
  } //EntitiesDataContext
  /// <summary>
  /// Extednds the Element class
  /// </summary>
  public partial class Element
  {
    public const string IDColunmName = "ID";
    public const string TitleColunmName = "Title";
    public const string IDPropertyName = "Identyfikator";
    internal const string TitlePropertyName = "Tytuł";
    /// <summary>
    /// Try to get at index. 
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">Element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index or null if <paramref name="_ID"/> is null or empty.</returns>
    public static t TryGetAtIndex<t>( EntityList<t> _list, string _ID )
      where t : Element
    {
      if (_ID.IsNullOrEmpty())
        return null;
      return GetAtIndex<t>(_list, _ID);
    }
    /// <summary>
    /// Gets at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="_list">The _list.</param>
    /// <param name="_ID">The _ ID.</param>
    /// <exception cref="ApplicationException">_ID is nuul or element cannot be found.</exception>
    /// <returns>An instance of the <see cref="t"/> for the selected index.</returns>
    public static t GetAtIndex<t>( EntityList<t> _list, string _ID )
      where t : Element
    {
      int? _index = _ID.String2Int();
      if (!_index.HasValue)
        throw new ApplicationException(typeof(t).Name + " index is null"); ;
      try
      {
        return (
              from idx in _list
              where idx.Identyfikator == _index.Value
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("{0} cannot be found at specified index{1}", typeof(t).Name, _index.Value));
      }
    }
    public static t FindAtIndex<t>( EntityList<t> _list, string _ID )
      where t : Element
    {
      int? _index = _ID.String2Int();
      if (!_index.HasValue)
        return null;
      try
      {
        return (
              from idx in _list
              where idx.Identyfikator == _index.Value
              select idx).FirstOrDefault();
      }
      catch (Exception)
      {
        return null;
      }
    }
  } //Element
  /// <summary>
  /// Alarms And Events list entry
  /// </summary>
  public partial class AlarmsAndEvents
  {
    /// <summary>
    /// Creates an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="_title">The evrnt title.</param>
    /// <param name="_partner">The partner associated with the event.</param>
    /// <param name="_shippingIndex">Index of the shipping.</param>
    public AlarmsAndEvents(string _title, Partner _partner, Shipping _shippingIndex)
    {
      Tytuł = _title;
      this.AlarmsAndEventsList2PartnerTitle = _partner;
      this.AlarmsAndEventsList2Shipping = _shippingIndex;
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="edc">Provides LINQ (Language Integrated Query) access to, and change tracking for,
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="_title">The evrnt title.</param>
    /// <param name="_partner">The partner associated with the event.</param>
    /// <param name="_shippingIndex">Index of the shipping.</param>
    internal static void WriteEntry(EntitiesDataContext edc, string _title, Partner _partner, Shipping _shippingIndex)
    {
      if (edc == null)
      {
        EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Event Log List\" list", EventLogEntryType.Error, 114);
        return;
      }
      AlarmsAndEvents _log = new AlarmsAndEvents(_title, _partner, _shippingIndex);
      edc.AlarmsAndEvents.InsertOnSubmit(_log);
      edc.SubmitChanges(Microsoft.SharePoint.Linq.ConflictMode.ContinueOnConflict);
    }
  } //AlarmsAndEvents
  /// <summary>
  /// Adds a message to the Event log list.
  /// </summary>
  public partial class Anons
  {
    /// <summary>
    /// Creates an entry with the given message text and application-defined event identifier to the event log list.
    /// </summary>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    public Anons(string source, string message)
    {
      Tytuł = source;
      Treść = message;
      this.Wygasa = DateTime.Now + new TimeSpan(2, 0, 0, 0);
    }
    /// <summary>
    /// Writes an entry with the given message text and application-defined event identifier to the event log list. 
    /// </summary>
    /// <param name="edc">Provides LINQ (Language Integrated Query) access to, and change tracking for, 
    /// the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="source">The source denominator of the message.</param>
    /// <param name="message">The string to write to the event log.</param>
    public static void WriteEntry( EntitiesDataContext edc, string source, string message )
    {
      if (edc == null)
      {
        EventLog.WriteEntry("CAS.SmartFActory", "Cannot open \"Event Log List\" list", EventLogEntryType.Error, 114);
        return;
      }
      Anons log = new Anons(source, message);
      edc.EventLogList.InsertOnSubmit(log);
      edc.SubmitChangesSilently(Microsoft.SharePoint.Linq.RefreshMode.OverwriteCurrentValues);
    }
  }
  /// <summary>
  /// Extend the DistributionList autogenerated class 
  /// </summary>
  public partial class DistributionList
  {
    public static string GetEmail( ShepherdRole _ccRole, EntitiesDataContext _EDC )
    {
      var _ccdl = (from _ccx in _EDC.DistributionList
                   where _ccx.ShepherdRole.GetValueOrDefault(Entities.ShepherdRole.Invalid) == _ccRole
                   select new { Email = _ccx.EmailAddress }).FirstOrDefault();
      if (_ccdl == null || String.IsNullOrEmpty(_ccdl.Email))
        _ccdl = (from _ccx in _EDC.DistributionList
                 where _ccx.ShepherdRole.GetValueOrDefault(Entities.ShepherdRole.Invalid) == Entities.ShepherdRole.Administrator
                 select new { Email = _ccx.EmailAddress }).FirstOrDefault();
      return (_ccdl == null ? String.Empty : _ccdl.Email).UnknownIfEmpty();
    }
  }
  /// <summary>
  /// Extend the CityType autogenerated class 
  /// </summary>
  public partial class CityType
  {
    public static void CreateCities(EntitiesDataContext _EDC)
    {
      for (int i = 0; i < 10; i++)
      {
        CityType _cmm = new CityType() { Tytuł = String.Format("City {0}", i) };
        _EDC.City.InsertOnSubmit(_cmm);
        _EDC.SubmitChanges();
      }
    }
  }
  /// <summary>
  /// Extend the Partner autogenerated class 
  /// </summary>
  public partial class Partner
  {
    public static Partner FindForUser( EntitiesDataContext edc, SPUser _user )
    {
      if (edc.Partner == null)
        return null;
      else
        return edc.Partner.FirstOrDefault(idx => idx.ShepherdUserTitle.IsNullOrEmpty() ? false : idx.ShepherdUserTitle.Contains(_user.Name));
    }
  }
  /// <summary>
  /// Extend the Shipping autogenerated class
  /// </summary>
  public partial class Shipping
  {
    #region private
    private TimeSpan _12h = new TimeSpan(12, 0, 0);
    private void RemoveDrivers(EntitiesDataContext _EDC, Partner _prtne)
    {
      if (_prtne == null)
        return;
      List<ShippingDriversTeam> _2Delete = new List<ShippingDriversTeam>();
      foreach (ShippingDriversTeam _drv in this.ShippingDriversTeam)
      {
        if (_prtne == _drv.DriverTitle.Driver2PartnerTitle)
        {
          //TODO clenup the code
          //_drv.ShippingIndex = null;
          //_drv.Driver = null;
          //_2Delete.Add(_drv);
          //_EDC.SubmitChanges();
          _2Delete.Add(_drv);
        }
      }
      _EDC.DriversTeam.DeleteAllOnSubmit(_2Delete);
      _EDC.SubmitChanges();
    }
    #endregion

    public void ChangeRout( Route _nr, EntitiesDataContext _EDC )
    {
      if (this.Shipping2RouteTitle == _nr)
        return;
      this.TrailerTitle = null;
      this.TruckTitle = null;
      _EDC.SubmitChanges();
      RemoveDrivers(_EDC, this.PartnerTitle);
      this.Shipping2RouteTitle = _nr;
      if (_nr == null)
      {
        this.BusinessDescription = String.Empty;
        this.PartnerTitle = null;
        return;
      }
      this.BusinessDescription = Shipping2RouteTitle.Route2BusinessDescriptionTitle == null ? String.Empty : Shipping2RouteTitle.Route2BusinessDescriptionTitle.Tytuł;
      this.PartnerTitle = Shipping2RouteTitle.PartnerTitle;
    }
    public void ChangeEscort( SecurityEscortCatalog _nr, EntitiesDataContext _EDC )
    {
      if (this.SecurityEscortCatalogTitle == _nr)
        return;
      this.Shipping2TruckTitle = null;
      _EDC.SubmitChanges();
      RemoveDrivers(_EDC, this.Shipping2PartnerTitle);
      this.SecurityEscortCatalogTitle = _nr;
      if (_nr == null)
      {
        this.Shipping2PartnerTitle = null;
        return;
      }
      this.SecurityEscortCatalogTitle = _nr;
      this.Shipping2PartnerTitle = _nr == null ? null : _nr.PartnerTitle;
    }
    public bool IsEditable()
    {
      switch (this.ShippingState.Value)
      {
        case Entities.ShippingState.Canceled:
        case Entities.ShippingState.Completed:
        case Entities.ShippingState.Cancelation:
          return false;
        case Entities.ShippingState.Confirmed:
        case Entities.ShippingState.Creation:
        case Entities.ShippingState.Delayed:
        case Entities.ShippingState.WaitingForCarrierData:
        case Entities.ShippingState.WaitingForConfirmation:
        case Entities.ShippingState.Underway:
          return true;
        case Entities.ShippingState.Invalid:
        case Entities.ShippingState.None:
        default:
          throw new ApplicationException("Wrong Shipping state");
      }
    }
    public bool Fixed()
    {
      return this.StartTime.Value - _12h < DateTime.Now;
    }
    public bool ReleaseBooking( int? _newTimeSlot )
    {
      if (this.TimeSlot == null || this.TimeSlot.Count == 0)
        return true;
      List<TimeSlot> _2release = new List<TimeSlot>();
      foreach (var item in this.TimeSlot)
      {
        if (item.Occupied != Entities.Occupied.Occupied0)
        {
          if (_newTimeSlot == item.Identyfikator.Value) //consistency check 
          {
            string _frmt = "ReleaseBooking tries to release the Timeslot ID = {0}, which has a wrong value of the field Occupied.";
            throw new ApplicationException(String.Format(_frmt, item.Identyfikator.Value));
          }
          continue;
        }
        if (item.Identyfikator == _newTimeSlot)
          return false; //we are tring to allocate the same time slot. 
        _2release.Add(item);
      }
      foreach (var item in _2release)
      {
        //item.Tytuł = "-- not assigned --";
        if (Fixed())
          item.Occupied = Entities.Occupied.Delayed;
        else
        {
          item.Occupied = Entities.Occupied.Free;
          item.TimeSlot2ShippingIndex = null;
          item.IsDouble = false;
        }
      }
      return true;
    }
    public void SetupTiming( TimeSlotTimeSlot _ts, bool _isDouble )
    {
      this.StartTime = _ts.StartTime;
      this.EndTime = _ts.EndTime;
      this.ShippingDuration = Convert.ToDouble((_ts.EndTime.Value - _ts.StartTime.Value).TotalMinutes);
      this.Shipping2WarehouseTitle = _ts.GetWarehouse();
      this.LoadingType = _isDouble ? Entities.LoadingType.Manual : Entities.LoadingType.Pallet;
    }
    public void UpdateTitle()
    {
      string _tf = "{0}{1:D6}";
      Tytuł = String.Format(_tf, IsOutbound.Value ? "O" : "I", Identyfikator.Value);
    }
    public void CalculateState()
    {
      switch (this.ShippingState.Value)
      {
        case Entities.ShippingState.Confirmed:
        case Entities.ShippingState.Creation:
        case Entities.ShippingState.Delayed:
        case Entities.ShippingState.WaitingForCarrierData:
        case Entities.ShippingState.WaitingForConfirmation:
          int _seDrivers = 0;
          int _crDrivers = 0;
          foreach (var _dr in this.ShippingDriversTeam)
            if (_dr.DriverTitle.Driver2PartnerTitle.ServiceType.Value == ServiceType.SecurityEscortProvider)
              _seDrivers++;
            else
              _crDrivers++;
          this.ShippingState = Entities.ShippingState.Creation;
          if (_crDrivers > 0 && this.TruckTitle != null)
          {
            if (this.SecurityEscortCatalogTitle == null || (_seDrivers > 0 && this.Shipping2TruckTitle != null))
              this.ShippingState = Entities.ShippingState.Confirmed;
            else
              this.ShippingState = Entities.ShippingState.WaitingForConfirmation;
          }
          else if (this.SecurityEscortCatalogTitle == null || (_seDrivers > 0 && this.Shipping2TruckTitle != null))
            this.ShippingState = Entities.ShippingState.WaitingForCarrierData;
          break;
        case Entities.ShippingState.Underway:
        case Entities.ShippingState.None:
        case Entities.ShippingState.Invalid:
        case Entities.ShippingState.Cancelation:
        case Entities.ShippingState.Canceled:
        case Entities.ShippingState.Completed:
        default:
          break;
      }
    }
    [Flags]
    public enum RequiredOperations
    {
      SendEmail2Carrier = 0x01,
      SendEmail2Escort = 0x02,
      AddAlarm2Carrier = 0x04,
      AddAlarm2Escort = 0x10
    }
    public const RequiredOperations CarrierOperations = Shipping.RequiredOperations.AddAlarm2Carrier | Shipping.RequiredOperations.SendEmail2Carrier;
    public enum Distance { UpTo72h, UpTo24h, UpTo2h, VeryClose, Late }
    public RequiredOperations CalculateOperations2Do( bool _email, bool _alarm, bool _TimeOutExpired )
    {
      RequiredOperations _ret = 0;
      if (!_TimeOutExpired)
        return _ret;
      RequiredOperations _cr = 0;
      RequiredOperations _escrt = 0;
      if (_alarm)
      {
        //Carrier
        if (this.PartnerTitle != null)
          _cr = RequiredOperations.AddAlarm2Carrier;
        //Escort
        if (this.Shipping2PartnerTitle != null)
          _escrt = RequiredOperations.AddAlarm2Escort;
      }
      if (_email)
      {
        if (this.PartnerTitle != null)
          _cr |= RequiredOperations.SendEmail2Carrier;
        if (this.Shipping2PartnerTitle != null)
          _escrt |= RequiredOperations.SendEmail2Escort;
      }
      switch (this.ShippingState.Value)
      {
        case Entities.ShippingState.WaitingForCarrierData:
          _ret = _cr;
          break;
        case Entities.ShippingState.WaitingForConfirmation:
          _ret = _escrt;
          break;
        default:
          _ret = _cr | _escrt;
          break;
      }
      return _ret;
    }
    public Distance CalculateDistance( out TimeSpan _ts )
    {
      TimeSpan _2h = new TimeSpan(2, 0, 0);
      TimeSpan _24h = new TimeSpan(24, 0, 0);
      TimeSpan _72h = new TimeSpan(3, 0, 0, 0);
      _ts = TimeSpan.Zero;
      if (this.StartTime.Value > DateTime.Now + _72h)
      {
        _ts = this.StartTime.Value - DateTime.Now - _72h;
        return Distance.UpTo72h;
      }
      else if (this.StartTime.Value > DateTime.Now + _24h)
      {
        _ts = this.StartTime.Value - DateTime.Now - _24h;
        return Distance.UpTo24h;
      }
      else if (this.StartTime.Value > DateTime.Now + _2h)
      {
        _ts = this.StartTime.Value - DateTime.Now - _2h;
        return Distance.UpTo2h;
      }
      else if (this.StartTime.Value > DateTime.Now)
      {
        _ts = this.StartTime.Value - DateTime.Now;
        return Distance.VeryClose;
      }
      else
        return Distance.Late;
    }
    public static bool InSet( RequiredOperations _set, RequiredOperations _item )
    {
      return (_set & _item) != 0;
    }
    public static TimeSpan WatchTolerance = new TimeSpan( 0, 15, 0 );
  }
  /// <summary>
  ///  Extend the Shipping autogenerated class
  /// </summary>
  public partial class TimeSlotTimeSlot
  {
    public const string NameOfIsDouble = "IsDouble";
    public static TimeSpan Span15min = new TimeSpan( 0, 15, 0 );
    public Warehouse GetWarehouse()
    {
      if (this.TimeSlot2ShippingPointLookup == null)
        throw new ApplicationException(m_ShippingNotFpundMessage);
      if (this.TimeSlot2ShippingPointLookup.WarehouseTitle == null)
        throw new ApplicationException("Warehouse not found");
      return this.TimeSlot2ShippingPointLookup.WarehouseTitle;
    }
    internal TimeSlotTimeSlot FindAdjacent(List<TimeSlotTimeSlot> _avlblTmslts)
    {
      for (int _i = 0; _i < _avlblTmslts.Count; _i++)
      {
        if ((_avlblTmslts[_i].StartTime.Value - this.EndTime.Value).Duration() <= Span15min)
          return _avlblTmslts[_i];
      }
      throw new ApplicationException("Cannot find the time slot to make the couple.");
    }
    private const string m_ShippingNotFpundMessage = "Shipping slot is not selected";
    public List<TimeSlotTimeSlot> MakeBooking( Shipping _sp, bool _isDouble )
    {
      if (this.Occupied.Value == Entities.Occupied.Occupied0)
        throw new ApplicationException("Time slot has been aleady reserved");
      List<TimeSlotTimeSlot> _ret = new List<TimeSlotTimeSlot>();
      _ret.Add(this);
      this.Occupied = Entities.Occupied.Occupied0;
      this.TimeSlot2ShippingIndex = _sp;
      this.IsDouble = _isDouble;
      //TODO cleanup commented code.
      //if (IsOutbound.Value)
      //  _ts.Tytuł = String.Format("Outbound No. {0} to {1}", this.Tytuł, this.City == null ? "--not assigned--" : City.Tytuł);
      //else
      //  _ts.Tytuł = String.Format("Inbound No. {0} by {1}", this.Tytuł, this.VendorName == null ? "--not assigned--" : VendorName.Tytuł);
      if (!_isDouble)
        return _ret;
      EntitySet<TimeSlot> _tslots = this.TimeSlot2ShippingPointLookup.TimeSlot;
      DateTime _tdy = this.StartTime.Value.Date;
      List<TimeSlotTimeSlot> _avlblTmslts = (from _tsidx in _tslots
                                             let _idx = _tsidx.StartTime.Value.Date
                                             where _tsidx.Occupied.Value == Entities.Occupied.Free && _idx >= _tdy && _idx <= _tdy.AddDays(1)
                                             orderby _tsidx.StartTime ascending
                                             select _tsidx).Cast<TimeSlotTimeSlot>().ToList<TimeSlotTimeSlot>();
      TimeSlotTimeSlot _next = this.FindAdjacent(_avlblTmslts);
      _ret.Add(_next);
      _next.Occupied = Entities.Occupied.Occupied0;
      _next.TimeSlot2ShippingIndex = _sp;
      _next.IsDouble = true;
      //this.EndTime = _next.EndTime;
      //this.Duration = Convert.ToDouble((_ts.EndTime.Value - _ts.EndTime.Value).TotalMinutes);
      return _ret;
    }
    public double? Duration()
    {
      if (!EndTime.HasValue || !StartTime.HasValue)
        return null;
      return (EndTime.Value - StartTime.Value).TotalMinutes;
    }
  }//TimeSlotTimeSlot
  /// <summary>
  /// Extensions
  /// </summary>
  public static class Extensions
  {
    #region public
    /// <summary>
    /// Convert int to string.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static string IntToString(this int? _val)
    {
      return _val.HasValue ? _val.Value.ToString() : String.Empty;
    }
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents <see cref="DateTime"/>.
    /// </summary>
    /// <param name="_val">The value to convert</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public static string ToString(this DateTime? _val)
    {
      return _val.HasValue ? _val.Value.ToString(CultureInfo.CurrentUICulture) : String.Empty;
    }
    /// <summary>
    /// Returns a <see cref="System.String"/> that represents this instance.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <param name="_format">The _format.</param>
    /// <returns>
    /// A <see cref="System.String"/> that represents this instance.
    /// </returns>
    public static string ToString(this DateTime? _val, string _format)
    {
      return _val.HasValue ? string.Format(_format, _val.Value.ToString(CultureInfo.CurrentUICulture)) : String.Empty;
    }
    internal const string UnknownEmail = "unknown@comapny.com";
    public static string UnknownIfEmpty(this String _val)
    {
      return String.IsNullOrEmpty(_val) ? UnknownEmail : _val;
    }
    /// <summary>
    /// String2s the int.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static int? String2Int(this string _val)
    {
      int _ret;
      if (int.TryParse(_val, out _ret))
      {
        return _ret;
      }
      return new Nullable<int>();
    }
    /// <summary>
    /// String2s the double.
    /// </summary>
    /// <param name="_val">The _val.</param>
    /// <returns></returns>
    public static double? String2Double(this string _val)
    {
      double _res = 0;
      if (double.TryParse(_val, out _res))
        return _res;
      else
        return null;
    }
    /// <summary>
    ///  Indicates whether the specified System.String object is null or an System.String.Empty string.
    /// </summary>
    /// <param name="_val"> A System.String reference.</param>
    /// <returns>
    ///   true if the value parameter is null or an empty string (""); otherwise, false.</c>.
    /// </returns>
    public static bool IsNullOrEmpty(this string _val)
    {
      return String.IsNullOrEmpty(_val);
    }
    #endregion
  }
}
