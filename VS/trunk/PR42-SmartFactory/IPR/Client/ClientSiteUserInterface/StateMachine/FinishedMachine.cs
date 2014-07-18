//<summary>
//  Title   : class FinishedMachine
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
  internal class FinishedMachine : AbstractMachineState<MainWindowModel>
  {
    internal FinishedMachine(MainWindowModel context)
      : base(context)
    {
      m_Me = this;
    }
    internal static FinishedMachine Get()
    {
      return m_Me;
    }
    //AbstractMachine
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      SetEventMask(Events.Next & Events.Previous);
      Context.ProgressChang(this, new System.ComponentModel.ProgressChangedEventArgs(0, "All operation have been finished, press >> to exit the program"));
    }
    public override void Previous()
    {
      base.Previous();
      Context.Machine = ActivationMachine.Get();
    }
    public override void Next()
    {
      Context.Close();
    }
    public override string ToString()
    {
      return "Finishing";
    }

    private static FinishedMachine m_Me;

  }
}
