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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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
      m_HiddenFieldDataName = hiddenFieldDataName;
    }
    #endregion

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        GetData();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Loaded error", MessageBoxButton.OK);
      }
    }
    private void GetData()
    {
      this.Dispatcher.BeginInvoke(
        () =>
        {
          //if ( String.IsNullOrEmpty( m_HiddenFieldDataName ) )
          //  throw new ArgumentNullException( "Disposal Request", "Is not selected." );
          //HtmlDocument doc = HtmlPage.Document;
          //HtmlElement hiddenField = doc.GetElementById( m_HiddenFieldDataName );
          //string _viewXml = hiddenField.GetAttribute( "value" ).ToString();
          //CamlQuery _camlQuery = new CamlQuery() { ViewXml = _viewXml };
          ClientContext clientContext = ClientContext.Current;
          if (clientContext == null)
            throw new ArgumentNullException("clientContext", String.Format("Cannot get the {0} ", "ClientContext"));
          Web _website = clientContext.Web;
          if (_website == null)
            throw new ArgumentNullException("_cwList", String.Format("Cannot get the {0} ", "Web"));
          List _cwList = _website.Lists.GetByTitle(CommonDefinition.CustomsWarehouseDisposalTitle);
          if (_cwList == null)
            throw new ArgumentNullException("_cwList", String.Format("Cannot get the {0} list", CommonDefinition.CustomsWarehouseDisposalTitle));
          ListItemCollection _itemsCollection = _cwList.GetItems(CamlQuery.CreateAllItemsQuery());
          clientContext.Load(_cwList);
          clientContext.Load(_itemsCollection);
          clientContext.ExecuteQuery();
          //x_DataGridListView.ItemsSource = _itemsCollection;
        });
    }
    private readonly string m_HiddenFieldDataName = String.Empty;

  }
}
