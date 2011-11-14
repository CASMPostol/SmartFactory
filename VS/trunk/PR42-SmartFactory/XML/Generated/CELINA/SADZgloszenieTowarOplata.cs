using System;

namespace CAS.SmartFactory.xml.CELINA.SAD
{
  public partial class SADZgloszenieTowarOplata : DutiesDescription
  {
    public override string GetDutyType()
    {
      return this.Typ;
    }
    public override double? GetAmount()
    {
      return this.KwotaSpecified ? new Nullable<Double>(Convert.ToDouble(this.Kwota)) : new Nullable<Double>();
    }
  }
}
