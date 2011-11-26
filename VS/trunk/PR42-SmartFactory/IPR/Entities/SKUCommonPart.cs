using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKUXml = CAS.SmartFactory.xml.IPR.SKU;
using CigarettesXml = CAS.SmartFactory.xml.IPR.Cigarettes;
using CutfillerXml = CAS.SmartFactory.xml.IPR.Cutfiller;
using MaterialXml = CAS.SmartFactory.xml.IPR.Material;

namespace CAS.SmartFactory.IPR.Entities
{
  public abstract partial class SKUCommonPart
  {
    public bool IPRMaterial { get; protected set; }
    public SKUCommonPart(MaterialXml document, Dokument parent, EntitiesDataContext edc)
    {
      this.FormatLookup = this.GetFormatLookup(document);
      this.ProductType = GetProductType();
      this.SKU = this.GetSKU(document);
      this.SKUDescription = this.GetSKUDescription(document);
      this.SKULibraryLookup = parent;
      this.Tytuł = this.GetSKUDescription(document);
      this.IPRMaterial = GetIPRMaterial(edc);
    }
    internal static SKUCommonPart GetLookup(EntitiesDataContext edc, string index)
    {
      SKUCommonPart newSKU = null;
      try
      {
        newSKU = (from item in edc.SKU where item.SKU.Contains(index) select item).First<SKUCommonPart>();
      }
      catch (Exception) { }
      return newSKU;
    }
    internal static void GetXmlContent(SKUXml document, EntitiesDataContext edc, Dokument entry)
    {
      switch (document.Type)
      {
        case CAS.SmartFactory.xml.IPR.SKU.SKUType.Cigarettes:
          SKUCigarette.GetXmlContent((CigarettesXml)document, edc, entry);
          break;
        case CAS.SmartFactory.xml.IPR.SKU.SKUType.Cutfiller:
          SKUCigarette.GetXmlContent((CutfillerXml)document, edc, entry);
          break;
      }
    }
    #region private
    protected abstract string GetSKU(MaterialXml document);
    protected abstract string GetSKUDescription(MaterialXml document);
    protected abstract ProductType? GetProductType();
    protected abstract Format GetFormatLookup(MaterialXml document);
    protected abstract bool GetIPRMaterial(EntitiesDataContext edc);
    #endregion
  }
}
