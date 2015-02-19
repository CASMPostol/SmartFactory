using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Browser;

namespace CAS.SmartFactory.CW.Dashboards.GenerateSadConsignmentWebPart
{
  public partial class MainPage : UserControl
  {
    public MainPage()
    {
      InitializeComponent();
    }

    private void x_ButtonGenerateSad_Click(object sender, RoutedEventArgs e)
    {
      string _content = System.Windows.Browser.HttpUtility.HtmlDecode(GetVmlFromContent());
      SaveFileDialog textDialog;
      textDialog = new SaveFileDialog();
      textDialog.Filter = "Text Files | *.txt";
      textDialog.DefaultExt = "txt";
      bool? result = textDialog.ShowDialog();
      if (result == true)
      {
        System.IO.Stream fileStream = textDialog.OpenFile();
        System.IO.StreamWriter sw = new System.IO.StreamWriter(fileStream);
        sw.WriteLine("Writing some text in the file.");
        sw.Flush();
        sw.Close();
      }
    }

    private string GetVmlFromContent()
    {
      throw new NotImplementedException();
    }

  }
}
