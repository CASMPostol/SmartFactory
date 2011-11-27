using System;
using System.Collections.Generic;
using System.Linq;
using CigarettesMaterialxML = CAS.SmartFactory.xml.IPR.CigarettesMaterial;
using CutfillerMaterialxML = CAS.SmartFactory.xml.IPR.CutfillerMaterial;
using MaterialXml = CAS.SmartFactory.xml.IPR.Material;
using SKUXml = CAS.SmartFactory.xml.IPR.SKU;

namespace CAS.SmartFactory.IPR.Entities
{
  public abstract partial class SKUCommonPart
  {
    #region public
    public SKUCommonPart(MaterialXml xml, Dokument parent, EntitiesDataContext edc)
    {
      this.SKULibraryLookup = parent;
      this.SKU = xml.GetMaterial();
      this.SKUDescription = xml.GetMaterialDescription();
      this.Tytuł = xml.GetMaterialDescription();
      this.FormatLookup = GetFormatLookup(xml, edc);
      this.IPRMaterial = GetIPRMaterial(edc);
    }
    public bool IPRMaterial { get; protected set; } //TODO to be replaced by new column
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
        case CAS.SmartFactory.xml.IPR.SKU.SKUType.Cigarettes:
          GetXmlContent(xmlDocument.GetMaterial(), edc, entry, delegate(MaterialXml xml, Dokument lib, EntitiesDataContext context)
          { return new SKUCigarette((CigarettesMaterialxML)xml, lib, context); });
          break;
        case CAS.SmartFactory.xml.IPR.SKU.SKUType.Cutfiller:
          GetXmlContent(xmlDocument.GetMaterial(), edc, entry, delegate(MaterialXml xml, Dokument lib, EntitiesDataContext context)
          { return new SKUCutfiller((CutfillerMaterialxML)xml, lib, context) as SKUCommonPart; });
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
          if (entity == null)
            entities.Add(creator(item, parent, edc));
        }
        catch (Exception ex)
        {
          string message = String.Format("Cannot create: {0}, because of the error: {1}", item.GetMaterialDescription(), ex.Message);
          Anons.WriteEntry(edc, "SKU entry  error", message);
        }
      }
      if (entities.Count > 0)
        edc.SKU.InsertAllOnSubmit(entities);
    }
    protected abstract Format GetFormatLookup(MaterialXml document, EntitiesDataContext edc);
    protected abstract bool GetIPRMaterial(EntitiesDataContext edc);
    #endregion
  }
}
