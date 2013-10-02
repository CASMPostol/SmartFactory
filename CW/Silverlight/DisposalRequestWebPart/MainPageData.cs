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
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.Entities;
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
      PagedCollectionView _npc = new PagedCollectionView( new Data.Entities.DisposalRequestObservable() );
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
      m_Singleton.Log = "Starting QueueUserWorkItem to get remore data;";
      System.Threading.ThreadPool.QueueUserWorkItem( m_Singleton.GetData, url );
      m_Singleton.Log = "Returned from QueueUserWorkItem";
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
    private DataContext m_DataContext;
    /// <summary>
    /// Called whena property value changes.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    private void OnPropertyChanged( string propertyName )
    {
      if ( ( null == this.PropertyChanged ) )
        return;
      Deployment.Current.Dispatcher.BeginInvoke( () => this.PropertyChanged( this, new PropertyChangedEventArgs( propertyName ) ) );
    }
    private void GetData( object url )
    {
      try
      {
        Log = "GetData starting";
        UpdateHeader();
        Log = String.Format( "GetData new DataContext for url={0}.", url );
        m_DataContext = new DataContext( (string)url );
        Log = "GetData GetList " + CommonDefinition.CustomsWarehouseDisposalTitle;
        EntityList<Data.Entities.CustomsWarehouseDisposalRowData> _list = m_DataContext.GetList<Data.Entities.CustomsWarehouseDisposalRowData>( CommonDefinition.CustomsWarehouseDisposalTitle, CamlQuery.CreateAllItemsQuery() );
        Log = "GetData DisposalRequestObservable.GetDataContext  " + CommonDefinition.CustomsWarehouseDisposalTitle;
        ( (DisposalRequestObservable)this.RequestCollection.SourceCollection ).GetDataContext( _list );
        m_Edited = false;
        Log = "GetData UpdateHeader";
        UpdateHeader();
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
      }
      catch ( Exception ex )
      {
        ExceptionHandling( ex );
      }
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
      m_Edited = true;
      UpdateHeader();
    }
    private void RequestCollection_PropertyChanged( object sender, PropertyChangedEventArgs e )
    {
      m_Edited = true;
      UpdateHeader();
    }
    #endregion

  }
}
