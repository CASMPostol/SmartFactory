﻿//<summary>
//  Title   : Name of Application
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


/// <summary>
/// The Infrastructure namespace.
/// </summary>
using Prism.Events;
namespace CAS.SmartFactory.Shepherd.Client.Management.Infrastructure
{
  
  /// <summary>
  /// Class MachineStateNameEvent - an event send every time the state machine enters new state.
  /// </summary>
  public class MachineStateNameEvent : PubSubEvent<string> { }

}
