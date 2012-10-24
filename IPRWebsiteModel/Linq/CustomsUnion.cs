using System;
using System.Linq;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class CustomsUnion
  {
    internal static bool? CheckIfUnion(string primeMarket, Entities edc)
    {
      if (String.IsNullOrEmpty(primeMarket))
        throw new InvalidProgramException("CustomsUnion.CheckIfUnion the primeMarket parameter cannot be null");
      return (from item in edc.CustomsUnion where item.EUPrimeMarket.Contains(primeMarket) select item).Any();
    }
  }
}
