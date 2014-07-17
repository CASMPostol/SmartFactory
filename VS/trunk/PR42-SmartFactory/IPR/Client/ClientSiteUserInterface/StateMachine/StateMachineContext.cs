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
using CAS.SmartFactory.IPR.Client.DataManagement;
using System.Windows.Input;
using CAS.SharePoint.ViewModel;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{

  /// <summary>
  /// StateMachineContext class 
  /// </summary>
  //TODO change name => WizardStateMachineContext
  internal abstract class StateMachineContext : SharePoint.ComponentModel.PropertyChangedBase
  {

    #region creator
    internal StateMachineContext()
    {
      ButtonCancel = new SynchronousCommandBase<object>(x => Machine.Cancel(), y => this.CanExecuteCancel);
      ButtonGoBackward = new SynchronousCommandBase<object>(x => Machine.Previous(), y => this.CanExecutePrevious);
      ButtonGoForward = new SynchronousCommandBase<object>(x => Machine.Next(), y => this.CanExecuteNext);
    }
    #endregion

    #region View Model
    public ICommandWithUpdate ButtonCancel
    {
      get
      {
        return b_ButtonCancel;
      }
      set
      {
        RaiseHandler(value, ref b_ButtonCancel, "ButtonCancel", this);
      }
    }
    public ICommandWithUpdate ButtonGoBackward
    {
      get
      {
        return b_ButtonGoBackward;
      }
      set
      {
        RaiseHandler<ICommandWithUpdate>(value, ref b_ButtonGoBackward, "ButtonGoBackward", this);
      }
    }
    public ICommandWithUpdate ButtonGoForward
    {
      get
      {
        return b_ButtonGoForward;
      }
      set
      {
        RaiseHandler<ICommandWithUpdate>(value, ref b_ButtonGoForward, "ButtonGoForward", this);
      }
    }
    #endregion

    #region internal
    internal AbstractMachine Machine
    {
      get { return this.m_Machine; }
      set
      {
        if (m_Machine != null)
          m_Machine.OnExitingState();
        m_Machine = value;
        m_Machine.OnEnteringState();
      }
    }
    internal Events SetupUserInterface
    {
      set
      {
        CanExecuteCancel = (value & StateMachine.Events.Cancel) != 0;
        CanExecutePrevious = (value & StateMachine.Events.Previous) != 0;
        CanExecuteNext = (value & StateMachine.Events.Next) != 0;
        RaiseCanExecuteChanged();
      }
    }
    internal abstract void Close();
    internal abstract void UpdateProgressBar(int progress);
    internal abstract void WriteLine(string value);
    internal abstract void Exception(Exception exception);
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
      UpdateProgressBar(1);
    }
    #endregion

    #region private

    //vars
    private AbstractMachine m_Machine = null;
    private ICommandWithUpdate b_ButtonCancel;
    private ICommandWithUpdate b_ButtonGoBackward;
    private ICommandWithUpdate b_ButtonGoForward;
    private bool b_CanExecuteCancel;
    private bool b_CanExecutePrevious;
    private bool b_CanExecuteNext;
    // Summary:
    //     Occurs when changes occur that affect whether or not the command should execute.
    private event EventHandler CanExecuteChanged;
    //procedures
    private void RaiseCanExecuteChanged()
    {
      EventHandler _cec = CanExecuteChanged;
      if (_cec == null)
        return;
      CanExecuteChanged(this, EventArgs.Empty);
    }
    private bool RaiseHandler(ICommandWithUpdate value, ref ICommandWithUpdate oldValue, string propertyName, object sender)
    {
      bool _ret = base.RaiseHandler<ICommandWithUpdate>(value, ref oldValue, propertyName, sender);
      if (_ret)
        this.CanExecuteChanged += (sevder, e) => value.RaiseCanExecuteChanged();
      return _ret;
    }
    protected void OpenEntryState()
    {
      Machine = AbstractMachine.SetupDataDialogMachine.Get();
    }
    private bool CanExecuteCancel
    {
      get { return b_CanExecuteCancel; }
      set { b_CanExecuteCancel = value; }
    }
    private bool CanExecutePrevious
    {
      get { return b_CanExecutePrevious; }
      set { b_CanExecutePrevious = value; }
    }
    private bool CanExecuteNext
    {
      get { return b_CanExecuteNext; }
      set { b_CanExecuteNext = value; }
    }
    #endregion

  }
}