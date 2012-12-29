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

    private void UpdateRoutesButton_Click( object sender, RoutedEventArgs e )
    {
      try
      {
        using ( UpdateData.EntitiesDataContext edc = new UpdateData.EntitiesDataContext( this.URLTextBox.Text ) )
        {
          Microsoft.SharePoint.Linq.EntityList<UpdateData.Currency> _curr = edc.Currency;
          Microsoft.SharePoint.Linq.EntityList<UpdateData.Currency> _curr2 = edc.Currency;
          Currency _newItem = new Currency()
          {
            ExchangeRate = 321.0,
            Tytuł = "New test Item"
          };
          _curr.InsertOnSubmit( _newItem );
          edc.SubmitChanges();

        }
      }
      catch ( Exception ex )
      {
        MessageBox.Show( ex.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error );
      }
    }
  }
}
