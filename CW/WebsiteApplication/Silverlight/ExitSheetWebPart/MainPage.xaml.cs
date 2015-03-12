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

using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Printing;
using CAS.SmartFactory.CW.Dashboards.Webparts.ExitSheetHost;

namespace CAS.SmartFactory.CW.Dashboards.ExitSheetWebPart
{

  /// <summary>
  /// class MainPage 
  /// </summary>
  public partial class MainPage: UserControl
  {

    #region ctors
    public MainPage()
    {
      InitializeComponent();
      m_PrintDocument = new PrintDocument();
      m_PrintDocument.PrintPage += PrintDocument_PrintPageEventHandler;
    }
    public MainPage( string hiddenFieldDataName )
      : this()
    {
      HtmlDocument doc = HtmlPage.Document;
      m_HiddenField = doc.GetElementById( hiddenFieldDataName );
    }
    #endregion

    #region private

    #region vars
    private PrintDocument m_PrintDocument = null;
    private HtmlElement m_HiddenField = null;
    private string m_at;
    #endregion

    #region handlers
    private void UserControl_Loaded( object sender, RoutedEventArgs e )
    {
      try
      {
        m_at = "MainPage.UserControl_Loaded";
        if ( m_HiddenField == null )
          return;
        string message = m_HiddenField.GetAttribute( "value" );
        if ( ! String.IsNullOrEmpty( message ) )
         MainPageData = CAS.Common.Serialization.JsonSerialization.Deserialize<ExitSheeDataContract>( message );
      }
      catch ( Exception ex )
      {
        ExceptionHandling( ex );
      }
    }
    private void UserControl_Unloaded( object sender, RoutedEventArgs e )
    {
    }
    private void PrintDocument_PrintPageEventHandler( object sender, PrintPageEventArgs e )
    {
      e.PageVisual = x_GridToBePrinted;
      e.HasMorePages = false;
    }
    private void x_ButtonPrint_Click( object sender, RoutedEventArgs e )
    {
      try
      {
        m_at = "MainPage.x_ButtonPrint_Click";
        m_PrintDocument.Print( "Exit Sheet" );
      }
      catch ( System.Exception ex )
      {
        MessageBox.Show( ex.Message, "Print Exception", MessageBoxButton.OK );
      }
    }
    #endregion

    private void ExceptionHandling( Exception ex )
    {
      MessageBox.Show( ex.Message + " AT: " + m_at, "Loaded event error", MessageBoxButton.OK );
    }
    private ExitSheeDataContract MainPageData
    {
      get { return ( (ExitSheeDataContract)x_GridToBePrinted.DataContext ); }
      set { x_GridToBePrinted.DataContext = value; this.UpdateLayout(); }
    }

    #endregion

  }

}
