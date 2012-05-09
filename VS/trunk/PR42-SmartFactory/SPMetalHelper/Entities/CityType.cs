using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class CityType
  {
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
