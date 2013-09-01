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
      private MainWindow m_Parent;
      public LocalMachine(MainWindow parent)
        : base()
      {
        m_Parent = parent;
      }
      #region StateMachineContext
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
      }
      internal override void WriteLine(string value)
      {
        m_Parent.WriteLine(value);
      }
      internal override void Exception(Exception exception)
      {
        string _mssg = String.Format("Program stoped by exception: {0}", exception.Message);
        MessageBox.Show(_mssg, "Operation error", MessageBoxButton.OK, MessageBoxImage.Error);
        Close();
      }
      internal override void EnteringState()
      {
      }
      #endregion
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      AssemblyName _name = Assembly.GetExecutingAssembly().GetName();
      this.Title = this.Title + String.Format(" Rel {0}.{1}.", _name.Version.Major, _name.Version.MajorRevision);
      this.ToolBarURLLabel.Content = Properties.Settings.Default.SiteURL;
      m_StateMAchine = new LocalMachine(this);
      m_StateMAchine.Machine.Next();
    }
    private LocalMachine m_StateMAchine;
    #region StateMachineContext View Model
    private void UpdateProgressBar(int progress)
    {
      while (x_ProgressBar.Maximum - x_ProgressBar.Value < progress)
        x_ProgressBar.Maximum *= 2;
      x_ProgressBar.Value += progress;
    }
    internal void WriteLine()
    {
      x_ListBox.Items.Add(string.Empty);
    }
    internal void WriteLine(string value)
    {
      x_ListBox.Items.Add(value);
    }
    #endregion
  }
}
