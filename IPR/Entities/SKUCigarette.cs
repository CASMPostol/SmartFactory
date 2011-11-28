using System;
using System.Collections.Generic;
using CigarettesMaterialxML = CAS.SmartFactory.xml.IPR.CigarettesMaterial;
using CigarettesXml = CAS.SmartFactory.xml.IPR.Cigarettes;
using MaterialXml = CAS.SmartFactory.xml.IPR.Material;

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
    protected override Format GetFormatLookup(MaterialXml document, EntitiesDataContext edc)
    {
      CigarettesMaterialxML xml = (CigarettesMaterialxML)document;
      double cl;
      //TODO use strings see http://itrserver/Bugs/BugDetail.aspx?bid=2867
      this.CigaretteLenght = Double.TryParse(xml.Cigarette_Length.Trim().Replace(" mm", ""), out cl) ? new Nullable<Double>(cl) : new Nullable<Double>(0);
      this.FilterLenght = Double.TryParse(xml.Filter_Segment_Length.Trim().Replace(" mm", ""), out cl) ? new Nullable<Double>(cl) : new Nullable<Double>(0);
      return Format.GetFormatLookup(xml.Cigarette_Length, xml.Filter_Segment_Length, edc);
    }
    protected override bool GetIPRMaterial(EntitiesDataContext edc)
    {
      return IPRMaterial = CustomsUnion.CheckIfUnion(this.PrimeMarket, edc);
    } 
    #endregion
  }
}
