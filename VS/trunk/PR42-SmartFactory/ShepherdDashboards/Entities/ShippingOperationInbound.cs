﻿using System;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Shipping
  {
    public Shipping(bool _isOutbound, string _title, Partner _prtnr, Entities.State _state, Route _route, DateTime _deliveryTime, DateTime? _startTime)
      : this(_isOutbound, _title, _prtnr, _state, _startTime)
    {
      this.Route = _route;
      EstimateDeliveryTime = _deliveryTime;
    }
    public Shipping(bool _isOutbound, string _title, Partner _prtnr, Entities.State _state, DateTime? _startTime)
      : this()
    {
      IsOutbound = _isOutbound;
      Tytuł = _title;
      VendorName = _prtnr;
      State = _state;
      StartTime = _startTime;
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
  }
}
