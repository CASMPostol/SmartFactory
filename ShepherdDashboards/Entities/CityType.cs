using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class CityType
  {
    internal static CityType GetdAtIndex(EntitiesDataContext edc, int? _index)
    {
      if (!_index.HasValue)
        throw new ApplicationException("CityType index is null"); ;
      try
      {
        return (
              from idx in edc.City
              where idx.Identyfikator == _index.Value
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException(String.Format("CityType item cannot be found at specified index{0}", _index.Value));
      }
    }
    public static void CreateCities(EntitiesDataContext _EDC)
    {
      for (int i = 0; i < 10; i++)
      {
        CityType _cmm = new CityType() { Tytuł = String.Format("City {0}", i) };
        _EDC.City.InsertOnSubmit(_cmm);
        _EDC.SubmitChanges();
      }
    }
  }
}
