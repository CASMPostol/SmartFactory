using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs.IE529
{
  public partial class IE529 : CustomsDocument
  {
    #region CustomsDocument
    /// <summary>
    /// Gets the reference number.
    /// </summary>
    /// <returns></returns>
    public override string GetReferenceNumber()
    {
      return this.NrWlasny;
    }
    /// <summary>
    ///  The name of the root messages.
    /// </summary>
    /// <returns></returns>
    public override DocumentType MessageRootName()
    {
      return DocumentType.IE529;
    }
    /// <summary>
    /// Gets the SAD good.
    /// </summary>
    /// <returns></returns>
    public override GoodDescription[] GetSADGood()
    {
      return this.Zwolnienie.Towar;
    }
    /// <summary>
    /// Gets the currency.
    /// </summary>
    /// <returns></returns>
    public override string GetCurrency()
    {
      return String.Empty;
    }
    public override DateTime? GetCustomsDebtDate()
    {
      return this.Zwolnienie.DataPrzyjecia;
    }
    /// <summary>
    /// Gets the document number.
    /// </summary>
    /// <returns></returns>
    public override string GetDocumentNumber()
    {
      return Zwolnienie.MRN;
    }
    /// <summary>
    /// Gets the exchange rate.
    /// </summary>
    /// <returns></returns>
    public override double? GetExchangeRate()
    {
      return null;
    }
    /// <summary>
    /// Gets the gross mass.
    /// </summary>
    /// <returns></returns>
    public override double? GetGrossMass()
    {
      return this.Zwolnienie.MasaBrutto.ConvertToDouble(this.Zwolnienie.MasaBruttoSpecified);
    }
    #endregion
  }
}
