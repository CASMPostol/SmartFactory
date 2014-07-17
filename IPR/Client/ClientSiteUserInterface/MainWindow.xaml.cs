//<summary>
//  Title   : MainWindow class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SmartFactory.IPR.Client.UserInterface.ViewModel;
using System.Windows;

namespace CAS.SmartFactory.IPR.Client.UserInterface
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }
    private MainWindowModel ThisDatacontext { get { return (MainWindowModel)this.DataContext; } }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      this.DataContext = new MainWindowModel();
    }
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      Properties.Settings.Default.Save();
      ThisDatacontext.Dispose();
      base.OnClosing(e);
    }
  }
}
