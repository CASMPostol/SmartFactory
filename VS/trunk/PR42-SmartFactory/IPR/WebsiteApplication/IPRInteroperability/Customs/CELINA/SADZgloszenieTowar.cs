//<summary>
//  Title   : partial class SADZgloszenieTowar
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

using CAS.SmartFactory.Customs;
using System;

namespace CAS.SmartFactory.xml.Customs.SAD
{
  /// <summary>
  /// Import ofthe XPath: SAD/Zgloszenie/Towar
  /// </summary>
  partial class SADZgloszenieTowar: GoodDescription
  {
    #region GoodDescription implementation
    public override string GetDescription()
    {
      return this.OpisTowaru;
    }
    public override double? GetNetMass()
    {
      return this.MasaNetto.ConvertToDouble( this.MasaNettoSpecified );
    }
    public override string GetUnits()
    {
      if ( this.IloscTowaru.NullOrEmpty<SADZgloszenieTowarIloscTowaru>() )
        return String.Empty;
      return this.IloscTowaru[ 0 ].Jm;
    }
    public override string GetPCNTariffCode()
    {
      return this.KodTowarowy + this.KodTaric;
    }
    public override double? GetGrossMass()
    {
      return this.MasaBrutto.ConvertToDouble( masaBruttoFieldSpecified );
    }
    public override string GetProcedure()
    {
      return this.Procedura;
    }
    public override string GetPackage()
    {
      if ( this.Opakowanie.NullOrEmpty<SADZgloszenieTowarOpakowanie>() )
        return String.Empty;
      return Opakowanie[ 0 ].Rodzaj;
    }
    public override double? GetTotalAmountInvoiced()
    {
      if ( WartoscTowaru == null )
        return null;
      return this.WartoscTowaru.WartoscPozycji.ConvertToDouble( WartoscTowaru.WartoscPozycjiSpecified );
    }
    public override double? GetCartonsInKg()
    {
      return null;
    }
    public override double? GetItemNo()
    {
      return Convert.ToDouble( this.PozId );
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
