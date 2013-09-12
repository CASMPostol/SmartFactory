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
    private class LocalMachine : StateMachine.StateMachineContext
    {

      #region ctor
      public LocalMachine(MainWindow parent)
        : base()
      {
        m_Parent = parent;
      }
      #endregion

      #region StateMachineContext
      internal override void SetupUserInterface(StateMachine.Events allowedEvents)
      {
        m_Parent.x_ButtonCancel.IsEnabled = (allowedEvents & StateMachine.Events.Cancel) != 0;
        m_Parent.x_ButtonGoBackward.IsEnabled = (allowedEvents & StateMachine.Events.Previous) != 0;
        m_Parent.x_ButtonGoForward.IsEnabled = (allowedEvents & StateMachine.Events.Next) != 0;
        m_Parent.x_ButtonRun.IsEnabled = (allowedEvents & StateMachine.Events.RunAsync) != 0;
      }
      internal override void Close()
      {
        m_Parent.Close();
      }
      internal override void Progress(int progress)
      {
        m_Parent.UpdateProgressBar(progress);
      }
      internal override void WriteLine()
      {
        m_Parent.WriteLine();
        m_Parent.UpdateProgressBar(1);
      }
      internal override void WriteLine(string value)
      {
        m_Parent.WriteLine(value);
        m_Parent.UpdateProgressBar(1);
      }
      internal override void Exception(Exception exception)
      {
        string _mssg = String.Format("Program stoped by exception: {0}", exception.Message);
        MessageBox.Show(_mssg, "Operation error", MessageBoxButton.OK, MessageBoxImage.Error);
        //Close();
      }
      internal override void EnteringState()
      {
      }
      #endregion

      #region private
      private MainWindow m_Parent;
      #endregion
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      AssemblyName _name = Assembly.GetExecutingAssembly().GetName();
      this.Title = this.Title + " Rel " + _name.Version.ToString(4);
      m_StateMAchine = new LocalMachine(this);
      x_ButtonCancel.Click += m_StateMAchine.ButtonCancel_Click;
      x_ButtonGoBackward.Click += m_StateMAchine.ButtonGoBackward_Click;
      x_ButtonGoForward.Click += m_StateMAchine.ButtonGoForward_Click;
      x_ButtonRun.Click += m_StateMAchine.ButtonRun_Click;
      x_TabControlContent.Items.Clear();
      AbstractMachine.SetupDataDialogMachine.Get().Entered += SetupDataDialogMachine_Entered;
      AbstractMachine.SetupDataDialogMachine.Get().Exiting += SetupDataDialogMachine_Exiting;
      m_StateMAchine.OpenEntryState();
    }
    private void SetupDataDialogMachine_Exiting(object sender, EventArgs e)
    {
      Properties.Settings.Default.DoActivate1800 = x_CheckBoxActivateRel182.IsChecked.GetValueOrDefault(false);
      Properties.Settings.Default.DoArchiveIPR = x_CheckBoxArchiveIPRAccounts.IsChecked.GetValueOrDefault(false);
      Properties.Settings.Default.DoArchiveBatch = x_CheckBoxArchiveBatch.IsChecked.GetValueOrDefault(false);
      Properties.Settings.Default.ArchiveIPRDelay = int.Parse(x_TextBoxIPRAccountArchivalDelay.Text);
      Properties.Settings.Default.ArchiveBatchDelay = int.Parse(x_TextBoxBatchArchivalDelay.Text);
      Properties.Settings.Default.Save();
      x_ToolBarURLLabel.Content = Properties.Settings.Default.SiteURL;
      x_TabControlContent.Items.Clear();
      x_TabControlContent.Items.Add(x_TabItemMonitorListBox);
      x_TextBoxURL.Text = Properties.Settings.Default.SiteURL;
      x_TabItemMonitorListBox.Focus();
    }
    private void SetupDataDialogMachine_Entered(object sender, EventArgs e)
    {
      x_TabControlContent.Items.Clear();
      x_TabControlContent.Items.Add(x_TabItemSetupDialog);
      x_TextBoxURL.Text = Properties.Settings.Default.SiteURL;
      x_CheckBoxActivateRel182.IsChecked = Properties.Settings.Default.DoActivate1800;
      x_CheckBoxArchiveIPRAccounts.IsChecked = Properties.Settings.Default.DoArchiveIPR;
      x_CheckBoxArchiveBatch.IsChecked = Properties.Settings.Default.DoArchiveBatch;
      x_TextBoxIPRAccountArchivalDelay.Text = Properties.Settings.Default.ArchiveIPRDelay.ToString();
      x_TextBoxBatchArchivalDelay.Text = Properties.Settings.Default.ArchiveBatchDelay.ToString();
      x_TabItemSetupDialog.Focus();
    }
    private LocalMachine m_StateMAchine;

    #region StateMachineContext View Model
    bool m_UpdateProgressBarBusy = false;
    private void UpdateProgressBar(int progress)
    {
      if (m_UpdateProgressBarBusy)
        return;
      m_UpdateProgressBarBusy = true;
      while (x_ProgressBar.Maximum - x_ProgressBar.Value < progress)
        x_ProgressBar.Maximum *= 2;
      x_ProgressBar.Value += progress;
      m_UpdateProgressBarBusy = false;
    }
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
