using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  public partial class Material
  {
    #region public
    public struct Ratios
    {
      public double dustRatio;
      public double shMentholRatio;
      public double wasteRatio;
    }
    public Material( Entities edc, string batch, string sku, string storLoc, string skuDescription, string units, decimal quantity, decimal tobaccoQuantity, string productID )
      : base()
    {
      Batch = batch;
      Material2BatchIndex = null;
      SKU = sku;
      StorLoc = storLoc;
      SKUDescription = skuDescription;
      Title = skuDescription;
      Units = units;
      FGQuantity = Convert.ToDouble( quantity );
      TobaccoQuantity = Convert.ToDouble( tobaccoQuantity );
      ProductID = productID;
      Entities.ProductDescription product = edc.GetProductType( this.SKU, this.StorLoc );
      ProductType = product.productType;
    }

    public Ratios DisposalsAnalisis( Entities edc, Ratios ratios, double overusage )
    {
      decimal material = Convert.ToDecimal( this.TobaccoQuantity );
      List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( edc, Batch );
      if ( _accounts.Count == 1 && Math.Abs( Convert.ToDecimal( _accounts[ 0 ].TobaccoNotAllocated.Value ) - material ) < 1 )
        material = Convert.ToDecimal( _accounts[ 0 ].TobaccoNotAllocated.Value );
      decimal _am;
      if ( overusage > 0 )
      {
        decimal _overuseInKg = ( material * Convert.ToDecimal( overusage ) ).RountMass();
        this.Overuse = Convert.ToDouble( _overuseInKg );
        _am = material - _overuseInKg;
      }
      else
        _am = material;
      decimal _dust = ( _am * Convert.ToDecimal( ratios.dustRatio ) ).RountMass();
      decimal _shMenthol = ( _am * Convert.ToDecimal( ratios.shMentholRatio ) ).RountMass();
      decimal _waste = ( _am * Convert.ToDecimal( ratios.wasteRatio ) ).RountMass();
      this.Dust = Convert.ToDouble( _dust );
      this.SHMenthol = Convert.ToDouble( _shMenthol );
      this.Waste = Convert.ToDouble( _waste );
      this.TobaccoQuantity = Convert.ToDouble( _am - _shMenthol - _waste - _dust );
      return ratios;
    }
    /// <summary>
    /// 
    /// </summary>
    public enum DisposalsEnum
    {
      /// <summary>
      /// The dust
      /// </summary>
      Dust,
      /// <summary>
      /// The SH menthol
      /// </summary>
      SHMenthol,
      /// <summary>
      /// The waste
      /// </summary>
      Waste,
      /// <summary>
      /// The overusage in kg
      /// </summary>
      OverusageInKg,
      /// <summary>
      /// The tobacco
      /// </summary>
      Tobacco
    };
    /// <summary>
    /// Gets the <see cref="System.Decimal" /> at the specified index.
    /// </summary>
    /// <value>
    /// The <see cref="System.Decimal" />.
    /// </value>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public decimal this[ DisposalsEnum index ]
    {
      get
      {
        double _ret = default( double );
        switch ( index )
        {
          case DisposalsEnum.Dust:
            _ret = this.Dust.Value;
            break;
          case DisposalsEnum.SHMenthol:
            _ret = this.SHMenthol.Value;
            break;
          case DisposalsEnum.Waste:
            _ret = this.Waste.Value;
            break;
          case DisposalsEnum.OverusageInKg:
            _ret = this.Overuse.Value;
            break;
          case DisposalsEnum.Tobacco:
            _ret = this.Tobacco.Value;
            break;
        }
        return Convert.ToDecimal( _ret );
      }
    }
    public double DisposedQuantity( double portion )
    {
      return ( this.TobaccoQuantity.Value * portion ).RountMass();
    }
    public List<Disposal> GetListOfDisposals()
    {
      Linq.DisposalStatus status = this.Material2BatchIndex.ProductType.Value == Linq.ProductType.Cigarette ? DisposalStatus.TobaccoInCigaretes : DisposalStatus.TobaccoInCutfiller;
      return
        (
            from _didx in this.Disposal
            let _ipr = _didx.Disposal2IPRIndex
            where _didx.CustomsStatus.Value == CustomsStatus.NotStarted && _didx.DisposalStatus.Value == status
            orderby _ipr.Identyfikator ascending
            select _didx
        ).ToList();
    }
    /// <summary>
    /// Gets the key.
    /// </summary>
    /// <returns></returns>
    public string GetKey()
    {
      return String.Format( m_keyForam, SKU, Batch, this.StorLoc );
    }

    private void DisposalsAnalisis( Entities edc, double _overusage, Ratios ratios )
    {
    }

    #endregion

    #region private
    private const string m_keyForam = "{0}:{1}:{2}";
    #endregion

  }
}
