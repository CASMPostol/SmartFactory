//<summary>
//  Title   : SetupDataDialogMachine
//  System  : Microsoft VisualStudio 2013 / C#
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

using CAS.Common.ViewModel.Wizard;
using CAS.Common.ViewModel.Wizard.ButtonsPanelStateTemplates;
using System;

/// <summary>
/// The StateMachines namespace.
/// </summary>
namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{
  /// <summary>
  /// Class SetupDataDialogMachine.
  /// </summary>
  public abstract class SetupDataDialogMachine<ViewModelContextType> : BackgroundWorkerMachine<ShellViewModel, ViewModelContextType>
    where ViewModelContextType : IViewModelContext
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="SetupDataDialogMachine"/> class.
    /// </summary>
    public SetupDataDialogMachine()
    {
      m_StateMachineActionsArray = new Action<object>[4]; 
      //{
      //  x => throw new NotImplementedException("This action is not handled")
      //}
          //{ x => m_ActionExecuted[(int)StateMachineEventIndex.LeftButtonEvent] = true , 
          //  x => m_ActionExecuted[(int)StateMachineEventIndex.LeftMiddleButtonEvent] = true ,
          //  x => m_ActionExecuted[(int)StateMachineEventIndex.RightMiddleButtonEvent] = true ,
          //  x => m_ActionExecuted[(int)StateMachineEventIndex.RightButtonEvent] = true 
          //};
    }

    #region IAbstractMachineState
    /// <summary>
    /// Gets the state machine actions array.
    /// </summary>
    /// <value>The state machine actions array.</value>
    /// <exception cref="System.NotImplementedException"></exception>
    public override Action<object>[] StateMachineActionsArray
    {
      get { throw new System.NotImplementedException(); }
    }

    #endregion

    /// <summary>
    /// Called on entering new state.
    /// </summary>
    public override void OnEnteringState()
    {
      base.OnEnteringState();
    }

    #region BackgroundWorkerMachine
    /// <summary>
    /// Handles the DoWork event of the BackgroundWorker control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="DoWorkEventArgs" /> instance containing the event data.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      throw new NotImplementedException();
    }
    /// <summary>
    /// Called when worker task has been completed.
    /// </summary>
    /// <param name="result">An object that represents the result of an asynchronous operation.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    protected override void RunWorkerCompleted(object result)
    {
      throw new NotImplementedException();
    }
    #endregion

    #region private
    protected abstract string URL { get; }
    protected abstract string DatabaseName { get; }
    protected abstract string SQLServer { get; }
    protected override void OnlyCancelActive()
    {
      m_ButtonsTemplate.OnlyCancel();
    }
    protected override ButtonsPanelState ButtonsPanelState
    {
      get { return m_ButtonsTemplate; }
    }
    private ConnectCancelTemplate m_ButtonsTemplate = new ConnectCancelTemplate();
    private Action<object>[] m_StateMachineActionsArray;
    #endregion

  }
}
