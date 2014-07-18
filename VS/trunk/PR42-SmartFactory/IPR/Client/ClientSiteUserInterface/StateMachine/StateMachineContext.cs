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

using CAS.SharePoint.Interactivity.InteractionRequest;
using CAS.SharePoint.ViewModel;
using CAS.SmartFactory.IPR.Client.DataManagement;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{

  /// <summary>
  /// StateMachineContext class 
  /// </summary>
  public abstract class StateMachineContext : SharePoint.ComponentModel.PropertyChangedBase, IDisposable
  {

    #region creator
    internal StateMachineContext()
    {
      ProgressBarMaximum = ProgressBarMaximumDefault;
      Progress = 0;
      ButtonCancel = new SynchronousCommandBase<object>(x => this.Cancel(), y => this.CanExecuteCancel);
      ButtonGoBackward = new SynchronousCommandBase<object>(x => Machine.Previous(), y => this.CanExecutePrevious);
      ButtonGoForward = new SynchronousCommandBase<object>(x => Machine.Next(), y => this.CanExecuteNext);
      CancelConfirmation = new InteractionRequest<IConfirmation>();
      CloseWindow = new InteractionRequest<INotification>();
      ExceptionNotification = new InteractionRequest<INotification>();
    }
    #endregion

    #region View Model
    public int Progress
    {
      get
      {
        return b_Progress;
      }
      set
      {
        RaiseHandler<int>(value, ref b_Progress, "Progress", this);
      }
    }
    public int ProgressBarMaximum
    {
      get
      {
        return b_ProgressBarMaximum;
      }
      set
      {
        RaiseHandler<int>(value, ref b_ProgressBarMaximum, "ProgressBarMaximum", this);
      }
    }
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
    public InteractionRequest<IConfirmation> CancelConfirmation { get; private set; }
    public InteractionRequest<INotification> CloseWindow;
    public InteractionRequest<INotification> ExceptionNotification { get; private set; }
    public string State
    {
      get
      {
        return b_State;
      }
      set
      {
        RaiseHandler<string>(value, ref b_State, "State", this);
      }
    }
    #endregion

    #region internal
    public IAbstractMachine Machine
    {
      get { return this.m_Machine; }
      set
      {
        if (m_Machine != null)
          m_Machine.OnExitingState();
        m_Machine = value;
        State = value.ToString();
        this.ProgressChang(value, new ProgressChangedEventArgs(0, String.Format("Entered state {0}", State)));
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
    internal virtual void Close()
    {
      CancelConfirmation.Raise(new Confirmation() { Title = "Closing confirmation", Content = "The window will be closed? Are you sure?" });
    }
    internal virtual void UpdateProgressBar(int progress)
    {
      if (progress <= 0)
      {
        Progress = 0;
        ProgressBarMaximum = ProgressBarMaximumDefault;
        return;
      }
      while (ProgressBarMaximum - Progress < progress)
        ProgressBarMaximum *= 2;
      Progress += progress;
    }
    internal virtual void Exception(Exception exception)
    {
      string _msg = String.Format("Exception {0} occurred at state {1}: {2}", exception.GetType().Name, Machine.ToString(), exception.Message);
      CancelConfirmation.Raise(new Confirmation() { Title = "Cancellation confirmation.", Content = "The operation is canceling. Are you sure?" });
      ProgressChang(Machine, new ProgressChangedEventArgs(0, _msg));
    }
    public virtual void ProgressChang(IAbstractMachine activationMachine, ProgressChangedEventArgs entitiesState)
    {
      UpdateProgressBar(entitiesState.ProgressPercentage);
    }
    #endregion

    #region private
    //backing fields.
    private int b_Progress;
    private int b_ProgressBarMaximum;
    private ICommandWithUpdate b_ButtonCancel;
    private ICommandWithUpdate b_ButtonGoBackward;
    private ICommandWithUpdate b_ButtonGoForward;
    private bool b_CanExecuteCancel;
    private bool b_CanExecutePrevious;
    private bool b_CanExecuteNext;
    private string b_State;
    //vars
    private const int ProgressBarMaximumDefault = 100;
    private IAbstractMachine m_Machine = null;
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
    private void Cancel()
    {
      bool _confirmed = false;
      CancelConfirmation.Raise(new Confirmation() { Title = "Cancellation confirmation.", Content = "The operation is canceling. Are you sure?" , Confirmed = true }, c => _confirmed = c.Confirmed);
      if (_confirmed)
        Machine.Cancel();
    }
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
