using System;
using System.Linq;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class CustomsUnion
  {
    internal static bool CheckIfUnion(string primeMarket, EntitiesDataContext edc)
    {
      try
      {
        CustomsUnion cu = (from item in edc.CustomsUnion where item.EUPrimeMarket.Contains(primeMarket) select item).DefaultIfEmpty(null).First();
        return true;
      }
      catch (Exception)
      {
        return false;
      };
    }
  }
}
