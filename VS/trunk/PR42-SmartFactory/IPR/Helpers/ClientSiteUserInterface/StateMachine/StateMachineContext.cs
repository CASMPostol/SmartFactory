//<summary>
//  Title   : StateMachineContext class
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
using System.Windows;
using CAS.SmartFactory.IPR.Client.FeatureActivation;
using CAS.SmartFactory.IPR.Client.FeatureActivation.Archival;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{

  /// <summary>
  /// StateMachineContext class 
  /// </summary>
  internal abstract class StateMachineContext
  {

    #region creator
    internal StateMachineContext()
    {
      AssignStateMachine(ProcessState.SetupDataDialog);
    }
    #endregion


    #region IAbstractMachineEvents Members
    internal IAbstractMachineEvents Machine
    {
      get { return this.m_Machine; }
    }
    #endregion

    #region public
    internal void OpenEntryState(ProcessState state)
    {
      AssignStateMachine(state);
    }
    #endregion

    #region private

    #region StateMachineContext View Model

    public event ProgressChangedEventHandler ProgressChanged;
    internal protected abstract void Close();
    //protected abstract void Write(char value);
    //protected abstract void WriteLine();
    //protected abstract void WriteLine(string value);
    #endregion
    internal void AssignStateMachine(ProcessState state)
    {

      switch (state)
      {
        case ProcessState.SetupDataDialog:
          m_Machine = new AbstractMachine.SetupDataDialogMachine(this);
          break;
        case ProcessState.Activation:
          m_Machine = new AbstractMachine.ActivationMachine(this);
          break;
        case ProcessState.Archiving:
          m_Machine = new AbstractMachine.ArchivingMachine(this);
          break;
        case ProcessState.Finisched:
          m_Machine = new AbstractMachine.FinishedMachine(this);
          break;
      }
      m_Machine.EnteringState();
    }

    #region vars
    private AbstractMachine m_Machine = null;
    #endregion

    #endregion

    internal void ProgressChang(AbstractMachine activationMachine, EntitiesChangedEventArgs entitiesChangedEventArgs)
    {
      throw new NotImplementedException();
    }
    internal void Exception(Exception exception)
    {
      string _mssg = String.Format("Program stoped by exception: {0}", exception.Message);
      MessageBox.Show(_mssg, "Operation error", MessageBoxButton.OK, MessageBoxImage.Error);
      Close();
    }
  }

}