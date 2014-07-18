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
  internal enum Events
  {
    Previous = 0x1,
    Next = 0x2,
    Cancel = 0x4,
  }
  internal abstract class AbstractMachine : IAbstractMachineEvents
  {

    #region constructor
    public AbstractMachine(StateMachineContext context)
    {
      Context = context;
    }
    #endregion

    internal event EventHandler Entered;
    internal event EventHandler Exiting;
    internal virtual void OnEnteringState()
    {
      if (Entered != null)
        Entered(this, EventArgs.Empty);
    }
    internal virtual void OnExitingState()
    {
      if (Exiting != null)
        Exiting(this, EventArgs.Empty);
    }
    internal abstract string State { get; }

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
    protected StateMachineContext Context { get; private set; }
    protected void SetEventMask(Events events)
    {
      Context.SetupUserInterface = events;
    }
    #endregion

  }
}
