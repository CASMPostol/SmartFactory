//<summary>
//  Title   : class MainPage 
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Windows;
using System.Windows.Controls;
using System.Windows.Printing;

namespace CAS.SmartFactory.CW.Dashboards.ExitSheetWebPart
{

  /// <summary>
  /// class MainPage 
  /// </summary>
  public partial class MainPage: UserControl
  {
    public MainPage()
    {
      InitializeComponent();
      m_PrintDocument = new PrintDocument();
      m_PrintDocument.PrintPage += PrintDocument_PrintPageEventHandler;
    }

    #region private
    private PrintDocument m_PrintDocument = null;

    #region handlers
    private void PrintDocument_PrintPageEventHandler( object sender, PrintPageEventArgs e )
    {
      e.PageVisual = x_GridToBePrinted;
      e.HasMorePages = false;
    }
    private void x_ButtonPrint_Click( object sender, RoutedEventArgs e )
    {
      try
      {
        m_PrintDocument.Print( "Exit Sheet" );
      }
      catch ( System.Exception ex )
      {
        MessageBox.Show( ex.Message, "Print Exception", MessageBoxButton.OK );
      }
    }
    #endregion

    #endregion
  }
}
