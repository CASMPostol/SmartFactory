//<summary>
//  Title   : Name of Application
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
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart
{
  /// <summary>
  /// Main page UserControl
  /// </summary>
  public partial class MainPage : UserControl
  {
    #region public
    public MainPage()
    {
      InitializeComponent();
    }
    public MainPage(string hiddenFieldDataName)
      : this()
    {
      m_at = "creator";
      m_HiddenFieldDataName = hiddenFieldDataName;
      HtmlDocument doc = HtmlPage.Document;
      HtmlElement hiddenField = doc.GetElementById( hiddenFieldDataName );
      string message = hiddenField.GetAttribute( "value" ).ToString();
      int _id = 0;
      if ( Int32.TryParse( message, out _id ) )
        m_SelectedID = _id;
    }
    #endregion

    #region private

    #region private vars
    private int? m_SelectedID = new Nullable<int>();
    private readonly string m_HiddenFieldDataName = String.Empty;
    private string m_at;
    #endregion

    private void ExceptionHandling(Exception ex)
    {
      MessageBox.Show(ex.Message + " AT: " + m_at, "Loaded event error", MessageBoxButton.OK);
    }

    #region event handlers
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        ClientContext _ClientContext = ClientContext.Current;
        if (_ClientContext == null)
          throw new ArgumentNullException("clientContext", String.Format("Cannot get the {0} ", "ClientContext"));
        MainPageData.MainPageDataInstance.GetData( _ClientContext.Url, m_SelectedID );
      }
      catch (Exception ex)
      {
        ExceptionHandling(ex);
      }
    }
    private void x_ButtonAddNew_Click(object sender, RoutedEventArgs e)
    {
      try
      {

      }
      catch (Exception _ex)
      {
        ExceptionHandling(_ex);
      }
    }
    private void x_ButtonEndofBatch_Click(object sender, RoutedEventArgs e)
    {
      try
      {

      }
      catch (Exception _ex)
      {
        ExceptionHandling(_ex);
      }

    }
    private void x_ButtonDelete_Click(object sender, RoutedEventArgs e)
    {
      try
      {

      }
      catch (Exception _ex)
      {
        ExceptionHandling(_ex);
      }
    }
    private void x_ButtonSave_Click(object sender, RoutedEventArgs e)
    {
      MainPageData.MainPageDataInstance.SubmitChanges();
    }
    private void x_ButtonCancel_Click( object sender, RoutedEventArgs e )
    {
      MainPageData.MainPageDataInstance.ReloadData();
    }
    #endregion

    #endregion

  }
}
