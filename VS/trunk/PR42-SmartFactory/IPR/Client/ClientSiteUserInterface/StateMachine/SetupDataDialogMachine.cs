//<summary>
//  Title   : class SetupDataDialogMachine
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint.ViewModel.Wizard;
using CAS.SmartFactory.IPR.Client.UserInterface.ViewModel;
using System;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class SetupDataDialogMachine : AbstractMachineState<MainWindowModel>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="SetupDataDialogMachine"/> class.
    /// </summary>
    public SetupDataDialogMachine() { }
    /// <summary>
    /// Initializes a new instance of the <see cref="SetupDataDialogMachine"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public SetupDataDialogMachine(MainWindowModel context)
      : base(context)
    {
      m_Me = this;
    }
    internal static SetupDataDialogMachine Get()
    {
      if (m_Me == null)
        throw new ApplicationException("Obsolete - not initialized");
      return m_Me;
    }

    #region AbstractMachine
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      SetEventMask(Events.Cancel | Events.Next);
    }
    public override void Next()
    {
      Context.Machine = new CleanupMachine(Context);
    }
    public override void Cancel()
    {
      Context.Close();
    }
    public override string ToString()
    {
      return "Setup";
    }
    public override void OnException(System.Exception exception)
    {
      Context.Exception(exception);
      Context.Machine = FinishedMachine.Get();
    }
    public override void OnCancellation()
    {
      Context.Close();
    }
    #endregion

    private static SetupDataDialogMachine m_Me;


  }
}
