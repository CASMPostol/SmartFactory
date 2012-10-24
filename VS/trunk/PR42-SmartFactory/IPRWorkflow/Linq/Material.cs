using System;
using System.Collections.Generic;
using System.ComponentModel;
using CAS.SharePoint;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using BatchMaterialXml = CAS.SmartFactory.xml.erp.BatchMaterial;

namespace CAS.SmartFactory.Linq.IPR
{
  internal static class MaterialExtension
  {
    #region private
    internal static Material Material( BatchMaterialXml item )
    {
      return new Material()
      {
        Batch = item.Batch,
        Material2BatchIndex = null,
        SKU = item.Material,
        StorLoc = item.Stor__Loc,
        SKUDescription = item.Material_description,
        Title = item.Material_description,
        Units = item.Unit,
        FGQuantity = Convert.ToDouble( item.Quantity ),
        TobaccoQuantity = Convert.ToDouble( item.Quantity_calculated ),
        ProductType = ProductType.Invalid,
        ProductID = item.material_group,
      };
    }
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
        if ( value.ProductType == ProductType.IPRTobacco || value.ProductType == ProductType.Tobacco )
          TotalTobacco += value.TobaccoQuantity.GetValueOrDefault( 0 );
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
            if ( _materialInBatch.ProductType.Value != ProductType.IPRTobacco )
              continue;
            DisposalsAnalisis _dspsls = new DisposalsAnalisis( _edc, _materialInBatch.Batch, _materialInBatch.TobaccoQuantity.Value, _dustRatio, _shMentholRatio, _wasteRatio, _overusageCoefficient );
            _progressChanged( this, new ProgressChangedEventArgs( 1, "AccumulatedDisposalsAnalisis" ) );
            AccumulatedDisposalsAnalisis.Accumutate( _dspsls );
            foreach ( KeyValuePair<CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum, double> _item in _dspsls )
            {
              try
              {
                if ( _item.Value <= 0 && ( _item.Key == CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.SHMenthol ) )
                  continue;
                List<CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR> _accounts = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.FindIPRAccountsWithNotAllocatedTobacco( _edc, _materialInBatch.Batch );
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
      public SummaryContentInfo( BatchMaterialXml[] xml, Entities edc, ProgressChangedEventHandler progressChanged )
      {
        AccumulatedDisposalsAnalisis = new DisposalsAnalisis();
        foreach ( BatchMaterialXml item in xml )
        {
          Material _newMaterial = Material( item );
          _newMaterial.GetProductType( edc );
          progressChanged( null, new ProgressChangedEventArgs( 1, String.Format( "SKU={0}", _newMaterial.SKU ) ) );
          Add( _newMaterial );
        }
        CheckConsistence();
        progressChanged( this, new ProgressChangedEventArgs( 1, "SummaryContentInfo created" ) );
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
    /// <summary>
    /// Contains all materials sorted using the following key: SKU,Batch,Location. <see cref="GetKey"/>
    /// </summary>
    internal class DisposalsAnalisis: SortedList<CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum, double>
    {
      public DisposalsAnalisis()
      {
        foreach ( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum _item in Enum.GetValues( typeof( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum ) ) )
          this.Add( _item, 0 );
      }
      public DisposalsAnalisis( Entities _edc, string batch, double _material, double _dustRatio, double _shMentholRatio, double _wasteRatio, double _overusage )
      {
        List<CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR> _accounts = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.FindIPRAccountsWithNotAllocatedTobacco( _edc, batch );
        bool _closing = false;
        if ( _accounts.Count == 1 && Math.Abs( _accounts[ 0 ].TobaccoNotAllocated.Value - _material ) < 1 )
        {
          _material = _accounts[ 0 ].TobaccoNotAllocated.Value;
          _closing = true;
        }
        double _am;
        if ( _overusage > 0 )
        {
          this.Add( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.OverusageInKg, ( _material * _overusage ).RountMass() );
          _am = _material - this[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.OverusageInKg ];
        }
        else
          _am = _material;
        this.Add( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Dust, ( _am * _dustRatio ).RountMass() );
        this.Add( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.SHMenthol, ( _am * _shMentholRatio ).RountMass() );
        this.Add( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Waste, ( _am * _wasteRatio ).RountMassUpper() );
        if ( _closing )
          this.Add( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Tobacco, _am - this[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.SHMenthol ] - this[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Waste ] - this[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Dust ] );
        else
          this.Add( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Tobacco, ( _am - this[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.SHMenthol ] - this[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Waste ] - this[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Dust ] ).RountMass() );
      }
      public void Accumutate( DisposalsAnalisis _dspsls )
      {
        foreach ( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum _item in Enum.GetValues( typeof( CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum ) ) )
          this[ _item ] += _dspsls.Keys.Contains( _item ) ? _dspsls[ _item ] : 0;
      }
    }

    #endregion
  }
}
