using System;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class Material
  {
    //TODO define the column
    public ProductType MaterialType { get; set; }
    //TODO serch Warechouses, SKU
    internal static ProductType GetMaterialType(string materialType)
    {
      materialType = materialType.Trim().ToUpper();
      if (materialType.Contains("SKU"))
        return ProductType.Cigarette;
      else if (materialType.Contains("CFT"))
        return ProductType.Cutfiller;
      else
        return ProductType.None;
    }
    private const string keyForam = "{0}:{1}:{2}";
    internal string GetKey()
    {
      return String.Format(keyForam, SKU, Batch, Location);
    }
  }
}
