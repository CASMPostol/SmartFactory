using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Summary Content Info
  /// </summary>
  public abstract class SummaryContentInfo: SortedList<string, Material>
  {
    #region public

    public Material Product { get; private set; }
    public DisposalsAnalisis AccumulatedDisposalsAnalisis { get; private set; }
    internal decimal TotalTobacco { get; private set; }
    internal void ProcessDisposals
      ( Entities _edc, Batch _parent, double _dustRatio, double _shMentholRatio, double _wasteRatio, double _overusageCoefficient, ProgressChangedEventHandler _progressChanged )
    {
      if ( Product == null )
        throw new IPRDataConsistencyException( "Material.ProcessDisposals", "Summary content info has unassigned Product property", null, "Wrong batch - product is unrecognized." );
      try
      {
        InsertAllOnSubmit( _edc, _parent );
        foreach ( Material _materialX in this.Values )
        {
          if ( _materialX.ProductType.Value != ProductType.IPRTobacco )
            continue;
          _progressChanged( this, new ProgressChangedEventArgs( 1, "DisposalsAnalisis" ) );
          Material.Ratios _mr = new Material.Ratios { dustRatio = _dustRatio, shMentholRatio = _shMentholRatio, wasteRatio = _wasteRatio };
          _materialX.DisposalsAnalisis( _edc, _mr, _overusageCoefficient );
          _progressChanged( this, new ProgressChangedEventArgs( 1, "AccumulatedDisposalsAnalisis" ) );
          AccumulatedDisposalsAnalisis.Accumutate( _materialX );
          foreach ( WebsiteModel.Linq.Material.DisposalsEnum _item in Enum.GetValues( typeof( WebsiteModel.Linq.Material.DisposalsEnum ) ) )
          {
            try
            {
              if ( _materialX[ _item ] <= 0 && ( _item == WebsiteModel.Linq.Material.DisposalsEnum.SHMenthol ) )
                continue;
              List<WebsiteModel.Linq.IPR> _accounts = WebsiteModel.Linq.IPR.FindIPRAccountsWithNotAllocatedTobacco( _edc, _materialX.Batch );
              if ( _accounts.Count == 0 )
              {
                string _mssg = "Cannot find any IPR account to dispose the tobacco: Tobacco batch: {0}, fg batch: {1}, disposal: {2}";
                throw new IPRDataConsistencyException( "Material.ProcessDisposals", String.Format( _mssg, _materialX.Batch, _parent.Batch0, _item ), null, "IPR unrecognized account" );
              }
              decimal _toDispose = _materialX[ _item ];
              _progressChanged( this, new ProgressChangedEventArgs( 1, String.Format( "AddDisposal {0}, batch {1}", _item, _materialX.Batch ) ) );
              //TODOD  [pr4-3572] Adjust the tobacco usage while importing batch 
              for ( int _aidx = 0; _aidx < _accounts.Count; _aidx++ )
              {
                _accounts[ _aidx ].AddDisposal( _edc, _item, ref _toDispose, _materialX );
                if ( _toDispose <= 0 )
                  break;
              }
              _edc.SubmitChanges();
            }
            catch ( IPRDataConsistencyException _ex )
            {
              _ex.Add2Log( _edc );
            }
          }
          _progressChanged( this, new ProgressChangedEventArgs( 1, "SubmitChanges" ) );
          _edc.SubmitChanges();
        }
      }
      catch ( Exception _ex )
      {
        throw new IPRDataConsistencyException( "Material.ProcessDisposals", _ex.Message, _ex, "Disposal processing error" );
      }
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
    protected void CheckConsistence()
    {
      if ( Product == null )
        throw new IPRDataConsistencyException( "Processing disposals", "Unrecognized finished good", null, "CheckConsistence error" );
    }
    protected SummaryContentInfo()
    {
      AccumulatedDisposalsAnalisis = new DisposalsAnalisis();
    }
    private void InsertAllOnSubmit( Entities edc, Batch parent )
    {
      foreach ( var item in Values )
        item.Material2BatchIndex = parent;
      edc.Material.InsertAllOnSubmit( this.Values );
    }
    #endregion
    /// <summary>
    /// Contains all materials sorted using the following key: SKU,Batch,Location.
    /// </summary>
    public class DisposalsAnalisis: SortedList<Material.DisposalsEnum, decimal>
    {
      public DisposalsAnalisis()
      {
        foreach ( WebsiteModel.Linq.Material.DisposalsEnum _item in Enum.GetValues( typeof( WebsiteModel.Linq.Material.DisposalsEnum ) ) )
          this.Add( _item, 0 );
      }
      internal void Accumutate( Material material )
      {
        foreach ( WebsiteModel.Linq.Material.DisposalsEnum _item in Enum.GetValues( typeof( WebsiteModel.Linq.Material.DisposalsEnum ) ) )
          this[ _item ] += material[ _item ];
      }
    } //SummaryContentInfo

    public bool Validate( ActionResult _ar )
    {
      bool _ret = true;
      foreach ( Material item in this.Values)
      {
        if ( item.ProductType.Value != ProductType.IPRTobacco )
          continue;
      }
      return _ret;
    }
  }
}
