using System;

namespace CAS.SmartFactory.xml.Customs.PZC
{
  public partial class PZCZwolnienieDoProceduryTowarOplata: DutiesDescription
  {
    public override string GetDutyType()
    {
      return this.Typ;
    }
    public override double? GetAmount()
    {
      return this.Kwota.ConvertToDouble( this.KwotaSpecified );
    }
  }
}
