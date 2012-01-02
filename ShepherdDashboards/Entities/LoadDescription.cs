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
      int? _intIndex = _shippingIndex.String2Int();
      if (!_intIndex.HasValue)
        throw new ApplicationException("Partner not found because the index is null");
      try
      {
        return (from idx in edc.LoadDescription
                where idx.ShippingIndex.Identyfikator == _intIndex
                select idx).ToList<LoadDescription>();
      }
      catch (Exception)
      {
        throw new ApplicationException("Partner not found");
      }
    }
  }
}
