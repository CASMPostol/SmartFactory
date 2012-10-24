using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Material
  {
    #region public
    public SKUCommonPart SKULookup { get; set; }
    public bool IPRMaterial { get; set; }
    public double DisposedQuantity( double portion )
    {
      return ( this.TobaccoQuantity.Value * portion ).RountMass();
    }
    public List<Disposal> GetListOfDisposals()
    {
      Linq.IPR.DisposalStatus status = this.Material2BatchIndex.ProductType.Value == Linq.IPR.ProductType.Cigarette ? DisposalStatus.TobaccoInCigaretes : DisposalStatus.TobaccoInCutfiller;
      return
        (
            from _didx in this.Disposal
            let _ipr = _didx.Disposal2IPRIndex
            where _didx.CustomsStatus.Value == CustomsStatus.NotStarted && _didx.DisposalStatus.Value == status
            orderby _ipr.Identyfikator ascending
            select _didx
        ).ToList();
    }
    public string GetKey()
    {
      return String.Format( m_keyForam, SKU, Batch, this.StorLoc );
    }
    public void GetProductType( Entities edc )
    {
      Entities.ProductDescription product = edc.GetProductType( this.SKU, this.StorLoc );
      this.ProductType = product.productType;
      this.SKULookup = product.skuLookup;
      this.IPRMaterial = product.IPRMaterial;
    }
    #endregion

    #region private
    private const string m_keyForam = "{0}:{1}:{2}";
    #endregion

  }
}
