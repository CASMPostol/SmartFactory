//<summary>
//  Title   : LogPanel 
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure;
using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure.Behaviors;
using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Interaction logic for LogPanel.xaml view
  /// </summary>
  [ViewExport(RegionName = RegionNames.DiagnosticRegion)]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public partial class LogPanel : UserControl
  {
    public LogPanel()
    {
      InitializeComponent();
    }    /// <summary>
    /// Sets the ViewModel.
    /// </summary>
    /// <remarks>
    /// This set-only property is annotated with the <see cref="ImportAttribute"/> so it is injected by MEF with
    /// the appropriate view model.
    /// </remarks>
    [Import]
    public LogPanelViewModel ViewModel
    {
      get { return (LogPanelViewModel)DataContext; }
      set
      {
        DataContext = value;
      }
    }
  }
}
