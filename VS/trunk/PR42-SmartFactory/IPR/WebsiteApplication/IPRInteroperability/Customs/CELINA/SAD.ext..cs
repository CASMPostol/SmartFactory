using System;

namespace CAS.SmartFactory.xml.Customs.SAD
{
  public partial class SAD : CustomsDocument
  {
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
      if (Wartosc == null)
        return null;
      return Wartosc.KursWaluty.ConvertToDouble( Wartosc.KursWalutySpecified);
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

