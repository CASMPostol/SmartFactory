//<summary>
//  Title   : Name of Application
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

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  [Flags]
  public enum Events
  {
    Previous = 0x1,
    Next = 0x2,
    Cancel = 0x4,
  }
  /// <summary>
  /// It implements basic functionality of the wizard state of the <see cref="StateMachineContext"/>
  /// </summary>
  public abstract class AbstractMachineState<ContextType> : IAbstractMachine
    where ContextType : StateMachineContext
  {

    #region constructor
    public AbstractMachineState(ContextType context)
    {
      Context = context;
    }
    #endregion

    public event EventHandler Entered;
    public event EventHandler Exiting;
    public virtual void OnEnteringState()
    {
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, String.Format("Entered state {0}", ToString())));
      if (Entered != null)
        Entered(this, EventArgs.Empty);
    }
    public virtual void OnExitingState()
    {
      Context.ProgressChang(this, new ProgressChangedEventArgs(0, String.Format("Exiting the state {0}", ToString())));
      if (Exiting != null)
        Exiting(this, EventArgs.Empty);
    }
    /// <summary>
    /// Called when exception has occurred.
    /// </summary>
    /// <param name="exception">The exception.</param>
    public abstract void OnException(Exception exception);
    /// <summary>
    /// Called when cancellation is requested.
    /// </summary>
    public abstract void OnCancellation();
    #region IAbstractMachineEvents Members
    /// <summary>
    /// Go to previous step.
    /// </summary>
    public virtual void Previous() { }
    /// <summary>
    /// Go to the next step.
    /// </summary>
    public virtual void Next() { }
    /// <summary>
    /// Cancels this operation.
    /// </summary>
    public virtual void Cancel()
    {
      OnCancellation();
    }
    #endregion

    #region private
    /// <summary>
    /// Gets the current context derived form <see cref="StateMachineContext"/>.
    /// </summary>
    /// <value>
    /// The context.
    /// </value>
    protected ContextType Context { get; private set; }
    /// <summary>
    /// Sets the event mask.
    /// </summary>
    /// <param name="events">The active events mask.</param>
    protected void SetEventMask(Events events)
    {
      Context.ActiveEvents = events;
    }
    #endregion

  }
}
