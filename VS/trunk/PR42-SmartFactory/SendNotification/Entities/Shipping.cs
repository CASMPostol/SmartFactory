using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification.Entities
{
  public partial class Shipping
  {
    [Flags]
    internal enum RequiredOperations
    {
      SendEmail2Carrier,
      SendEmail2Escort,
      AddAlarm2Carrier,
      AddAlarm2Escort
    }
    internal const RequiredOperations CarrierOperations = Shipping.RequiredOperations.AddAlarm2Carrier | Shipping.RequiredOperations.SendEmail2Carrier;
    internal enum Distance { UpTo72h, UpTo24h, UpTo2h, VeryClose, Late }
    internal RequiredOperations CalculateOperations2Do(bool _email, bool _alarm)
    {
      RequiredOperations _ret = 0;
      RequiredOperations _cr = 0;
      RequiredOperations _escrt = 0;
      if (_alarm)
      {
        if (this.VendorName != null)
          _cr = RequiredOperations.AddAlarm2Carrier;
        if (this.SecurityEscortProvider != null)
          _escrt = RequiredOperations.AddAlarm2Escort;
      }
      if (_email)
      {
        if (this.VendorName != null)
          _cr |= RequiredOperations.SendEmail2Carrier;
        if (this.SecurityEscortProvider != null)
          _escrt |= RequiredOperations.SendEmail2Escort;
      }
      switch (this.State.Value)
      {
        case Entities.State.Confirmed:
        case Entities.State.Creation:
        case Entities.State.Delayed:
        case Entities.State.Underway:
          _ret = _cr | _escrt;
          break;
        case Entities.State.WaitingForCarrierData:
          _ret = _cr;
          break;
        case Entities.State.WaitingForSecurityData:
          _ret = _escrt;
          break;
        default:
          break;
      }
      return _ret;
    }
    internal Distance CalculateDistance(out TimeSpan _ts)
    {
      TimeSpan _2h = new TimeSpan(2, 0, 0);
      TimeSpan _24h = new TimeSpan(24, 0, 0);
      TimeSpan _72h = new TimeSpan(3, 0, 0, 0);
      _ts = TimeSpan.Zero;
      if (this.StartTime.Value > DateTime.Now + _72h)
      {
        _ts = this.StartTime.Value - DateTime.Now - _24h;
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
      else if (this.StartTime.Value > DateTime.Now - WatchTolerance)
      {
        _ts = this.StartTime.Value - DateTime.Now + WatchTolerance;
        return Distance.VeryClose;
      }
      else
        return Distance.Late;
    }
    internal static bool InSet(RequiredOperations _set, RequiredOperations _item)
    {
      return (_set & _item) != 0;
    }
    internal static TimeSpan WatchTolerance = new TimeSpan(0, 15, 0);
  }
}
