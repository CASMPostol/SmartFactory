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

using CAS.SharePoint.Interactivity.InteractionRequest;
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
      ThisDatacontext.CloseWindow.Raised += CloseWindow_Raised;
      ThisDatacontext.CancelConfirmation.Raised += CancelConfirmation_Raised;
      ThisDatacontext.ExceptionNotification.Raised += ExceptionNotification_Raised;
    }
    private void ExceptionNotification_Raised(object sender, InteractionRequestedEventArgs<INotification> e)
    {
      MessageBox.Show((string)e.Context.Content, e.Context.Title, MessageBoxButton.OK, MessageBoxImage.Error);
      e.Callback();
    }
    private void CancelConfirmation_Raised(object sender, InteractionRequestedEventArgs<IConfirmation> e)
    {
      if (MessageBox.Show((string)e.Context.Content, e.Context.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
        e.Context.Confirmed = true;
      else
        e.Context.Confirmed = false;
      e.Callback();
    }
    private void CloseWindow_Raised(object sender, InteractionRequestedEventArgs<INotification> e)
    {
      e.Callback();
      this.Close();
    }
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      Properties.Settings.Default.Save();
      ThisDatacontext.Dispose();
      base.OnClosing(e);
    }
  }
}
