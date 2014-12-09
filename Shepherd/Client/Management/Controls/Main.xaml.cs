//<summary>
//  Title   : class Main 
//  System  : Microsoft Visual C# .NET 2012
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel.Composition;
using Microsoft.SharePoint.Linq;
using Microsoft.Win32;

namespace  CAS.SmartFactory.Shepherd.Client.Management
{
  /// <summary>
  /// Interaction logic for Main.xaml
  /// </summary>
  public partial class Main : UserControl
  {    
    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public Main()
    {
      InitializeComponent();
    }
    private MainViewmodel m_MainViewmodel = null;
    /// <summary>
    /// Raises the <see cref="E:System.Windows.Window.Closing" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
    //protected override void OnClosing(System.ComponentModel.CancelEventArgs e) //TODO mp
    //{
    //  if (m_MainViewmodel.Connected)
    //    if (MessageBox.Show("You are about to close application. All changes will be lost.", "Closing application", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.Cancel)
    //    {
    //      e.Cancel = true;
    //      return;
    //    }
    //  m_MainViewmodel.Dispose();
    //  m_MainViewmodel = null;
    //  base.OnClosing(e);
    //}
    /// <summary>
    /// Handles the Loaded event of the Window control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      //this.Title = String.Format("Shepherd Route Editor rel. {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()); TODO mp
      this.m_MainViewmodel = new MainViewmodel();
      this.x_MainGrid.DataContext = m_MainViewmodel;
      this.UpdateLayout();
    }
    private void HelpButton_Click(object sender, RoutedEventArgs e)
    {
      var newWindow = new HelpWindow();
      newWindow.Show();
    }
    private void x_ConnectButton_Click(object sender, RoutedEventArgs e)
    {
      if (m_MainViewmodel.Connected)
        if (MessageBox.Show("You are about to reestablish connection. All changes will be lost.", "Closing application", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.Cancel)
          return;
      m_MainViewmodel.ReadSiteContent();
    }
    private void UpdateRoutesButton_Click(object sender, RoutedEventArgs e)
    {
      m_MainViewmodel.UpdateRoutes();
    }
    private void x_ImportXMLButton_Click(object sender, RoutedEventArgs e)
    {
      OpenFileDialog _mofd = new OpenFileDialog()
      {
        CheckFileExists = true,
        CheckPathExists = true,
        Filter = "XML Documents|*.XML|XML Documents|*.xml|All files |*.*",
        DefaultExt = ".xml",
        AddExtension = true,
      };
      if (!_mofd.ShowDialog().GetValueOrDefault(false))
      {
        ErrorList.Items.Add("Operation aborted by the user.");
        return;
      }
      m_MainViewmodel.ReadXMLFile(_mofd.FileName);
    }
  }
}
