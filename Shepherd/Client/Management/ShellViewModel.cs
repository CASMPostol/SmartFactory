//<summary>
//  Title   : ShellViewModel
//  System  : Microsoft VisulaStudio 2013 / C#
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
  [Export]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class ShellViewModel : StateMachineContext
  {
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
    private IRegionManager m_RegionManager = null;
    private IEventAggregator m_EventAggregator = null;

  }
}
