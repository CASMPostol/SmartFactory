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

    internal event EventHandler Entered;
    internal event EventHandler Exiting;
    public virtual void OnEnteringState()
    {
      if (Entered != null)
        Entered(this, EventArgs.Empty);
    }
    public virtual void OnExitingState()
    {
      if (Exiting != null)
        Exiting(this, EventArgs.Empty);
    }

    #region IAbstractMachineEvents Members
    public virtual void Previous()
    {
      throw new ApplicationException();
    }
    public virtual void Next()
    {
      throw new ApplicationException();
    }
    public virtual void Cancel()
    {
      Context.Close();
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
