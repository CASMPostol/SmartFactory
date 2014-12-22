﻿//<summary>
//  Title   : ShellViewModel
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
using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;

namespace CAS.SmartFactory.Shepherd.Client.Management
{
  /// <summary>
  /// Class ShellViewModel.
  /// </summary>
  [Export]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class ShellViewModel : StateMachineContext
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ShellViewModel"/> class.
    /// </summary>
    /// <param name="regionManager">The region manager service.</param>
    /// <param name="eventAggregator">The event aggregator service.</param>
    /// <exception cref="System.ArgumentNullException">
    /// watchListService
    /// or
    /// eventAggregator
    /// </exception>
    [ImportingConstructor]
    public ShellViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, ILoggerFacade loggingService)
    {
      if (regionManager == null)
      {
        throw new ArgumentNullException("watchListService");
      }
      m_RegionManager = regionManager;
      if (eventAggregator == null)
      {
        throw new ArgumentNullException("eventAggregator");
      }
      m_EventAggregator = eventAggregator;
      m_LoggingService = loggingService;
      m_LoggingService.Log("Entered ShellViewModel state machine context", Category.Info, Priority.Low);
    }
    /// <summary>
    /// Is called by the event handler of the <see cref="BackgroundWorker.ProgressChanged" />.
    /// </summary>
    /// <param name="activationMachine">The activation machine.</param>
    /// <param name="entitiesState">The <see cref="ProgressChangedEventArgs" /> instance containing the event data.</param>
    public override void ProgressChang(IAbstractMachineState activationMachine, ProgressChangedEventArgs entitiesState)
    {
      base.ProgressChang(activationMachine, entitiesState);
      m_EventAggregator.GetEvent<ProgressChangeEvent>().Publish(entitiesState);
    }
    /// <summary>
    /// Sets the new name of the new view to be displayed. New view is responsible to enter new machine state.
    /// </summary>
    /// <value>The state of the switch.</value>
    public override string SwitchState
    {
      set 
      {
        m_LoggingService.Log(String.Format("RequestNavigate to {0}", value), Category.Debug, Priority.Low);
        m_RegionManager.RequestNavigate(Infrastructure.RegionNames.ActionRegion, new Uri(value, UriKind.Relative)); 
      }
    }
    /// <summary>
    /// Reports state name change.
    /// </summary>
    /// <param name="machineStateName">Current name of the machine state.</param>
    protected override void StateNameProgressChang(string machineStateName)
    {
      m_EventAggregator.GetEvent<MachineStateNameEvent>().Publish(machineStateName);
    }
    private IRegionManager m_RegionManager = null;
    private IEventAggregator m_EventAggregator = null;
    private ILoggerFacade m_LoggingService;
  }
}
