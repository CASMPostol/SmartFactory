﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Material 
  /// </summary>
  public partial class Material//: IComparable<Material>, IEquatable<Material>
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
      if ( !( this.ProductType.Value == Linq.ProductType.IPRTobacco || this.ProductType.Value == Linq.ProductType.Tobacco ) )
        return;
      if ( this.ProductType.Value == Linq.ProductType.IPRTobacco )
      {
        List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( edc, Batch );
        if ( _accounts.Count == 1 && Math.Abs( _accounts[ 0 ].TobaccoNotAllocated.Value - TobaccoQuantity.Value ) < 1 )
          TobaccoQuantity = _accounts[ 0 ].TobaccoNotAllocated;
      }
      decimal material = TobaccoQuantityDec;
      decimal overuseInKg = 0;
      if ( overusage > 0 )
      {
        overuseInKg = this[ DisposalEnum.OverusageInKg ] = ( TobaccoQuantityDec * Convert.ToDecimal( overusage ) ).Rount2Decimals();
        material -= overuseInKg;
      }
      else
        this[ DisposalEnum.OverusageInKg ] = 0;
      decimal dust = this[ DisposalEnum.Dust ] = 0;
      decimal shMenthol = this[ DisposalEnum.SHMenthol ] = 0;
      decimal waste = this[ DisposalEnum.Waste ] = 0;
      if ( this.ProductType.Value == Linq.ProductType.IPRTobacco )
      {
        dust = this[ DisposalEnum.Dust ] = ( material * Convert.ToDecimal( ratios.dustRatio ) ).Rount2Decimals();
        shMenthol = this[ DisposalEnum.SHMenthol ] = ( material * Convert.ToDecimal( ratios.shMentholRatio ) ).Rount2Decimals();
        waste = this[ DisposalEnum.Waste ] = ( material * Convert.ToDecimal( ratios.wasteRatio ) ).Rount2Decimals();
      }
      this[ DisposalEnum.TobaccoInCigaretess ] = material - shMenthol - waste - dust;
    }
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
            throw new ArgumentException( "Index out of range at Material.Indexer", "index" );
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
            throw new ArgumentException( "Index out of range at Material.Indexer", "index" );
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
      return Convert.ToDecimal( ( this.Tobacco.Value * invoice.Quantity.Value / this.Material2BatchIndex.FGQuantity.Value ).Rount2Decimals() );
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
    /// Exports the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="closingBatch">if set to <c>true</c> [closing batch].</param>
    /// <param name="invoiceContent">Content of the invoice.</param>
    /// <param name="disposals">The disposals.</param>
    /// <exception cref="CAS">internal error: it is imposible to mark as exported the material;Material export`</exception>
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
      if ( closingBatch || _quantity == 0 )
        return;
      string _error = String.Format(
        "There are {0} kg of material {1}/Id={2} that cannot be found for invoice {3}/Content Id={4}.",
        _quantity, this.Batch, this.Identyfikator, invoiceContent.InvoiceIndex.BillDoc, invoiceContent.Identyfikator.Value );
      throw new CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException( "internal error: it is imposible to mark as exported the material", "Material export`", _error, false );
    }
    public override string ToString()
    {
      return GetKey();
    }
    #endregion

    #region internal
    /// <summary>
    /// Gets the tobacco total.
    /// </summary>
    /// <value>
    /// The tobacco total.
    /// </value>
    internal decimal TobaccoQuantityDec { get { return Convert.ToDecimal( this.TobaccoQuantity.GetValueOrDefault( 0 ) ); } }
    internal Material ReplaceByExistingOne( List<Material> _oldMaterials, List<Material> _newMaterials, Dictionary<string, Material> parentsMaterials, Batch parent )
    {
      Material _old = null;
      if ( !parentsMaterials.TryGetValue( this.GetKey(), out _old ) )
      {
        Debug.Assert( this.Material2BatchIndex == null, "Material2BatchIndex must be equl null for new materials" );
        this.Material2BatchIndex = parent;
        _newMaterials.Add( this );
        return this;
      }
      Debug.Assert( this != _old, "this cannot be the same as old" );
      Debug.Assert( _old.Material2BatchIndex == parent, "Material2BatchIndex must be equl parent for old materials" );
      Material _newOld = ( from _idx in parent.Material where _old.Identyfikator == _idx.Identyfikator select _idx ).First();
      Debug.Assert( _old == _newOld, "Material2BatchIndex must be equl parent for old materials" );
      Debug.Assert( ( (Microsoft.SharePoint.Linq.ITrackEntityState)_old ).EntityState != Microsoft.SharePoint.Linq.EntityState.ToBeInserted, "EntityState is in wrong state: ToBeInserted" );
      _old = _newOld;
      _oldMaterials.Add( _old );
      _old.FGQuantity = 0;
      _old.FGQuantity = this.FGQuantity;
      _old.TobaccoQuantity = this.TobaccoQuantity;
      Debug.Assert( ( (Microsoft.SharePoint.Linq.ITrackEntityState)_old ).EntityState == Microsoft.SharePoint.Linq.EntityState.ToBeUpdated, "EntityState is in wrong state: must be ToBeInserted" );
      return _old;
    }
    internal void UpdateDisposals( Entities edc, Batch parent, ProgressChangedEventHandler progressChanged )
    {
      if ( this.ProductType.Value != Linq.ProductType.IPRTobacco )
        return;
      foreach ( WebsiteModel.Linq.DisposalEnum _kind in Enum.GetValues( typeof( WebsiteModel.Linq.DisposalEnum ) ) )
      {
        try
        {
          if ( _kind == DisposalEnum.Tobacco || _kind == DisposalEnum.Cartons )
            continue;
          decimal _toDispose = this[ _kind ];
          IQueryable<Disposal> _disposals = Linq.Disposal.Disposals( this.Disposal, _kind );
          if ( _disposals.Count<Disposal>() > 0 )
          {
            bool _break = false;
            _toDispose -= _disposals.Sum<Disposal>( x => x.SettledQuantityDec );
            _disposals = _disposals.Where( v => v.CustomsStatus.Value == CustomsStatus.NotStarted );
            foreach ( Linq.Disposal _dx in _disposals )
            {
              _dx.Adjust( ref _toDispose );
              if ( _toDispose <= 0 )
              {
                _break = true;
                break;
              }
            }
            if ( _break )
              continue;
          }
          progressChanged( this, new ProgressChangedEventArgs( 1, String.Format( "AddDisposal {0}, batch {1}", _kind, this.Batch ) ) );
          if ( ( ( _kind == DisposalEnum.SHMenthol ) || ( _kind == DisposalEnum.OverusageInKg ) ) && this[ _kind ] <= 0 )
            continue;
          AddNewDisposals( edc, _kind, ref _toDispose );
          if ( _toDispose <= 0 )
            continue;
          string _mssg = "Cannot find IPR account to dispose the tobacco of {3} kg: Tobacco batch: {0}, fg batch: {1}, disposal: {2}";
          throw new IPRDataConsistencyException( "Material.UpdateDisposals", String.Format( _mssg, this.Batch, parent.Batch0, _kind, _toDispose ), null, "IPR unrecognized account" );
        }
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
    internal void GetInventory( Balance.StockDictionary balanceStock, Balance.StockDictionary.StockValueKey key, double portion )
    {
      switch ( this.ProductType.Value )
      {
        case Linq.ProductType.Cutfiller:
        case Linq.ProductType.Cigarette:
        case Linq.ProductType.Tobacco:
        case Linq.ProductType.Other:
          break;
        case Linq.ProductType.IPRTobacco:
          balanceStock.Sum( Convert.ToDecimal( this.TobaccoQuantity * portion ), this.Batch, key );
          break;
      }
    }
    #endregion

    #region private
    /// <summary>
    /// Gets the key.
    /// </summary>
    /// <returns></returns>
    internal string GetKey()
    {
      return String.Format( m_keyForam, SKU, Batch, this.StorLoc );
    }
    private const string m_keyForam = "{0}:{1}:{2}";
    #endregion

    #region IComparable<Material> Members
    /// <summary>
    /// Compares to.
    /// </summary>
    /// <param name="other">The other.</param>
    /// <returns></returns>
    public int CompareTo( Material other )
    {
      return GetKey().CompareTo( other.GetKey() );
    }
    #endregion

    #region IEquatable<Material> Members
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// true if the current object is equal to the other parameter; otherwise, false.
    /// </returns>
    public bool Equals( Material other )
    {
      return GetKey().CompareTo( other.GetKey() ) == 0;
    }
    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
    /// </returns>
    public override int GetHashCode()
    {
      return GetKey().GetHashCode();
    }
    /// <summary>
    /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
    /// </summary>
    /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
    /// <returns>
    ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
    /// </returns>
    public override bool Equals( object obj )
    {
      return Equals( (Material)obj );
    }
    #endregion
  }
}
