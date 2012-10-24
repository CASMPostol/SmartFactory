using StockXmlRow = CAS.SmartFactory.xml.erp.StockRow;

namespace CAS.SmartFactory.Linq.IPR
{
  public static class StockEntryExtension
  {
    public static StockEntry StockEntry( StockXmlRow xml, Stock parent )
    {
      return new StockEntry()
      {
        StockIndex = parent,
        Batch = xml.Batch,
        Blocked = xml.Blocked,
        InQualityInsp = xml.InQualityInsp,
        IPRType = false,
        StorLoc = xml.SLoc,
        RestrictedUse = xml.RestrictedUse,
        SKU = xml.Material,
        Title = xml.MaterialDescription,
        Units = xml.BUn,
        Unrestricted = xml.Unrestricted,
        BatchIndex = null,
        ProductType = Linq.IPR.ProductType.Invalid,
        Quantity = xml.Blocked.GetValueOrDefault( 0 ) + xml.InQualityInsp.GetValueOrDefault( 0 ) + xml.RestrictedUse.GetValueOrDefault( 0 ) + xml.Unrestricted.GetValueOrDefault( 0 ),
      };
    }
  }
}
