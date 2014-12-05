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
using System;
using System.ComponentModel.Composition;

namespace CAS.SmartFactory.Shepherd.Client.Management
{
  [Export]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class ShellViewModel : StateMachineContext
  {
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
                
  }
}
