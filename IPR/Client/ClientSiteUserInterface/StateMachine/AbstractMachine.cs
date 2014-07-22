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
    public abstract void OnCancelation();
    #region IAbstractMachineEvents Members
    public virtual void Previous() { }
    public virtual void Next() { }
    public virtual void Cancel()
    {
      OnCancelation();
    }
    #endregion

    #region private
    protected ContextType Context { get; private set; }
    protected void SetEventMask(Events events)
    {
      Context.SetupUserInterface = events;
    }
    #endregion

  }
}
