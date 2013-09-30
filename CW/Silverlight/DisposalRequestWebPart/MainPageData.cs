//<summary>
//  Title   : public class MainPageData
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.Windows.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.Entities;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart
{
  /// <summary>
  /// public class MainPageData
  /// </summary>
  public sealed class MainPageData : INotifyPropertyChanged, IDisposable
  {

    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPageData"/> class.
    /// </summary>
    public MainPageData()
    {
      PagedCollectionView _npc = new PagedCollectionView(new Data.Entities.DisposalRequestObservable());
      _npc.PropertyChanged += RequestCollection_PropertyChanged;
      _npc.CollectionChanged += RequestCollection_CollectionChanged;
      if (_npc.CanGroup == true)
      {
        // Group by 
        _npc.GroupDescriptions.Add(new PropertyGroupDescription("Batch"));
      }
      if (_npc.CanSort == true)
      {
        // By default, sort by Batch.
        _npc.SortDescriptions.Add(new SortDescription("Batch", ListSortDirection.Ascending));
      }
      RequestCollection = _npc;
      m_Singleton = this;
    }
    #endregion

    #region public properties
    public string HeaderLabel
    {
      get { return b_HeaderLabel; }
      set
      {
        if (b_HeaderLabel == value)
          return;
        b_HeaderLabel = value;
        OnPropertyChanged("HeaderLabel");
      }
    }
    public PagedCollectionView RequestCollection
    {
      get { return b_RequestCollection; }
      set { b_RequestCollection = value; }
    }
    #endregion

    #region INotifyPropertyChanged Members
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      if (m_DataContext != null)
        m_DataContext.Dispose();
    }
    #endregion

    #region internal
    internal static void GetData(string url)
    {
      System.Threading.ThreadPool.QueueUserWorkItem(m_Singleton.GetData, url);
    }
    internal static void SubmitChanges()
    {
      m_Singleton.SubmitChangesLoc();
    }
    #endregion

    #region private

    #region backing fields
    private PagedCollectionView b_RequestCollection = null;
    private string b_HeaderLabel = "Disposal request content:";
    #endregion

    private static MainPageData m_Singleton = null;
    private bool m_Edited = false;
    private DataContext m_DataContext;
    /// <summary>
    /// Called whena property value changes.
    /// </summary>
    /// <param name="propertyName">Name of the property.</param>
    private void OnPropertyChanged(string propertyName)
    {
      if ((null != this.PropertyChanged))
        this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }
    private void GetData(object url)
    {
      try
      {
        m_DataContext = new DataContext((string)url);
        EntityList<Data.Entities.CustomsWarehouseDisposalRowData> _list = m_DataContext.GetList<Data.Entities.CustomsWarehouseDisposalRowData>(CommonDefinition.CustomsWarehouseDisposalTitle, CamlQuery.CreateAllItemsQuery());
        ((DisposalRequestObservable)this.RequestCollection.SourceCollection).GetDataContext(_list);
        m_Edited = false;
        UpdateHeader();
      }
      catch (Exception ex)
      {
        ExceptionHandling(ex);
      }
    }
    private void SubmitChangesLoc()
    {
      try
      {
        m_DataContext.SubmitChanges();
        m_Edited = false;
        UpdateHeader();
      }
      catch (Exception _ex)
      {
        ExceptionHandling(_ex);
      }
    }
    private void ExceptionHandling(Exception ex)
    {
      this.HeaderLabel = "Exception:" + ex.Message;
    }
    private void UpdateHeader()
    {
      int items = RequestCollection.TotalItemCount;
      string _pattern = "Disposal request content: {0} items; {1}";
      string _star = m_Edited ? "*" : " ";
      this.HeaderLabel = String.Format(_pattern, items, _star);
    }
    private void RequestCollection_CollectionChanged(object sender, EventArgs e)
    {
      m_Edited = true;
      UpdateHeader();
    }
    void RequestCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      m_Edited = true;
      UpdateHeader();
    }
    #endregion

  }
}
