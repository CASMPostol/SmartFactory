﻿using System;
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
  public partial class Main: Window
  {
    public Main()
    {
      InitializeComponent();
      URLTextBox.Text = Properties.Settings.Default.URL;
    }
    private Random m_RandomValue = new Random( 12345 );
    private void UpdateRoutesButton_Click( object sender, RoutedEventArgs e )
    {
      try
      {
        using ( EntitiesDataDictionary edc = new EntitiesDataDictionary( this.URLTextBox.Text ) )
        {
          ErrorList.Items.Add( "Starting read current data from selected site." );
          edc.ReadSiteContent();
          ErrorList.Items.Add( "Data from current site has been read" );
          OpenFileDialog _mofd = new OpenFileDialog()
          {
            CheckFileExists = true,
            CheckPathExists = true,
            Filter = "XML Documents|*.XML|XML Documents|*.xml|All files |*.*",
            DefaultExt = ".xml",
            AddExtension = true,
          };
          if ( !_mofd.ShowDialog().GetValueOrDefault( false ) )
          {
            ErrorList.Items.Add( "Operation aborted by the user." );
            return;
          }
          RoutesCatalog _catalog = default( RoutesCatalog );
          using ( Stream _file = _mofd.OpenFile() )
            _catalog = RoutesCatalog.ImportDocument( _file );
          ErrorList.Items.Add( "Starting data ipdate" );
          edc.ImportTable( _catalog.CommodityTable );
          ErrorList.Items.Add( "Commodity updated." );
          edc.ImportTable( _catalog.PartnersTable, false );
          ErrorList.Items.Add( "Partners updated." );
          edc.ImportTable( _catalog.MarketTable );
          ErrorList.Items.Add( "Market updated." );
          edc.ImportTable( _catalog.GlobalPricelist, false );
          edc.SubmitChages();
          ErrorList.Items.Add( "GlobalPricelist updated." );
        }
      }
      catch ( Exception _ex )
      {
        MessageBox.Show( _ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error );
        ErrorList.Items.Add( String.Format( "Catched Exception {0}.", _ex.Message ) );
        this.UpdateLayout();
      }
    }
  }
}
