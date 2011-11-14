using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.CELINA.PZC
{
  /// <summary>
  /// Import of the XPath: PZC/ZwolnienieDoProcedury/Towar
  /// </summary>
  public partial class PZCZwolnienieDoProceduryTowar : GoodDescription
  {
    #region implemetation of the GoodDescription
    public override string GetDescription()
    {
      return this.OpisTowaru;
    }
    public override double? GetNetMass()
    {
      if (!this.MasaNettoSpecified)
        return null;
      return Convert.ToDouble(this.MasaNetto);
    }
    public override string GetUnits()
    {
      if (this.IloscTowaru == null || this.IloscTowaru.Length == 0)
        return String.Empty;
      return this.IloscTowaru[0].Jm;
    }
    public override string GetPCNTariffCode()
    {
      return this.KodTowarowy + " " + this.KodTaric;
    }
    public override double? GetGrossMass()
    {
      if (!masaBruttoFieldSpecified)
        return null;
      return Convert.ToDouble(this.MasaBrutto);
    }
    public override string GetProcedure()
    {
      return this.Procedura;
    }
    public override string GetPackage()
    {
      if (this.Opakowanie.Length == 0)
        return String.Empty;
      return Opakowanie[0].Rodzaj;
    }
    public override double? GetTotalAmountInvoiced()
    {
      if (WartoscTowaru == null || !WartoscTowaru.WartoscPozycjiSpecified)
        return null;
      return Convert.ToDouble(this.WartoscTowaru.WartoscPozycji);
    }
    public override double? GetCartonsInKg()
    {
      return null;
    }
    public override double? GetItemNo()
    {
      return Convert.ToDouble(this.PozId);
    }
    public override DutiesDescription[] GetSADDuties()
    {
      return this.Oplata;
    }
    public override PackageDescription[] GetSADPackage()
    {
      return this.Opakowanie;
    }
    #endregion

    public override QuantityDescription[] GetSADQuantity()
    {
      return this.IloscTowaru;
    }
  }
}
