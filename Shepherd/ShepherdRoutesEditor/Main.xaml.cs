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
        using ( UpdateData.EntitiesDataContext edc = new UpdateData.EntitiesDataContext( this.URLTextBox.Text ) )
        {
          EntityList<UpdateData.CityType> _city = edc.City;
          EntityList<UpdateData.CountryType> _Countr = edc.Country;
          foreach ( CountryType _countre in _Countr )
          {
            CityType _newCity = new CityType()
            {
              CountryTitle = _countre,
              Tytuł = "New City"
            };
            _city.InsertOnSubmit( _newCity );
          }
          EntityList<UpdateData.Currency> _curr = edc.Currency;
          EntityList<UpdateData.Currency> _curr2 = edc.Currency;
          if ( _curr != _curr2 )
            ErrorList.Items.Add( "Entities are not equal" );
          foreach ( var item in _curr )
          {
            ErrorList.Items.Add( String.Format( "Title={0}, ID = {1}, Rate={2}.", item.Tytuł, item.Identyfikator, item.ExchangeRate ) );
            item.ExchangeRate = m_RandomValue.NextDouble();
          }
          this.UpdateLayout();
          Currency _newItem = new Currency()
          {
            ExchangeRate = m_RandomValue.NextDouble(),
            Tytuł = "New test Item"
          };
          _curr.InsertOnSubmit( _newItem );
          edc.SubmitChanges();
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
