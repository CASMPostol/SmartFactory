using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Material 
  /// </summary>
  public partial class Material
  {
    #region ctor
    public Material( Entities edc, Entities.ProductDescription product, string batch, string sku, string storLoc, string skuDescription, string units, decimal fgQuantity, decimal tobaccoQuantity, string productID )
      : this()
    {
      Batch = batch;
      Material2BatchIndex = null;
      SKU = sku;
      StorLoc = storLoc;
      SKUDescription = skuDescription;
      Title = skuDescription;
      Units = units;
      FGQuantity = Convert.ToDouble( fgQuantity );
      TobaccoQuantity = Convert.ToDouble( tobaccoQuantity );
      ProductID = productID;
      ProductType = product.productType;
      Overuse = 0;
      Dust = 0;
      SHMenthol = 0;
      Waste = 0;
      Tobacco = 0;
    }
    #endregion

    #region public
    /// <summary>
    /// Ratios 
    /// </summary>
    public struct Ratios
    {
      /// <summary>
      /// The dust ratio
      /// </summary>
      public double dustRatio;
      /// <summary>
      /// The sh menthol ratio
      /// </summary>
      public double shMentholRatio;
      /// <summary>
      /// The waste ratio
      /// </summary>
      public double wasteRatio;
    }
    /// <summary>
    /// Disposalses the analisis.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="ratios">The ratios.</param>
    /// <param name="overusage">The overusage.</param>
    public void DisposalsAnalisis( Entities edc, Ratios ratios, double overusage )
    {
      decimal material = Convert.ToDecimal( this.TobaccoQuantity );
      List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( edc, Batch );
      if ( _accounts.Count == 1 && Math.Abs( Convert.ToDecimal( _accounts[ 0 ].TobaccoNotAllocated.Value ) - material ) < 1 )
        material = Convert.ToDecimal( _accounts[ 0 ].TobaccoNotAllocated.Value );
      if ( overusage > 0 )
      {
        decimal _overuseInKg = this[ DisposalsEnum.OverusageInKg ] = ( material * Convert.ToDecimal( overusage ) ).RountMass();
        material -= _overuseInKg;
      }
      decimal _dust = this[ DisposalsEnum.Dust ] = ( material * Convert.ToDecimal( ratios.dustRatio ) ).RountMass();
      decimal _shMenthol = this[ DisposalsEnum.SHMenthol ] = ( material * Convert.ToDecimal( ratios.shMentholRatio ) ).RountMass();
      decimal _waste = this[ DisposalsEnum.Waste ] = ( material * Convert.ToDecimal( ratios.wasteRatio ) ).RountMass();
      this[ DisposalsEnum.Tobacco ] = material - _shMenthol - _waste - _dust;
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
      set
      {
        double _val = Convert.ToDouble( value );
        switch ( index )
        {
          case DisposalsEnum.Dust:
            this.Dust = _val;
            break;
          case DisposalsEnum.SHMenthol:
            this.SHMenthol = _val;
            break;
          case DisposalsEnum.Waste:
            this.Waste = _val;
            break;
          case DisposalsEnum.OverusageInKg:
            this.Overuse = _val;
            break;
          case DisposalsEnum.Tobacco:
            this.Tobacco = _val;
            break;
        }
      }
    }
    /// <summary>
    /// Get portion of calculated tobacco quantity.
    /// </summary>
    /// <param name="portion">The portion.</param>
    /// <returns>Returns portion of calculated tobacco quantity. i.e. after removing compensation wast, dust, SHMethol, etc.</returns>
    public decimal CalculatedQuantity( double portion )
    {
      return Convert.ToDecimal( ( this.Tobacco.Value * portion ).RountMass() );
    }
    /// <summary>
    /// Get portion of useds tobacco quantity, i.e. tobacco reported by the batch record.
    /// </summary>
    /// <param name="portion">Returns portion of used tobacco quantity. i.e. tobacco used for production.</param>
    /// <returns></returns>
    public double UsedQuantity( double portion )
    {
      return  (this.TobaccoQuantity.Value * portion ).RountMass();
    }
    /// <summary>
    /// Gets the list of disposals.
    /// </summary>
    /// <returns></returns>
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
    /// <summary>
    /// Gets the tobacco total.
    /// </summary>
    /// <value>
    /// The tobacco total.
    /// </value>
    public decimal TobaccoTotal { get { return Convert.ToDecimal( this.TobaccoQuantity.GetValueOrDefault( 0 ) ); } }
    #endregion

    #region private
    private const string m_keyForam = "{0}:{1}:{2}";
    #endregion

  }
}
