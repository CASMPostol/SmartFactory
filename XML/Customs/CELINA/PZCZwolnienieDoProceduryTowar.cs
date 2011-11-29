using System;

namespace CAS.SmartFactory.xml.Customs.PZC
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
      if (this.IloscTowaru.NullOrEmpty<PZCZwolnienieDoProceduryTowarIloscTowaru>())
        return String.Empty;
      return this.IloscTowaru[0].Jm;
    }
    public override string GetPCNTariffCode()
    {
      return this.KodTowarowy + this.KodTaric;
    }
    public override double? GetGrossMass()
    {
      return this.MasaBrutto.ConvertToDouble(masaBruttoFieldSpecified);
    }
    public override string GetProcedure()
    {
      return this.Procedura;
    }
    public override string GetPackage()
    {
      if (this.Opakowanie.NullOrEmpty<PZCZwolnienieDoProceduryTowarOpakowanie>())
        return String.Empty;
      return Opakowanie[0].Rodzaj;
    }
    public override double? GetTotalAmountInvoiced()
    {
      if (WartoscTowaru == null)
        return null;
      return this.WartoscTowaru.WartoscPozycji.ConvertToDouble(WartoscTowaru.WartoscPozycjiSpecified);
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
    public override QuantityDescription[] GetSADQuantity()
    {
      return this.IloscTowaru;
    }
    public override RequiredDocumentsDescription[] GetSADRequiredDocuments()
    {
      return this.DokumentWymagany;
    }
    #endregion
  }
}
