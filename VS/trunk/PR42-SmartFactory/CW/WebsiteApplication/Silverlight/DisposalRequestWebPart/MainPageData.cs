//<summary>
//  Title   : public class MainPageData
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

using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Data;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart
{
  /// <summary>
  /// public class MainPageData
  /// </summary>
  public sealed class MainPageData: INotifyPropertyChanged, IDisposable
  {

    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPageData"/> class.
    /// </summary>
    public MainPageData()
    {
      PagedCollectionView _npc = new PagedCollectionView( new DisposalRequestObservable() );
      _npc.PropertyChanged += RequestCollection_PropertyChanged;
      _npc.CollectionChanged += RequestCollection_CollectionChanged;
      RequestCollection = _npc;
      this.DisposalRequestObservable.ProgressChanged += DisposalRequestObservable_ProgressChanged;
      Log = "MainPageData created.";
    }
    #endregion

    #region public properties
    public string HeaderLabel
    {
      get { return b_HeaderLabel; }
      set
      {
        if ( b_HeaderLabel == value )
          return;
        b_HeaderLabel = value;
        OnPropertyChanged( "HeaderLabel" );
      }
    }
    /// <summary>
    /// Gets or sets the log.
    /// </summary>
    /// <value>
    /// The log.
    /// </value>
    public string Log
    {
      get { return b_Log; }
      set
      {
        if ( b_Log == value )
          return;
        b_Log = value;
        OnPropertyChanged( "Log" );
      }
    }
    public TextWriter LogList { get; set; }
    /// <summary>
    /// Gets or sets the request collection.
    /// </summary>
    /// <value>
    /// The request collection.
    /// </value>
    public PagedCollectionView RequestCollection
    {
      get { return b_RequestCollection; }
      set { b_RequestCollection = value; }
    }
    internal DataContextAsync DataContextAsync { get { return m_Context; } }
    #endregion

    #region INotifyPropertyChanged Members
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      if ( m_Context != null )
        m_Context.Dispose();
      m_Disposed = true;
    }
    #endregion

    #region internal
    internal void GetData( string url, int? selectedID )
    {
      CheckDisposed();
      m_URL = url;
      m_DisposalRequestLibId = selectedID;
      if ( !m_DisposalRequestLibId.HasValue )
        return;
      m_Context.CreateContextAsyncCompletedEvent += m_Context_CreateContextAsyncCompletedEvent;
      Log = String.Format( "GetDataAsync: CreateContextAsync for URL={0}.", m_URL );
      m_Context.CreateContextAsync( m_URL );
    }
    /// <summary>
    /// Creates the disposal request.
    /// </summary>
    /// <param name="list">The list of <see cref="CustomsWarehouse"/> with the same batch.</param>
    /// <param name="toDispose">The tobacco to dispose.</param>
    internal void CreateDisposalRequest(List<CustomsWarehouse> list, double toDispose, string customsProcedure)
    {
      CheckDisposed();
      this.DisposalRequestObservable.CreateDisposalRequest( list, toDispose, customsProcedure );
    }
    internal void SubmitChanges()
    {
      CheckDisposed();
      try
      {
        this.DisposalRequestObservable.RecalculateDisposals( m_DisposalRequestLibId.Value, m_Context );
        m_Context.SubmitChangesCompleted += m_Context_SubmitChangesCompleted;
        m_Context.SubmitChangesAsyn();
        m_Edited = false;
        UpdateHeader();
      }
      catch ( Exception _ex )
      {
        ExceptionHandling( _ex );
      }
    }
    #endregion

    #region private

    #region backing fields
    private PagedCollectionView b_RequestCollection = null;
    private string b_HeaderLabel = "N/A";
    private string b_Log = "Log before starting";
    #endregion

    #region vars
    private bool m_Disposed = false;
    private string m_URL = String.Empty;
    private int? m_DisposalRequestLibId = new Nullable<int>();
    private bool m_Edited = false;
    private DataContextAsync m_Context = new DataContextAsync();
    #endregion

    /// <summary>
    /// Called when property value changes.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    private void OnPropertyChanged( string propertyName )
    {
      if ( ( null == this.PropertyChanged ) )
        return;
      this.PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
    }
    private void m_Context_SubmitChangesCompleted( object sender, AsyncCompletedEventArgs e )
    {
      m_Context.SubmitChangesCompleted -= m_Context_SubmitChangesCompleted;
      Log = "SubmitChangesCompleted";
    }
    private void ExceptionHandling( Exception ex )
    {
      this.HeaderLabel = "Exception:" + ex.Message;
    }
    private void UpdateHeader()
    {
      try
      {
        int items = RequestCollection == null ? -1 : RequestCollection.TotalItemCount;
        string _pattern = "Disposal request {0} content: {1} items; {2}";
        string _star = m_Edited ? "*" : " ";
        string _rid = m_DisposalRequestLibId.HasValue ? m_DisposalRequestLibId.ToString() : "Not connected";
        this.HeaderLabel = String.Format( _pattern, _rid, items, _star );
      }
      catch ( Exception ex )
      {
        this.HeaderLabel = "UpdateHeader exception " + ex.Message;
      }
    }
    private void RequestCollection_CollectionChanged( object sender, EventArgs e )
    {
      UpdateHeader();
    }
    private void RequestCollection_PropertyChanged( object sender, PropertyChangedEventArgs e )
    {
      UpdateHeader();
    }
    private void DisposalRequestObservable_ProgressChanged( object sender, ProgressChangedEventArgs e )
    {
      Log = e.UserState as string;
    }
    private void m_Context_CreateContextAsyncCompletedEvent( object sender, AsyncCompletedEventArgs e )
    {
      m_Context.CreateContextAsyncCompletedEvent -= m_Context_CreateContextAsyncCompletedEvent;
      Log = String.Format( "GetData DoWork: new DataContext for url={0}.", m_URL );
      if ( e.Cancelled )
      {
        Log = "CreateContextAsync canceled";
        return;
      }
      if ( e.Error != null )
      {
        ExceptionHandling( e.Error );
        return;
      }
      Log = String.Format( ": new DataContext for url={0}.", m_URL );
      Debug.Assert( m_DisposalRequestLibId.HasValue, "m_SelectedID must have value" );
      m_Context.GetListCompleted += m_Context_GetDisposals4RequestCompleted;
      //Get all disposals for the request with id of m_DisposalRequestLibId
      m_Context.GetListAsync<CustomsWarehouseDisposal>
        ( CommonDefinition.CustomsWarehouseDisposalTitle, CommonDefinition.GetCAMLSelectedID( m_DisposalRequestLibId.Value, CommonDefinition.FieldCWDisposal2DisposalRequestLibraryID, CommonDefinition.CAMLTypeNumber ) );
    }
    private void m_Context_GetDisposals4RequestCompleted( object siurce, GetListAsyncCompletedEventArgs e )
    {
      m_Context.GetListCompleted -= m_Context_GetDisposals4RequestCompleted;
      if ( e.Cancelled )
      {
        Log = "GetList has been canceled";
        return;
      }
      if ( e.Error != null )
      {
        ExceptionHandling( e.Error );
        return;
      }
      Log = "GetListCompleted .GetDataContext  " + CommonDefinition.CustomsWarehouseDisposalTitle;
      List<CustomsWarehouseDisposal> _list = e.Result<CustomsWarehouseDisposal>();
      this.DisposalRequestObservable.GetDataContext( m_DisposalRequestLibId.Value, _list, m_Context );
      m_Edited = false;
      if ( this.RequestCollection.CanSort == true )
      {
        // By default, sort by Batch.
        this.RequestCollection.SortDescriptions.Add( new SortDescription( "Batch", ListSortDirection.Ascending ) );
      }
      UpdateHeader();
      Log = "GetData RunWorker Completed";
    }
    private DisposalRequestObservable DisposalRequestObservable { get { return (DisposalRequestObservable)this.RequestCollection.SourceCollection; } }
    private void CheckDisposed()
    {
      if ( m_Disposed )
        throw new ObjectDisposedException( typeof( MainPageData ).Name );
    }
    #endregion //private

  }
}
