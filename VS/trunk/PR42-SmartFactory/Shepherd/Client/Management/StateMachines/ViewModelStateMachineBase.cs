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
using System.ComponentModel.Composition;

namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{
  public class ViewModelStateMachineBase<StateType> : ViewModelBase<StateType>
    where StateType : IAbstractMachineState, new()
  {

    /// <summary>
    /// Sets the master shell view model. It activates the state <typeparamref name="StateType"/>
    /// </summary>
    /// <value>The master shell view model <see cref="ShellViewModel" />.</value>
    [Import]
    public ShellViewModel MasterShellViewModel
    {
      set
      {
        base.EnterState(value);
      }
    }

  }
}
