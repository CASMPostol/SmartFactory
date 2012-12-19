using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.SharePoint.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Summary Content Info
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
          this[ _item ] += material[ _item ];
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
    internal decimal TotalTobacco { get; private set; }
    internal void ProcessMaterials
      ( Entities edc, Batch parent, double dustRatio, double shMentholRatio, double wasteRatio, double overusageCoefficient, ProgressChangedEventHandler progressChanged )
    {
      if ( Product == null )
        throw new IPRDataConsistencyException( "SummaryContentInfo.ProcessMaterials", "Summary content info has unassigned Product property", null, "Wrong batch - product is unrecognized." );
      try
      {
        //InsertAllOnSubmit( edc, parent );
        List<Material> _newMaterialList = new List<Material>();
        List<Material> _oldMaterialList = new List<Material>();
        Material.Ratios _mr = new Material.Ratios { dustRatio = dustRatio, shMentholRatio = shMentholRatio, wasteRatio = wasteRatio };
        List<Material> _copyThis = new List<Material>();
        _copyThis.AddRange( this.Values );
        foreach ( Material _materialX in _copyThis )
        {
          progressChanged( this, new ProgressChangedEventArgs( 1, "DisposalsAnalisis" ) );
          Material _material = _materialX.ReplaceByExistingOne( _oldMaterialList, _newMaterialList, parent );
          progressChanged( this, new ProgressChangedEventArgs( 1, "CalculateCompensationComponents" ) );
          _material.CalculateCompensationComponents( edc, _mr, overusageCoefficient );
          if ( _material.ProductType.Value == ProductType.IPRTobacco )
          {
            progressChanged( this, new ProgressChangedEventArgs( 1, "AccumulatedDisposalsAnalisis" ) );
            AccumulatedDisposalsAnalisis.Accumutate( _material );
          }
          if ( _newMaterialList.Count > 0 )
          {
            progressChanged( this, new ProgressChangedEventArgs( 1, "InsertAllOnSubmit" ) );
            edc.Material.InsertAllOnSubmit( _newMaterialList );
          }
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
        _validationErrors.Add( "Unrecognized finished good" );
      SKULookup = SKUCommonPart.Find( edc, Product.SKU );
      if ( SKULookup == null )
      {
        string _msg = "Cannot find finished good SKU={0} in the SKU dictionary - dictionary update is required";
        _validationErrors.Add( String.Format( _msg, Product.SKU ) );
      }
      Dictionary<string, decimal> _materials = ( from _mx in this.Values
                                                 where _mx.ProductType.Value == Linq.ProductType.IPRTobacco
                                                 select new { batchId = _mx.Batch, quantity = _mx.TobaccoQuantityDec } ).ToDictionary( k => k.batchId, v => v.quantity );
      foreach ( Disposal _dx in disposals )
        if ( !_materials.Keys.Contains<string>( _dx.Disposal2BatchIndex.Batch0 ) )
          _materials.Add( _dx.Disposal2BatchIndex.Batch0, _dx.SettledQuantityDec );
        else
          _materials[ _dx.Disposal2BatchIndex.Batch0 ] -= _dx.SettledQuantityDec;
      foreach ( var _qutty in _materials )
      {
        decimal _diff = IPR.IsAvailable( edc, _qutty.Key, _qutty.Value );
        if ( Math.Abs(_diff) > 1 )  //TODO add common cheking point and get data from settings.
        {
          string _mssg = "Cannot find any IPR account to dispose the tobacco quantity {3}: Tobacco batch: {0}, fg batch: {1}, quantity: {2} kg";
          _validationErrors.Add( String.Format( _mssg, _qutty.Key, Product.Batch, _qutty.Value, _diff ) );
        }
      }
      if ( _validationErrors.Count > 0 )
        throw new InputDataValidationException( "Batch content validation failed", "XML content validation", _validationErrors );
    }
    #endregion

    #region private
    /// <summary>
    /// Adds an element with the specified key and value into the System.Collections.Generic.SortedList<TKey,TValue>.
    /// </summary>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
    /// <exception cref="System.ArgumentNullException">key is null</exception>
    /// <exception cref="System.ArgumentException">An element with the same key already exists in the <paramref name="System.Collections.Generic.SortedList<TKey,TValue>"/>.</exception>
    protected void Add( Material value )
    {
      Material ce = null;
      if ( value.ProductType == ProductType.IPRTobacco || value.ProductType == ProductType.Tobacco )
        TotalTobacco += value.TobaccoQuantityDec;
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
        TotalTobacco -= value.TobaccoQuantityDec;
      if ( this.TryGetValue( value.GetKey(), out value ) )
      {
        value.FGQuantity -= value.FGQuantity;
        value.TobaccoQuantity -= value.TobaccoQuantity;
      }
      else
        _warnings.Add( String.Format( "Cannot find material {0} to subtract", value.GetKey() ) );
    }
    private void InsertAllOnSubmit( Entities edc, Batch parent )
    {
      foreach ( Material item in Values )
        item.Material2BatchIndex = parent;
      edc.Material.InsertAllOnSubmit( this.Values );
    }
    #endregion
  }//SummaryContentInfo
}
