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
using CAS.SmartFactory.Shepherd.Client.Management.Controls;
using CAS.SmartFactory.Shepherd.Client.Management.StateMachines;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.Regions;
using System;
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
    }
    private Controls.IButtonsPanelViewModel b_ButtonPanelState;
    public Controls.IButtonsPanelViewModel ButtonPanelState
    {
      get
      {
        return b_ButtonPanelState;
      }
      set
      {
        RaiseHandler<Controls.IButtonsPanelViewModel>(value, ref b_ButtonPanelState, "ButtonPanelState", this);
      }
    }
    [Import]
    public EnteringStateProvider EnteringState 
    {
      set { value.ActivateEnteringState(this); }
    }
    private IRegionManager m_RegionManager = null;
    private IEventAggregator m_EventAggregator = null;
  }
}
