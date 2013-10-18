//<summary>
//  Title   : class DisposalRequestObservable
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.SharePoint.Client;
using System.Linq;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  /// <summary>
  /// class DisposalRequestObservable
  /// </summary>
  public class DisposalRequestObservable: ObservableCollection<DisposalRequest>
  {
    internal void GetDataContext( List<CustomsWarehouseDisposal> _list, DataContextAsync context )
    {
      IEnumerable<IGrouping<string, CustomsWarehouseDisposal>> _requests = _list.GroupBy<CustomsWarehouseDisposal, string>( x => x.CWL_CWDisposal2CustomsWarehouseID.Batch );
      foreach ( IGrouping<string, CustomsWarehouseDisposal> _grx in _requests )
      {
        CraeteRequest _creator = new CraeteRequest( context, this, _grx );
        _creator.DoAsync();
      }
    }
    internal event ProgressChangedEventHandler ProgressChanged;
    internal void OnProgressChanged( ProgressChangedEventArgs args )
    {
      if ( ProgressChanged == null )
        return;
      ProgressChanged( this, args );
    }
    private class CraeteRequest
    {
      internal CraeteRequest( DataContextAsync context, DisposalRequestObservable parent, IGrouping<string, CustomsWarehouseDisposal> grouping )
      {
        m_DataContext = context;
        m_Grouping = grouping;
        m_Parent = parent;
      }
      internal void DoAsync()
      {
        m_DataContext.GetListCompleted += context_GetListCompleted;
        m_DataContext.GetListAsync<CustomsWarehouse>( CommonDefinition.CustomsWarehouseTitle,
                                                      CommonDefinition.GetCAMLSelectedID( m_Grouping.Key, CommonDefinition.FieldBatch, CommonDefinition.CAMLTypeText )
                                                     );
      }
      private DataContextAsync m_DataContext = null;
      private DisposalRequestObservable m_Parent;
      private IGrouping<string, CustomsWarehouseDisposal> m_Grouping = null;
      private void context_GetListCompleted( object siurce, GetListAsyncCompletedEventArgs e )
      {
        try
        {
          m_DataContext.GetListCompleted -= context_GetListCompleted;
          if ( e.Cancelled )
            return;
          if ( e.Error != null )
            m_Parent.OnProgressChanged( new ProgressChangedEventArgs( 0, String.Format( "Exception {0} at m_DataContext.GetListAsync", e.Error.Message ) ) );
          CustomsWarehouseDisposal _first = m_Grouping.First<CustomsWarehouseDisposal>();
          CustomsWarehouse _cw = _first.CWL_CWDisposal2CustomsWarehouseID != null ? _first.CWL_CWDisposal2CustomsWarehouseID : new CustomsWarehouse() { Units = "N/A", SKU = "N/A", CW_MassPerPackage = 0 };
          DisposalRequest _oc = DefaultDisposalRequestnew( m_Grouping.Key, _first.SKUDescription, _cw );
          _oc.GetDataContext( e.Result<CustomsWarehouse>() );
          foreach ( CustomsWarehouseDisposal _cwdrdx in m_Grouping )
            _oc.GetDataContext( _cwdrdx );
          _oc.Update();
          m_Parent.Add( _oc );
          _oc.AutoCalculation = true;
        }
        catch ( Exception _ex )
        {
          m_Parent.OnProgressChanged( new ProgressChangedEventArgs( 0, String.Format( "Exception {0} at context_GetListCompleted", _ex ) ) );
        }
      }
    }//class CraeteRequest
    internal void AddDisposal( List<CustomsWarehouse> list, double toDispose )
    {
      if ( list.Count == 0 )
        throw new AggregateException( "list must contain at least one element" );
      string _ntc = list.First<CustomsWarehouse>().Batch;
      DisposalRequest _firs = this.FirstOrDefault( ( x ) => { return x.Batch == _ntc; } );
      if ( _firs != null )
        _firs.AddedKg += toDispose;
      else
        ;
    }
    private static DisposalRequest DefaultDisposalRequestnew( string batch, string skuDescription, CustomsWarehouse cw )
    {
      return new DisposalRequest()
       {
         AddedKg = 0,
         DeclaredNetMass = 0,
         Batch = batch,
         MassPerPackage = cw.CW_MassPerPackage.Value,
         PackagesToClear = 0,
         QuantityyToClearSum = 0,
         QuantityyToClearSumRounded = 0,
         RemainingOnStock = 0,
         RemainingPackages = 0,
         SKUDescription = skuDescription,
         Title = "Title TBD",
         TotalStock = 0,
         Units = cw.Units,
         SKU = cw.SKU,
       };
    }
  }
}
