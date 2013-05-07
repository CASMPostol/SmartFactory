
//<summary>
//  Title   : Entities helper classes.
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Linq;
using System.Globalization;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities
{
  /// <summary>
  /// ActionResult
  /// </summary>
  public class ActionResult: List<string>
  {
    #region public
    internal bool Valid { get { return this.Count == 0; } }
    /// <summary>
    /// Adds the exception.
    /// </summary>
    /// <param name="_src">The _SRC.</param>
    /// <param name="_excptn">The _excptn.</param>
    public void AddException( string _src, Exception _excptn )
    {
      string _msg = String.Format("The operation interrupted at {0} by the error: {1}.", _src, _excptn.Message);
      base.Add(_msg);
    }
    /// <summary>
    /// Reports the action result.
    /// </summary>
    /// <param name="_url">The _url.</param>
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
    /// <summary>
    /// Adds the message.
    /// </summary>
    /// <param name="_src">The _SRC.</param>
    /// <param name="_message">The _message.</param>
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
    /// <summary>
    /// Resolves the change conflicts.
    /// </summary>
    /// <param name="rsult">The rsult.</param>
    /// <exception cref="System.ApplicationException"></exception>
    public void ResolveChangeConflicts( ActionResult rsult )
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
          rsult.AddMessage("ResolveChangeConflicts at: " + _cp, _tmp);
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
    /// <summary>
    /// The ID colunm name
    /// </summary>
    public const string IDColunmName = "ID";
    /// <summary>
    /// The title colunm name
    /// </summary>
    public const string TitleColunmName = "Title";
    /// <summary>
    /// The ID property name
    /// </summary>
    public const string IDPropertyName = "Identyfikator";
    /// <summary>
    /// The title property name
    /// </summary>
    internal const string TitlePropertyName = "Tytuł";
    /// <summary>
    /// Try to get at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="list">The _list.</param>
    /// <param name="id">The id.</param>
    /// <returns>
    /// An instance of the <typeparamref name="t"/> for the selected index or null if <paramref name="id" /> is null or empty.
    /// </returns>
    /// <exception cref="ApplicationException">Element cannot be found.</exception>
    public static t TryGetAtIndex<t>( EntityList<t> list, string id )
      where t : Element
    {
      if (id.IsNullOrEmpty())
        return null;
      return GetAtIndex<t>(list, id);
    }
    /// <summary>
    /// Gets at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="id">The id.</param>
    /// <returns>
    /// An instance of the <typeparamref name="t"/> for the selected index.
    /// </returns>
    /// <exception cref="System.ApplicationException">
    /// </exception>
    /// <exception cref="ApplicationException">id is nuul or element cannot be found.</exception>
    public static t GetAtIndex<t>( EntityList<t> list, string id )
      where t : Element
    {
      int? _index = id.String2Int();
      if (!_index.HasValue)
        throw new ApplicationException(typeof(t).Name + " index is null"); ;
      try
      {
        return (
              from idx in list
              where idx.Identyfikator == _index.Value
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("{0} cannot be found at specified index{1}", typeof(t).Name, _index.Value));
      }
    }
    /// <summary>
    /// Finds at index.
    /// </summary>
    /// <typeparam name="t"></typeparam>
    /// <param name="list">The list.</param>
    /// <param name="id">The id.</param>
    /// <returns></returns>
    public static t FindAtIndex<t>( EntityList<t> list, string id )
      where t : Element
    {
      int? _index = id.String2Int();
      if (!_index.HasValue)
        return null;
      try
      {
        return (
              from idx in list
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
    /// <param name="title">The evrnt title.</param>
    /// <param name="partner">The partner associated with the event.</param>
    /// <param name="shippingIndex">Index of the shipping.</param>
    public AlarmsAndEvents(string title, Partner partner, Shipping shippingIndex)
    {
      Tytuł = title;
      this.AlarmsAndEventsList2PartnerTitle = partner;
      this.AlarmsAndEventsList2Shipping = shippingIndex;
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
    /// <summary>
    /// Gets the email.
    /// </summary>
    /// <param name="ccRole">The cc role.</param>
    /// <param name="EDC">The EDC.</param>
    /// <returns></returns>
    public static string GetEmail( ShepherdRole ccRole, EntitiesDataContext EDC )
    {
      var _ccdl = (from _ccx in EDC.DistributionList
                   where _ccx.ShepherdRole.GetValueOrDefault(Entities.ShepherdRole.Invalid) == ccRole
                   select new { Email = _ccx.EmailAddress }).FirstOrDefault();
      if (_ccdl == null || String.IsNullOrEmpty(_ccdl.Email))
        _ccdl = (from _ccx in EDC.DistributionList
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
    /// <summary>
    /// Creates the cities.
    /// </summary>
    /// <param name="EDC">The EDC.</param>
    public static void CreateCities(EntitiesDataContext EDC)
    {
      for (int i = 0; i < 10; i++)
      {
        CityType _cmm = new CityType() { Tytuł = String.Format("City {0}", i) };
        EDC.City.InsertOnSubmit(_cmm);
        EDC.SubmitChanges();
      }
    }
  }
  /// <summary>
  /// Extend the Partner autogenerated class 
  /// </summary>
  public partial class Partner
  {
    /// <summary>
    /// Finds for user.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="user">The user.</param>
    /// <returns></returns>
    public static Partner FindForUser( EntitiesDataContext edc, SPUser user )
    {
      if (edc.Partner == null)
        return null;
      else
        return edc.Partner.FirstOrDefault(idx => idx.ShepherdUserTitle.IsNullOrEmpty() ? false : idx.ShepherdUserTitle.Contains(user.Name));
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

    /// <summary>
    /// Changes the rout.
    /// </summary>
    /// <param name="nr">The nr.</param>
    /// <param name="EDC">The EDC.</param>
    public void ChangeRout( Route nr, EntitiesDataContext EDC )
    {
      if (this.Shipping2RouteTitle == nr)
        return;
      this.TrailerTitle = null;
      this.TruckTitle = null;
      EDC.SubmitChanges();
      RemoveDrivers(EDC, this.PartnerTitle);
      this.Shipping2RouteTitle = nr;
      if (nr == null)
      {
        this.BusinessDescription = String.Empty;
        this.PartnerTitle = null;
        return;
      }
      this.BusinessDescription = Shipping2RouteTitle.Route2BusinessDescriptionTitle == null ? String.Empty : Shipping2RouteTitle.Route2BusinessDescriptionTitle.Tytuł;
      this.PartnerTitle = Shipping2RouteTitle.PartnerTitle;
    }
    /// <summary>
    /// Changes the escort.
    /// </summary>
    /// <param name="nr">The nr.</param>
    /// <param name="EDC">The EDC.</param>
    public void ChangeEscort( SecurityEscortCatalog nr, EntitiesDataContext EDC )
    {
      if (this.SecurityEscortCatalogTitle == nr)
        return;
      this.Shipping2TruckTitle = null;
      EDC.SubmitChanges();
      RemoveDrivers(EDC, this.Shipping2PartnerTitle);
      this.SecurityEscortCatalogTitle = nr;
      if (nr == null)
      {
        this.Shipping2PartnerTitle = null;
        return;
      }
      this.SecurityEscortCatalogTitle = nr;
      this.Shipping2PartnerTitle = nr == null ? null : nr.PartnerTitle;
    }
    /// <summary>
    /// Determines whether this instance is editable.
    /// </summary>
    /// <returns>
    ///   <c>true</c> if this instance is editable; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="System.ApplicationException">Wrong Shipping state</exception>
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
    /// <summary>
    /// Fixeds this instance.
    /// </summary>
    /// <returns></returns>
    public bool Fixed()
    {
      return this.StartTime.Value - _12h < DateTime.Now;
    }
    /// <summary>
    /// Releases the booking.
    /// </summary>
    /// <param name="_newTimeSlot">The _new time slot.</param>
    /// <returns></returns>
    /// <exception cref="System.ApplicationException"></exception>
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
    /// <summary>
    /// Setups the timing.
    /// </summary>
    /// <param name="_ts">The _TS.</param>
    /// <param name="_isDouble">if set to <c>true</c> [_is double].</param>
    public void SetupTiming( TimeSlotTimeSlot _ts, bool _isDouble )
    {
      this.StartTime = _ts.StartTime;
      this.EndTime = _ts.EndTime;
      this.ShippingDuration = Convert.ToDouble((_ts.EndTime.Value - _ts.StartTime.Value).TotalMinutes);
      this.Shipping2WarehouseTitle = _ts.GetWarehouse();
      this.LoadingType = _isDouble ? Entities.LoadingType.Manual : Entities.LoadingType.Pallet;
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    public void UpdateTitle()
    {
      string _tf = "{0}{1:D6}";
      Tytuł = String.Format(_tf, IsOutbound.Value ? "O" : "I", Identyfikator.Value);
    }
    /// <summary>
    /// Calculates the state.
    /// </summary>
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
    /// <summary>
    /// RequiredOperations
    /// </summary>
    [Flags]
    public enum RequiredOperations
    {
      /// <summary>
      /// The send email to carrier
      /// </summary>
      SendEmail2Carrier = 0x01,
      /// <summary>
      /// The send email to escort
      /// </summary>
      SendEmail2Escort = 0x02,
      /// <summary>
      /// The add alarm to carrier
      /// </summary>
      AddAlarm2Carrier = 0x04,
      /// <summary>
      /// The add alarm to escort
      /// </summary>
      AddAlarm2Escort = 0x10
    }
    /// <summary>
    /// The carrier operations
    /// </summary>
    public const RequiredOperations CarrierOperations = Shipping.RequiredOperations.AddAlarm2Carrier | Shipping.RequiredOperations.SendEmail2Carrier;
    /// <summary>
    /// Time distance
    /// </summary>
    public enum Distance 
    {
      /// <summary>
      /// Up to 72h
      /// </summary>
      UpTo72h,
      /// <summary>
      /// Up to 24h
      /// </summary>
      UpTo24h,
      /// <summary>
      /// Up to 2h
      /// </summary>
      UpTo2h,
      /// <summary>
      /// The very close
      /// </summary>
      VeryClose,
      /// <summary>
      /// The late
      /// </summary>
      Late 
    }
    /// <summary>
    /// Calculates the operations2 do.
    /// </summary>
    /// <param name="email">if set to <c>true</c> [email].</param>
    /// <param name="alarm">if set to <c>true</c> [alarm].</param>
    /// <param name="TimeOutExpired">if set to <c>true</c> [time out expired].</param>
    /// <returns></returns>
    public RequiredOperations CalculateOperations2Do( bool email, bool alarm, bool TimeOutExpired )
    {
      RequiredOperations _ret = 0;
      if (!TimeOutExpired)
        return _ret;
      RequiredOperations _cr = 0;
      RequiredOperations _escrt = 0;
      if (alarm)
      {
        //Carrier
        if (this.PartnerTitle != null)
          _cr = RequiredOperations.AddAlarm2Carrier;
        //Escort
        if (this.Shipping2PartnerTitle != null)
          _escrt = RequiredOperations.AddAlarm2Escort;
      }
      if (email)
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
    /// <summary>
    /// Calculates the distance.
    /// </summary>
    /// <param name="ts">The ts.</param>
    /// <returns></returns>
    public Distance CalculateDistance( out TimeSpan ts )
    {
      TimeSpan _2h = new TimeSpan(2, 0, 0);
      TimeSpan _24h = new TimeSpan(24, 0, 0);
      TimeSpan _72h = new TimeSpan(3, 0, 0, 0);
      ts = TimeSpan.Zero;
      if (this.StartTime.Value > DateTime.Now + _72h)
      {
        ts = this.StartTime.Value - DateTime.Now - _72h;
        return Distance.UpTo72h;
      }
      else if (this.StartTime.Value > DateTime.Now + _24h)
      {
        ts = this.StartTime.Value - DateTime.Now - _24h;
        return Distance.UpTo24h;
      }
      else if (this.StartTime.Value > DateTime.Now + _2h)
      {
        ts = this.StartTime.Value - DateTime.Now - _2h;
        return Distance.UpTo2h;
      }
      else if (this.StartTime.Value > DateTime.Now)
      {
        ts = this.StartTime.Value - DateTime.Now;
        return Distance.VeryClose;
      }
      else
        return Distance.Late;
    }
    /// <summary>
    /// Ins the set.
    /// </summary>
    /// <param name="set">The set.</param>
    /// <param name="item">The item.</param>
    /// <returns></returns>
    public static bool InSet( RequiredOperations set, RequiredOperations item )
    {
      return (set & item) != 0;
    }
    /// <summary>
    /// The watch tolerance
    /// </summary>
    public static TimeSpan WatchTolerance = new TimeSpan( 0, 15, 0 );
  }
  /// <summary>
  ///  Extend the Shipping autogenerated class
  /// </summary>
  public partial class TimeSlotTimeSlot
  {
    /// <summary>
    /// The name of is double
    /// </summary>
    public const string NameOfIsDouble = "IsDouble";
    /// <summary>
    /// The span15min
    /// </summary>
    public static TimeSpan Span15min = new TimeSpan( 0, 15, 0 );
    /// <summary>
    /// Gets the warehouse.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.ApplicationException">
    /// Warehouse not found
    /// </exception>
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
    /// <summary>
    /// Makes the booking.
    /// </summary>
    /// <param name="_sp">The _SP.</param>
    /// <param name="_isDouble">if set to <c>true</c> [_is double].</param>
    /// <returns></returns>
    /// <exception cref="System.ApplicationException">Time slot has been aleady reserved</exception>
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
    /// <summary>
    /// Durations this instance.
    /// </summary>
    /// <returns></returns>
    public double? Duration()
    {
      if (!EndTime.HasValue || !StartTime.HasValue)
        return null;
      return (EndTime.Value - StartTime.Value).TotalMinutes;
    }
    /// <summary>
    /// The m_ shipping not fpund message
    /// </summary>
    private const string m_ShippingNotFpundMessage = "Shipping slot is not selected";
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
    /// <summary>
    /// Unknowns if empty.
    /// </summary>
    /// <param name="val">The val.</param>
    /// <returns></returns>
    public static string UnknownIfEmpty(this String val)
    {
      return String.IsNullOrEmpty(val) ? UnknownEmail : val;
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
    ///   true if the value parameter is null or an empty string (""); otherwise, false.
    /// </returns>
    public static bool IsNullOrEmpty(this string _val)
    {
      return String.IsNullOrEmpty(_val);
    }
    #endregion
  }
}
