using CAS.SmartFactory.Customs;
using System;

namespace CAS.SmartFactory.xml.Customs.IE529
{
  /// <summary>
  /// Import of the XPath: IE529/Zwolnienie/Towar
  /// </summary>
  partial class IE529ZwolnienieTowar : GoodDescription
  {
    #region GoodDescription implementation
    public override string GetDescription()
    {
      return this.OpisTowaru;
    }
    public override double? GetNetMass()
    {
      return Convert.ToDouble(this.MasaNetto);
    }
    public override string GetUnits()
    {
      if (this.IloscTowaru.NullOrEmpty<IE529ZwolnienieTowarIloscTowaru>())
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
      return this.ProceduraWnioskowana + ProceduraPoprzednia;
    }
    public override string GetPackage()
    {
      if (this.Opakowanie.NullOrEmpty<IE529ZwolnienieTowarOpakowanie>())
        return String.Empty;
      return Opakowanie[0].Rodzaj;
    }
    public override double? GetTotalAmountInvoiced()
    {
      if (WartoscTowaru == null)
        return null;
      return Convert.ToDouble(this.WartoscTowaru.WartoscStatystyczna);
    }
    public override double? GetCartonsInKg()
    {
      return null;
    }
    public override double? GetItemNo()
    {
      return Convert.ToDouble(this.Nr);
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
