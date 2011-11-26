using System;
using System.Collections.Generic;
using CutfillerMaterialxML = CAS.SmartFactory.xml.IPR.CutfillerMaterial;
using CutfillerXml = CAS.SmartFactory.xml.IPR.Cutfiller;
using MaterialXml = CAS.SmartFactory.xml.IPR.Material;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class SKUCutfiller
  {
    public SKUCutfiller(CutfillerMaterialxML document, Dokument parent)
      : base(document, parent)
    {
      this.BlendPurpose = document.BlendPurpose;
    }
    protected static void GetXmlContent(CutfillerXml document, EntitiesDataContext edc, Dokument parent)
    {
      List<SKUCommonPart> entities = new List<SKUCommonPart>();
      foreach (CutfillerMaterialxML item in document.Material)
      {
        try
        {
          SKUCutfiller newEntity = new SKUCutfiller(item, parent);
          entities.Add(newEntity);
        }
        catch (Exception ex)
        {
          string message = String.Format("Cannot create: {0}, because of the error: {1}", item.MaterialDescription, ex.Message);
          Anons.WriteEntry(edc, "SKU cutfiller entry  error", message);
        }
      }
      if (entities.Count > 0)
        edc.SKU.InsertAllOnSubmit(entities);
    }
    protected override string GetSKU(MaterialXml document)
    {
      return ((CutfillerMaterialxML)document).Material;
    }
    protected override string GetSKUDescription(MaterialXml document)
    {
      return ((CutfillerMaterialxML)document).MaterialDescription;
    }
    protected override ProductType? GetProductType()
    {
      return Entities.ProductType.Cutfiller;
    }
    protected override Format GetFormatLookup(MaterialXml document)
    {
      return Format.GetCutfillerFormatLookup();
    }
    protected override bool GetIPRMaterial()
    {
      return SKUDescription.EndsWith("NEU");
    }
  }
}
