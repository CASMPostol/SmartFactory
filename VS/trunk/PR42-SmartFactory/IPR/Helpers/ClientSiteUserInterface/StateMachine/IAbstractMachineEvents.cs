﻿//<summary>
//  Title   : IAbstractMachineEvents
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;
using System.IO;
using System.IO.Packaging;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  /// <summary>
  /// Abstract Machine Events Interface
  /// </summary>
  interface IAbstractMachineEvents
  {
    /// <summary>
    /// Cancels this instance.
    /// </summary>
    void Cancel();
    /// <summary>
    /// Go to the next step.
    /// </summary>
    void Next();
    /// <summary>
    /// Go to previous step.
    /// </summary>
    void Previous();
    /// <summary>
    ///  Starts execution of a background operation.
    /// </summary>
    /// <exception cref="System.InvalidOperationException">Background operation is in progress</exception>
    void RunAsync();
  }
}
