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

using CAS.SmartFactory.IPR.Client.UserInterface.ViewModel;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class SetupDataDialogMachine : AbstractMachineState<MainWindowModel>
  {
    public SetupDataDialogMachine(MainWindowModel context)
      : base(context)
    {
      m_Me = this;
    }
    internal static SetupDataDialogMachine Get()
    {
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
      Context.Machine = ActivationMachine.Get();
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
    public override void OnCancelation()
    {
      Context.Close();
    }
    #endregion

    private static SetupDataDialogMachine m_Me;


  }
}
