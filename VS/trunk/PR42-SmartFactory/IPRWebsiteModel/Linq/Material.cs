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
    internal struct Ratios
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
    internal void CalculateCompensationComponents( Entities edc, Ratios ratios, double overusage )
    {
      decimal material = Convert.ToDecimal( this.TobaccoQuantity );
      List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( edc, Batch );
      if ( _accounts.Count == 1 && Math.Abs( Convert.ToDecimal( _accounts[ 0 ].TobaccoNotAllocated.Value ) - material ) < 1 )
        material = _accounts[ 0 ].TobaccoNotAllocatedDec;
      if ( overusage > 0 )
      {
        decimal _overuseInKg = this[ DisposalEnum.OverusageInKg ] = ( material * Convert.ToDecimal( overusage ) ).RountMass();
        material -= _overuseInKg;
      }
      decimal _dust = this[ DisposalEnum.Dust ] = ( material * Convert.ToDecimal( ratios.dustRatio ) ).RountMass();
      decimal _shMenthol = this[ DisposalEnum.SHMenthol ] = ( material * Convert.ToDecimal( ratios.shMentholRatio ) ).RountMass();
      decimal _waste = this[ DisposalEnum.Waste ] = ( material * Convert.ToDecimal( ratios.wasteRatio ) ).RountMass();
      this[ DisposalEnum.TobaccoInCigaretess ] = material - _shMenthol - _waste - _dust;
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
          case DisposalEnum.TobaccoInCigaretess:
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
          case DisposalEnum.TobaccoInCigaretess:
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
    /// <param name="invoice">The invoice.</param>
    /// <returns>
    /// Returns portion of calculated tobacco quantity. i.e. after removing compensation wast, dust, SHMethol, etc.
    /// </returns>
    public decimal CalculatedQuantity( InvoiceContent invoice )
    {
      return Convert.ToDecimal( ( this.Tobacco.Value * invoice.Quantity.Value / this.Material2BatchIndex.FGQuantity.Value ).RountMass() );
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
    public void Export( Entities entities, bool closingBatch, InvoiceContent invoiceContent, List<Disposal> disposals )
    {
      decimal _quantity = this.CalculatedQuantity( invoiceContent );
      foreach ( Disposal _disposalX in this.GetListOfDisposals() )
      {
        if ( !closingBatch && _quantity == 0 )
          break;
        _disposalX.Export( entities, ref _quantity, closingBatch, invoiceContent );
        disposals.Add( _disposalX );
      }
      if ( _quantity == 0 )
        return;
      string _error = String.Format(
        "There are {0} kg of material {1}/Id={2} that cannot be found for invoice {3}/Content Id={4}.",
        _quantity, this.Batch, this.Identyfikator, invoiceContent.InvoiceIndex.BillDoc, invoiceContent.Identyfikator.Value );
      throw new CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException( "internal error: it is imposible to mark as exported the material", "Material export`", _error );
    }
    internal Material ReplaceByExistingOne( List<Material> _newMaterials, Linq.Batch parent )
    {
      Material _old = ( from _mx in parent.Material where _mx.Batch.Contains( this.Batch ) select _mx ).FirstOrDefault<Material>();
      if ( _old == null )
      {
        this.Material2BatchIndex = parent;
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
          if ( _kind == DisposalEnum.Tobacco || _kind == DisposalEnum.Cartons )
            continue;
          if ( ( ( _kind == DisposalEnum.SHMenthol ) || ( _kind == DisposalEnum.OverusageInKg ) ) && this[ _kind ] <= 0 )
            continue;
          decimal _toDispose = this[ _kind ];
          IQueryable<Disposal> _disposals = Linq.Disposal.Disposals( this.Disposal, _kind );
          if ( _disposals.Count<Disposal>() > 0 )
          {
            _toDispose -= _disposals.Sum<Disposal>( x => x.SettledQuantityDec );
            _disposals = _disposals.Where( v => v.CustomsStatus.Value == CustomsStatus.NotStarted );
            foreach ( Linq.Disposal _dx in _disposals )
            {
              _dx.Adjust( ref _toDispose );
              if ( _toDispose <= 0 )
                throw new Updated();
            }
          }
          progressChanged( this, new ProgressChangedEventArgs( 1, String.Format( "AddDisposal {0}, batch {1}", _kind, this.Batch ) ) );
          AddNewDisposals( edc, _kind, ref _toDispose );
          if ( _toDispose <= 0 )
            throw new Updated();
          string _mssg = "Cannot find IPR account to dispose the tobacco of {3} kg: Tobacco batch: {0}, fg batch: {1}, disposal: {2}";
          throw new IPRDataConsistencyException( "Material.ProcessDisposals", String.Format( _mssg, this.Batch, parent.Batch0, _kind, _toDispose ), null, "IPR unrecognized account" );
        }
        catch ( Updated ) { }
        catch ( IPRDataConsistencyException _ex )
        {
          _ex.Add2Log( edc );
        }
      }
    }
    internal void AddNewDisposals( Entities edc, Linq.DisposalEnum _kind, ref decimal _toDispose )
    {
      List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( edc, this.Batch );
      foreach ( IPR _iprx in _accounts )
      {
        _iprx.AddDisposal( edc, _kind, ref _toDispose, this );
        if ( _toDispose <= 0 )
          return;
      }
    }
    internal void AddNewDisposals( Entities edc, Linq.DisposalEnum _kind, ref decimal _toDispose, InvoiceContent invoiceContent )
    {
      List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( edc, this.Batch );
      foreach ( IPR _iprx in _accounts )
      {
        _iprx.AddDisposal( edc, _kind, ref _toDispose, this, invoiceContent );
        if ( _toDispose <= 0 )
          break;
      }
    }
    #endregion

    #region private
    private class Updated: Exception { }
    private const string m_keyForam = "{0}:{1}:{2}";
    #endregion

  }
}
