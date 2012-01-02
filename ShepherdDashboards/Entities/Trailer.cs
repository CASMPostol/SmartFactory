using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class Trailer
  {
    internal static Trailer FindTrailer(EntitiesDataContext edc, int? _index)
    {
      if (!_index.HasValue)
        return null;
      try
      {
        return (
              from idx in edc.Trailer
              where idx.Identyfikator == _index.Value
              select idx).First();
      }
      catch (Exception)
      {
        return null;
      }
    }
    internal static IQueryable<Trailer> GetAllForUser(EntitiesDataContext edc, int _partner)
    {
      try
      {
        return from idx in edc.Trailer
               where idx.VendorName.Identyfikator == _partner
               //orderby idx.Tytuł
               select idx;
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Trailer.GetAllForUser failed: " + ex.Message);
      }
    }

    internal static Trailer GetAtIndex(EntitiesDataContext edc, string _ID)
    {
      int? _intID = _ID.String2Int();
      try
      {
        return (from idx in edc.Trailer
               where idx.Identyfikator == _intID
               select idx).First();
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Trailer.GetAtIndex failed: " + ex.Message);
      }
    }
  }
}
