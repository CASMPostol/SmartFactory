﻿//<summary>
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// DataContextAsync
  /// </summary>
  /// <typeparam name="TContext">The type of the context.</typeparam>
  public class DataContextAsync: IDisposable
  {
    #region public

    public bool Busy { get { return m_busy; } }
    public DataContextAsync() { }

    #region CreateContextAsync
    public event AsyncCompletedEventHandler CreateContextAsyncCompletedEvent;
    public void CreateContextAsync( string requestUrl )
    {
      EntryCheck();
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
    public void GetListAsync<TEntity>( string listName, CamlQuery camlQuery )
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      EntryCheck();
      m_busy = true;
      m_OnCompletedDelegate += GetListAsyncCompleted;
      // Create an AsyncOperation for taskId.
      AsyncOperation _AsyncOp = AsyncOperationManager.CreateOperation( m_Counter++ );
      m_processor.Do( () => { GetListAsyncWorker<TEntity>( listName, camlQuery, _AsyncOp ); } );
    }
    #endregion

    #region SubmitChanges
    public event AsyncCompletedEventHandler SubmitChangesCompleted;
    public void SubmitChangesAsyn()
    {
      EntryCheck();
      m_busy = true;
      m_OnCompletedDelegate += SubmitChangesAsynCompleted;
      AsyncOperation _AsyncOp = AsyncOperationManager.CreateOperation( m_Counter++ );
      m_processor.Do( () => { SubmitChangesAsynWorker( _AsyncOp ); } );
    }
    #endregion

    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      m_Disposed = true;
      if ( m_Context != null )
        m_Context.Dispose();
    }
    #endregion

    #region private

    #region CreateContext
    private void CreateContextAsynCompleted( object state )
    {
      m_busy = false;
      if ( CreateContextAsyncCompletedEvent == null )
        return;
      AsyncCompletedEventArgs e = state as AsyncCompletedEventArgs;
      CreateContextAsyncCompletedEvent( this, e );
    }
    private void CreateContextAsyncWorker( string requestUrl, AsyncOperation asyncOp )
    {
      Exception _Exception = null;
      DataContext _Context = new DataContext();
      try
      {
        _Context.CreateContext( requestUrl );
      }
      catch ( Exception _Ex )
      {
        _Exception = _Ex;
      }
      m_Context = _Context;
      // Package the results of the operation in a  CreateContextAsyncCompletedEventArgs.
      AsyncCompletedEventArgs _EventArgs = new AsyncCompletedEventArgs( _Exception, false, asyncOp.UserSuppliedState );
      asyncOp.PostOperationCompleted( m_OnCompletedDelegate, _EventArgs );
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
    private void GetListAsyncWorker<TEntity>( string listName, CamlQuery camlQuery, AsyncOperation asyncOp )
       where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
    {
      Exception _Exception = null;
      List<TEntity> _Result = null;
      try
      {
        _Result = m_Context.GetList<TEntity>( listName ).Filter<TEntity>( camlQuery ).ToList<TEntity>();
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
      if ( SubmitChangesCompleted == null )
        return;
      AsyncCompletedEventArgs _EventArgs = (AsyncCompletedEventArgs)state;
      SubmitChangesCompleted( this, _EventArgs );
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

    private class Processor
    {
      public Processor()
      {
        m_thred = new Thread( Worker );
        m_thred.Name = "Processor";
        m_thred.Start();
      }
      internal void Do( Action opration )
      {
        lock ( this )
        {
          if ( m_Action != null )
            throw new InvalidOperationException( "The processot is busy." );
          m_Action = opration;
          m_Signal.Set();
        }
      }
      private Thread m_thred = null;
      private Action m_Action = null;
      private AutoResetEvent m_Signal = new AutoResetEvent( false );
      private void Worker( object obj )
      {
        while ( true )
        {
          Action _actn;
          m_Signal.WaitOne();
          lock ( this )
          {
            _actn = m_Action;
            m_Action = null;
          }
          if ( _actn == null )
            continue;
          _actn();
        }
      }
    }
    private SendOrPostCallback m_OnCompletedDelegate;
    private bool m_busy = false;
    private int m_Counter = 0;
    private DataContext m_Context = null;
    private Processor m_processor = new Processor();
    private bool m_Disposed = false;
    private void EntryCheck()
    {
      if ( m_busy )
        throw new InvalidOperationException( "Context is busy" );
      if ( m_Disposed )
        throw new ObjectDisposedException( typeof( DataContextAsync ).Name );
    }

    #endregion

  }
}
