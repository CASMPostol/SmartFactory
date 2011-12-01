using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Entities
{
  public static class Extensions
  {
    public static ProductType ParseProductType(this string entry)
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
    public static CompensationGood? ParseCompensationGood(this string entry)
    {
      try
      {
        return String.IsNullOrEmpty(entry) ? new Nullable<CompensationGood>() : (CompensationGood)Enum.Parse(typeof(Entities.CompensationGood), entry);
      }
      catch (Exception)
      {
        return CompensationGood.Invalid;
      }
    }
  }
}
