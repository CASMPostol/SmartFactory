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

using System;

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD
{
  /// <summary>
  /// Import ofthe XPath: SAD/Zgloszenie/Towar
  /// </summary>
  partial class SADZgloszenieTowar: GoodDescription
  {

    /// <summary>
    /// Creates the instance of <see cref="SADZgloszenieTowar" /> with some default values.
    /// </summary>
    /// <param name="description">The description.</param>
    /// <param name="packages">The packages.</param>
    /// <param name="referencePrevious">The reference.</param>
    /// <param name="value">The value.</param>
    /// <param name="pozId">The poz unique identifier.</param>
    /// <param name="productCode">The product code.</param>
    /// <param name="ProductCodeTaric">The product code taric.</param>
    /// <param name="customsProcedure">The customs procedure.</param>
    /// <param name="attachments">The attachments.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns>SADZgloszenieTowar.</returns>
    public static SADZgloszenieTowar Create
      ( string description, decimal packages, string referencePrevious, decimal value, ref decimal pozId, string productCode, string ProductCodeTaric, string customsProcedure,
        SADZgloszenieTowarDokumentWymagany[] attachments, SADZgloszenieTowarIloscTowaru[] quantity )
    {
      decimal _MasaBrutto = 0;
      decimal _MasaNetto = 0;
      foreach ( SADZgloszenieTowarIloscTowaru _qunttyx in quantity )
      {
        _MasaBrutto += _qunttyx.GrossMas;
        _MasaNetto += _qunttyx.Ilosc;
      }
      return new SADZgloszenieTowar()
      {
        PozId = pozId++,
        OpisTowaru = description,
        KodTowarowy = productCode,
        KodTaric = ProductCodeTaric,
        MasaBrutto = Math.Round( _MasaBrutto, 0 ),
        MasaBruttoSpecified = true,
        Procedura = customsProcedure,
        MasaNetto = Math.Round( _MasaNetto, 0 ),
        MasaNettoSpecified = true,
        IloscTowaru = quantity,
        Opakowanie = new SADZgloszenieTowarOpakowanie[] { new SADZgloszenieTowarOpakowanie() { PozId = 1, Rodzaj = "", Znaki = ".", LiczbaOpakowan = packages, LiczbaOpakowanSpecified = true, IloscSztukSpecified = false } },
        DokumentPoprzedni = new SADZgloszenieTowarDokumentPoprzedni[] { new SADZgloszenieTowarDokumentPoprzedni() { PozId = 1, Kategoria = "Z", Kod = "", Nr = referencePrevious, NrCelina = referencePrevious } },
        DokumentWymagany = attachments,
        WartoscTowaru = new SADZgloszenieTowarWartoscTowaru()
        {
          Korekta = null,
          WartoscPozycji = Math.Round(value, 2),
          WartoscPozycjiSpecified = true,
          MetodaWartosciowania = "1",
          WartoscStatystyczna = Math.Round( value, 0 ),
          WartoscStatystycznaSpecified = true,
          SzczegolyWartosci = new SADZgloszenieTowarWartoscTowaruSzczegolyWartosci[] { new SADZgloszenieTowarWartoscTowaruSzczegolyWartosci() { PozId = 1, Kod = "B00PL" } }
        }
      };
    }// Create

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
      return this.MasaNetto.ConvertToDouble( this.MasaNettoSpecified );
    }
    /// <summary>
    /// Gets the units.
    /// </summary>
    /// <returns></returns>
    public override string GetUnits()
    {
      if ( this.IloscTowaru.NullOrEmpty<SADZgloszenieTowarIloscTowaru>() )
        return String.Empty;
      return this.IloscTowaru[ 0 ].Jm;
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
      return this.MasaBrutto.ConvertToDouble( masaBruttoFieldSpecified );
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
      if ( this.Opakowanie.NullOrEmpty<SADZgloszenieTowarOpakowanie>() )
        return String.Empty;
      return Opakowanie[ 0 ].Rodzaj;
    }
    /// <summary>
    /// Gets the total amount invoiced.
    /// </summary>
    /// <returns></returns>
    public override double? GetTotalAmountInvoiced()
    {
      if ( WartoscTowaru == null )
        return null;
      return this.WartoscTowaru.WartoscPozycji.ConvertToDouble( WartoscTowaru.WartoscPozycjiSpecified );
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
      return Convert.ToDouble( this.PozId );
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
