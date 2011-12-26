using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class LoadDescription
  {
    internal static List<LoadDescription> GetForShipping(EntitiesDataContext edc, string _shippingIndex)
    {
      if (String.IsNullOrEmpty(_shippingIndex))
        throw new ApplicationException("Partner not found because the index is null");
      int _intIndex = int.Parse(_shippingIndex);
      try
      {
        var ret =
              from idx in edc.LoadDescription
              where idx.ShippingIndex.Identyfikator == _intIndex
              select idx;
        return new List<LoadDescription>(ret);
      }
      catch (Exception)
      {
        throw new ApplicationException("Partner not found");
      }
    }
  }
}
