using CAS.SharePoint;
namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Entities
  {
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <param name="procedure">The procedure.</param>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public static string ToString( ClearenceProcedure procedure )
    {
      switch ( procedure )
      {
        case ClearenceProcedure._3151:
          return "3151";
        case ClearenceProcedure._3171:
          return "3171";
        case ClearenceProcedure._4051:
          return "3151";
        case ClearenceProcedure._4071:
          return "4051";
        case ClearenceProcedure._5100:
          return "5100";
        case ClearenceProcedure._5171:
          return "5171";
        case ClearenceProcedure._7100:
          return "7100";
        case ClearenceProcedure._7171:
          return "7171";
      }
      return string.Empty.NotAvailable();
    }
    internal class ProductDescription
    {
      internal ProductType productType;
      internal bool IPRMaterial;
      internal SKUCommonPart skuLookup;
      internal ProductDescription( ProductType type, bool ipr, SKUCommonPart lookup )
      {
        productType = type;
        IPRMaterial = ipr;
        skuLookup = lookup;
      }
      internal ProductDescription( ProductType type, bool ipr )
        : this( type, ipr, null )
      { }
    }
    internal ProductDescription GetProductType( string sku, string location )
    {
      SKUCommonPart entity = SKUCommonPart.Find( this, sku );
      if ( entity != null )
        return new ProductDescription( entity.ProductType.GetValueOrDefault( ProductType.Other ), entity.IPRMaterial.GetValueOrDefault( false ), entity );
      Warehouse wrh = Linq.Warehouse.Find( this, location );
      if ( wrh != null )
      {
        switch ( wrh.ProductType )
        {
          case ProductType.Tobacco:
            return new ProductDescription( ProductType.Tobacco, false );
          case ProductType.IPRTobacco:
            return new ProductDescription( ProductType.IPRTobacco, true );
          case ProductType.Invalid:
          case ProductType.Cutfiller:
          case ProductType.Cigarette:
          case ProductType.Other:
          default:
            break;
        }
      }
      return new ProductDescription( ProductType.Other, false );
    }
    private const string m_Source = "Data Context";
    private const string m_WrongProductTypeMessage = "I cannot recognize product type of the stock entry SKU: {0} in location: {1}";
  }
}

