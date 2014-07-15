//<summary>
//  Title   : StateMachineContext class
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
using System.Windows;
using CAS.SmartFactory.IPR.Client.DataManagement.Linq;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{

  /// <summary>
  /// StateMachineContext class 
  /// </summary>
  internal abstract class StateMachineContext: SharePoint.ComponentModel.PropertyChangedBase
  {

    #region creator
    internal StateMachineContext()
    {
      AbstractMachine.CreateStates(this);
    }
    #endregion

    #region IAbstractMachineEvents Members
    internal IAbstractMachineEvents Machine
    {
      get { return this.m_Machine; }
    }
    #endregion

    #region public
    internal void OpenEntryState()
    {
      AssignStateMachine(ProcessState.SetupDataDialog);
    }
    internal void AssignStateMachine(ProcessState state)
    {
      if (m_Machine != null)
        m_Machine.OnExitingState();
      switch (state)
      {
        case ProcessState.SetupDataDialog:
          m_Machine = AbstractMachine.SetupDataDialogMachine.Get();
          break;
        case ProcessState.Activation:
          m_Machine = AbstractMachine.ActivationMachine.Get();
          break;
        case ProcessState.Archiving:
          m_Machine = AbstractMachine.ArchivingMachine.Get();
          break;
        case ProcessState.Finisched:
          m_Machine = AbstractMachine.FinishedMachine.Get();
          break;
      }
      m_Machine.OnEnteringState();
    }
    #endregion

    #region StateMachineContext View Model
    internal abstract void SetupUserInterface(Events allowedEvents);
    internal abstract void Close();
    internal abstract void Progress(int progress);
    internal abstract void WriteLine();
    internal abstract void WriteLine(string value);
    internal abstract void Exception(Exception exception);
    internal abstract void EnteringState();
    internal void ProgressChang(AbstractMachine activationMachine, EntitiesChangedEventArgs.EntitiesState entitiesState)
    {
      if (entitiesState == null)
      {
        throw new ArgumentNullException("entitiesState");
      }
      if (entitiesState.UserState != null && entitiesState.UserState is String)
      {
        WriteLine((string)entitiesState.UserState);
        return;
      }
      Progress(1);
    }

    #endregion

    #region private

    #region vars
    private AbstractMachine m_Machine = null;
    #endregion

    #endregion

    #region event handlers
    internal void ButtonGoBackward_Click(object sender, RoutedEventArgs e)
    {
      Machine.Previous();
    }
    internal void ButtonGoForward_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        Machine.Next();
      }
      catch (Exception ex)
      {
        string _mssg = String.Format("Operation interrupted by exception: {0}", ex.Message);
        MessageBox.Show(_mssg, "Operation error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
    internal void ButtonRun_Click(object sender, RoutedEventArgs e)
    {
      Machine.RunAsync();
    }
    internal void ButtonCancel_Click(object sender, RoutedEventArgs e)
    {
      Machine.Cancel();
    }
    #endregion


  }

}