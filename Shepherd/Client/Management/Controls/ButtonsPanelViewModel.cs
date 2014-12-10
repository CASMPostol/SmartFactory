//<summary>
//  Title   : ButtonsPanelViewModel
//  System  : Microsoft VisulaStudio 2013 / C#
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

using CAS.Common.ComponentModel;
using CAS.Common.ViewModel;
using CAS.Common.ViewModel.Wizard;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  [Export]
  internal class ButtonsPanelViewModel : PropertyChangedBase, IButtonsPanelViewModel
  {
    [ImportingConstructor]
    public ButtonsPanelViewModel(ShellViewModel parentViewMode)
    {
      if (parentViewMode == null)
        throw new ArgumentNullException("parentViewMode");
      m_ParentViewMode = parentViewMode;
      GetState(parentViewMode.ButtonPanelState);
      parentViewMode.PropertyChanged += parentViewMode_PropertyChanged;
      LeftButtonCommand = new SynchronousCommandBase<object>(x => m_ParentViewMode.OnLeftButtonCommand(), y => (this.m_EnabledEvents & StateMachineEvents.LeftButtonEvent) != 0);
      LeftMiddleButtonCommand = new SynchronousCommandBase<object>(x => m_ParentViewMode.OnLeftMiddleButtonCommand(), y => (this.m_EnabledEvents & StateMachineEvents.LeftMiddleButtonEvent) != 0);
      RightMiddleButtonCommand = new SynchronousCommandBase<object>(x => m_ParentViewMode.OnRightMiddleButtonCommand(), y => (this.m_EnabledEvents & StateMachineEvents.RightMiddleButtonEvent) != 0);
      RightButtonCommand = new SynchronousCommandBase<object>(x => m_ParentViewMode.OnRightButtonCommand(), y => (this.m_EnabledEvents & StateMachineEvents.RightButtonEvent) != 0);
    }
    private ICommandWithUpdate b_LeftButtonCommand;
    /// <summary>
    /// Gets or sets the left button command.
    /// </summary>
    /// <value>The left button command.</value>
    public ICommandWithUpdate LeftButtonCommand
    {
      get
      {
        return b_LeftButtonCommand;
      }
      set
      {
        RaiseHandlerICommandWithUpdate(value, ref b_LeftButtonCommand, "LeftButtonCommand", this);
      }
    }
    private ICommandWithUpdate b_LeftMiddleButtonCommand;
    public ICommandWithUpdate LeftMiddleButtonCommand
    {
      get
      {
        return b_LeftMiddleButtonCommand;
      }
      set
      {
        RaiseHandlerICommandWithUpdate(value, ref b_LeftMiddleButtonCommand, "LeftMiddleButtonCommand", this);
      }
    }
    private ICommandWithUpdate b_RightMiddleButtonCommand;
    public ICommandWithUpdate RightMiddleButtonCommand
    {
      get
      {
        return b_RightMiddleButtonCommand;
      }
      set
      {
        RaiseHandlerICommandWithUpdate(value, ref b_RightMiddleButtonCommand, "RightMiddleButtonCommand", this);
      }
    }
    private ICommandWithUpdate b_RightButtonCommand;
    public ICommandWithUpdate RightButtonCommand
    {
      get
      {
        return b_RightButtonCommand;
      }
      set
      {
        RaiseHandlerICommandWithUpdate(value, ref b_RightButtonCommand, "RightButtonCommand", this);
      }
    }
    public string LeftButtonTitle
    {
      get
      {
        return b_LeftButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_LeftButtonTitle, "LeftButtonTitle", this);
      }
    }
    public string LeftMiddleButtonTitle
    {
      get
      {
        return b_LeftMiddleButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_LeftMiddleButtonTitle, "LeftMiddleButtonTitle", this);
      }
    }
    public string RightMiddleButtonTitle
    {
      get
      {
        return b_RightMiddleButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_RightMiddleButtonTitle, "RightMiddleButtonTitle", this);
      }
    }
    public string RightButtonTitle
    {
      get
      {
        return b_RightButtonTitle;
      }
      set
      {
        RaiseHandler<string>(value, ref b_RightButtonTitle, "RightButtonTitle", this);
      }
    }
    private Visibility b_LeftButtonVisibility;
    public Visibility LeftButtonVisibility
    {
      get
      {
        return b_LeftButtonVisibility;
      }
      set
      {
        RaiseHandler<System.Windows.Visibility>(value, ref b_LeftButtonVisibility, "LeftButtonVisibility", this);
      }
    }
    private Visibility b_LeftMiddleButtonVisibility;
    public Visibility LeftMiddleButtonVisibility
    {
      get
      {
        return b_LeftMiddleButtonVisibility;
      }
      set
      {
        RaiseHandler<Visibility>(value, ref b_LeftMiddleButtonVisibility, "LeftMiddleButtonVisibility", this);
      }
    }
    private Visibility b_RightMiddleButtonVisibility;
    public Visibility RightMiddleButtonVisibility
    {
      get
      {
        return b_RightMiddleButtonVisibility;
      }
      set
      {
        RaiseHandler<Visibility>(value, ref b_RightMiddleButtonVisibility, "RightMiddleButtonVisibility", this);
      }
    }
    private Visibility b_RightButtonVisibility;
    public Visibility RightButtonVisibility
    {
      get
      {
        return b_RightButtonVisibility;
      }
      set
      {
        RaiseHandler<Visibility>(value, ref b_RightButtonVisibility, "RightButtonVisibility", this);
      }
    }

    private string b_LeftButtonTitle = "Left";
    private string b_LeftMiddleButtonTitle = "Left Middle";
    private string b_RightMiddleButtonTitle = "Right Middle";
    private string b_RightButtonTitle = "Right";

    private ShellViewModel m_ParentViewMode;
    private void parentViewMode_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == "ButtonPanelState")
        GetState(m_ParentViewMode.ButtonPanelState);
    }
    private void GetState(ButtonsPanelState state)
    {
      this.LeftButtonTitle = state.LeftButtonTitle;
      this.LeftButtonVisibility = state.LeftButtonVisibility;
      this.LeftMiddleButtonTitle = state.LeftMiddleButtonTitle;
      this.LeftMiddleButtonVisibility = state.LeftMiddleButtonVisibility;
      this.RightButtonTitle = state.RightButtonTitle;
      this.RightButtonVisibility = state.RightButtonVisibility;
      this.RightMiddleButtonTitle = state.RightMiddleButtonTitle;
      this.RightMiddleButtonVisibility = state.RightButtonVisibility;
      this.m_EnabledEvents = state.EnabledEvents;
      RaiseCanExecuteChanged();
    }
    private StateMachineEvents m_EnabledEvents = (StateMachineEvents)0;
    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    private event EventHandler CanExecuteChanged;
    //procedures
    private void RaiseCanExecuteChanged()
    {
      EventHandler _cec = CanExecuteChanged;
      if (_cec == null)
        return;
      CanExecuteChanged(this, EventArgs.Empty);
    }
    private bool RaiseHandlerICommandWithUpdate(ICommandWithUpdate value, ref ICommandWithUpdate oldValue, string propertyName, object sender)
    {
      bool _ret = base.RaiseHandler<ICommandWithUpdate>(value, ref oldValue, propertyName, sender);
      if (_ret)
        this.CanExecuteChanged += (sevder, e) => value.RaiseCanExecuteChanged();
      return _ret;
    }

  }
}
