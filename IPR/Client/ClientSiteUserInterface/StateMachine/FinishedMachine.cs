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


using CAS.SharePoint.ViewModel.Wizard;
using CAS.SmartFactory.IPR.Client.UserInterface.ViewModel;
using System;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class FinishedMachine : AbstractMachineState<MainWindowModel>
  {
    //constructor
    public FinishedMachine() { }
    //AbstractMachine
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      SetEventMask(Events.Next | Events.Previous);
      Context.ButtonNextTitle = Properties.Resources.ButtonExit;
      Context.ProgressChang(this, new System.ComponentModel.ProgressChangedEventArgs(0, "All operation have been finished, press >> to exit the program"));
    }
    public override void Previous()
    {
      base.Previous();
      Context.EnterState<SetupDataDialogMachine>();
    }
    public override void Next()
    {
      Context.Close();
    }
    public override string ToString()
    {
      return "Finishing";
    }
    public override void OnException(System.Exception exception)
    {
      Context.Exception(exception);
      Context.Close();
    }
    public override void OnCancellation()
    {
      Context.Close();
    }

  }
}
