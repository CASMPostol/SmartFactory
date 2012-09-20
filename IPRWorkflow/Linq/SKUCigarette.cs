using System;
using System.Collections.Generic;
using CigarettesMaterialxML = CAS.SmartFactory.xml.erp.CigarettesMaterial;
using CigarettesXml = CAS.SmartFactory.xml.erp.Cigarettes;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;
using System.ComponentModel;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class SKUCigarette
  {
    #region public
    public SKUCigarette
      (CigarettesMaterialxML xmlDocument, Dokument parent, Entities edc)
      : base(xmlDocument, parent, edc)
    {
      this.ProductType = CAS.SmartFactory.Linq.IPR.ProductType.Cigarette;
      this.Brand = xmlDocument.Brand_Description;
      this.Family = xmlDocument.Family_Des;
      this.Menthol = xmlDocument.Menthol;
      this.MentholMaterial = this.Menthol.StartsWith( "M" ); 
      this.PrimeMarket = xmlDocument.Prime_Market;
    }
    #endregion

    #region private
    protected override Format GetFormatLookup(MaterialXml xml, Entities edc)
    {
      CigarettesMaterialxML cxml = (CigarettesMaterialxML)xml;
      this.CigaretteLenght = cxml.Cigarette_Length;
      this.FilterLenght = cxml.Filter_Segment_Length;
      Format frmt = Format.GetFormatLookup(cxml.Cigarette_Length, cxml.Filter_Segment_Length, edc);
      if (frmt == null)
        Anons.WriteEntry(edc, m_Source, string.Format(m_FrmtTemplate, this.SKU));
      return frmt;
    }
    protected override bool? GetIPRMaterial(Entities edc)
    {

      if (String.IsNullOrEmpty(this.PrimeMarket))
      {
        Anons.WriteEntry(edc, m_Source, string.Format(m_PMTemplate, this.Title));
        return new Nullable<bool>();
      }
      return !CustomsUnion.CheckIfUnion(this.PrimeMarket, edc);
    }
    #endregion
    private const string m_Source = "Cigarettes SKU processing";
    private const string m_PMTemplate = "I cannot analize the market for {0} because the name is epty";
    private const string m_FrmtTemplate = "I cannot recognize the format for {0}";
  }
}
