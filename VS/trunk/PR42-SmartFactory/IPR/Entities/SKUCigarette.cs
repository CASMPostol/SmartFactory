using System;
using System.Collections.Generic;
using CigarettesMaterialxML = CAS.SmartFactory.xml.IPR.CigarettesMaterial;
using CigarettesXml = CAS.SmartFactory.xml.IPR.Cigarettes;
using MaterialXml = CAS.SmartFactory.xml.IPR.Material;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class SKUCigarette
  {
    public SKUCigarette(CigarettesMaterialxML document, Dokument parent)
      : base(document, parent)
    {
      this.Brand = document.Brand;
      this.Family = document.Family;
      this.Menthol = document.Menthol;
      this.PrimeMarket = document.Prime_Market;
    }
    protected static void GetXmlContent(CigarettesXml document, EntitiesDataContext edc, Dokument parent)
    {
      List<SKUCommonPart> entities = new List<SKUCommonPart>();
      foreach (CigarettesMaterialxML item in document.Material)
      {
        try
        {
          SKUCigarette newEntity = new SKUCigarette(item, parent);
          entities.Add(newEntity);
        }
        catch (Exception ex)
        {
          string message = String.Format("Cannot create: {0}, because of the error: {1}", item.Material_Description, ex.Message);
          Anons.WriteEntry(edc, "SKU cigarette entry error", message);
        }
        if (entities.Count > 0)
          edc.SKU.InsertAllOnSubmit(entities);
      }
    }
    protected override string GetSKU(MaterialXml document)
    {
      return ((CigarettesMaterialxML)document).Material;
    }
    protected override string GetSKUDescription(MaterialXml document)
    {
      return ((CigarettesMaterialxML)document).Material_Description;
    }
    protected override ProductType? GetProductType()
    {
      return Entities.ProductType.Cigarette;
    }
    protected override Format GetFormatLookup(MaterialXml document)
    {
      CigarettesMaterialxML xml = (CigarettesMaterialxML)document;
      double cl;
      this.CigaretteLenght = Double.TryParse(xml.Cigarette_Length.Trim().Replace(" mm", ""), out cl) ? new Nullable<Double>(cl) : new Nullable<Double>(0);
      this.FilterLenght = Double.TryParse(xml.Filter_Segment_Length.Trim().Replace(" mm", ""), out cl) ? new Nullable<Double>(cl) : new Nullable<Double>(0);
      return Format.GetFormatLookup(xml.Cigarette_Length, xml.Filter_Segment_Length);
    }
    protected override bool GetIPRMaterial()
    {
      return IPRMaterial = CheckIfUnion();
    }
    private bool CheckIfUnion()
    {
      throw new NotImplementedException();
    }
  }
}
