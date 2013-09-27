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
      this.Unloaded += MainPage_Unloaded;
    }
    DataContext m_DataContext = null;
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
    private bool b_Edited = false;
    #endregion

    private void UpdateHeader(int items, bool modified)
    {
      string _pattern = "Disposal request content: {0} items; {1}";
      string _star = modified ? "*" : " ";
      x_LabelHeader.Content = String.Format(_pattern, items, _star);
    }
    private void GetData(object state)
    {
      try
      {
        DataContext _dc = (DataContext)state;
        EntityList<Data.Entities.CustomsWarehouseDisposalRowData> _list = _dc.GetList<Data.Entities.CustomsWarehouseDisposalRowData>(CommonDefinition.CustomsWarehouseDisposalTitle, CamlQuery.CreateAllItemsQuery());
        PagedCollectionView _data = Data.Entities.DisposalRequest.GetDataContext(_list);
        _data.CurrentChanged += DataContext_CurrentChanged;
        Deployment.Current.Dispatcher.BeginInvoke(() =>
        {
          x_DataGridListView.ItemsSource = _data;
          UpdateHeader(_data.TotalItemCount, false);
          x_DataGridListView.UpdateLayout();
        });
      }
      catch (Exception ex)
      {
        ExceptionHandling(ex);
      }
    }
    private void ExceptionHandling(Exception ex)
    {
      MessageBox.Show(ex.Message + " AT: " + m_at, "Loaded event error", MessageBoxButton.OK);
    }

    #region event handlers
    private void DataContext_CurrentChanged(object sender, EventArgs e)
    {
      PagedCollectionView _pcv = (PagedCollectionView)sender;
      Deployment.Current.Dispatcher.BeginInvoke(() => { UpdateHeader(_pcv.TotalItemCount, true); });
    }
    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        ClientContext _ClientContext = ClientContext.Current;
        if (_ClientContext == null)
          throw new ArgumentNullException("clientContext", String.Format("Cannot get the {0} ", "ClientContext"));
        m_DataContext = new DataContext(_ClientContext.Url);
        System.Threading.ThreadPool.QueueUserWorkItem(GetData, m_DataContext);
      }
      catch (Exception ex)
      {
        ExceptionHandling(ex);
      }
    }
    private void MainPage_Unloaded(object sender, RoutedEventArgs e)
    {
      if (m_DataContext != null)
        m_DataContext.Dispose();
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
      try
      {
        m_DataContext.SubmitChanges();
      }
      catch (Exception _ex)
      {
        ExceptionHandling(_ex);
      }
    }
    #endregion

    #endregion

  }
}
