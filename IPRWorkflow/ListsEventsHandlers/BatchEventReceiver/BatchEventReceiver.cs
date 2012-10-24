using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint;
using BatchMaterialXml = CAS.SmartFactory.xml.erp.BatchMaterial;
using BatchXml = CAS.SmartFactory.xml.erp.Batch;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class BatchEventReceiver: SPItemEventReceiver
  {
    #region public
    /// <summary>
    /// An item was added.
    /// </summary>
    /// <param name="_properties">Contains properties for asynchronous list item event handlers.</param>
    public override void ItemAdded( SPItemEventProperties _properties )
    {
      base.ItemAdded( _properties );
      try
      {
        if ( !_properties.List.Title.Contains( "Batch Library" ) )
        {
          //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
          base.ItemAdded( _properties );
          return;
          //throw new IPRDataConsistencyException(m_Title, "Wrong library name", null, "Wrong library name");
        }
        this.EventFiringEnabled = false;
        using ( Entities _edc = new Entities( _properties.WebUrl ) )
        {
          BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>( _edc.BatchLibrary, _properties.ListItemId );
          At = "ImportBatchFromXml";
          ImportBatchFromXml
            (
              _edc,
              _properties.ListItem.File.OpenBinaryStream(),
              _entry,
              _properties.ListItem.File.ToString(),
              ( object obj, ProgressChangedEventArgs progres ) => { At = (string)progres.UserState; }
            );
          At = "ListItem assign";
          _entry.BatchLibraryOK = true;
          _entry.BatchLibraryComments = "Batch message import succeeded.";
          At = "SubmitChanges";
          _edc.SubmitChanges();
        }
      }
      catch ( IPRDataConsistencyException _ex )
      {
        _ex.Source += " at " + At;
        using ( Entities _edc = new Entities( _properties.WebUrl ) )
        {
          _ex.Add2Log( _edc );
          BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>( _edc.BatchLibrary, _properties.ListItemId );
          _entry.BatchLibraryOK = false;
          _entry.BatchLibraryComments = _ex.Comments;
          _edc.SubmitChanges();
        }
      }
      catch ( Exception _ex )
      {
        using ( Entities _edc = new Entities( _properties.WebUrl ) )
        {
          Anons.WriteEntry( _edc, "BatchEventReceiver.ItemAdded" + " at " + At, _ex.Message );
          BatchLib _entry = _entry = Element.GetAtIndex<BatchLib>( _edc.BatchLibrary, _properties.ListItemId );
          _entry.BatchLibraryComments = "Batch message import error";
          _entry.BatchLibraryOK = false;
          _edc.SubmitChanges();
        }
      }
      finally
      {
        this.EventFiringEnabled = true;
      }
    }
    /// <summary>
    /// Imports the batch from XML.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="url">The URL.</param>
    /// <param name="listIndex">Index of the list.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="progressChanged">The progress changed delegate <see cref="ProgressChangedEventHandler"/>.</param>
    public static void ImportBatchFromXml( Entities _edc, Stream stream, BatchLib _entry, string fileName, ProgressChangedEventHandler progressChanged )
    {
      try
      {
        progressChanged( null, new ProgressChangedEventArgs( 1, "Importing XML" ) );
        Anons.WriteEntry( _edc, m_Title, String.Format( m_Message, fileName ) );
        _edc.SubmitChanges();
        BatchXml _xml = BatchXml.ImportDocument( stream );
        progressChanged( null, new ProgressChangedEventArgs( 1, "Getting Data" ) );
        GetXmlContent( _xml, _edc, _entry, progressChanged );
        progressChanged( null, new ProgressChangedEventArgs( 1, "Submiting Changes" ) );
        Anons.WriteEntry( _edc, m_Title, "Import of the batch message finished" );
        _edc.SubmitChanges();
      }
      catch ( IPRDataConsistencyException _ex )
      {
        throw _ex;
      }
      catch ( Exception ex )
      {
        string _src = "BatchEventReceiver.ImportBatchFromXml";
        string _Comments = "Batch message import error";
        throw new IPRDataConsistencyException( _src, ex.Message, ex, _Comments );
      }
    }
    #endregion

    #region private
    /// <summary>
    /// Gets the content of the XML.
    /// </summary>
    /// <param name="xml">The document.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="parent">The entry.</param>
    private static void GetXmlContent( BatchXml xml, Entities edc, BatchLib parent, ProgressChangedEventHandler progressChanged )
    {
      SummaryContentInfo xmlBatchContent = new SummaryContentInfo( xml.Material, edc, progressChanged );
      Batch batch =
          ( from idx in edc.Batch where idx.Batch0.Contains( xmlBatchContent.Product.Batch ) && idx.BatchStatus.Value == BatchStatus.Preliminary select idx ).FirstOrDefault();
      if ( batch == null )
      {
        batch = new Batch();
        edc.Batch.InsertOnSubmit( batch );
      }
      progressChanged( null, new ProgressChangedEventArgs( 1, "GetXmlContent: BatchProcessing" ) );
      BatchProcessing( batch, edc, GetBatchStatus( xml.Status ), xmlBatchContent, parent, progressChanged );
    }

    private static void BatchProcessing( Batch _this, Entities edc, BatchStatus status, SummaryContentInfo content, BatchLib parent, ProgressChangedEventHandler progressChanged )
    {
      _this.BatchLibraryIndex = parent;
      _this.BatchStatus = status;
      _this.Batch0 = content.Product.Batch;
      _this.SKU = content.Product.SKU;
      _this.Title = String.Format( "{0} SKU: {1}; Batch: {2}", content.Product.ProductType, _this.SKU, _this.Batch0 );
      _this.FGQuantity = content.Product.FGQuantity;
      _this.MaterialQuantity = content.TotalTobacco;
      _this.ProductType = content.Product.ProductType;
      progressChanged( _this, new ProgressChangedEventArgs( 1, "BatchProcessing: interconnect" ) );
      //interconnect 
      _this.SKUIndex = SKUCommonPart.GetLookup( edc, content.Product.SKU );
      _this.CutfillerCoefficientIndex = CutfillerCoefficient.GetLookup( edc );
      _this.UsageIndex = Usage.GetLookup( _this.SKUIndex.FormatIndex, edc );
      progressChanged( _this, new ProgressChangedEventArgs( 1, "BatchProcessing: Coefficients" ) );
      //Coefficients
      _this.DustIndex = Dust.GetLookup( _this.ProductType.Value, edc );
      _this.BatchDustCooeficiency = _this.DustIndex.DustRatio;
      _this.DustCooeficiencyVersion = _this.DustIndex.Wersja;
      _this.SHMentholIndex = SHMenthol.GetLookup( _this.ProductType.Value, edc );
      _this.BatchSHCooeficiency = _this.SHMentholIndex.SHMentholRatio;
      _this.SHCooeficiencyVersion = _this.SHMentholIndex.Wersja;
      _this.WasteIndex = Waste.GetLookup( _this.ProductType.Value, edc );
      _this.BatchWasteCooeficiency = _this.WasteIndex.WasteRatio;
      _this.WasteCooeficiencyVersion = _this.WasteIndex.Wersja;
      progressChanged( _this, new ProgressChangedEventArgs( 1, "BatchProcessing: processing" ) );
      //processing
      _this.CalculatedOveruse = GetOverusage( _this.MaterialQuantity.Value, _this.FGQuantity.Value, _this.UsageIndex.UsageMax.Value, _this.UsageIndex.UsageMin.Value );
      _this.FGQuantityAvailable = _this.FGQuantity;
      _this.FGQuantityBlocked = 0;
      _this.FGQuantityPrevious = 0; //TODO [pr4-3421] Intermediate batches processing http://itrserver/Bugs/BugDetail.aspx?bid=3421
      _this.MaterialQuantityPrevious = 0;
      double _shmcf = 0;
      if ( ( _this.SKUIndex is SKUCigarette ) && ( (SKUCigarette)_this.SKUIndex ).MentholMaterial.Value )
        _shmcf = ( (SKUCigarette)_this.SKUIndex ).MentholMaterial.Value ? _this.SHMentholIndex.SHMentholRatio.Value : 0;
      progressChanged( _this, new ProgressChangedEventArgs( 1, "BatchProcessing: ProcessDisposals" ) );
      content.ProcessDisposals( edc, _this, _this.DustIndex.DustRatio.Value, _shmcf, _this.WasteIndex.WasteRatio.Value, _this.CalculatedOveruse.GetValueOrDefault( 0 ), progressChanged );
      _this.Dust = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Dust ];
      _this.SHMenthol = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.SHMenthol ];
      _this.Waste = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Waste ];
      _this.Tobacco = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.Tobacco ];
      _this.Overuse = content.AccumulatedDisposalsAnalisis[ CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum.OverusageInKg ];
      foreach ( var _invoice in _this.InvoiceContent )
      {
        _invoice.CreateTitle();
        if ( _this.Available( _invoice.Quantity.Value ) )
          _invoice.InvoiceContentStatus = InvoiceContentStatus.OK;
        else
          _invoice.InvoiceContentStatus = InvoiceContentStatus.NotEnoughQnt;
      }
    }
    private static BatchStatus GetBatchStatus( xml.erp.BatchStatus batchStatus )
    {
      switch ( batchStatus )
      {
        case CAS.SmartFactory.xml.erp.BatchStatus.Final:
          return BatchStatus.Final;
        case CAS.SmartFactory.xml.erp.BatchStatus.Intermediate:
          return BatchStatus.Intermediate;
        case CAS.SmartFactory.xml.erp.BatchStatus.Progress:
          return BatchStatus.Progress;
        default:
          return BatchStatus.Preliminary;
      }
    }
    /// <summary>
    /// Gets the overuse as the ratio of overused tobacco divided by totaly usage of tobacco.
    /// </summary>
    /// <param name="_materialQuantity">The _material quantity.</param>
    /// <param name="_fGQuantity">The finished goods quantity.</param>
    /// <param name="_usageMax">The cutfiller usage max.</param>
    /// <param name="_usageMin">The cutfiller usage min.</param>
    /// <returns></returns>
    private static double GetOverusage( double _materialQuantity, double _fGQuantity, double _usageMax, double _usageMin )
    {
      double _ret = ( _materialQuantity - _fGQuantity * _usageMax / 1000 );
      if ( _ret > 0 )
        return _ret / _materialQuantity; // Overusage
      _ret = ( _materialQuantity - _fGQuantity * _usageMin / 1000 );
      if ( _ret < 0 )
        return _ret / _materialQuantity; //Underusage
      return 0;
    }
    private const string m_Source = "Batch processing";
    private const string m_LookupFailedMessage = "I cannot recognize batch {0}.";
    private class SummaryContentInfo: SortedList<string, Material>
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
    private class DisposalsAnalisis: SortedList<CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR.DisposalEnum, double>
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
    private static Material Material( BatchMaterialXml item )
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
    #endregion


    #region private
    private const string _batchLibraryOK = "BatchLibraryOK";
    private const string _batchLibraryComments = "BatchLibraryComments";
    private string At { get; set; }
    private const string m_Title = "Batch Message Import";
    private const string m_Message = "Import of the batch message {0} starting.";
    #endregion
  }
}
