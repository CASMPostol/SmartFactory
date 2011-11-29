using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs.PZC
{
  public partial class PZCZwolnienieDoProceduryTowarOpakowanie : PackageDescription
  {
    public override double? GetItemNo()
    {
      return Convert.ToDouble(this.PozId);
    }
    public override string GetPackage()
    {
      return this.Rodzaj;
    }
  }
}
