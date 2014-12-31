//<summary>
//  Title   : ArchivingViewModel
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

using CAS.SmartFactory.Shepherd.Client.Management.StateMachines;
using Microsoft.Practices.Prism.Logging;
using System.ComponentModel.Composition;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Class ArchivingViewModel - it is view model for the <see cref="ArchivingMachineState{ArchivingViewModel}"/>
  /// </summary>
  [Export]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class ArchivingViewModel : ViewModelStateMachineBase<ArchivingViewModel.ArchivingMachineStateLocal>
  {
    /// <summary>
    /// Importing constructor that initializes a new instance of the <see cref="ArchivingViewModel"/> class. 
    /// The constructor is to be used by the composition infrastructure.
    /// </summary>
    /// <param name="loggingService">The logging service.</param>
    [ImportingConstructor]
    public ArchivingViewModel(ILoggerFacade loggingService)
      : base(loggingService)
    {
      loggingService.Log("Created ArchivingViewModel.", Category.Debug, Priority.Low);
    }
    public class ArchivingMachineStateLocal: ArchivingMachineState<ArchivingViewModel>
    {
      /// <summary>
      /// Logs the specified message.
      /// </summary>
      /// <param name="message">The message.</param>
      /// <param name="category">The category.</param>
      /// <param name="priority">The priority.</param>
      public override void Log(string message, Category category, Priority priority)
      {
        ViewModelContext.Log(message, category, priority);
      }
    }
  }
}
