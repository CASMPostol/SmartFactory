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
  public partial class MainPage: UserControl
  {
    #region public
    public MainPage()
    {
      InitializeComponent();
    }
    public MainPage( string hiddenFieldDataName )
      : this()
    {
      m_at = "creator";
      m_HiddenFieldDataName = hiddenFieldDataName;
    }
    #endregion

    private void UserControl_Loaded( object sender, RoutedEventArgs e )
    {
      try
      {
        this.x_LabelHeader.Content += "Test 1";
        GetData();
      }
      catch ( Exception ex )
      {
        MessageBox.Show( ex.Message + " AT: " + m_at, "Loaded event error", MessageBoxButton.OK );
      }
    }
    private void GetData()
    {

      //if ( String.IsNullOrEmpty( m_HiddenFieldDataName ) )
      //  throw new ArgumentNullException( "Disposal Request", "Is not selected." );
      //HtmlDocument doc = HtmlPage.Document;
      //HtmlElement hiddenField = doc.GetElementById( m_HiddenFieldDataName );
      //string _viewXml = hiddenField.GetAttribute( "value" ).ToString();
      //CamlQuery _camlQuery = new CamlQuery() { ViewXml = _viewXml };
      m_at = "GetData";
      ClientContext clientContext = ClientContext.Current;
      if ( clientContext == null )
        throw new ArgumentNullException( "clientContext", String.Format( "Cannot get the {0} ", "ClientContext" ) );
      m_at = "clientContext";
      Web _website = clientContext.Web;
      if ( _website == null )
        throw new ArgumentNullException( "_cwList", String.Format( "Cannot get the {0} ", "Web" ) );
      m_at = "GetByTitle";
      List _cwList = _website.Lists.GetByTitle( CommonDefinition.CustomsWarehouseDisposalTitle );
      if ( _cwList == null )
        throw new ArgumentNullException( "_cwList", String.Format( "Cannot get the {0} list", CommonDefinition.CustomsWarehouseDisposalTitle ) );
      m_at = "Load cwList ";
      ListItemCollection _itemsCollection = _cwList.GetItems( CamlQuery.CreateAllItemsQuery() );
      m_at = "GetItems";
      clientContext.Load( _cwList );
      m_at = "Load itemsCollection ";
      clientContext.Load( _itemsCollection );
      m_at = "ExecuteQuery";
      clientContext.ExecuteQueryAsync(
        ( x, y ) =>
        {
          Deployment.Current.Dispatcher.BeginInvoke( () => x_DataGridListView.ItemsSource = _itemsCollection );
          x_LabelHeader.Content = x_DataGridListView.ItemsSource == null? "ItemsSource is null" :  String.Format("{} items have been read.", x_DataGridListView. );
        },
        ( x, y ) =>
        {
          Deployment.Current.Dispatcher.BeginInvoke( () => MessageBox.Show( y.Message + " AT: ExecuteQueryAsync", "Loaded event error", MessageBoxButton.OK ) );
        } );
    }
    private readonly string m_HiddenFieldDataName = String.Empty;
    private string m_at;

  }
}
