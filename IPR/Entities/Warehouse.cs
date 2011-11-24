using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Warehouse
  {
    internal static Warehouse GetLookup(EntitiesDataContext edc, string index)
    {
      Warehouse newWarehouse  = null;
      try
      {
        newWarehouse = (from Warehouse item in edc.Warehouse where item.Tytuł.Contains(index) select item).First<Warehouse>();
      }
      catch (Exception) { }
      return newWarehouse;
    }
  }
}
