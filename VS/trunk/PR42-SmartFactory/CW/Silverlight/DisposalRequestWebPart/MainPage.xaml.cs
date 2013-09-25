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
      m_ClientContext = ClientContext.Current;
    }
    public MainPage(string hiddenFieldDataName)
      : this()
    {
      m_at = "creator";
      m_HiddenFieldDataName = hiddenFieldDataName;
    }
    #endregion

    #region private

    #region private vars
    private readonly string m_HiddenFieldDataName = String.Empty;
    private string m_at;
    private ClientContext m_ClientContext = null;
    private Dictionary<int, ListItem> m_ItemsEdited = new Dictionary<int, ListItem>();
    private ListItemCollection m_ItemsCollection = null;
    private bool b_Edited = false;
    #endregion

    private bool Edited
    {
      get { return b_Edited; }
      set
      {
        b_Edited = value;
        UpdateHeader();
      }
    }
    private void UpdateHeader()
    {
      string _pattern = "Disposal request content: {0} items; {1}";
      int _items = 0;
      string _star = Edited ? "*" : " ";
      if (m_ItemsCollection == null)
        _items = m_ItemsCollection.Count;
      x_LabelHeader.Content = String.Format(_pattern, _items, _star);
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
      if (m_ClientContext == null)
        throw new ArgumentNullException("clientContext", String.Format("Cannot get the {0} ", "ClientContext"));
      m_at = "clientContext";
      Web _website = m_ClientContext.Web;
      if (_website == null)
        throw new ArgumentNullException("_cwList", String.Format("Cannot get the {0} ", "Web"));
      m_at = "GetByTitle";
      List _cwList = _website.Lists.GetByTitle(CommonDefinition.CustomsWarehouseDisposalTitle);
      if (_cwList == null)
        throw new ArgumentNullException("_cwList", String.Format("Cannot get the {0} list", CommonDefinition.CustomsWarehouseDisposalTitle));
      m_at = "Load cwList ";
      m_ItemsCollection = _cwList.GetItems(CamlQuery.CreateAllItemsQuery());
      m_at = "GetItems";
      m_ClientContext.Load(_cwList);
      m_at = "Load itemsCollection ";
      m_ClientContext.Load(m_ItemsCollection);
      m_at = "ExecuteQuery";
      m_ClientContext.ExecuteQueryAsync(
        (x, y) =>
        {
          Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
              x_DataGridListView.ItemsSource = m_ItemsCollection;
              x_LabelHeader.Content = x_DataGridListView.ItemsSource == null ? "ItemsSource is null" : String.Format("{0} items have been read.", m_ItemsCollection.Count);
            });
        },
        (x, y) =>
        {
          Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show(y.Message + " AT: ExecuteQueryAsync", "Loaded event error", MessageBoxButton.OK));
        });
    }

    #region event handlers
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        GetData();
        UpdateHeader();
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message + " AT: " + m_at, "Loaded event error", MessageBoxButton.OK);
      }
    }
    private void x_DataGridListView_CellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
    {
      ListItem _selectedItem = (ListItem)x_DataGridListView.SelectedItem;
      int _Id = (int)_selectedItem[CommonDefinition.ColumnNameId];
      if (m_ItemsEdited.ContainsKey(_Id))
        return;
      m_ItemsEdited.Add(_Id, _selectedItem);
      Edited = true;
    }
    private void x_ButtonAddNew_Click(object sender, RoutedEventArgs e)
    {

    }
    private void x_ButtonEndofBatch_Click(object sender, RoutedEventArgs e)
    {

    }
    private void x_ButtonDelete_Click(object sender, RoutedEventArgs e)
    {

    }
    private void x_ButtonSave_Click(object sender, RoutedEventArgs e)
    {
      if (m_ItemsEdited.Count == 0)
        return;
      foreach (ListItem _lix in m_ItemsEdited.Values)
        _lix.Update();
      m_ItemsEdited.Clear();
      Edited = false;
      m_ClientContext.ExecuteQueryAsync
        (
        // Succeeded Callback
          (ss, se) =>
          {
            // Execute on the UI thread 
            Deployment.Current.Dispatcher.BeginInvoke(
              () =>
              {
                // Remember the selected index before refresh
                int index = x_DataGridListView.SelectedIndex == 0 ? 1 : x_DataGridListView.SelectedIndex - 1;
                // refresh the databinding to the List Items Collection
                x_DataGridListView.ItemsSource = null;
                x_DataGridListView.ItemsSource = m_ItemsCollection;
                // Set the selected item and show it
                x_DataGridListView.SelectedIndex = index;
                x_DataGridListView.UpdateLayout();
                x_DataGridListView.ScrollIntoView(x_DataGridListView.SelectedItem, x_DataGridListView.Columns[0]);
              });
          },
          (ss, se) => { }
        );
    }
    #endregion

    #endregion

  }
}
