using System;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CutfillerMaterialxML = CAS.SmartFactory.xml.erp.CutfillerMaterial;

namespace CAS.SmartFactory.Linq.IPR
{
  public static class SKUCutfillerExtensions
  {
    public static SKUCutfiller SKUCutfiller( CutfillerMaterialxML xmlDocument, Dokument parent, Entities edc )
    {
      SKUCutfiller _ret = new SKUCutfiller()
      {
        ProductType = ProductType.Cutfiller,
        BlendPurpose = String.IsNullOrEmpty( xmlDocument.BlendPurpose ) ? String.Empty : xmlDocument.BlendPurpose
      };
      _ret.ProcessData( String.Empty, String.Empty, edc );
      SKUCommonPartExtensions.UpdateSKUCommonPart( _ret, xmlDocument, parent);
      return _ret;
    }
  }
}
