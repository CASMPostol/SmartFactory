using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Shipping
  {
    public Shipping(Route _route, DateTime _deliveryTime, string _title, Partner _prtnr, Entities.State _state, DateTime? _startTime)
      : this()
    {
      this.Route = _route;
      EstimateDeliveryTime = _deliveryTime;
      Tytuł = _title;
      VendorName = _prtnr;
      State = _state;
      StartTime = _startTime;
    }
  }
}
