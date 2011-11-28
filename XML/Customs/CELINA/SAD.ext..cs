using System;

namespace CAS.SmartFactory.xml.CELINA.SAD
{
  public partial class SAD : CustomsDocument
  {
    #region CustomsDocument

    public override string GetReferenceNumber()
    {
      return Zgloszenie.NrWlasny;
    }
    public override string MessageRootName()
    {
      return "SAD";
    }
    public override GoodDescription[] GetSADGood()
    {
      return this.Zgloszenie.Towar;
    }
    public override string GetCurrency()
    {
      return Wartosc == null ? String.Empty : Wartosc.Waluta;
    }
    public override DateTime? GetCustomsDebtDate()
    {
      return null;
    }
    public override string GetDocumentNumber()
    {
      return String.Empty;
    }
    public override double? GetExchangeRate()
    {
      if (Wartosc == null)
        return null;
      return Wartosc.KursWaluty.ConvertToDouble( Wartosc.KursWalutySpecified);
    }
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

