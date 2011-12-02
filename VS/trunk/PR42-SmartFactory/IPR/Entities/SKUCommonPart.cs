using System;
using System.Collections.Generic;
using System.Linq;
using CigarettesMaterialxML = CAS.SmartFactory.xml.erp.CigarettesMaterial;
using CutfillerMaterialxML = CAS.SmartFactory.xml.erp.CutfillerMaterial;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;
using SKUXml = CAS.SmartFactory.xml.erp.SKU;

namespace CAS.SmartFactory.IPR.Entities
{
  public abstract partial class SKUCommonPart
  {
    #region public
    public SKUCommonPart(MaterialXml xml, Dokument parent, EntitiesDataContext edc)
      : this()
    {
      this.SKULibraryLookup = parent;
      this.SKU = xml.GetMaterial();
      this.Tytuł = xml.GetMaterialDescription();
    }
    internal static SKUCommonPart GetLookup(EntitiesDataContext edc, string index)
    {
      SKUCommonPart newSKU = null;
      try
      {
        newSKU = (
            from idx in edc.SKU
            where idx.SKU.Contains(index)
            orderby idx.Wersja descending
            select idx).First();
      }
      catch (Exception) { }
      return newSKU;
    }
    internal static void GetXmlContent(SKUXml xmlDocument, EntitiesDataContext edc, Dokument entry)
    {
      switch (xmlDocument.Type)
      {
        case CAS.SmartFactory.xml.erp.SKU.SKUType.Cigarettes:
          GetXmlContent(xmlDocument.GetMaterial(), edc, entry, delegate(MaterialXml xml, Dokument lib, EntitiesDataContext context)
          { return new SKUCigarette((CigarettesMaterialxML)xml, lib, context); });
          break;
        case CAS.SmartFactory.xml.erp.SKU.SKUType.Cutfiller:
          GetXmlContent(xmlDocument.GetMaterial(), edc, entry, delegate(MaterialXml xml, Dokument lib, EntitiesDataContext context)
          { return new SKUCutfiller((CutfillerMaterialxML)xml, lib, context); });
          break;
      }
    }
    #endregion

    #region private
    private delegate SKUCommonPart CreateMaterialXml(MaterialXml xml, Dokument lib, EntitiesDataContext context);
    private static void GetXmlContent(MaterialXml[] material, EntitiesDataContext edc, Dokument parent, CreateMaterialXml creator)
    {
      List<SKUCommonPart> entities = new List<SKUCommonPart>();
      foreach (MaterialXml item in material)
      {
        try
        {
          SKUCommonPart entity = GetLookup(edc, item.GetMaterial());
          if (entity != null)
            break;
          SKUCommonPart sku = creator(item, parent, edc);
          sku.ProcessData(item, edc);
          entities.Add(sku);
        }
        catch (Exception ex)
        {
          string message = String.Format("Cannot create: {0}, because of the error: {1}", item.GetMaterialDescription(), ex.Message);
          Anons.WriteEntry(edc, "SKU entry error", message);
        }
      }
      if (entities.Count > 0)
        edc.SKU.InsertAllOnSubmit(entities);
    }
    private void ProcessData(MaterialXml xml, EntitiesDataContext edc)
    {
      this.FormatLookup = GetFormatLookup(xml, edc);
      this.IPRMaterial = GetIPRMaterial(edc);
    }
    protected abstract Format GetFormatLookup(MaterialXml document, EntitiesDataContext edc);
    protected abstract bool GetIPRMaterial(EntitiesDataContext edc);
    #endregion
  }
}
