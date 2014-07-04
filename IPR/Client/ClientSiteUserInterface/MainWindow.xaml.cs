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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CAS.SmartFactory.IPR.Client.UserInterface.StateMachine;

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
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      AssemblyName _name = Assembly.GetExecutingAssembly().GetName();
      this.Title = this.Title + " Rel " + _name.Version.ToString(4);
      //x_TabControlContent.Items.Clear();
      AbstractMachine.SetupDataDialogMachine.Get().Entered += SetupDataDialogMachine_Entered;
      AbstractMachine.SetupDataDialogMachine.Get().Exiting += SetupDataDialogMachine_Exiting;
    }
    private void SetupDataDialogMachine_Exiting(object sender, EventArgs e)
    {
      //Properties.Settings.Default.DoActivate1800 = x_CheckBoxActivateRel182.IsChecked.GetValueOrDefault(false);
      //Properties.Settings.Default.DoArchiveIPR = x_CheckBoxArchiveIPRAccounts.IsChecked.GetValueOrDefault(false);
      //Properties.Settings.Default.DoArchiveBatch = x_CheckBoxArchiveBatch.IsChecked.GetValueOrDefault(false);
      //Properties.Settings.Default.ArchiveIPRDelay = int.Parse(x_TextBoxIPRAccountArchivalDelay.Text);
      //Properties.Settings.Default.ArchiveBatchDelay = int.Parse(x_TextBoxBatchArchivalDelay.Text);
      //Properties.Settings.Default.SiteURL = x_TextBoxURL.Text;
      Properties.Settings.Default.Save();
      x_ToolBarURLLabel.Content = Properties.Settings.Default.SiteURL;
      //x_TabControlContent.Items.Clear();
      //x_TabControlContent.Items.Add(x_TabItemMonitorListBox);
      //x_TextBoxURL.Text = Properties.Settings.Default.SiteURL;
      //x_TabItemMonitorListBox.Focus();
    }
    private void SetupDataDialogMachine_Entered(object sender, EventArgs e)
    {
      //x_TabControlContent.Items.Clear();
      //x_TabControlContent.Items.Add(x_TabItemSetupDialog);
      //x_TextBoxURL.Text = Properties.Settings.Default.SiteURL;
      //x_CheckBoxActivateRel182.IsChecked = Properties.Settings.Default.DoActivate1800;
      //x_CheckBoxArchiveIPRAccounts.IsChecked = Properties.Settings.Default.DoArchiveIPR;
      //x_CheckBoxArchiveBatch.IsChecked = Properties.Settings.Default.DoArchiveBatch;
      //x_TextBoxIPRAccountArchivalDelay.Text = Properties.Settings.Default.ArchiveIPRDelay.ToString();
      //x_TextBoxBatchArchivalDelay.Text = Properties.Settings.Default.ArchiveBatchDelay.ToString();
      //x_TabItemSetupDialog.Focus();
    }

    #region StateMachineContext View Model
    private void WriteLine()
    {
      x_ListBox.Items.Add(string.Empty);
    }
    private void WriteLine(string value)
    {
      x_ListBox.Items.Add(value);
    }
    #endregion

  }
}
