using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Summary Content Info
  /// </summary>
  public abstract class SummaryContentInfo: SortedList<string, Material>
  {
    #region ctor
    public SummaryContentInfo()
    {
      AccumulatedDisposalsAnalisis = new DisposalsAnalisis();
    }
    #endregion

    #region public
    /// <summary>
    /// Contains all materials sorted using the following key: SKU,Batch,Location.
    /// </summary>
    public class DisposalsAnalisis: SortedList<Material.DisposalsEnum, decimal>
    {
      internal DisposalsAnalisis()
      {
        foreach ( WebsiteModel.Linq.Material.DisposalsEnum _item in Enum.GetValues( typeof( WebsiteModel.Linq.Material.DisposalsEnum ) ) )
          this.Add( _item, 0 );
      }
      internal void Accumutate( Material material )
      {
        foreach ( WebsiteModel.Linq.Material.DisposalsEnum _item in Enum.GetValues( typeof( WebsiteModel.Linq.Material.DisposalsEnum ) ) )
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
    ///// <summary>
    ///// Initializes a new instance of the <see cref="SummaryContentInfo" /> class.
    ///// </summary>
    ///// <param name="oldOne">The old <see cref="Batch" />.</param>
    //public void Subtract( Batch oldOne )
    //{
    //  List<string> _warnings = new List<string>();
    //  foreach ( Material _mix in oldOne.Material )
    //    Subtract( _mix, _warnings );
    //  if ( _warnings.Count == 0 )
    //    return;
    //  string _msg = String.Format( " problems to associate intermediate batch with {0}", oldOne.Batch0 );
    //  throw new InputDataValidationException( _msg, "Subtract", _warnings );
    //}
    internal void ProcessDisposals
      ( Entities edc, Batch parent, double dustRatio, double shMentholRatio, double wasteRatio, double overusageCoefficient, ProgressChangedEventHandler progressChanged )
    {
      if ( Product == null )
        throw new IPRDataConsistencyException( "Material.ProcessDisposals", "Summary content info has unassigned Product property", null, "Wrong batch - product is unrecognized." );
      try
      {
        InsertAllOnSubmit( edc, parent );
        foreach ( Material _materialX in this.Values )
        {
          if ( _materialX.ProductType.Value != ProductType.IPRTobacco )
            continue;
          progressChanged( this, new ProgressChangedEventArgs( 1, "DisposalsAnalisis" ) );
          Material.Ratios _mr = new Material.Ratios { dustRatio = dustRatio, shMentholRatio = shMentholRatio, wasteRatio = wasteRatio };
          _materialX.DisposalsAnalisis( edc, _mr, overusageCoefficient );
          progressChanged( this, new ProgressChangedEventArgs( 1, "AccumulatedDisposalsAnalisis" ) );
          AccumulatedDisposalsAnalisis.Accumutate( _materialX );
          foreach ( WebsiteModel.Linq.Material.DisposalsEnum _kind in Enum.GetValues( typeof( WebsiteModel.Linq.Material.DisposalsEnum ) ) )
          {
            try
            {
              if ( _materialX[ _kind ] <= 0 && ( ( _kind == WebsiteModel.Linq.Material.DisposalsEnum.SHMenthol ) || ( _kind == WebsiteModel.Linq.Material.DisposalsEnum.OverusageInKg ) ) )
                continue;
              List<WebsiteModel.Linq.IPR> _accounts = WebsiteModel.Linq.IPR.FindIPRAccountsWithNotAllocatedTobacco( edc, _materialX.Batch );
              if ( _accounts.Count == 0 )
              {
                string _mssg = "Cannot find any IPR account to dispose the tobacco: Tobacco batch: {0}, fg batch: {1}, disposal: {2}";
                throw new IPRDataConsistencyException( "Material.ProcessDisposals", String.Format( _mssg, _materialX.Batch, parent.Batch0, _kind ), null, "IPR unrecognized account" );
              }
              decimal _toDispose = _materialX[ _kind ];
              progressChanged( this, new ProgressChangedEventArgs( 1, String.Format( "AddDisposal {0}, batch {1}", _kind, _materialX.Batch ) ) );
              for ( int _aidx = 0; _aidx < _accounts.Count; _aidx++ )
              {
                _accounts[ _aidx ].AddDisposal( edc, _kind, ref _toDispose, _materialX );
                if ( _toDispose <= 0 )
                  break;
              }
              edc.SubmitChanges();
            }
            catch ( IPRDataConsistencyException _ex )
            {
              _ex.Add2Log( edc );
            }
          }
          progressChanged( this, new ProgressChangedEventArgs( 1, "SubmitChanges" ) );
          edc.SubmitChanges();
        }
      }
      catch ( Exception _ex )
      {
        throw new IPRDataConsistencyException( "Material.ProcessDisposals", _ex.Message, _ex, "Disposal processing error" );
      }
    }
    /// <summary>
    /// Validates this instance.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <exception cref="InputDataValidationException">Batch content validate failed;XML content validation</exception>
    public void Validate( Entities edc, Microsoft.SharePoint.Linq.EntitySet<Disposal> disposals )
    {
      List<string> _validationErrors = new List<string>();
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
                                                 select new { BatchId = _mx.Batch, quantity = _mx.TobaccoTotal } ).ToDictionary( k => k.BatchId, v => v.quantity );
      foreach ( Disposal _dx in disposals )
        if ( !_materials.Keys.Contains<string>( _dx.Disposal2BatchIndex.Batch0 ) )
          _materials.Add( _dx.Disposal2BatchIndex.Batch0, Convert.ToDecimal( _dx.SettledQuantity.Value ) );
        else
          _materials[ _dx.Disposal2BatchIndex.Batch0 ] -= Convert.ToDecimal( _dx.SettledQuantity.Value );
      foreach ( var item in _materials )
      {
        if ( !IPR.IsAvailable( edc, item.Key, Convert.ToDouble( item.Value ) ) )
        {
          string _mssg = "Cannot find any IPR account to dispose the tobacco: Tobacco batch: {0}, fg batch: {1}, quantity: {2} kg";
          _validationErrors.Add( String.Format( _mssg, item.Key, Product.Batch, item.Value ) );
        }
      }
      if ( _validationErrors.Count > 0 )
        throw new InputDataValidationException( "Batch content validate failed", "XML content validation", _validationErrors );
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
        TotalTobacco += value.TobaccoTotal;
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
        TotalTobacco -= value.TobaccoTotal;
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
