using System;
using System.Collections.Generic;
using CigarettesMaterialxML = CAS.SmartFactory.xml.erp.CigarettesMaterial;
using CigarettesXml = CAS.SmartFactory.xml.erp.Cigarettes;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class SKUCigarette
  {
    #region public
    public SKUCigarette(CigarettesMaterialxML xmlDocument, Dokument parent, EntitiesDataContext edc)
      : base(xmlDocument, parent, edc)
    {
      this.ProductType = Entities.ProductType.Cigarette;
      this.Brand = xmlDocument.Brand;
      this.Family = xmlDocument.Family;
      this.Menthol = xmlDocument.Menthol;
      this.PrimeMarket = xmlDocument.Prime_Market;
    }
    #endregion

    #region private
    protected override Format GetFormatLookup(MaterialXml xml, EntitiesDataContext edc)
    {
      CigarettesMaterialxML cxml = (CigarettesMaterialxML)xml;
      this.CigaretteLenght = cxml.Cigarette_Length.Trim();
      this.FilterLenght = cxml.Filter_Segment_Length.Trim();
      return Format.GetFormatLookup(cxml.Cigarette_Length, cxml.Filter_Segment_Length, edc);
    }
    protected override bool GetIPRMaterial(EntitiesDataContext edc)
    {
      return ! CustomsUnion.CheckIfUnion(this.PrimeMarket, edc);
    }
    #endregion
  }
}
