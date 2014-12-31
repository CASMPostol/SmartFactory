//<summary>
//  Title   : ViewModelStateMachineBase
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
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Regions;
using System.ComponentModel.Composition;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Class ViewModelStateMachineBase - base ViewModel for all View classes hosting a state the state machine <see cref=""/>
  /// </summary>
  /// <typeparam name="StateType">The type of the state.</typeparam>
  public class ViewModelStateMachineBase<StateType> : ViewModelBase<StateType>, INavigationAware, ILoggerFacade
    where StateType : class, IAbstractMachineState, new()
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelStateMachineBase{StateType}"/> class.
    /// </summary>
    /// <param name="loggerFacade">The logger facade class.</param>
    public ViewModelStateMachineBase(ILoggerFacade loggerFacade)
    {
      m_ILoggerFacade = loggerFacade;
    }
    /// <summary>
    /// Sets the master shell view model. It activates the state <typeparamref name="StateType"/>
    /// </summary>
    /// <value>The master shell view model <see cref="ShellViewModel" />.</value>
    [Import]
    public ShellViewModel MasterShellViewModel
    {
      set
      {
        m_ShellViewModel = value;
      }
    }

    #region INavigationAware
    /// <summary>
    /// Called when the implementer has been navigated to.
    /// </summary>
    /// <param name="navigationContext">The navigation context.</param>
    public virtual void OnNavigatedTo(NavigationContext navigationContext)
    {
      base.EnterState(m_ShellViewModel);
    }
    public virtual bool IsNavigationTarget(NavigationContext navigationContext)
    {
      return true;
    }
    public virtual void OnNavigatedFrom(NavigationContext navigationContext)
    {
      base.ExitState();
    }
    #endregion


    #region ILoggerFacade
    /// <summary>
    /// Write a new log entry with the specified category and priority.
    /// </summary>
    /// <param name="message">Message body to log.</param>
    /// <param name="category">Category of the entry.</param>
    /// <param name="priority">The priority of the entry.</param>
    public void Log(string message, Category category, Priority priority)
    {
      m_ILoggerFacade.Log(message, category, priority);
    }
    #endregion

    #region private
    private ShellViewModel m_ShellViewModel;
    private ILoggerFacade m_ILoggerFacade;
    #endregion

  }
}
