//<summary>
//  Title   : public partial class PZCZwolnienieDoProceduryTowar
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;

namespace CAS.SmartFactory.Customs.Messages.CELINA.PZC 
{
  /// <summary>
  /// Import of the XPath: PZC/ZwolnienieDoProcedury/Towar
  /// </summary>
  public partial class PZCZwolnienieDoProceduryTowar : GoodDescription
  {
    #region implemetation of the GoodDescription
    /// <summary>
    /// Gets the description.
    /// </summary>
    /// <returns></returns>
    public override string GetDescription()
    {
      return this.OpisTowaru;
    }
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <returns></returns>
    public override double? GetNetMass()
    {
      if (!this.MasaNettoSpecified)
        return null;
      return Convert.ToDouble(this.MasaNetto);
    }
    /// <summary>
    /// Gets the units.
    /// </summary>
    /// <returns></returns>
    public override string GetUnits()
    {
      if (this.IloscTowaru.NullOrEmpty<PZCZwolnienieDoProceduryTowarIloscTowaru>())
        return String.Empty;
      return this.IloscTowaru[0].Jm;
    }
    /// <summary>
    /// Gets the PCN tariff code.
    /// </summary>
    /// <returns></returns>
    public override string GetPCNTariffCode()
    {
      return this.KodTowarowy + this.KodTaric;
    }
    /// <summary>
    /// Gets the gross mass.
    /// </summary>
    /// <returns></returns>
    public override double? GetGrossMass()
    {
      return this.MasaBrutto.ConvertToDouble(masaBruttoFieldSpecified);
    }
    /// <summary>
    /// Gets the procedure.
    /// </summary>
    /// <returns></returns>
    public override string GetProcedure()
    {
      return this.Procedura;
    }
    /// <summary>
    /// Gets the package.
    /// </summary>
    /// <returns></returns>
    public override string GetPackage()
    {
      if (this.Opakowanie.NullOrEmpty<PZCZwolnienieDoProceduryTowarOpakowanie>())
        return String.Empty;
      return Opakowanie[0].Rodzaj;
    }
    /// <summary>
    /// Gets the total amount invoiced.
    /// </summary>
    /// <returns></returns>
    public override double? GetTotalAmountInvoiced()
    {
      if (WartoscTowaru == null)
        return null;
      return this.WartoscTowaru.WartoscPozycji.ConvertToDouble(WartoscTowaru.WartoscPozycjiSpecified);
    }
    /// <summary>
    /// Gets the cartons in kg.
    /// </summary>
    /// <returns></returns>
    public override double? GetCartonsInKg()
    {
      return null;
    }
    /// <summary>
    /// Gets the item no.
    /// </summary>
    /// <returns></returns>
    public override double? GetItemNo()
    {
      return Convert.ToDouble(this.PozId);
    }
    /// <summary>
    /// Gets the SAD duties.
    /// </summary>
    /// <returns></returns>
    public override DutiesDescription[] GetSADDuties()
    {
      return this.Oplata;
    }
    /// <summary>
    /// Gets the SAD package.
    /// </summary>
    /// <returns></returns>
    public override PackageDescription[] GetSADPackage()
    {
      return this.Opakowanie;
    }
    /// <summary>
    /// Gets the SAD quantity.
    /// </summary>
    /// <returns></returns>
    public override QuantityDescription[] GetSADQuantity()
    {
      return this.IloscTowaru;
    }
    /// <summary>
    /// Gets the SAD required documents.
    /// </summary>
    /// <returns></returns>
    public override RequiredDocumentsDescription[] GetSADRequiredDocuments()
    {
      return this.DokumentWymagany;
    }
    #endregion
  }
}
