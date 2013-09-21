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
      m_HiddenFieldDataName = hiddenFieldDataName;
    }
    #endregion

    private void UserControl_Loaded( object sender, RoutedEventArgs e )
    {
      try
      {
        GetData();
      }
      catch ( Exception ex )
      {
        MessageBox.Show( ex.Message, "Loaded error", MessageBoxButton.OK );
      }
    }
    private void GetData()
    {
      if ( String.IsNullOrEmpty( m_HiddenFieldDataName ) )
        throw new ArgumentNullException( "Disposal Request", "Is not selected." );
      HtmlDocument doc = HtmlPage.Document;
      HtmlElement hiddenField = doc.GetElementById( m_HiddenFieldDataName );
      string _viewXml = hiddenField.GetAttribute( "value" ).ToString();
      ClientContext clientContext = ClientContext.Current;
      Web _website = clientContext.Web;
      List _cwList = _website.Lists.GetByTitle( CommonDefinition.CustomsWarehouseDisposalTitle );
      CamlQuery _camlQuery = new CamlQuery() { ViewXml = _viewXml };
      ListItemCollection _itemsCollection = _cwList.GetItems( _camlQuery );
      clientContext.Load( _itemsCollection );
      clientContext.ExecuteQuery();
    }
    private readonly string m_HiddenFieldDataName = String.Empty;

  }
}
