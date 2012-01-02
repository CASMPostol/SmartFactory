using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Driver
  {
    internal static Driver Find(EntitiesDataContext edc, int? _index)
    {
      if (!_index.HasValue)
        return null;
      try
      {
        return (
              from idx in edc.Driver
              where idx.Identyfikator == _index
              select idx).First();
      }
      catch (Exception)
      {
        return null;
      }
    }
    internal static IQueryable<Driver> GetAllForUser(EntitiesDataContext edc, int _partner)
    {
      try
      {
        IQueryable<Driver> _ret = from idx in edc.Driver
                                  where idx.VendorName.Identyfikator == _partner
                                  orderby idx.Tytuł ascending
                                  select idx;
        return _ret;
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Driver.GetAllForUser failed: " + ex.Message);
      }
    }
    internal static IEnumerable<Driver> GetAllForShipping(EntitiesDataContext edc, int _shippingIdx)
    {
      try
      {
        return from idx in edc.DriversTeam
               where idx.ShippingIndex.Identyfikator == _shippingIdx
               select idx.Driver;
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Driver.GetAllForShipping failed: " + ex.Message);
      }
    }
    internal static Driver GetAtIndex(EntitiesDataContext edc, string _index)
    {
      if (String.IsNullOrEmpty(_index))
        throw new ApplicationException("Driver not found because the index is null");
      int _intIndex = int.Parse(_index);
      try
      {
        return (
              from idx in edc.Driver
              where idx.Identyfikator == _intIndex
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException(string.Format("Driver not found at index {0}", _index));
      }
    }
  }
}
