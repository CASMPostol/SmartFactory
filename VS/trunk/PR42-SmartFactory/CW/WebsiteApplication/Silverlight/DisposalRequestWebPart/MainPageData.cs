//<summary>
//  Title   : public class MainPageData
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

using CAS.Common.ComponentModel;
using CAS.Common.ViewModel;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Data;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart
{
  /// <summary>
  /// public class MainPageData
  /// </summary>
  public sealed class MainPageData : PropertyChangedBase, IDisposable
  {

    #region public properties
    /// <summary>
    /// Gets or sets the header label.
    /// </summary>
    /// <value>
    /// The header label.
    /// </value>
    public string HeaderLabel
    {
      get
      {
        return b_HeaderLabel;
      }
      set
      {
        RaiseHandler<string>(value, ref b_HeaderLabel, "HeaderLabel", this);
      }
    }
    /// <summary>
    /// Gets or sets the log.
    /// </summary>
    /// <value>
    /// The log.
    /// </value>
    public string Log
    {
      get
      {
        return b_Log;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Log, "Log", this);
      }
    }
    /// <summary>
    /// Gets or sets a value indicating whether this item is read only.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this item is read only; otherwise, <c>false</c>.
    /// </value>
    public bool ReadOnly
    {
      get
      {
        return b_ReadOnly;
      }
      set
      {
        RaiseHandler<bool>(value, ref b_ReadOnly, "ReadOnly", this);
      }
    }
    /// <summary>
    /// Gets or sets the save command.
    /// </summary>
    /// <value>
    /// The save command.
    /// </value>
    public SynchronousCommandBase<Object> SaveCommand
    {
      get
      {
        return b_SaveCommand;
      }
      set
      {
        RaiseHandler<SynchronousCommandBase<Object>>(value, ref b_SaveCommand, "SaveCommand", this);
      }
    }

    /// <summary>
    /// Gets or sets the request collection.
    /// </summary>
    /// <value>
    /// The request collection.
    /// </value>
    public PagedCollectionView RequestCollection
    {
      get
      {
        return b_RequestCollection;
      }
      set
      {
        RaiseHandler<PagedCollectionView>(value, ref b_RequestCollection, "RequestCollection", this);
      }
    }
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      if (m_Context != null)
        m_Context.Dispose();
      m_Disposed = true;
    }
    #endregion

    #region internal
    internal DataContextAsync DataContextAsync { get { return m_Context; } }
    internal static MainPageData GetData(string url, int? selectedID)
    {
      MainPageData _ret = new MainPageData();
      if (selectedID.HasValue)
        _ret.GetRealData(url, selectedID);
      else
        _ret.GetDemoData();
      return _ret;
    }
    /// <summary>
    /// Creates the disposal request.
    /// </summary>
    /// <param name="list">The list of <see cref="CustomsWarehouse"/> with the same batch.</param>
    /// <param name="toDispose">The tobacco to dispose.</param>
    internal void CreateDisposalRequest(List<CustomsWarehouse> list, double toDispose, string customsProcedure)
    {
      CheckDisposed();
      if (m_DemoDataLoaded)
        return;
      this.DisposalRequestObservable.CreateDisposalRequest(list, toDispose, customsProcedure);
    }
    internal void SubmitChanges()
    {
      CheckDisposed();
      try
      {
        if (m_DemoDataLoaded)
          return;
        this.DisposalRequestObservable.RecalculateDisposals(m_DisposalRequestLibId.Value, m_Context);
        m_Context.SubmitChangesCompleted += m_Context_SubmitChangesCompleted;
        m_Context.SubmitChangesAsyn();
        UpdateHeader();
        Modified = false;
      }
      catch (Exception _ex)
      {
        ExceptionHandling(_ex);
      }
      finally
      {
        UpdateHeader();
        Modified = false;
      }
    }
    #endregion

    #region private

    #region backing fields
    private PagedCollectionView b_RequestCollection = null;
    private string b_HeaderLabel = "N/A";
    private string b_Log = "Log before starting";
    private bool b_ReadOnly = true;
    private SynchronousCommandBase<Object> b_SaveCommand = null;
    #endregion

    #region vars
    private bool m_Disposed = false;
    private string m_URL = String.Empty;
    private int? m_DisposalRequestLibId = new Nullable<int>();
    private DataContextAsync m_Context = new DataContextAsync();
    private bool m_DemoDataLoaded = false;
    #endregion

    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="MainPageData"/> class.
    /// </summary>
    private MainPageData()
    {
      b_SaveCommand = new CAS.Common.ViewModel.SynchronousCommandBase<Object>(x => SubmitChanges(), x => Modified && !ReadOnly);
      PagedCollectionView _npc = new PagedCollectionView(new DisposalRequestObservable());
      RequestCollection = _npc;
      this.DisposalRequestObservable.ProgressChanged += DisposalRequestObservable_ProgressChanged;
      Log = "MainPageData created.";
    }
    #endregion

    protected override void OnModified()
    {
      SaveCommand.RaiseCanExecuteChanged();
    }
    private void GetDemoData()
    {
      this.DisposalRequestObservable.GetDemoData();
      m_DemoDataLoaded = true;
      Log = "Demo data loaded";
      OnDataLoaded();
    }
    private void GetRealData(string url, int? selectedID)
    {
      m_URL = url;
      m_DisposalRequestLibId = selectedID;
      m_Context.CreateContextAsyncCompletedEvent += m_Context_CreateContextAsyncCompletedEvent;
      Log = String.Format("GetDataAsync: CreateContextAsync for URL={0}.", m_URL);
      m_Context.CreateContextAsync(m_URL);
    }
    private void m_Context_SubmitChangesCompleted(object sender, AsyncCompletedEventArgs e)
    {
      m_Context.SubmitChangesCompleted -= m_Context_SubmitChangesCompleted;
      Log = "SubmitChangesCompleted";
    }
    private void ExceptionHandling(Exception ex)
    {
      this.HeaderLabel = "Exception:" + ex.Message;
    }
    private void UpdateHeader()
    {
      try
      {
        int items = RequestCollection == null ? -1 : RequestCollection.TotalItemCount;
        string _pattern = "Disposal request {0} content: {1} items;";
        string _rid = m_DisposalRequestLibId.HasValue ? m_DisposalRequestLibId.ToString() : "Not connected";
        this.HeaderLabel = String.Format(_pattern, _rid, items);
      }
      catch (Exception ex)
      {
        this.HeaderLabel = "UpdateHeader exception " + ex.Message;
      }
    }
    private void RequestCollection_CollectionChanged(object sender, EventArgs e)
    {
      Modified = true;
    }
    private void RequestCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      Modified = true;
    }
    private void DisposalRequestObservable_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      Log = e.UserState as string;
    }
    private void m_Context_CreateContextAsyncCompletedEvent(object sender, AsyncCompletedEventArgs e)
    {
      m_Context.CreateContextAsyncCompletedEvent -= m_Context_CreateContextAsyncCompletedEvent;
      Log = String.Format("GetData DoWork: new DataContext for url={0}.", m_URL);
      if (e.Cancelled)
      {
        Log = "CreateContextAsync canceled";
        return;
      }
      if (e.Error != null)
      {
        ExceptionHandling(e.Error);
        return;
      }
      Log = String.Format(": new DataContext for url={0}.", m_URL);
      Debug.Assert(m_DisposalRequestLibId.HasValue, "m_SelectedID must have value");
      m_Context.GetListCompleted += m_Context_GetDisposals4RequestCompleted;
      //Get all disposals for the request with id of m_DisposalRequestLibId
      m_Context.GetListAsync<CustomsWarehouseDisposal>
        (CommonDefinition.CustomsWarehouseDisposalTitle, CommonDefinition.GetCAMLSelectedID(m_DisposalRequestLibId.Value, CommonDefinition.FieldCWDisposal2DisposalRequestLibraryID, CommonDefinition.CAMLTypeNumber));
    }
    private void m_Context_GetDisposals4RequestCompleted(object siurce, GetListAsyncCompletedEventArgs e)
    {
      m_Context.GetListCompleted -= m_Context_GetDisposals4RequestCompleted;
      if (e.Cancelled)
      {
        Log = "GetList has been canceled";
        return;
      }
      if (e.Error != null)
      {
        ExceptionHandling(e.Error);
        return;
      }
      Log = "GetListCompleted .GetDataContext  " + CommonDefinition.CustomsWarehouseDisposalTitle;
      List<CustomsWarehouseDisposal> _list = e.Result<CustomsWarehouseDisposal>();
      this.DisposalRequestObservable.GetDataContext(m_DisposalRequestLibId.Value, _list, m_Context, () => OnDataLoaded());
      if (this.RequestCollection.CanSort == true)
        this.RequestCollection.SortDescriptions.Add(new SortDescription("Batch", ListSortDirection.Ascending)); // By default, sort by Batch.
      Log = "GetData RunWorker Completed";
    }
    private void OnDataLoaded()
    {
      bool _readOnly = false;
      foreach (DisposalRequest _request in DisposalRequestObservable)
      {
        if (!_request.ReadOnly)
          continue;
        _readOnly = true;
        break;
      }
      ReadOnly = _readOnly;
      UpdateHeader();
      Modified = false;
      DisposalRequestObservable.PropertyChanged += RequestCollection_PropertyChanged;
      DisposalRequestObservable.CollectionChanged += RequestCollection_CollectionChanged;
    }
    private DisposalRequestObservable DisposalRequestObservable { get { return (DisposalRequestObservable)this.RequestCollection.SourceCollection; } }
    private void CheckDisposed()
    {
      if (m_Disposed)
        throw new ObjectDisposedException(typeof(MainPageData).Name);
    }
    #endregion //private

  }
}
