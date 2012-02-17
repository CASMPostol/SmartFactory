using System;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Shipping
  {
    public Shipping(bool _isOutbound, string _title, Partner _prtnr, Entities.State _state, Route _route, DateTime _deliveryTime, DateTime? _startTime, CityType _city, TransportUnitTypeTranspotUnit _tut, Warehouse _wrs)
      : this(_isOutbound, _title, _prtnr, _state, _startTime, _wrs)
    {
      ChangeRout(_route);
      this.EstimateDeliveryTime = _deliveryTime;
      this.City = _city;
      this.TransportUnit = _tut;
    }
    public Shipping(bool _isOutbound, string _title, Partner _prtnr, Entities.State _state, DateTime? _startTime, Warehouse _wrs)
      : this()
    {
      IsOutbound = _isOutbound;
      Tytuł = _title;
      VendorName = _prtnr;
      State = _state;
      StartTime = _startTime;
      this.Warehouse = _wrs.Tytuł;
    }
    internal void ChangeRout(Route _nr)
    {
      if (this.Route == null)
      {
        this.BusinessDescription = String.Empty;
        this.VendorName = null;
        return;
      }
      this.BusinessDescription = Route.BusinessDescription == null ? String.Empty : Route.BusinessDescription.Tytuł;
      this.VendorName = Route.VendorName;
    }
    internal static Shipping GetAtIndex(EntitiesDataContext edc, int? _index)
    {
      if (!_index.HasValue)
        throw new ApplicationException("ShippingOperationInbound index is null"); ;
      try
      {
        return (
              from idx in edc.Shipping
              where idx.Identyfikator == _index.Value
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("ShippingOperationInbound cannot be found at specified index{0}", _index.Value));
      }
    }
    internal bool IsEditable()
    {
      switch (this.State.Value)
      {

        case Entities.State.Canceled:
        case Entities.State.Completed:
          return false;
        case Entities.State.Confirmed:
        case Entities.State.Creation:
        case Entities.State.Delayed:
        case Entities.State.Waiting4ExternalApproval:
        case Entities.State.Waiting4InternalApproval:
        case Entities.State.Underway:
          return true;
        case Entities.State.Invalid:
        case Entities.State.None:
        default:
          throw new ApplicationException("Wrong Shipping state");
      }
    }
  }
}
