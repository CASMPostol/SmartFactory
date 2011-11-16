using System;

namespace CAS.SmartFactory.xml.CELINA.PZC
{
  /// <summary>
  /// 
  /// </summary>
  public partial class PZC : CustomsDocument
  {
    #region CustomsDocument
    public override string GetReferenceNumber()
    {
      return this.NrWlasny;
    }
    public override string MessageRootName()
    {
      return "PZC";
    }
    public override GoodDescription[] GetSADGood()
    {
      return this.ZwolnienieDoProcedury.Towar;
    }
    public override string GetCurrency()
    {
      return this.ZwolnienieDoProcedury.WartoscTowarow == null ? String.Empty : this.ZwolnienieDoProcedury.WartoscTowarow.Waluta;
    }
    public override DateTime? GetCustomsDebtDate()
    {
      return this.ZwolnienieDoProcedury.DataPrzyjecia;
    }
    public override string GetDocumentNumber()
    {
      return this.ZwolnienieDoProcedury.NrCelina;
    }
    public override double? GetExchangeRate()
    {
      PZCZwolnienieDoProceduryWartoscTowarow elmnt = this.ZwolnienieDoProcedury.WartoscTowarow;
      if (elmnt == null)
        return null;
      return elmnt.KursWaluty.ConvertToDouble( elmnt.KursWalutySpecified);
    }
    public override double? GetGrossMass()
    {
      return Convert.ToDouble(this.ZwolnienieDoProcedury.MasaBrutto);
    }
    #endregion
  }
}
