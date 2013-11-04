//<summary>
//  Title   : partial class SAD
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
  /// partial class SAD
  /// </summary>
  public partial class SAD: CustomsDocument
  {
    /// <summary>
    /// Creates the specified email podmiotu.
    /// </summary>
    /// <param name="emailPodmiotu">The email podmiotu.</param>
    /// <param name="zgloszenie">The zgloszenie.</param>
    /// <returns></returns>
    public static SAD Create( string emailPodmiotu, SADZgloszenie zgloszenie )
    {
      SAD _new = new SAD()
      {
        EmailPodmiotu = emailPodmiotu,
        Zgloszenie = zgloszenie
      };
      return _new;
    }
    /// <summary>
    /// Gets the stylesheet nmane.
    /// </summary>
    /// <value>
    /// The stylesheet nmane.
    /// </value>
    public static string StylesheetNmane { get { return "SADStylesheet"; } }

    #region CustomsDocument
    /// <summary>
    /// Gets the reference number.
    /// </summary>
    /// <returns></returns>
    public override string GetReferenceNumber()
    {
      return Zgloszenie.NrWlasny;
    }
    /// <summary>
    /// The name of the root message.
    /// </summary>
    /// <returns></returns>
    public override DocumentType MessageRootName()
    {
      return DocumentType.SAD;
    }
    /// <summary>
    /// Gets the SAD good.
    /// </summary>
    /// <returns></returns>
    public override GoodDescription[] GetSADGood()
    {
      return this.Zgloszenie.Towar;
    }
    /// <summary>
    /// Gets the currency.
    /// </summary>
    /// <returns></returns>
    public override string GetCurrency()
    {
      return Wartosc == null ? String.Empty : Wartosc.Waluta;
    }
    /// <summary>
    /// Gets the customs debt date.
    /// </summary>
    /// <returns></returns>
    public override DateTime? GetCustomsDebtDate()
    {
      return new Nullable<DateTime>();
    }
    /// <summary>
    /// Gets the document number.
    /// </summary>
    /// <returns></returns>
    public override string GetDocumentNumber()
    {
      return String.Empty;
    }
    /// <summary>
    /// Gets the exchange rate.
    /// </summary>
    /// <returns></returns>
    public override double? GetExchangeRate()
    {
      if ( Wartosc == null )
        return null;
      return Wartosc.KursWaluty.ConvertToDouble( Wartosc.KursWalutySpecified );
    }
    /// <summary>
    /// Gets the gross mass.
    /// </summary>
    /// <returns></returns>
    public override double? GetGrossMass()
    {
      return null;
    }
    #endregion

    #region private
    private SADZgloszenieWartoscTowarow Wartosc { get { return this.Zgloszenie.WartoscTowarow; } }
    #endregion

  }
}

