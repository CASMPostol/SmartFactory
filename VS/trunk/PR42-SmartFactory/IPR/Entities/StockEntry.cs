using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockXmlRow = CAS.SmartFactory.xml.IPR.StockRow;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class StockEntry
  {
    internal Entities.ProductType Type { get; set; } //TODO replace by the column: http://itrserver/Bugs/BugDetail.aspx?bid=2875
    public StockEntry(StockXmlRow xml, Stock parent)
    {
      StockListLookup = parent;
      Batch = xml.Batch.Trim();
      Blocked = xml.Blocked;
      DocumentNo = "To be removed"; //TODO remove after http://itrserver/Bugs/BugDetail.aspx?bid=2871
      InQualityInsp = xml.InQualityInsp;
      IPRType = false;
      Location = xml.SLoc;
      RestrictedUse = xml.RestrictedUse;
      SKU = xml.Material.Trim();
      Tytuł = xml.MaterialDescription.Trim();
      Units = xml.BUn;
      Unrestricted = xml.Unrestricted;
      Quantity = 0;
      BatchLookup = null;
      Type = ProductType.Invalid;
      Quantity = xml.Blocked.GetValueOrDefault(0) + xml.InQualityInsp.GetValueOrDefault(0) + xml.RestrictedUse.GetValueOrDefault(0) + xml.Unrestricted.GetValueOrDefault(0);
    }
    internal void ProcessEntry(EntitiesDataContext edc)
    {
      GetProductType();
      GetBatchLookup(edc);
    }
    private void GetProductType()
    {
      Type = ProductType.Invalid;  //TODO implement GetProductType:http://itrserver/Bugs/BugDetail.aspx?bid=2876
    }
    private void GetBatchLookup(EntitiesDataContext edc)
    {
      if (Type != ProductType.Cigarette || Type != ProductType.Cutfiller)
        return;
      if (!IPRType.GetValueOrDefault(false))
        return;
      BatchLookup = Entities.Batch.GetCreateLookup(edc, this.Batch);
    }
  }
}
