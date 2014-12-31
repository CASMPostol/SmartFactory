//<summary>
//  Title   : ArchivingPanel
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
      
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Interaction logic for ArchivingPanel.xaml
  /// </summary>
  [Export(Infrastructure.ViewNames.ArchivalStateName)]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public partial class ArchivingPanel : UserControl
  {
    public ArchivingPanel()
    {
      InitializeComponent();
    }
    #region Imports
    /// Sets the ViewModel.
    /// </summary>
    /// <remarks>
    /// This set-only property is annotated with the <see cref="ImportAttribute"/> so it is injected by MEF with
    /// the appropriate view model.
    /// </remarks>
    [Import]
    public ArchivingViewModel StateMachineContext
    {
      set
      {
        DataContext = value;
      }
    }
    #endregion

  }
}
