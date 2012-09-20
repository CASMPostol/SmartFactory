using System;
using System.Linq;
using System.Collections.Generic;
using CutfillerMaterialxML = CAS.SmartFactory.xml.erp.CutfillerMaterial;
using CutfillerXml = CAS.SmartFactory.xml.erp.Cutfiller;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;
using System.ComponentModel;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class SKUCutfiller
  {
    public SKUCutfiller(CutfillerMaterialxML document, Dokument parent, Entities edc)
      : base(document, parent, edc)
    {
      ProductType = Linq.IPR.ProductType.Cutfiller;
      BlendPurpose = String.IsNullOrEmpty(document.BlendPurpose) ? String.Empty : document.BlendPurpose;
    }
    protected override Format GetFormatLookup(MaterialXml document, Entities edc)
    {
      return Format.GetCutfillerFormatLookup(edc);
    }
    protected override bool? GetIPRMaterial(Entities edc)
    {
      return (!String.IsNullOrEmpty(BlendPurpose)) && BlendPurpose.Contains("NEU");
    }
  }
}
