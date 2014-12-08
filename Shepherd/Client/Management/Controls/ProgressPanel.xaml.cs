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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Interaction logic for ProgressPanel.xaml
  /// </summary>
  public partial class ProgressPanel : UserControl
  {
    public ProgressPanel()
    {
      InitializeComponent();
    }
    private void HelpButton_Click(object sender, RoutedEventArgs e)
    {
      var newWindow = new HelpWindow();
      newWindow.Show();
    }
  }
}
