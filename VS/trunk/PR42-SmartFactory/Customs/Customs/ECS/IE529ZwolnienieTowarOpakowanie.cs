using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs.IE529
{
  public partial class IE529ZwolnienieTowarOpakowanie: PackageDescription
  {
    public override double? GetItemNo()
    {
      return null;
    }
    public override string GetPackage()
    {
      return this.Rodzaj;
    }
  }
}
