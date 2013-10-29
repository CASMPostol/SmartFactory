using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs.SAD
{
  public partial class SADZgloszenieTowarIloscTowaru: QuantityDescription
  {
    public override double? GetItemNo()
    {
      return Convert.ToDouble(this.PozId);
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
