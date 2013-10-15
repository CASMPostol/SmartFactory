//<summary>
//  Title   : public class DataContext
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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  ///  Provides client site LINQ (Language Integrated Query) access to, and change tracking for, the lists and document libraries of a Windows SharePoint Services "14" Web site.
  /// </summary>
  public class DataContext: IDisposable
  {

    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="DataContext" /> class.
    /// </summary>
    /// <param name="requestUrl">The URL of a Windows SharePoint Services "14" Web site that provides client site access and change tracking for the specified Web site..</param>
    public DataContext()
    {
#if DEBUG
      Log = new StringWriter();
#endif
      InitializeDelegates();
      this.ObjectTrackingEnabled = true;
      this.DeferredLoadingEnabled = true;
      // Open the current ClientContext
    }
    public DataContext( string requestUrl )
      : this()
    {
      CreateContext( requestUrl );
    }
    internal virtual void CreateContext( string requestUrl )
    {
      m_ClientContext = new ClientContext( requestUrl );
      m_site = m_ClientContext.Site;
      m_ClientContext.Load<Site>( m_site, s => s.Url );
      m_RootWeb = m_site.RootWeb;
      m_ClientContext.Load<Web>( m_RootWeb, w => w.Title );
      ExecuteQuery( this, new ProgressChangedEventArgs( 1, String.Format( "Loading Site={0}", requestUrl ) ) );
      Trace( new ProgressChangedEventArgs( 1, String.Format( "Loaded site={0} Web={1}", m_site.Url, m_RootWeb.Title ) ) );
    }
    /// <summary>
    /// Gets a collection of objects that represent discrepancies between the current client value and the current database value of a field in a list item.
    /// </summary>
    /// <value>
    ///  A <see cref="ChangeConflictCollection"/> each of whose members represents a discrepancy.
    /// </value>
    public ChangeConflictCollection ChangeConflicts { get; internal set; }
    /// <summary>
    /// Gets or sets a value indicating whether the LINQ to SharePoint provider should allow delay loading of <see cref="EntityRef"/> and <see cref="EntitySet"/> objects.
    /// </summary>
    /// <value>
    /// true if deferred loading enabled, otherwise, false.
    /// </value>
    public bool DeferredLoadingEnabled { get; set; }
    /// <summary>
    /// Gets or sets an object that will write the Collaborative Application Markup Language (CAML) query that results from the translation of the LINQ query.
    /// </summary>
    /// <value>
    /// A System.IO.TextWriter that can be called to write the CAML query.
    /// </value>
    public TextWriter Log { get; set; }
    /// <summary>
    /// Gets or sets a value that indicates whether changes in objects are tracked.
    /// </summary>
    /// <value>
    /// true, if changes are tracked; false otherwise.
    /// </value>
    public bool ObjectTrackingEnabled { get; set; }
    /// <summary>
    /// Gets the full URL of the Web site whose data is represented by the Microsoft.SharePoint.Linq.DataContext object.
    /// </summary>
    /// <value>
    /// A System.String that represents the full URL of the Web site.
    /// </value>
    public string Web { get { return m_ClientContext.Url; } }
    public event ProgressChangedEventHandler GetListAsyncProgressChange;
    public event GetListAsyncCompletedEventHandler GetListCompleted;
    public delegate void GetListAsyncCompletedEventHandler( object siurce, GetListAsyncCompletedEventArgs e );
    /// <summary>
    /// Gets the list.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="listName">Name of the list.</param>
    public void GetListAsync<TEntity>( string listName )
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      //IEntityListItemsCollection _nwLst = GetOrCreateListEntry<TEntity>( listName );
      //return new EntityList<TEntity>( this, (EntityListItemsCollection<TEntity>)_nwLst );
    }
    /// <summary>
    /// Gets the list.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="listName">Name of the list.</param>
    /// <param name="userState">State of the user object.</param>
    public void GetListAsync<TEntity>( string listName, object userState )
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      // Create an AsyncOperation for taskId.
      AsyncOperation asyncOp = AsyncOperationManager.CreateOperation( userState );
      // Multiple threads will access the task dictionary, 
      // so it must be locked to serialize access. 
      lock ( m_UserStateToLifetime )
      {
        if ( m_UserStateToLifetime.ContainsKey( userState ) )
        {
          throw new ArgumentException( "Task ID parameter must be unique", "taskId" );
        }
        m_UserStateToLifetime[ userState ] = asyncOp;
      }
      new Thread( () => { GetList( listName, asyncOp ); } ).Start();

      // Start the asynchronous operation.
      WorkerEventHandler workerDelegate = new WorkerEventHandler( GetList );
      workerDelegate.BeginInvoke( listName, asyncOp, null, null );
    }
    /// <summary>
    /// Gets the list.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="listName">Name of the list.</param>
    /// <returns></returns>
    public virtual EntityList<TEntity> GetList<TEntity>( string listName )
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      IEntityListItemsCollection _nwLst = GetOrCreateListEntry<TEntity>( listName );
      return new EntityList<TEntity>( this, (EntityListItemsCollection<TEntity>)_nwLst );
    }
    /// <summary>
    /// Cancels the asynchronous.
    /// </summary>
    /// <param name="userState">State of the user.</param>
    public void CancelAsync( object userState )
    {
    }
    /// <summary>
    /// Refreshes a collection of entities with the latest data from the content database according to the specified mode.
    /// </summary>
    /// <param name="mode">A value that specifies how to resolve differences between the current client values and the database values.</param>
    /// <param name="entities"> The entities that are refreshed.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Refresh( RefreshMode mode, IEnumerable entities ) { throw new NotImplementedException(); }
    /// <summary>
    /// Refreshes the specified entity with the latest data from the content database according to the specified mode.
    /// </summary>
    /// <param name="mode">A value that specifies how to resolve differences between the current client values and the database values.</param>
    /// <param name="entity">The object that is refreshed.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Refresh( RefreshMode mode, object entity ) { throw new NotImplementedException(); }
    /// <summary>
    /// Refreshes an array of entities with the latest data from the content database according to the specified mode.
    /// </summary>
    /// <param name="mode">A value that specifies how to resolve differences between the current client values and the database values.</param>
    /// <param name="entities">The entities that are refreshed.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Refresh( RefreshMode mode, params object[] entities ) { throw new NotImplementedException(); }
    /// <summary>
    /// Enables continued reading and writing to an <see cref="EntityList"/>EntityList even after it has been renamed.
    /// </summary>
    /// <typeparam name="T">The type of the list items.</typeparam>
    /// <param name="newListName">The new name of the list.</param>
    /// <param name="oldListName">The old name of the list.</param>
    public void RegisterList<T>( string newListName, string oldListName ) { throw new NotImplementedException(); }
    /// <summary>
    /// Enables continued reading and writing to an Microsoft.SharePoint.Linq.EntityList even after it has been moved to another Web site.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newListName">The new name of the list.</param>
    /// <param name="newWebUrl">The URL of the Web site to which the list was moved.</param>
    /// <param name="oldListName">The old name of the list.</param>
    public void RegisterList<T>( string newListName, string newWebUrl, string oldListName ) { throw new NotImplementedException(); }
    /// <summary>
    /// Persists to the content database changes made by the current user to one
    /// or more lists using the specified failure mode; or, if a concurrency conflict
    /// is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    /// property.
    /// </summary>
    public void SubmitChanges()
    {
      foreach ( IEntityListItemsCollection _elx in m_AllLists.Values )
        _elx.SubmitingChanges( this.ExecuteQuery );
    }
    /// <summary>
    /// Persists to the content database changes made by the current user to one
    /// or more lists using the specified failure mode; or, if a concurrency conflict
    /// is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    /// property.
    /// </summary>
    /// <param name="failureMode">A value that specifies when a concurrency conflict should stop the attempt to persist changes and roll back any changes already made.</param>
    /// <exception cref="System.NotImplementedException">
    /// Microsoft.SharePoint.Linq.DataContext.ObjectTrackingEnabled is false- or
    ///-At least one conflict in Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    ///     from the last time Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    ///     was called is not yet resolved.
    ///     </exception>
    public void SubmitChanges( ConflictMode failureMode ) { throw new NotImplementedException(); }
    /// <summary>
    /// Persists, to the content database, changes made by the current user to one
    /// or more lists using the specified failure mode and the specified indication
    /// of whether the versions of changed list items should be incremented; or,
    /// if a concurrency conflict is found, populates the Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    /// property.
    /// </summary>
    /// <param name="failureMode">A value that specifies when a concurrency conflict should stop the attempt
    /// to persist changes and roll back any changes already made.</param>
    /// <param name="systemUpdate">if set to true [system update].</param>
    /// <exception cref="System.NotImplementedException">Microsoft.SharePoint.Linq.DataContext.ObjectTrackingEnabled is false- or
    ///     -At least one conflict in Microsoft.SharePoint.Linq.DataContext.ChangeConflicts
    ///     from the last time Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    ///     was called is not yet resolved.</exception>
    public void SubmitChanges( ConflictMode failureMode, bool systemUpdate ) { throw new NotImplementedException(); }
    #endregion

    #region internal
    internal void SubmitChanges( string listName )
    {
      if ( m_AllLists.ContainsKey( listName ) )
        m_AllLists[ listName ].SubmitingChanges( ExecuteQuery );
    }
    internal FieldLookupValue GetFieldLookupValue( string listName, Object entity )
    {
      return m_AllLists[ listName ].GetFieldLookupValue( entity );
    }
    internal Object GetFieldLookupValue<TEntity>( string listName, FieldLookupValue fieldLookupValue )
      where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      IEntityListItemsCollection _newList = GetOrCreateListEntry<TEntity>( listName );
      return _newList.GetFieldLookupValue( fieldLookupValue );
    }
    internal ListItemCollection GetListItemCollection( List list, CamlQuery query )
    {
      ListItemCollection _ListItemCollection = list.GetItems( query );
      m_ClientContext.Load( _ListItemCollection );
      // Execute the prepared command against the target ClientContext
      ExecuteQuery( this, new ProgressChangedEventArgs( 1, String.Format( "Loading ListItemCollection for list {0}.", list.Title ) ) );
      Trace( new ProgressChangedEventArgs( 1, String.Format( "Loaded Items = {0}.", _ListItemCollection.Count ) ) );
      return _ListItemCollection;
    }
    internal void LoadListItem<TEntity>( int fieldLookupValue, EntityListItemsCollection<TEntity> entityListItemsCollection )
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      ListItemCollection m_ListItemCollection = GetListItemCollection( entityListItemsCollection.MyList, CommonDefinition.GetCAMLSelectedID( fieldLookupValue, CommonDefinition.FieldID, CommonDefinition.CAMLTypeNumber ) );
      foreach ( ListItem _listItemx in m_ListItemCollection )
        entityListItemsCollection.Add( _listItemx );
    }
    #endregion

    #region IDisposing
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose( true );
      // This object will be cleaned up by the Dispose method. 
      // Therefore, you should call GC.SupressFinalize to 
      // take this object off the finalization queue 
      // and prevent finalization code for this object 
      // from executing a second time.
      GC.SuppressFinalize( this );
    }

    //
    /// <summary>
    /// Releases unmanaged and - optionally - managed resources.  Dispose(bool disposing) executes in two distinct scenarios. 
    /// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and unmanaged resources
    /// can be disposed. If disposing equals false, the method has been called by the runtime from inside the finalizer 
    /// and you should not reference other objects. Only unmanaged resources can be disposed. 
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose( bool disposing )
    {
      // Check to see if Dispose has already been called. 
      if ( !this.disposed )
      {
        // If disposing equals true, dispose all managed 
        // and unmanaged resources. 
        if ( disposing )
        {
          // Dispose managed resources.
          m_ClientContext.Dispose();
        }
        // Note disposing has been done.
        disposed = true;
      }
    }
    // Use C# destructor syntax for finalization code. 
    // This destructor will run only if the Dispose method 
    // does not get called. 
    // It gives your base class the opportunity to finalize. 
    // Do not provide destructors in types derived from this class.
    ~DataContext()
    {
      // Do not re-create Dispose clean-up code here. 
      // Calling Dispose(false) is optimal in terms of 
      // readability and maintainability.
      Dispose( false );
    }
    // Track whether Dispose has been called. 
    private bool disposed = false;
    #endregion

    #region private
    private ClientContext m_ClientContext = default( ClientContext );
    private Site m_site { get; set; }
    private Web m_RootWeb { get; set; }
    private Dictionary<string, IEntityListItemsCollection> m_AllLists = new Dictionary<string, IEntityListItemsCollection>();
    private void ExecuteQuery( object sender, ProgressChangedEventArgs args )
    {
      m_ClientContext.ExecuteQuery();
      Trace( args );
    }
    private void Trace( ProgressChangedEventArgs e )
    {
      if ( Log != null )
        Log.WriteLine( String.Format( "{0};{1}", DateTime.Now, e.UserState ) );
    }
    private IEntityListItemsCollection GetOrCreateListEntry<TEntity>( string listName )
      where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      IEntityListItemsCollection _nwLst = null;
      if ( !m_AllLists.TryGetValue( listName, out _nwLst ) )
      {
        List _list = m_RootWeb.Lists.GetByTitle( listName );
        m_ClientContext.Load( _list );
        // Execute the prepared commands against the target ClientContext
        ExecuteQuery( this, new ProgressChangedEventArgs( 1, String.Format( "Loading list = {0}", listName ) ) );
        _nwLst = new EntityListItemsCollection<TEntity>( this, _list );
        m_AllLists.Add( listName, _nwLst );
      }
      else
        _nwLst = m_AllLists[ listName ];
      return _nwLst;
    }

    #region Event-based Asynchronous Pattern

    private SendOrPostCallback onProgressReportDelegate;
    private SendOrPostCallback onCompletedDelegate;
    private delegate void WorkerEventHandler( string list2Read, AsyncOperation asyncOp );
    private Dictionary<object, object> m_UserStateToLifetime = new Dictionary<object, object>();
    // This method is invoked via the AsyncOperation object, 
    // so it is guaranteed to be executed on the correct thread. 
    private void CalculateCompleted( object operationState )
    {
      GetListAsyncCompletedEventArgs e = operationState as GetListAsyncCompletedEventArgs;
      GetListAsyncCompleted( e );
    }
    // This method is invoked via the AsyncOperation object, 
    // so it is guaranteed to be executed on the correct thread. 
    private void ReportProgress( object state )
    {
      ProgressChangedEventArgs e = state as ProgressChangedEventArgs;
      OnProgressChanged( e );
    }
    protected void GetListAsyncCompleted( GetListAsyncCompletedEventArgs e )
    {
      if ( GetListCompleted != null )
        GetListCompleted( this, e );
    }
    protected void OnProgressChanged( ProgressChangedEventArgs e )
    {
      if ( GetListAsyncProgressChange != null )
        GetListAsyncProgressChange( this, e );
    }
    protected virtual void InitializeDelegates()
    {
      onProgressReportDelegate += ReportProgress;
      onCompletedDelegate += CalculateCompleted;
    }
    /// <summary>
    /// This is the method that the underlying, free-threaded asynchronous behavior will invoke.
    /// This will happen on an arbitrary thread.
    /// </summary>
    /// <param name="result">The result.</param>
    /// <param name="exception">The exception.</param>
    /// <param name="canceled">if set to <c>true</c> [canceled].</param>
    /// <param name="asyncOp">The asynchronous property.</param>
    private void CompletionMethod( object result, Exception exception, bool canceled, AsyncOperation asyncOp )
    {
      // If the task was not previously canceled, remove the task from the lifetime collection. 
      if ( !canceled )
      {
        lock ( m_UserStateToLifetime )
        {
          m_UserStateToLifetime.Remove( asyncOp.UserSuppliedState );
        }
      }
      // Package the results of the operation in a  GetListAsyncCompletedEventArgs.
      GetListAsyncCompletedEventArgs e = new GetListAsyncCompletedEventArgs( result, exception, canceled, asyncOp.UserSuppliedState );
      // End the task. The asyncOp object is responsible for marshaling the call.
      asyncOp.PostOperationCompleted( onCompletedDelegate, e );
      // Note that after the call to OperationCompleted, asyncOp is no longer usable, and any attempt to use it 
      // will cause an exception to be thrown.
    }
    // Utility method for determining if a task has been canceled. 
    private bool TaskCanceled( object taskId )
    {
      return ( m_UserStateToLifetime[ taskId ] == null );
    }
    // This method performs the actual prime number computation. It is executed on the worker thread. 
    private void GetList( string listToRead, AsyncOperation asyncOp )
    {
      Exception e = null;
      object _result = null;
      // Check that the task is still active. The operation may have been canceled before the thread was scheduled. 
      if ( !TaskCanceled( asyncOp.UserSuppliedState ) )
      {
        try
        {
        }
        catch ( Exception ex )
        {
          e = ex;
        }
      }
      this.CompletionMethod( _result, e, TaskCanceled( asyncOp.UserSuppliedState ), asyncOp );
      //completionMethodDelegate(calcState);
    }
    // This method starts an asynchronous calculation.  
    // First, it checks the supplied task ID for uniqueness. 
    // If taskId is unique, it creates a new WorkerEventHandler  
    // and calls its BeginInvoke method to start the calculation. 
    public virtual void CalculatePrimeAsync( string listName, object taskId )
    {
    }
    #endregion

    #endregion

  }
}
