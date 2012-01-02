using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Truck
  {
    internal static IQueryable<Truck> GetAllForUser(EntitiesDataContext edc, int _partner)
    {
      try
      {
        return from idx in edc.Truck
               where idx.VendorName.Identyfikator == _partner
               //orderby idx.Tytuł
               select idx;
      }
      catch (Exception ex)
      {
        throw new ApplicationException( "Truck.GetAllForUserex failed: " + ex.Message);
      }
    }
    internal static Truck GetAtIndex(EntitiesDataContext edc, string _ID)
    {
      try
      {
        int? _intID = _ID.String2Int();
        return (from idx in edc.Truck
               where idx.Identyfikator == _intID
               select idx).First();
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Truck.GetAtIndex failed: " + ex.Message);
      }
    }

  }
}
