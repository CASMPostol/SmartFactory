//<summary>
//  Title   : class DataContextAsync
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
using System.ComponentModel;
using System.Threading;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// DataContextAsync
  /// </summary>
  /// <typeparam name="TContext">The type of the context.</typeparam>
  public class DataContextAsync
  {
    #region public
    public bool Busy { get { return m_busy; } }

    #region CreateContextAsync
    public event CreateContextAsyncCompletedEventHandler CreateContextAsyncCompletedEvent;
    public delegate void CreateContextAsyncCompletedEventHandler( object sender, CreateContextAsyncCompletedEventArgs e );
    public void CreateContextAsync( string requestUrl )
    {
      if ( Busy )
        throw new InvalidOperationException( "Context is busy" );
      m_busy = true;
      m_OnCompletedDelegate += CreateContextAsynCompleted;
      AsyncOperation m_AsyncOp = AsyncOperationManager.CreateOperation( m_Counter++ );
      m_processor.Do( () => { CreateContextAsyncWorker( requestUrl, m_AsyncOp ); } );
    }
    #endregion

    #region GetListAsync
    public event GetListAsyncCompletedEventHandler GetListCompleted;
    public delegate void GetListAsyncCompletedEventHandler( object siurce, GetListAsyncCompletedEventArgs e );
    /// <summary>
    /// Gets the list.
    /// </summary>
    /// <param name="listName">Name of the list.</param>
    public void GetListAsync<TEntity>( string listName )
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      if ( Busy )
        throw new InvalidOperationException( "Context is busy" );
      m_busy = true;
      m_OnCompletedDelegate += GetListAsyncCompleted;
      // Create an AsyncOperation for taskId.
      AsyncOperation _AsyncOp = AsyncOperationManager.CreateOperation( m_Counter++ );
      m_processor.Do( () => { GetListAsyncWorker<TEntity>( listName, _AsyncOp ); } );
    }
    #endregion
    public event AsyncCompletedEventHandler SubmitChangesCompleted;
    public void SubmitChangesAsyn()
    {
      if ( Busy )
        throw new InvalidOperationException( "Context is busy" );
      m_busy = true;
      m_OnCompletedDelegate += SubmitChangesAsynCompleted;
      AsyncOperation _AsyncOp = AsyncOperationManager.CreateOperation( m_Counter++ );
      m_processor.Do( () => { SubmitChangesAsynWorker( _AsyncOp ); } );
    }
    #endregion

    #region private

    #region CreateContext
    private void CreateContextAsynCompleted( object state )
    {
      if ( CreateContextAsyncCompletedEvent == null )
        return;
      CreateContextAsyncCompletedEventArgs e = state as CreateContextAsyncCompletedEventArgs;
      m_busy = false;
      CreateContextAsyncCompletedEvent( this, e );
    }
    private void CreateContextAsyncWorker( string requestUrl, AsyncOperation asyncOp )
    {
      Exception exception = null;
      try
      {
        m_Context.CreateContext( requestUrl );
      }
      catch ( Exception ex )
      {
        exception = ex;
      }
      // Package the results of the operation in a  CreateContextAsyncCompletedEventArgs.
      CreateContextAsyncCompletedEventArgs e = new CreateContextAsyncCompletedEventArgs( m_Context, exception, false, asyncOp.UserSuppliedState );
      OperationCompleted( asyncOp, e );
    }
    private void OperationCompleted( AsyncOperation asyncOp, object e )
    {
      // End the task. The asyncOp object is responsible for marshaling the call.
      m_busy = false;
      asyncOp.PostOperationCompleted( m_OnCompletedDelegate, e );
      // Note that after the call to OperationCompleted, asyncOp is no longer usable, and any attempt to use it 
      // will cause an exception to be thrown.
    }
    #endregion

    #region GetListAsync
    private void GetListAsyncCompleted( object state )
    {
      m_busy = false;
      if ( GetListCompleted == null )
        return;
      GetListAsyncCompletedEventArgs _EventArgs = (GetListAsyncCompletedEventArgs)state;
      GetListCompleted( this, _EventArgs );
    }
    /// <summary>
    /// This method performs the actual workload to gets the list.
    /// </summary>
    /// <param name="listName">Name of the list.</param>
    /// <param name="asyncOp">The _ asynchronous property.</param>
    private void GetListAsyncWorker<TEntity>( string listName, AsyncOperation asyncOp )
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      Exception _Exception = null;
      EntityList<TEntity> _Result = null;
      try
      {
        _Result = m_Context.GetList<TEntity>( listName );
      }
      catch ( Exception ex )
      {
        _Exception = ex;
      }
      // Package the results of the operation in a  GetListAsyncCompletedEventArgs.
      GetListAsyncCompletedEventArgs e = new GetListAsyncCompletedEventArgs( _Result, _Exception, false, asyncOp.UserSuppliedState );
      // End the task. The asyncOp object is responsible for marshaling the call.
      asyncOp.PostOperationCompleted( m_OnCompletedDelegate, e );
    }
    #endregion

    #region SubmitChangesAsyn
    private void SubmitChangesAsynCompleted( object state )
    {
      if ( GetListCompleted == null )
        return;
      GetListAsyncCompletedEventArgs _EventArgs = (GetListAsyncCompletedEventArgs)state;
      GetListCompleted( this, _EventArgs );
    }
    private void SubmitChangesAsynWorker( AsyncOperation asyncOp )
    {
      Exception _Exception = null;
      try
      {
        m_Context.SubmitChanges();
      }
      catch ( Exception ex )
      {
        _Exception = ex;
      }
      // Package the results of the operation in a  GetListAsyncCompletedEventArgs.
      AsyncCompletedEventArgs e = new AsyncCompletedEventArgs( _Exception, false, asyncOp.UserSuppliedState );
      // End the task. The asyncOp object is responsible for marshaling the call.
      asyncOp.PostOperationCompleted( m_OnCompletedDelegate, e );
    }
    #endregion

    private SendOrPostCallback m_OnCompletedDelegate;
    private bool m_busy = false;
    private int m_Counter = 0;
    private DataContextAsync() { }
    private DataContext m_Context = new DataContext();
    private Processor m_processor = new Processor();
    private class Processor
    {
      internal void Do( Action opration )
      {
      }
    }

    #endregion

  }
  public class CreateContextAsyncCompletedEventArgs: AsyncCompletedEventArgs
  {

    public CreateContextAsyncCompletedEventArgs( DataContext context, Exception e, bool canceled, object state )
      : base( e, canceled, state )
    {
      Result = context;
    }
    internal DataContext Result { get; private set; }

  }
}
