using System;

namespace CAS.SmartFactory.xml.Customs.PZC
{
  /// <summary>
  /// 
  /// </summary>
  public partial class PZC : CustomsDocument
  {
    #region CustomsDocument
    public override string GetReferenceNumber()
    {
      return this.ZwolnienieDoProcedury.NrWlasny;
    }
    /// <summary>
    /// The name of the root message.
    /// </summary>
    /// <returns></returns>
    public override DocumentType MessageRootName()
    {
      return DocumentType.PZC;
    }
    /// <summary>
    /// Gets the SAD good.
    /// </summary>
    /// <returns></returns>
    public override GoodDescription[] GetSADGood()
    {
      return this.ZwolnienieDoProcedury.Towar;
    }
    /// <summary>
    /// Gets the currency.
    /// </summary>
    /// <returns></returns>
    public override string GetCurrency()
    {
      return this.ZwolnienieDoProcedury.WartoscTowarow == null ? String.Empty : this.ZwolnienieDoProcedury.WartoscTowarow.Waluta;
    }
    /// <summary>
    /// Gets the customs debt date.
    /// </summary>
    /// <returns></returns>
    public override DateTime? GetCustomsDebtDate()
    {
      return this.ZwolnienieDoProcedury.DataPrzyjecia;
    }
    /// <summary>
    /// Gets the document number.
    /// </summary>
    /// <returns></returns>
    public override string GetDocumentNumber()
    {
      return this.ZwolnienieDoProcedury.NrCelina;
    }
    /// <summary>
    /// Gets the exchange rate.
    /// </summary>
    /// <returns></returns>
    public override double? GetExchangeRate()
    {
      PZCZwolnienieDoProceduryWartoscTowarow elmnt = this.ZwolnienieDoProcedury.WartoscTowarow;
      if (elmnt == null)
        return null;
      return elmnt.KursWaluty.ConvertToDouble( elmnt.KursWalutySpecified);
    }
    /// <summary>
    /// Gets the gross mass.
    /// </summary>
    /// <returns></returns>
    public override double? GetGrossMass()
    {
      return Convert.ToDouble(this.ZwolnienieDoProcedury.MasaBrutto);
    }
    #endregion
  }
}
