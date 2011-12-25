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
              where idx.Identyfikator == _index.Value
              select idx).First();
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
