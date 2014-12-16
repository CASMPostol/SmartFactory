//<summary>
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
    public ShellViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
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
    }
    public override void ProgressChang(IAbstractMachineState activationMachine, ProgressChangedEventArgs entitiesState)
    {
      base.ProgressChang(activationMachine, entitiesState);
      m_EventAggregator.GetEvent<ProgressChangeEvent>().Publish(entitiesState);
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

  }
}
