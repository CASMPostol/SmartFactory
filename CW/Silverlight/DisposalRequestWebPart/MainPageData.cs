//<summary>
//  Title   : public class MainPageData
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
using System.Windows;
using System.Windows.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq;
using Microsoft.SharePoint.Client;
using System.Linq;

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
      m_Singleton = this;
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
      if ( m_DataContext != null )
        m_DataContext.Dispose();
    }
    #endregion

    #region internal
    internal static void GetData( string url )
    {
      m_Singleton.RunWorkerAsync( url );
    }
    internal static void SubmitChanges()
    {
      m_Singleton.SubmitChangesLoc();
    }
    #endregion

    #region private

    #region backing fields
    private PagedCollectionView b_RequestCollection = null;
    private string b_HeaderLabel = "N/A";
    private string b_Log = "Log before starting";
    #endregion

    private static MainPageData m_Singleton = null;
    private bool m_Edited = false;
    private Entities m_DataContext;
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
        m_DataContext.SubmitChanges();
        m_Edited = false;
        UpdateHeader();
      }
      catch ( Exception _ex )
      {
        ExceptionHandling( _ex );
      }
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
        string _pattern = "Disposal request content: {0} items; {1}";
        string _star = m_Edited ? "*" : " ";
        this.HeaderLabel = String.Format( _pattern, items, _star );
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
    private void RunWorkerAsync( string url )
    {
      m_Worker.DoWork += Worker_DoWork;
      m_Worker.ProgressChanged += Worker_ProgressChanged;
      m_Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
      m_Worker.WorkerReportsProgress = true;
      m_Worker.WorkerSupportsCancellation = false;
      m_Singleton.Log = "GetData RunWorkerAsync";
      m_Worker.RunWorkerAsync( url );
    }
    private void Worker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
    {
      if ( e.Error != null )
      {
        ExceptionHandling( e.Error );
        return;
      }
      if ( e.Cancelled )
      {
        Log = "GetData has been canceled";
        return;
      }
      List<CustomsWarehouseDisposal> _list = (List<CustomsWarehouseDisposal>)e.Result;
      Log = "GetData DisposalRequestObservable.GetDataContext  " + CommonDefinition.CustomsWarehouseDisposalTitle;
      ( (DisposalRequestObservable)this.RequestCollection.SourceCollection ).GetDataContext( _list );
      m_Edited = false;
      //if (_npc.CanGroup == true)
      //{
      //  // Group by 
      //  _npc.GroupDescriptions.Add(new PropertyGroupDescription("Batch"));
      //}
      if ( this.RequestCollection.CanSort == true )
      {
        // By default, sort by Batch.
        this.RequestCollection.SortDescriptions.Add( new SortDescription( "Batch", ListSortDirection.Ascending ) );
      }
      UpdateHeader();
      Log = "GetData RunWorker Completed";
    }
    private void Worker_ProgressChanged( object sender, ProgressChangedEventArgs e )
    {
      Log = (String)e.UserState;
    }
    private void Worker_DoWork( object sender, DoWorkEventArgs e )
    {
      BackgroundWorker _mq = (BackgroundWorker)sender;
      _mq.ReportProgress( 1, String.Format( "GetData DoWork: new DataContext for url={0}.", e.Argument ) );
      m_DataContext = new Entities( (string)e.Argument );
      _mq.ReportProgress( 1, "GetData DoWork: GetList " + CommonDefinition.CustomsWarehouseDisposalTitle );
      e.Result = m_DataContext.CustomsWarehouseDisposal.Filter( CamlQuery.CreateAllItemsQuery() ).ToList();
    }
    #endregion

    #endregion

  }
}
