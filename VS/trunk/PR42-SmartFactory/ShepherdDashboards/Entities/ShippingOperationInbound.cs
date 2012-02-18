using System;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Shipping
  {
    internal void ChangeRout(Route _nr)
    {
      this.Route = _nr;
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
    internal void ReleaseBooking()
    {
      if (this.TimeSlot == null || this.TimeSlot.Count == 0)
        return;
      foreach (var item in this.TimeSlot)
      {
        if (item.Occupied != Entities.Occupied.Occupied0)
          continue;
        item.Occupied = Entities.Occupied.Free;
        item.ShippingIndex = null;
        item.IsDouble = false;
      }
    }
  }
}
