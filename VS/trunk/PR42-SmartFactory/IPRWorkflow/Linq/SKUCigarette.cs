using CigarettesMaterialxML = CAS.SmartFactory.xml.erp.CigarettesMaterial;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;

namespace CAS.SmartFactory.Linq.IPR
{
  internal static class SKUCigaretteExtension
  {
    #region public
    internal static SKUCigarette SKUCigarette( CigarettesMaterialxML xmlDocument, Dokument parent, Entities edc )
    {
      bool _menthol = xmlDocument.Menthol.StartsWith( "M" );
      SKUCigarette _ret = new SKUCigarette()
      {
        ProductType = CAS.SmartFactory.Linq.IPR.ProductType.Cigarette,
        Brand = xmlDocument.Brand_Description,
        Family = xmlDocument.Family_Des,
        Menthol = xmlDocument.Menthol,
        MentholMaterial = _menthol,
        PrimeMarket = xmlDocument.Prime_Market,
      };
      _ret.ProcessData( xmlDocument.Cigarette_Length, xmlDocument.Filter_Segment_Length, edc );
      SKUCommonPartExtensions.UpdateSKUCommonPart( _ret, xmlDocument, parent );
      return _ret;
    }
    #endregion

    #region private
    internal static Format GetFormatLookup( this SKUCigarette _this, MaterialXml xml, Entities edc )
    {
      CigarettesMaterialxML cxml = (CigarettesMaterialxML)xml;
      _this.CigaretteLenght = cxml.Cigarette_Length;
      _this.FilterLenght = cxml.Filter_Segment_Length;
      Format frmt = Format.GetFormatLookup( cxml.Cigarette_Length, cxml.Filter_Segment_Length, edc );
      if ( frmt == null )
        Anons.WriteEntry( edc, m_Source, string.Format( m_FrmtTemplate, _this.SKU ) );
      return frmt;
    }
    #endregion
    private const string m_Source = "Cigarettes SKU processing";
    private const string m_FrmtTemplate = "I cannot recognize the format for {0}";
  }
}
