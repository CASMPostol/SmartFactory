using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public static class Extensions
  {
    public static ProductType ParseProductType( this string entry)
    {
      try
      {
        return (ProductType)Enum.Parse(typeof(Entities.ProductType), entry);
      }
      catch (Exception)
      {
        return ProductType.None;
      }
    }
  }
}
