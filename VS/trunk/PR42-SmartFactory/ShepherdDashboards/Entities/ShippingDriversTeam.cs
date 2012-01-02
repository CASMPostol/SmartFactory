using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Dashboards.Entities
{
  public partial class ShippingDriversTeam
  {
    internal static ShippingDriversTeam GetAtIndex(EntitiesDataContext edc, string _index)
    {
      if (String.IsNullOrEmpty(_index))
        throw new ApplicationException("Team member not found because the index is null");
      int _intIndex = int.Parse(_index);
      try
      {
        return (
              from idx in edc.DriversTeam
              where idx.Identyfikator == _intIndex
              select idx).First();
      }
      catch (Exception)
      {
        throw new ApplicationException("Team member not found");
      }
    }
  }
}
