using System;

namespace CAS.SmartFactory.xml.Customs.CLNE
{
  public partial class CLNE : CustomsDocument
  {
    #region CustomsDocument
    public override string GetReferenceNumber()
    {
      if (Przyjecie == null)
        return null;
      return this.Przyjecie.NrWlasny;
    }
    public override DocumentType MessageRootName()
    {
      return DocumentType.CLNE;
    }
    public override GoodDescription[] GetSADGood()
    {
      return null;
    }
    public override string GetCurrency()
    {
      return String.Empty;
    }
    public override DateTime? GetCustomsDebtDate()
    {
      if (Przyjecie == null)
        return null;
      return this.Przyjecie.CzasPrzyjecia;
    }
    public override string GetDocumentNumber()
    {
      if (Przyjecie == null)
        return null;
      return this.Przyjecie.NrCelina;
    }
    public override double? GetExchangeRate()
    {
      return null;
    }
    public override double? GetGrossMass()
    {
      return null;
    }
    #endregion
  }
}
