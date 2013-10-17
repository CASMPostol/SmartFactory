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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq;
using Microsoft.SharePoint.Client;

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
      if ( m_Disposed )
        throw new ObjectDisposedException( typeof( MainPageData ).Name );
      m_URL = url;
      m_SelectedID = selectedID;
      if ( !m_SelectedID.HasValue )
        return;
      m_Context.CreateContextAsyncCompletedEvent += m_Context_CreateContextAsyncCompletedEvent;
      Log = String.Format( "GetDataAsync: CreateContextAsync for url={0}.", m_URL );
      m_Context.CreateContextAsync( m_URL );
    }
    internal void AddDisposal( List<CustomsWarehouse> list, double toDispose )
    {
      if ( m_Disposed )
        throw new ObjectDisposedException( typeof( MainPageData ).Name );
      ( (DisposalRequestObservable)this.RequestCollection.SourceCollection ).AddDisposal( list, toDispose );
    }
    internal void SubmitChanges()
    {
      if ( m_Disposed )
        throw new ObjectDisposedException( typeof( MainPageData ).Name );
      SubmitChangesLoc();
    }
    #endregion

    #region private

    #region backing fields
    private PagedCollectionView b_RequestCollection = null;
    private string b_HeaderLabel = "N/A";
    private string b_Log = "Log before starting";
    #endregion

    private bool m_Disposed = false;
    private string m_URL = String.Empty;
    private int? m_SelectedID = new Nullable<int>();
    private bool m_Edited = false;
    /// <summary>
    /// Called whena property value changes.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    private void OnPropertyChanged( string propertyName )
    {
      if ( ( null == this.PropertyChanged ) )
        return;
      this.PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) );
    }
    private void SubmitChangesLoc()
    {
      try
      {
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
        string _rid = m_SelectedID.HasValue ? m_SelectedID.ToString() : "Not connected";
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

    #region Worker
    private BackgroundWorker m_Worker = new BackgroundWorker();
    private DataContextAsync m_Context = new DataContextAsync();
    private void m_Context_CreateContextAsyncCompletedEvent( object sender, AsyncCompletedEventArgs e )
    {
      m_Context.CreateContextAsyncCompletedEvent -= m_Context_CreateContextAsyncCompletedEvent;
      Log = String.Format( "GetData DoWork: new DataContext for url={0}.", m_URL );
      if ( e.Cancelled )
      {
        Log = "CreateContextAsync Cancelled";
        return;
      }
      if ( e.Error != null )
      {
        ExceptionHandling( e.Error );
        return;
      }
      Log = String.Format( ": new DataContext for url={0}.", m_URL );
      Debug.Assert( m_SelectedID.HasValue, "m_SelectedID must have value" );
      m_Context.GetListCompleted += m_Context_GetListCompleted;
      m_Context.GetListAsync<CustomsWarehouseDisposal>( CommonDefinition.CustomsWarehouseDisposalTitle, CommonDefinition.GetCAMLSelectedID( m_SelectedID.Value, CommonDefinition.FieldCWDisposal2DisposalRequestLibraryID, CommonDefinition.CAMLTypeNumber ) );
    }
    private void m_Context_GetListCompleted( object siurce, GetListAsyncCompletedEventArgs e )
    {
      m_Context.GetListCompleted -= m_Context_GetListCompleted;
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
      List<CustomsWarehouseDisposal> _list = e.Result<CustomsWarehouseDisposal>().ToList<CustomsWarehouseDisposal>();
      ( (DisposalRequestObservable)this.RequestCollection.SourceCollection ).GetDataContext( _list );
      m_Edited = false;
      if ( this.RequestCollection.CanSort == true )
      {
        // By default, sort by Batch.
        this.RequestCollection.SortDescriptions.Add( new SortDescription( "Batch", ListSortDirection.Ascending ) );
      }
      UpdateHeader();
      Log = "GetData RunWorker Completed";
    }
    #endregion

    #endregion


  }
}
