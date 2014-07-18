//<summary>
//  Title   : class SetupDataDialogMachine
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class SetupDataDialogMachine : AbstractMachine
  {
    public SetupDataDialogMachine(StateMachineContext context)
      : base(context)
    {
      m_Me = this;
    }
    internal static SetupDataDialogMachine Get()
    {
      return m_Me;
    }

    #region AbstractMachine
    internal override void OnEnteringState()
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
    internal override string State
    {
      get { return "Setup"; }
    }
    #endregion

    private static SetupDataDialogMachine m_Me;

  }
}
