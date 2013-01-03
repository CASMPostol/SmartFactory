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
          edc.GetDictionaries();
          ErrorList.Items.Add( "Imported data from current site" );
          OpenFileDialog _mofd = new OpenFileDialog()
          {
            CheckFileExists = true,
            CheckPathExists = true,
            Filter = "XML Documents|*.xml |All files |*.*"
          };
          _mofd.ShowDialog();
          RoutesCatalog _catalog = default( RoutesCatalog );
          using ( Stream _file = _mofd.OpenFile() )
          {
            _catalog = RoutesCatalog.ImportDocument( _file );
            if ( _catalog.CommodityTable != null )
              foreach ( var item in _catalog.CommodityTable )
                edc.AddCommodity( item );
            if ( _catalog.PartnersTable != null )
              foreach ( var item in _catalog.PartnersTable )
                edc.AddPartner( item, false );
            if ( _catalog.MarketTable != null )
              foreach ( var item in _catalog.MarketTable )
                edc.AddMarket( item );
            if ( _catalog.GlobalPricelist == null )
              foreach ( var item in _catalog.GlobalPricelist )
                edc.AddRoute( item, false );
          }
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
