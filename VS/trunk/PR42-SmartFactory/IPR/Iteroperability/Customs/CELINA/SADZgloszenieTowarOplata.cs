using System;

namespace CAS.SmartFactory.xml.Customs.SAD
{
  public partial class SADZgloszenieTowarOplata : DutiesDescription
  {
    public override string GetDutyType()
    {
      return this.Typ;
    }
    public override double? GetAmount()
    {
      return this.Kwota.ConvertToDouble(this.KwotaSpecified);
    }
  }
}
