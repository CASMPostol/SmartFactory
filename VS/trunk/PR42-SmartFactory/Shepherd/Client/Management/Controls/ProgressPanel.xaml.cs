//<summary>
//  Title   : ProgressPanel
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

using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure;
using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure.Behaviors;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Interaction logic for ProgressPanel.xaml
  /// </summary>
  [ViewExport(RegionName = RegionNames.ProgressRegion)]
  public partial class ProgressPanel : UserControl
  {

    #region public
    public ProgressPanel()
    {
      InitializeComponent();
    }
    /// <summary>
    /// Sets the view model.
    /// </summary>
    /// <value>The view model.</value>
    [Import]
    public ProgressPanelViewModel ViewModel
    {
      set { this.DataContext = value; }
    }
    #endregion

    #region private
    private void HelpButton_Click(object sender, RoutedEventArgs e)
    {
      var newWindow = new HelpWindow();
      newWindow.Show();
    }
    #endregion

  }
}
