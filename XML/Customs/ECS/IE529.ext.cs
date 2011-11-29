using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml.Customs.IE529
{
  public partial class IE529 : CustomsDocument
  {
    #region CustomsDocument
    public override string GetReferenceNumber()
    {
      return this.NrWlasny;
    }
    public override string MessageRootName()
    {
      return "IE529";
    }
    public override GoodDescription[] GetSADGood()
    {
      return this.Zwolnienie.Towar;
    }
    public override string GetCurrency()
    {
      return String.Empty;
    }
    public override DateTime? GetCustomsDebtDate()
    {
      return this.Zwolnienie.DataPrzyjecia;
    }
    public override string GetDocumentNumber()
    {
      return Zwolnienie.MRN;
    }
    public override double? GetExchangeRate()
    {
      return null;
    }
    public override double? GetGrossMass()
    {
      return this.Zwolnienie.MasaBrutto.ConvertToDouble(this.Zwolnienie.MasaBruttoSpecified);
    }
    #endregion
  }
}
