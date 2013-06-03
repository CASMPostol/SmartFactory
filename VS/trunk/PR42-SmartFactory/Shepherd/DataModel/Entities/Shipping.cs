﻿//<summary>
//  Title   : class Shipping
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
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Linq;
using Microsoft.SharePoint.Utilities;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities
{
  /// <summary>
  /// Extend the Shipping autogenerated class
  /// </summary>
  public partial class Shipping
  {

    #region public
    /// <summary>
    /// Changes the rout.
    /// </summary>
    /// <param name="nr">The nr.</param>
    /// <param name="EDC">The EDC.</param>
    public void ChangeRout( Route nr, EntitiesDataContext EDC )
    {
      if ( this.Shipping2RouteTitle == nr )
        return;
      this.TrailerTitle = null;
      this.TruckTitle = null;
      EDC.SubmitChanges();
      RemoveDrivers( EDC, this.PartnerTitle );
      this.Shipping2RouteTitle = nr;
      if ( nr == null )
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
      if ( this.SecurityEscortCatalogTitle == nr )
        return;
      this.Shipping2TruckTitle = null;
      EDC.SubmitChanges();
      RemoveDrivers( EDC, this.Shipping2PartnerTitle );
      this.SecurityEscortCatalogTitle = nr;
      if ( nr == null )
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
      switch ( this.ShippingState.Value )
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
          throw new ApplicationException( "Wrong Shipping state" );
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
    public void ReleaseBooking()
    {
      if ( this.TimeSlot == null || this.TimeSlot.Count == 0 )
        return;
      List<TimeSlot> _2release = new List<TimeSlot>();
      foreach ( var item in this.TimeSlot )
      {
        if ( item.Occupied != Entities.Occupied.Occupied0 )
          continue;
        _2release.Add( item );
      }
      foreach ( var item in _2release )
      {
        //item.Tytuł = "-- not assigned --";
        if ( Fixed() )
          item.Occupied = Entities.Occupied.Delayed;
        else
        {
          item.Occupied = Entities.Occupied.Free;
          item.TimeSlot2ShippingIndex = null;
          item.IsDouble = false;
        }
      }
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    public void UpdateTitle()
    {
      string _tf = "{0}{1:D6}";
      Tytuł = String.Format( _tf, IsOutbound.Value ? "O" : "I", Identyfikator.Value );
    }
    /// <summary>
    /// Calculates the state.
    /// </summary>
    public void CalculateState()
    {
      switch ( this.ShippingState.GetValueOrDefault( Entities.ShippingState.None ) )
      {
        case Entities.ShippingState.Confirmed:
        case Entities.ShippingState.Creation:
        case Entities.ShippingState.Delayed:
        case Entities.ShippingState.WaitingForCarrierData:
        case Entities.ShippingState.WaitingForConfirmation:
          int _seDrivers = 0;
          int _crDrivers = 0;
          foreach ( var _dr in this.ShippingDriversTeam )
            if ( _dr.DriverTitle.Driver2PartnerTitle.ServiceType.Value == ServiceType.SecurityEscortProvider )
              _seDrivers++;
            else
              _crDrivers++;
          this.ShippingState = Entities.ShippingState.Creation;
          if ( _crDrivers > 0 && this.TruckTitle != null )
          {
            if ( this.SecurityEscortCatalogTitle == null || ( _seDrivers > 0 && this.Shipping2TruckTitle != null ) )
              this.ShippingState = Entities.ShippingState.Confirmed;
            else
              this.ShippingState = Entities.ShippingState.WaitingForConfirmation;
          }
          else if ( this.SecurityEscortCatalogTitle == null || ( _seDrivers > 0 && this.Shipping2TruckTitle != null ) )
            this.ShippingState = Entities.ShippingState.WaitingForCarrierData;
          break;
        case Entities.ShippingState.None:
          this.ShippingState = Entities.ShippingState.Creation;
          break;
        case Entities.ShippingState.Underway:
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
      if ( !TimeOutExpired )
        return _ret;
      RequiredOperations _cr = 0;
      RequiredOperations _escrt = 0;
      if ( alarm )
      {
        //Carrier
        if ( this.PartnerTitle != null )
          _cr = RequiredOperations.AddAlarm2Carrier;
        //Escort
        if ( this.Shipping2PartnerTitle != null )
          _escrt = RequiredOperations.AddAlarm2Escort;
      }
      if ( email )
      {
        if ( this.PartnerTitle != null )
          _cr |= RequiredOperations.SendEmail2Carrier;
        if ( this.Shipping2PartnerTitle != null )
          _escrt |= RequiredOperations.SendEmail2Escort;
      }
      switch ( this.ShippingState.Value )
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
      TimeSpan _2h = new TimeSpan( 2, 0, 0 );
      TimeSpan _24h = new TimeSpan( 24, 0, 0 );
      TimeSpan _72h = new TimeSpan( 3, 0, 0, 0 );
      ts = TimeSpan.Zero;
      if ( this.StartTime.Value > DateTime.Now + _72h )
      {
        ts = this.StartTime.Value - DateTime.Now - _72h;
        return Distance.UpTo72h;
      }
      else if ( this.StartTime.Value > DateTime.Now + _24h )
      {
        ts = this.StartTime.Value - DateTime.Now - _24h;
        return Distance.UpTo24h;
      }
      else if ( this.StartTime.Value > DateTime.Now + _2h )
      {
        ts = this.StartTime.Value - DateTime.Now - _2h;
        return Distance.UpTo2h;
      }
      else if ( this.StartTime.Value > DateTime.Now )
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
      return ( set & item ) != 0;
    }
    /// <summary>
    /// The watch tolerance
    /// </summary>
    public static TimeSpan WatchTolerance = new TimeSpan( 0, 15, 0 );
    /// <summary>
    /// Makes the booking.
    /// </summary>
    /// <param name="timeSlot">The time slot.</param>
    /// <param name="isDouble">if set to <c>true</c> [_is double].</param>
    /// <returns></returns>
    /// <exception cref="System.ApplicationException">Time slot has been aleady reserved</exception>
    public List<TimeSlotTimeSlot> MakeBooking( List<TimeSlotTimeSlot> timeSlotsCollection, bool isDouble )
    {
      StartTime = timeSlotsCollection[ 0 ].StartTime;
      TSStartTime = timeSlotsCollection[ 0 ].StartTime;
      Shipping2WarehouseTitle = timeSlotsCollection[ 0 ].GetWarehouse();
      TimeSlotTimeSlot _next = timeSlotsCollection[ 0 ];
      foreach ( TimeSlotTimeSlot _tsx in timeSlotsCollection )
      {
        _tsx.TimeSlot2ShippingIndex = this;
        _next = _tsx;
      }
      EndTime = _next.EndTime;
      TSEndTime = _next.EndTime;
      ShippingDuration = Convert.ToDouble( ( _next.EndTime.Value - this.StartTime.Value ).TotalMinutes );
      LoadingType = isDouble ? Entities.LoadingType.Manual : Entities.LoadingType.Pallet;
      return timeSlotsCollection;
    }
    /// <summary>
    /// Creates the shipping.
    /// </summary>
    /// <param name="outbound">if set to <c>true</c> [outbound].</param>
    /// <returns></returns>
    public static Shipping CreateShipping( bool outbound )
    {
      return new Shipping()
      {
        AdditionalCosts = 0,
        EditorIdentyfikator = 0,
        TotalCostsPerKU = 0,
        TotalQuantityKU = 0,
        TrailerCondition = Entities.TrailerCondition._5Excellent,
        InduPalletsQuantity = 0,
        ShippingSecurityCost = 0,
        EuroPalletsQuantity = 0,
        ShippingDuration = 0,
        ShippingState = Entities.ShippingState.Invalid,
        TruckAwaiting = false,
        IsOutbound = outbound,
        Tytuł = "Creating new shippment"
      };
    }
    /// <summary>
    /// Updates the purchase order info.
    /// </summary>
    public void UpdatePOInfo()
    {
      this.PoLastModification = DateTime.Now;
      StringBuilder _po = new StringBuilder();
      foreach ( LoadDescription _ldx in LoadDescription )
        _po.AppendLine( SPEncode.HtmlEncode( _ldx.Tytuł ) );
      this.PoNumberMultiline = _po.ToString();
    }
    #endregion

    #region private
    private TimeSpan _12h = new TimeSpan( 12, 0, 0 );
    private void RemoveDrivers( EntitiesDataContext EDC, Partner partner )
    {
      if ( partner == null )
        return;
      List<ShippingDriversTeam> _2Delete = new List<ShippingDriversTeam>();
      foreach ( ShippingDriversTeam _drv in this.ShippingDriversTeam )
        if ( partner == _drv.DriverTitle.Driver2PartnerTitle )
          _2Delete.Add( _drv );
      EDC.DriversTeam.DeleteAllOnSubmit( _2Delete );
      EDC.SubmitChanges();
    }
    #endregion

  }
}
