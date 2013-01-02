using System;
using System.Collections.Generic;
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
using CAS.SmartFactory.Shepherd.RouteEditor.UpdateData;
using Microsoft.SharePoint.Linq;

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
