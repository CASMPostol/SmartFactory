//<summary>
//  Title   : partial class IE529ZwolnienieTowar
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

namespace CAS.SmartFactory.Customs.Messages.ECS
{
  /// <summary>
  /// Import of the XPath: IE529/Zwolnienie/Towar
  /// </summary>
  partial class IE529ZwolnienieTowar : GoodDescription
  {

    #region GoodDescription implementation
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
      return Convert.ToDouble(this.MasaNetto);
    }
    /// <summary>
    /// Gets the units.
    /// </summary>
    /// <returns></returns>
    public override string GetUnits()
    {
      if (this.IloscTowaru.NullOrEmpty<IE529ZwolnienieTowarIloscTowaru>())
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
      return this.ProceduraWnioskowana + ProceduraPoprzednia;
    }
    /// <summary>
    /// Gets the package.
    /// </summary>
    /// <returns></returns>
    public override string GetPackage()
    {
      if (this.Opakowanie.NullOrEmpty<IE529ZwolnienieTowarOpakowanie>())
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
      return Convert.ToDouble(this.WartoscTowaru.WartoscStatystyczna);
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
      return Convert.ToDouble(this.Nr);
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
