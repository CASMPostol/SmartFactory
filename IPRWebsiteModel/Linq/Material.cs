using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR;
using Microsoft.SharePoint.Linq;

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
    public void CalculateCompensationComponents( Entities edc, Ratios ratios, double overusage )
    {
      decimal material = Convert.ToDecimal( this.TobaccoQuantity );
      List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( edc, Batch );
      if ( _accounts.Count == 1 && Math.Abs( Convert.ToDecimal( _accounts[ 0 ].TobaccoNotAllocated.Value ) - material ) < 1 )
        material = Convert.ToDecimal( _accounts[ 0 ].TobaccoNotAllocated.Value );
      if ( overusage > 0 )
      {
        decimal _overuseInKg = this[ DisposalEnum.OverusageInKg ] = ( material * Convert.ToDecimal( overusage ) ).RountMass();
        material -= _overuseInKg;
      }
      decimal _dust = this[ DisposalEnum.Dust ] = ( material * Convert.ToDecimal( ratios.dustRatio ) ).RountMass();
      decimal _shMenthol = this[ DisposalEnum.SHMenthol ] = ( material * Convert.ToDecimal( ratios.shMentholRatio ) ).RountMass();
      decimal _waste = this[ DisposalEnum.Waste ] = ( material * Convert.ToDecimal( ratios.wasteRatio ) ).RountMass();
      this[ DisposalEnum.Tobacco ] = material - _shMenthol - _waste - _dust;
    }
    /// <summary>
    /// 
    /// </summary>
    //public enum DisposalsEnum
    //{
    //  /// <summary>
    //  /// The dust
    //  /// </summary>
    //  Dust,
    //  /// <summary>
    //  /// The SH menthol
    //  /// </summary>
    //  SHMenthol,
    //  /// <summary>
    //  /// The waste
    //  /// </summary>
    //  Waste,
    //  /// <summary>
    //  /// The overusage in kg
    //  /// </summary>
    //  OverusageInKg,
    //  /// <summary>
    //  /// The tobacco
    //  /// </summary>
    //  Tobacco
    //};
    /// <summary>
    /// Gets the <see cref="System.Decimal" /> at the specified index.
    /// </summary>
    /// <value>
    /// The <see cref="System.Decimal" />.
    /// </value>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public decimal this[ DisposalEnum index ]
    {
      get
      {
        double _ret = default( double );
        switch ( index )
        {
          case DisposalEnum.Dust:
            _ret = this.Dust.Value;
            break;
          case DisposalEnum.SHMenthol:
            _ret = this.SHMenthol.Value;
            break;
          case DisposalEnum.Waste:
            _ret = this.Waste.Value;
            break;
          case DisposalEnum.OverusageInKg:
            _ret = this.Overuse.Value;
            break;
          case DisposalEnum.Tobacco:
            _ret = this.Tobacco.Value;
            break;
          default:
            break;
        }
        return Convert.ToDecimal( _ret );
      }
      set
      {
        double _val = Convert.ToDouble( value );
        switch ( index )
        {
          case DisposalEnum.Dust:
            this.Dust = _val;
            break;
          case DisposalEnum.SHMenthol:
            this.SHMenthol = _val;
            break;
          case DisposalEnum.Waste:
            this.Waste = _val;
            break;
          case DisposalEnum.OverusageInKg:
            this.Overuse = _val;
            break;
          case DisposalEnum.Tobacco:
            this.Tobacco = _val;
            break;
          default:
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
      return ( this.TobaccoQuantity.Value * portion ).RountMass();
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
    internal Material ReplaceByExistingOne( EntitySet<Material> materials, List<Material> _newMaterials )
    {
      Material _old = ( from _mx in materials where _mx.Batch.Contains( this.Batch ) select _mx ).FirstOrDefault<Material>();
      if ( _old == null )
      {
        _newMaterials.Add( this );
        return this;
      }
      Material _ret = _old;
      _ret.FGQuantity = this.FGQuantity;
      _ret.TobaccoQuantity = this.TobaccoQuantity;
      return _ret;
    }
    internal void UpdateDisposals( Entities edc, Batch parent, ProgressChangedEventHandler progressChanged )
    {
      foreach ( WebsiteModel.Linq.DisposalEnum _kind in Enum.GetValues( typeof( WebsiteModel.Linq.DisposalEnum ) ) )
      {
        try
        {
          if ( _kind == DisposalEnum.TobaccoInCigaretess || _kind == DisposalEnum.Cartons )
            continue;
          if ( ( ( _kind == DisposalEnum.SHMenthol ) || ( _kind == DisposalEnum.OverusageInKg ) ) && this[ _kind ] <= 0 )
            continue;
          decimal _toDispose = this[ _kind ];
          List<Disposal> _disposals = Linq.Disposal.Disposals( this.Disposal, _kind );
          foreach ( Linq.Disposal _dx in _disposals )
          {
            _dx.Adjust( ref _toDispose );
            if ( _toDispose <= 0 )
              throw new Updated();
          }
          progressChanged( this, new ProgressChangedEventArgs( 1, String.Format( "AddDisposal {0}, batch {1}", _kind, this.Batch ) ) );
          List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( edc, this.Batch );
          for ( int _aidx = 0; _aidx < _accounts.Count; _aidx++ )
          {
            _accounts[ _aidx ].AddDisposal( edc, _kind, ref _toDispose, this );
            if ( _toDispose <= 0 )
              throw new Updated();
          }
          string _mssg = "Cannot find IPR account to dispose the tobacco: Tobacco batch: {0}, fg batch: {1}, disposal: {2}";
          throw new IPRDataConsistencyException( "Material.ProcessDisposals", String.Format( _mssg, this.Batch, parent.Batch0, _kind ), null, "IPR unrecognized account" );
        }
        catch ( Updated ) { }
        catch ( IPRDataConsistencyException _ex )
        {
          _ex.Add2Log( edc );
        }
      }
    }
    private class Updated: Exception { }
    #endregion

    #region private
    private const string m_keyForam = "{0}:{1}:{2}";
    #endregion

  }
}
