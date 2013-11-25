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
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.ExitSheetWebPart
{

  /// <summary>
  /// class MainPage 
  /// </summary>
  public partial class MainPage : UserControl
  {

    #region ctors
    public MainPage()
    {
      InitializeComponent();
      m_PrintDocument = new PrintDocument();
      m_PrintDocument.PrintPage += PrintDocument_PrintPageEventHandler;
    }
    public MainPage(string hiddenFieldDataName)
      : this()
    {
      HtmlDocument doc = HtmlPage.Document;
      HtmlElement hiddenField = doc.GetElementById(hiddenFieldDataName);
      string message = hiddenField.GetAttribute("value");
      int _id = 0;
      if (Int32.TryParse(message, out _id))
        m_SelectedID = _id;
    }
    #endregion

    #region private

    #region vars
    private PrintDocument m_PrintDocument = null;
    private int? m_SelectedID = new Nullable<int>();
    private string m_at;
    private string m_URL = string.Empty;
    #endregion

    #region handlers
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        ClientContext _ClientContext = ClientContext.Current;
        if (_ClientContext == null)
          throw new ArgumentNullException("clientContext", String.Format("Cannot get the {0} ", "ClientContext"));
        m_URL = _ClientContext.Url;
        this.MainPageData.GetData(m_URL, m_SelectedID);
      }
      catch (Exception ex)
      {
        ExceptionHandling(ex);
      }
    }
    private void UserControl_Unloaded(object sender, RoutedEventArgs e)
    {
      DisposeMainPageData();
    }
    private void PrintDocument_PrintPageEventHandler(object sender, PrintPageEventArgs e)
    {
      e.PageVisual = x_GridToBePrinted;
      e.HasMorePages = false;
    }
    private void x_ButtonPrint_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        m_PrintDocument.Print("Exit Sheet");
      }
      catch (System.Exception ex)
      {
        MessageBox.Show(ex.Message, "Print Exception", MessageBoxButton.OK);
      }
    }
    #endregion

    private void ExceptionHandling(Exception ex)
    {
      MessageBox.Show(ex.Message + " AT: " + m_at, "Loaded event error", MessageBoxButton.OK);
    }
    private MainPageData MainPageData
    {
      get { return ((MainPageData)x_GridToBePrinted.DataContext); }
      set { x_GridToBePrinted.DataContext = value; this.UpdateLayout(); }
    }
    private void DisposeMainPageData()
    {
      if (this.MainPageData == null)
        return;
      MainPageData _MainPageData = MainPageData;
      MainPageData = null;
      _MainPageData.Dispose();
    }

    #endregion

  }

}
