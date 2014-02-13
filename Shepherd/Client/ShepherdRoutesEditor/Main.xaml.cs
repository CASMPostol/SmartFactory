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
using CAS.SmartFactory.Shepherd.RouteEditor.InputData;
using CAS.SmartFactory.Shepherd.RouteEditor.UpdateData;
using Microsoft.SharePoint.Linq;
using Microsoft.Win32;

namespace CAS.SmartFactory.Shepherd.RouteEditor
{
  /// <summary>
  /// Interaction logic for Main.xaml
  /// </summary>
  public partial class Main : Window
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Main"/> class.
    /// </summary>
    public Main()
    {
      InitializeComponent();
      URLTextBox.Text = Properties.Settings.Default.URL;
    }
    /// <summary>
    /// Raises the <see cref="E:System.Windows.Window.Closing" /> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs" /> that contains the event data.</param>
    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
      base.OnClosing(e);
      Properties.Settings.Default.URL = URLTextBox.Text;
      Properties.Settings.Default.Save();
    }
    private void UpdateRoutesButton_Click(object sender, RoutedEventArgs e)
    {
      m_MainViewmodel.UpdateRoutes();
    }
    private void ReadXMLFileButton_Click(object sender, RoutedEventArgs e)
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
    private MainViewmodel m_MainViewmodel = new MainViewmodel();
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
      
      this.Title = String.Format("Shepherd Route Editor rel. {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
      this.UpdateLayout();
    }

    private void HelpButton_Click(object sender, RoutedEventArgs e)
    {
      var newWindow = new HelpWindow();
      newWindow.Show();
    }
  }
}
