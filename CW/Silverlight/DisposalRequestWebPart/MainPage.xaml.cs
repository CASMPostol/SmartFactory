using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DisposalRequestWebPart
{
  public partial class MainPage : UserControl
  {
    public MainPage()
    {
      InitializeComponent();
    }

    public MainPage( string HiddenFieldDataName )
      : this()
    {
      m_HiddenFieldDataName = HiddenFieldDataName;
    }

    private void UserControl_Loaded( object sender, RoutedEventArgs e )
    {
      try
      {
        HtmlDocument doc = HtmlPage.Document;
        HtmlElement hiddenField = doc.GetElementById( m_HiddenFieldDataName );
        string _hiddenFieldvalue = hiddenField.GetAttribute( "value" ).ToString();
        h_HiddenFieldDataName.Text = _hiddenFieldvalue;
      }
      catch ( Exception ex)
      {
        MessageBox.Show( ex.Message, "Loaded error", MessageBoxButton.OK );
      }
    }
    private readonly string m_HiddenFieldDataName = String.Empty;

  }
}
