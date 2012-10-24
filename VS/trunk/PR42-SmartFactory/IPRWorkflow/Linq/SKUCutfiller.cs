using System;
using System.Linq;
using System.Collections.Generic;
using CutfillerMaterialxML = CAS.SmartFactory.xml.erp.CutfillerMaterial;
using CutfillerXml = CAS.SmartFactory.xml.erp.Cutfiller;
using MaterialXml = CAS.SmartFactory.xml.erp.Material;
using System.ComponentModel;

namespace CAS.SmartFactory.Linq.IPR
{
  public static class SKUCutfillerExtensions
  {
    public static SKUCutfiller SKUCutfiller( CutfillerMaterialxML xmlDocument, Dokument parent, Entities edc )
    {
      SKUCutfiller _ret = new SKUCutfiller()
      {
        ProductType = Linq.IPR.ProductType.Cutfiller,
        BlendPurpose = String.IsNullOrEmpty( xmlDocument.BlendPurpose ) ? String.Empty : xmlDocument.BlendPurpose
      };
      _ret.ProcessData( String.Empty, String.Empty, edc );
      SKUCommonPartExtensions.UpdateSKUCommonPart( _ret, xmlDocument, parent);
      return _ret;
    }
  }
}
