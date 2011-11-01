using System;

namespace CAS.SmartFactory.xml.CELINA.SAD
{
  /// <summary>
  /// Import ofthe XPath: SAD/Zgloszenie/Towar
  /// </summary>
  partial class SADZgloszenieTowar : GoodDescription
  {
    #region GoodDescription implementation
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
    public override string GetTotalAmountInvoiced()
    {
      if (WartoscTowaru == null || ! WartoscTowaru.WartoscPozycjiSpecified)
        return string.Empty;
      return Convert.ToString(this.WartoscTowaru.WartoscPozycji);
    }
    public override double? GetCartonsInKg()
    {
      return null;
    }
    public override double? GetItemNo()
    {
      return Convert.ToDouble(this.PozId);
    }
    #endregion
  }
}
