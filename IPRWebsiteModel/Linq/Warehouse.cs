using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Warehouse
  {
    public static Warehouse Find( Entities edc, string index )
    {
      Warehouse newWarehouse = null;
      try
      {
        newWarehouse = (from Warehouse item in edc.Warehouse where item.Title.Contains(index) select item).First<Warehouse>();
      }
      catch (Exception) { }
      return newWarehouse;
    }
  }
}
