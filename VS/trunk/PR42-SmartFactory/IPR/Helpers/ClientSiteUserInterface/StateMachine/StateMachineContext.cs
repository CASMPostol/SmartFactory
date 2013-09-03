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
using CAS.SmartFactory.IPR.Client.FeatureActivation;

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
    #endregion

    #region StateMachineContext View Model
    internal abstract void SetSiteURL(string URL);
    internal abstract string GetSiteUrl();
    internal abstract void SetupUserInterface(Events allowedEvents);
    internal abstract void Close();
    internal abstract void Progress(int progress);
    internal abstract void WriteLine();
    internal abstract void WriteLine(string value);
    internal abstract void Exception(Exception exception);
    internal abstract void EnteringState();
    internal abstract void DisplayStatusURL( string url );
    internal void ProgressChang(AbstractMachine activationMachine, EntitiesChangedEventArgs.EntitiesState entitiesState)
    {
      if (entitiesState == null)
      {
        throw new ArgumentNullException("entitiesState");
      }
      if (entitiesState.UserState != null && entitiesState.UserState is String)
      {
        WriteLine((string)entitiesState.UserState);
        //dotCounter = 0;
        //entitiesState.Entities.SubmitChanges();
        return;
      }
      //dotCounter++;
      Progress(1);
      //if (dotCounter % 100 == 0)
      //  entitiesState.Entities.SubmitChanges();
    }

    #endregion

    #region private

    #region vars
    private AbstractMachine m_Machine = null;
    #endregion

    //private static uint dotCounter = 0;
    #endregion

    #region event handlers
    internal void ButtonGoBackward_Click(object sender, RoutedEventArgs e)
    {
      Machine.Previous();
    }
    internal void ButtonGoForward_Click(object sender, RoutedEventArgs e)
    {
      Machine.Next();
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