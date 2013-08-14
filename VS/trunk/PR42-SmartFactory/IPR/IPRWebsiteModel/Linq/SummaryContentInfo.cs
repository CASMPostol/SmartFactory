//<summary>
//  Title   : Summary Batch Content Info
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SmartFactory.Customs;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Summary Batch Content Info
  /// </summary>
  public abstract class SummaryContentInfo: SortedList<string, Material>
  {
    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="SummaryContentInfo" /> class.
    /// </summary>
    protected SummaryContentInfo()
    {
      AccumulatedDisposalsAnalisis = new DisposalsAnalisis();
    }
    #endregion

    #region public
    /// <summary>
    /// Contains all materials sorted using the following key: SKU,Batch,Location.
    /// </summary>
    public class DisposalsAnalisis: SortedList<DisposalEnum, decimal>
    {
      internal DisposalsAnalisis()
      {
        foreach ( Linq.DisposalEnum _item in Enum.GetValues( typeof( WebsiteModel.Linq.DisposalEnum ) ) )
          this.Add( _item, 0 );
      }
      internal void Accumutate( Material material )
      {
        foreach ( Linq.DisposalEnum _item in Enum.GetValues( typeof( WebsiteModel.Linq.DisposalEnum ) ) )
        {
          if ( _item == DisposalEnum.Tobacco || _item == DisposalEnum.Cartons )
            continue;
          this[ _item ] += material[ _item ];
        }
      }
    } //DisposalsAnalisis
    /// <summary>
    /// Gets the product.
    /// </summary>
    /// <value>
    /// The product.
    /// </value>
    public Material Product { get; private set; }
    /// <summary>
    /// Gets the SKU lookup.
    /// </summary>
    /// <value>
    /// The SKU lookup.
    /// </value>
    public SKUCommonPart SKULookup { get; private set; }
    /// <summary>
    /// Gets the accumulated disposals analisis.
    /// </summary>
    /// <value>
    /// The accumulated disposals analisis.
    /// </value>
    public DisposalsAnalisis AccumulatedDisposalsAnalisis { get; private set; }
    /// <summary>
    /// Gets the total quantity of the tobacco.
    /// </summary>
    /// <value>
    /// The total tobacco.
    /// </value>
    internal decimal TotalTobacco
    {
      get { return myTotalTobacco; }
    }
    internal void ProcessMaterials( Entities entities, Batch parent, Material.Ratios materialRatios, double overusageCoefficient, ProgressChangedEventHandler progressChanged )
    {
      if ( Product == null )
        throw new IPRDataConsistencyException( "SummaryContentInfo.ProcessMaterials", "Summary content info has unassigned Product property", null, "Wrong batch - product is unrecognized." );
      try
      {
        List<Material> _newMaterialList = new List<Material>();
        List<Material> _oldMaterialList = new List<Material>();
        List<Material> _copyThis = new List<Material>();
        _copyThis.AddRange( this.Values );
        Dictionary<string, Material> _parentsMaterials = parent.Material.ToDictionary<Material, string>( x => x.GetKey() );
        foreach ( Material _materialX in _copyThis )
        {
          progressChanged( this, new ProgressChangedEventArgs( 1, "DisposalsAnalisis" ) );
          Material _material = _materialX.ReplaceByExistingOne( _oldMaterialList, _newMaterialList, _parentsMaterials, parent );
          progressChanged( this, new ProgressChangedEventArgs( 1, "CalculateCompensationComponents" ) );
          if ( !_materialX.IsTobacco )
            continue;
          _material.CalculateCompensationComponents( entities, materialRatios, overusageCoefficient );
          progressChanged( this, new ProgressChangedEventArgs( 1, "if ( _material.ProductType.Value" ) );
          if ( _material.ProductType.Value == ProductType.IPRTobacco )
          {
            progressChanged( this, new ProgressChangedEventArgs( 1, "AccumulatedDisposalsAnalisis" ) );
            AccumulatedDisposalsAnalisis.Accumutate( _material );
          }
        }
        if ( _newMaterialList.Count > 0 )
        {
          progressChanged( this, new ProgressChangedEventArgs( 1, "InsertAllOnSubmit" ) );
          entities.Material.InsertAllOnSubmit( _newMaterialList );
        }
        foreach ( Material _omx in _oldMaterialList )
        {
          this.Remove( _omx.GetKey() );
          this.Add( _omx.GetKey(), _omx );
        }
      }
      catch ( Exception _ex )
      {
        throw new IPRDataConsistencyException( "SummaryContentInfo.ProcessMaterials", _ex.Message, _ex, "Disposal processing error" );
      }
    }
    internal void UpdateNotStartedDisposals( Entities edc, Batch parent, ProgressChangedEventHandler progressChanged )
    {
      progressChanged( this, new ProgressChangedEventArgs( 1, "UpdateDisposals" ) );
      foreach ( Material _materialX in this.Values )
        _materialX.UpdateDisposals( edc, parent, progressChanged );
    }
    /// <summary>
    /// Validates this instance.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/>.</param>
    /// <param name="disposals">The list of disposals created already.</param>
    /// <exception cref="InputDataValidationException">Batch content validate failed;XML content validation</exception>
    public void Validate( Entities edc, EntitySet<Disposal> disposals )
    {
      ErrorsList _validationErrors = new ErrorsList();
      if ( Product == null )
        _validationErrors.Add( new Warnning( "Unrecognized finished good", true ) );
      SKULookup = SKUCommonPart.Find( edc, Product.SKU );
      if ( SKULookup == null )
      {
        string _msg = "Cannot find finished good SKU={0} in the SKU dictionary - dictionary update is required";
        _validationErrors.Add( new Warnning( String.Format( _msg, Product.SKU ), true ) );
      }
      if ( Product.ProductType != ProductType.Cigarette )
        return;
      Dictionary<string, decimal> _materials = ( from _mx in disposals
                                                 select new { batchId = _mx.Disposal2BatchIndex.Batch0, quantity = _mx.SettledQuantityDec } ).ToDictionary( k => k.batchId, v => v.quantity );
      foreach ( Material _qutty in this.Values )
      {
        if ( _qutty.ProductType.Value != Linq.ProductType.IPRTobacco )
          continue;
        decimal _disposed = 0;
        _materials.TryGetValue( _qutty.Batch, out _disposed );
        decimal _diff = _qutty.Accounts2Dispose.Sum<IPR>( a => a.TobaccoNotAllocatedDec ) + _disposed - _qutty.TobaccoQuantityDec;
        if ( _diff < -Settings.MinimalOveruse ) 
        {
          string _mssg = "Cannot find any IPR account to dispose the quantity {3}: Tobacco batch: {0}, fg batch: {1}, quantity to dispose: {2} kg";
          _validationErrors.Add( new Warnning( String.Format( _mssg, _qutty.Batch, Product.Batch, _qutty.TobaccoQuantityDec, -_diff ), true ) );
        }
      }
      if ( _validationErrors.Count > 0 )
        throw new InputDataValidationException( "Batch content validation failed", "XML content validation", _validationErrors );
    }
    internal void AdjustMaterialQuantity( bool finalBatch, ProgressChangedEventHandler progressChanged )
    {
      if ( this.Product.ProductType.Value != ProductType.IPRTobacco || !finalBatch )
        return;
      foreach ( Material _mx in this.Values )
        _mx.AdjustTobaccoQuantity( ref myTotalTobacco, progressChanged );
    }
    internal void AdjustOveruse( Material.Ratios materialRatios )
    {
      List<Material> _tobacco = this.Values.Where<Material>( x => x.ProductType.Value == ProductType.IPRTobacco ).ToList<Material>();
      decimal _2remove = 0;
      List<Material> _2Add = new List<Material>();
      foreach ( Material _mx in _tobacco )
        _2remove += _mx.RemoveOveruseIfPossible( _2Add, materialRatios );
      if ( _2Add.Count == 0 )
        _2Add.Add( _tobacco.Max<Material, Material>( x => x ) );
      decimal _AddingCff = _2remove / _2Add.Sum<Material>( x => x.TobaccoQuantityDec );
      foreach ( Material _mx in _2Add )
        _mx.IncreaseOveruse( _AddingCff, materialRatios );
    }
    #endregion

    #region private
    private decimal myTotalTobacco = 0;
    /// <summary>
    /// Adds an element with the specified key and value.
    /// </summary>
    /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
    /// <exception cref="System.ArgumentNullException">key is null</exception>
    /// <exception cref="System.ArgumentException">An element with the same key as the <paramref name="value" /> that already exists in the collection.</exception>
    protected void Add( Material value )
    {
      Material ce = null;
      if ( value.ProductType == ProductType.IPRTobacco || value.ProductType == ProductType.Tobacco )
        myTotalTobacco += value.TobaccoQuantityDec;
      if ( this.TryGetValue( value.GetKey(), out ce ) )
      {
        ce.FGQuantity += value.FGQuantity;
        ce.TobaccoQuantity += value.TobaccoQuantity;
      }
      else
      {
        if ( value.ProductType == ProductType.Cigarette )
          Product = value;
        else if ( Product == null && value.ProductType == ProductType.Cutfiller )
          Product = value;
        base.Add( value.GetKey(), value );
      }
    }
    private void Subtract( Material value, List<string> _warnings )
    {
      if ( value.ProductType == ProductType.IPRTobacco || value.ProductType == ProductType.Tobacco )
        myTotalTobacco -= value.TobaccoQuantityDec;
      if ( this.TryGetValue( value.GetKey(), out value ) )
      {
        value.FGQuantity -= value.FGQuantity;
        value.TobaccoQuantity -= value.TobaccoQuantity;
      }
      else
        _warnings.Add( String.Format( "Cannot find material {0} to subtract", value.ToString() ) );
    }
    private void InsertAllOnSubmit( Entities entities, Batch parent )
    {
      foreach ( Material item in Values )
        item.Material2BatchIndex = parent;
      entities.Material.InsertAllOnSubmit( this.Values );
    }
    #endregion
  }//SummaryContentInfo
}
