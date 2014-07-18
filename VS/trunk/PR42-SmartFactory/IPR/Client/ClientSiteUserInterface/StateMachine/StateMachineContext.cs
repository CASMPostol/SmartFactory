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

using CAS.SharePoint.ViewModel;
using CAS.SmartFactory.IPR.Client.DataManagement;
using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{

  /// <summary>
  /// StateMachineContext class 
  /// </summary>
  //TODO change name => WizardStateMachineContext
  internal abstract class StateMachineContext : SharePoint.ComponentModel.PropertyChangedBase, IDisposable
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
      Machine = SetupDataDialogMachine.Get();
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
    protected readonly List<IDisposable> Components = new List<IDisposable>();
    #endregion

    #region IDisposable
    /// <summary>
    /// Implement IDisposable. Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// Do not make this method virtual. A derived class should not be able to override this method. 
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      // This object will be cleaned up by the Dispose method. 
      // Therefore, you should call GC.SupressFinalize to 
      // take this object off the finalization queue 
      // and prevent finalization code for this object 
      // from executing a second time.
      GC.SuppressFinalize(this);
    }
    // Dispose(bool disposing) executes in two distinct scenarios. 
    // If disposing equals true, the method has been called directly 
    // or indirectly by a user's code. Managed and unmanaged resources 
    // can be disposed. 
    // If disposing equals false, the method has been called by the 
    // runtime from inside the finalizer and you should not reference 
    // other objects. Only unmanaged resources can be disposed. 
    protected virtual void Dispose(bool disposing)
    {
      // Check to see if Dispose has already been called. 
      if (!this.disposed)
      {
        // If disposing equals true, dispose all managed 
        // and unmanaged resources. 
        if (disposing)
          foreach (IDisposable _item in Components)
            // Dispose managed resources.
            _item.Dispose();

        // Note disposing has been done.
        disposed = true;
      }
    }
    // Track whether Dispose has been called. 
    private bool disposed = false;
    // Use C# destructor syntax for finalization code. 
    // This destructor will run only if the Dispose method 
    // does not get called. 
    // It gives your base class the opportunity to finalize. 
    // Do not provide destructors in types derived from this class.
    ~StateMachineContext()
    {
      // Do not re-create Dispose clean-up code here. Calling Dispose(false) is optimal in terms of readability and maintainability.
      Dispose(false);
    }
    #endregion

  }
}
