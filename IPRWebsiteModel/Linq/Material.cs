using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SharePoint;
using CAS.SmartFactory.IPR;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Material
  {
    #region public
    /// <summary>
    /// Contains all materials sorted using the following key: SKU,Batch,Location. <see cref="GetKey"/>
    /// </summary>
    internal class SummaryContentInfo: SortedList<string, Material>
    {
      #region public
      internal Material Product { get; private set; }
      internal double TotalTobacco { get; private set; }
      /// <summary>
      /// Adds an element with the specified key and value into the System.Collections.Generic.SortedList<TKey,TValue>.
      /// </summary>
      /// <param name="key">The key of the element to add.</param>
      /// <param name="value">The value of the element to add. The value can be null for reference types.</param>
      /// <exception cref="System.ArgumentNullException">key is null</exception>
      /// <exception cref="System.ArgumentException">An element with the same key already exists in the <paramref name="System.Collections.Generic.SortedList<TKey,TValue>"/>.</exception>
      internal void Add( Material value )
      {
        Material ce = null;
        if ( value.ProductType == Linq.IPR.ProductType.IPRTobacco || value.ProductType == Linq.IPR.ProductType.Tobacco )
          TotalTobacco += value.TobaccoQuantity.GetValueOrDefault( 0 );
        if ( this.TryGetValue( value.GetKey(), out ce ) )
        {
          ce.FGQuantity += value.FGQuantity;
          ce.TobaccoQuantity += value.TobaccoQuantity;
        }
        else
        {
          if ( value.ProductType == Linq.IPR.ProductType.Cigarette )
            Product = value;
          else if ( Product == null && value.ProductType == Linq.IPR.ProductType.Cutfiller )
            Product = value;
          base.Add( value.GetKey(), value );
        }
      }
      internal DisposalsAnalisis AccumulatedDisposalsAnalisis { get; private set; }
      internal void ProcessDisposals
        ( Entities _edc, Batch _parent, double _dustRatio, double _shMentholRatio, double _wasteRatio, double _overusageCoefficient, ProgressChangedEventHandler _progressChanged )
      {
        if ( Product == null )
          throw new IPRDataConsistencyException( "Material.ProcessDisposals", "Summary content info has unassigned Product property", null, "Wrong batch - product is unrecognized." );
        try
        {
          InsertAllOnSubmit( _edc, _parent );
          foreach ( Material _materialInBatch in this.Values )
          {
            if ( _materialInBatch.ProductType.Value != Linq.IPR.ProductType.IPRTobacco )
              continue;
            DisposalsAnalisis _dspsls = new DisposalsAnalisis( _edc, _materialInBatch.Batch, _materialInBatch.TobaccoQuantity.Value, _dustRatio, _shMentholRatio, _wasteRatio, _overusageCoefficient );
            _progressChanged( this, new ProgressChangedEventArgs( 1, "AccumulatedDisposalsAnalisis" ) );
            AccumulatedDisposalsAnalisis.Accumutate( _dspsls );
            foreach ( KeyValuePair<IPR.DisposalEnum, double> _item in _dspsls )
            {
              try
              {
                if ( _item.Value <= 0 && ( _item.Key == IPR.DisposalEnum.SHMenthol ) )
                  continue;
                List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( _edc, _materialInBatch.Batch );
                if ( _accounts.Count == 0 )
                {
                  string _mssg = "Cannot find any IPR account to dispose the tobacco: Tobacco batch: {0}, fg batch: {1}, disposal: {2}";
                  throw new IPRDataConsistencyException( "Material.ProcessDisposals", String.Format( _mssg, _materialInBatch.Batch, _parent.Batch0, _item.Key ), null, "IPR unrecognized account" );
                }
                double _toDispose = _item.Value;
                _progressChanged( this, new ProgressChangedEventArgs( 1, String.Format( "AddDisposal {0}, batch {1}", _item.Key, _materialInBatch.Batch ) ) );
                //TODOD  [pr4-3572] Adjust the tobacco usage while importing batch 
                for ( int _aidx = 0; _aidx < _accounts.Count; _aidx++ )
                {
                  _accounts[ _aidx ].AddDisposal( _edc, _item.Key, ref _toDispose, _materialInBatch );
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
      internal IEnumerable<Material> GeContentEnumerator()
      {
        return this.Values;
      }
      internal void CheckConsistence()
      {
        if ( Product == null )
          throw new IPRDataConsistencyException( "Processing disposals", "Unrecognized finisched good", null, "CheckConsistence error" );
      }
      #endregion

      #region private
      private void InsertAllOnSubmit( Entities edc, Batch parent )
      {
        foreach ( var item in Values )
          item.Material2BatchIndex = parent;
        edc.Material.InsertAllOnSubmit( GeContentEnumerator() );
      }
      #endregion
    } //SummaryContentInfo
    internal class DisposalsAnalisis: SortedList<IPR.DisposalEnum, double>
    {

      internal DisposalsAnalisis()
      {
        foreach ( IPR.DisposalEnum _item in Enum.GetValues( typeof( IPR.DisposalEnum ) ) )
          this.Add( _item, 0 );
      }
      internal DisposalsAnalisis( Entities _edc, string batch, double _material, double _dustRatio, double _shMentholRatio, double _wasteRatio, double _overusage )
      {
        List<IPR> _accounts = IPR.FindIPRAccountsWithNotAllocatedTobacco( _edc, batch );
        bool _closing = false;
        if ( _accounts.Count == 1 && Math.Abs( _accounts[ 0 ].TobaccoNotAllocated.Value - _material ) < 1 )
        {
          _material = _accounts[ 0 ].TobaccoNotAllocated.Value;
          _closing = true;
        }
        double _am;
        if ( _overusage > 0 )
        {
          this.Add( IPR.DisposalEnum.OverusageInKg, ( _material * _overusage ).RountMass() );
          _am = _material - this[ IPR.DisposalEnum.OverusageInKg ];
        }
        else
          _am = _material;
        this.Add( IPR.DisposalEnum.Dust, ( _am * _dustRatio ).RountMass() );
        this.Add( IPR.DisposalEnum.SHMenthol, ( _am * _shMentholRatio ).RountMass() );
        this.Add( IPR.DisposalEnum.Waste, ( _am * _wasteRatio ).RountMassUpper() );
        if ( _closing )
          this.Add( IPR.DisposalEnum.Tobacco, _am - this[ IPR.DisposalEnum.SHMenthol ] - this[ IPR.DisposalEnum.Waste ] - this[ IPR.DisposalEnum.Dust ] );
        else
          this.Add( IPR.DisposalEnum.Tobacco, ( _am - this[ IPR.DisposalEnum.SHMenthol ] - this[ IPR.DisposalEnum.Waste ] - this[ IPR.DisposalEnum.Dust ] ).RountMass() );
      }
      internal void Accumutate( DisposalsAnalisis _dspsls )
      {
        foreach ( IPR.DisposalEnum _item in Enum.GetValues( typeof( IPR.DisposalEnum ) ) )
          this[ _item ] += _dspsls.Keys.Contains( _item ) ? _dspsls[ _item ] : 0;
      }
    }
    internal SKUCommonPart SKULookup { get; set; }
    internal bool IPRMaterial { get; set; }
    #endregion

    #region private
    private string GetKey()
    {
      return String.Format( m_keyForam, SKU, Batch, this.StorLoc );
    }
    private void GetProductType( Entities edc )
    {
      Entities.ProductDescription product = edc.GetProductType( this.SKU, this.StorLoc );
      this.ProductType = product.productType;
      this.SKULookup = product.skuLookup;
      this.IPRMaterial = product.IPRMaterial;
    }
    private const string m_keyForam = "{0}:{1}:{2}";
    #endregion

  }
}
