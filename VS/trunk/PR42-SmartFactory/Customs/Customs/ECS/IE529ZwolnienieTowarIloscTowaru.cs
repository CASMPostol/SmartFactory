using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs.IE529
{
  public partial class IE529ZwolnienieTowarIloscTowaru : QuantityDescription
  {
    public override double? GetItemNo()
    {
      return null;
    }
    public override double? GetNetMass()
    {
      return Convert.ToDouble(this.Ilosc);
    }
    public override string GetUnits()
    {
      return this.Jm;
    }
  }
}
