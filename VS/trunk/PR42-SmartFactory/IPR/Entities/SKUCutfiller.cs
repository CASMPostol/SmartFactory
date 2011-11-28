using System;
using System.Linq;
using System.Collections.Generic;
using CutfillerMaterialxML = CAS.SmartFactory.xml.erp.CutfillerMaterial;
using CutfillerXml = CAS.SmartFactory.xml.erp.Cutfiller;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;

namespace CAS.SmartFactory.IPR.Entities
{
  public partial class SKUCutfiller
  {
    public SKUCutfiller(CutfillerMaterialxML document, Dokument parent, EntitiesDataContext edc)
      : base(document, parent, edc)
    {
      ProductType = Entities.ProductType.Cutfiller;
      BlendPurpose = document.BlendPurpose.Trim();
    }
    protected override Format GetFormatLookup(MaterialXml document, EntitiesDataContext edc)
    {
      return Format.GetCutfillerFormatLookup(edc);
    }
    protected override bool GetIPRMaterial(EntitiesDataContext edc)
    {
      return SKUDescription.EndsWith("NEU");
    }
  }
}
