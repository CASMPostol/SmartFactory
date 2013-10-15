//<summary>
//  Title   : class DataContextAsync
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
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// DataContextAsync
  /// </summary>
  /// <typeparam name="TContext">The type of the context.</typeparam>
  public class DataContextAsync<TContext>
    where TContext: DataContext, new()
  {

    public event CreateContextAsyncCompletedEventHandler CreateContextAsyncCompletedEvent;
    public delegate void CreateContextAsyncCompletedEventHandler( object sender, CreateContextAsyncCompletedEventArgs<TContext> e );
    public void CreateContextAsync( string requestUrl )
    {
      if ( m_AsyncOp != null )
        throw new ArgumentException( "Task ID parameter must be unique", "taskId" );
      m_OnCompletedDelegate += CreateContextAsynCompleted;
      m_AsyncOp = AsyncOperationManager.CreateOperation( m_Counter++ );
      m_processor.Do( () => { CreateContextWorker( requestUrl, m_AsyncOp ); } );
    }
    private void CreateContextAsynCompleted( object state )
    {
      if ( CreateContextAsyncCompletedEvent == null )
        return;
      CreateContextAsyncCompletedEventArgs<TContext> e = state as CreateContextAsyncCompletedEventArgs<TContext>;
      CreateContextAsyncCompletedEvent( this, e );
    }
    private void CreateContextWorker( string requestUrl, AsyncOperation asyncOp )
    {
      Exception e = null;
      try
      {
        m_Context.CreateContext( requestUrl );
      }
      catch ( Exception ex )
      {
        e = ex;
      }
      this.CompletionMethod( m_Context, e, false, asyncOp );
    }
    private void CompletionMethod( TContext result, Exception exception, bool canceled, AsyncOperation asyncOp )
    {
      // If the task was not previously canceled, remove the task from the lifetime collection. 
      if ( !canceled )
      {
        lock ( this )
        {
          m_AsyncOp = null;
        }
      }
      // Package the results of the operation in a  GetListAsyncCompletedEventArgs.
      CreateContextAsyncCompletedEventArgs<TContext> e = new CreateContextAsyncCompletedEventArgs<TContext>( result, exception, canceled, asyncOp.UserSuppliedState );
      // End the task. The asyncOp object is responsible for marshaling the call.
      asyncOp.PostOperationCompleted( m_OnCompletedDelegate, e );
      // Note that after the call to OperationCompleted, asyncOp is no longer usable, and any attempt to use it 
      // will cause an exception to be thrown.
    }
    private SendOrPostCallback m_OnCompletedDelegate;
    private AsyncOperation m_AsyncOp = null;
    private int m_Counter = 0;
    private DataContextAsync() { }
    private TContext m_Context = new TContext();
    private Processor m_processor = new Processor();
    private class Processor
    {
      internal void Do( Action opration )
      {
      }
    }
  }
  public class CreateContextAsyncCompletedEventArgs<TContext>: AsyncCompletedEventArgs
    where TContext: DataContext, new()
  {
    public CreateContextAsyncCompletedEventArgs( object TContext, Exception e, bool canceled, object state )
      : base( e, canceled, state )
    {

    }
    internal TContext Result { get; private set; }
  }
}
