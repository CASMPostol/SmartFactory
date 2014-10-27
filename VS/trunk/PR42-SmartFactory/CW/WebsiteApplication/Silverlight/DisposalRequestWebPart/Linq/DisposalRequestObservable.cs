//<summary>
//  Title   : class DisposalRequestObservable
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

using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  /// <summary>
  /// class DisposalRequestObservable
  /// </summary>
  public class DisposalRequestObservable : ObservableCollection<DisposalRequest>, INotifyCollectionChanged
  {

    #region internal
    /// <summary>
    /// The property changed
    /// </summary>
    internal new PropertyChangedEventHandler PropertyChanged;
    /// <summary>
    /// Gets the data context.
    /// </summary>
    /// <param name="disposalRequestLibId">The disposal request library identifier.</param>
    /// <param name="list">The list.</param>
    /// <param name="context">The context.</param>
    internal void GetDataContext(int disposalRequestLibId, List<CustomsWarehouseDisposal> list, DataContextAsync context)
    {
      m_DisposalRequestLibId = disposalRequestLibId;
      IEnumerable<IGrouping<string, CustomsWarehouseDisposal>> _requests = list.GroupBy<CustomsWarehouseDisposal, string>(x => x.CWL_CWDisposal2CustomsWarehouseID.Batch);
      RequestsQueue _Queue = new RequestsQueue(this, context);
      _Queue.DoAsync(_requests);
    }
    internal void GetDemoData()
    {
      List<CustomsWarehouse> listOfAccounts = new List<CustomsWarehouse>();
      IGrouping<string, CustomsWarehouseDisposal> groupOfDisposals = null;
      SampleData.RequestSampleData.GetData(listOfAccounts, out groupOfDisposals);
      DisposalRequest _new = DisposalRequest.Create(listOfAccounts, groupOfDisposals, (x, y) => RaisePropertyChanged(y));
      this.Add(_new);
       _new.AutoCalculation = true;
     _new = DisposalRequest.Create(listOfAccounts, groupOfDisposals, (x, y) => RaisePropertyChanged(y));
      this.Add(_new);
      _new.AutoCalculation = true;
    }
    internal void CreateDisposalRequest(List<CustomsWarehouse> list, double toDispose, string customsProcedure)
    {
      if (list.Count == 0)
        throw new AggregateException("list must contain at least one element");
      CustomsWarehouse _fcw = list.First<CustomsWarehouse>();
      DisposalRequest _fDspRqs = this.FirstOrDefault<DisposalRequest>((x) => { return x.Batch == _fcw.Batch; });
      if (_fDspRqs != null)
        _fDspRqs.AddedKg += toDispose;
      else
      {
        DisposalRequest _dr = DisposalRequest.Create(list, customsProcedure, (x, y) => RaisePropertyChanged(y));
        this.Add(_dr);
        _dr.AutoCalculation = true;
        _dr.AddedKg += toDispose;
      }
    }
    internal event ProgressChangedEventHandler ProgressChanged;  //TODO report progress
    internal virtual void OnProgressChanged(ProgressChangedEventArgs args)
    {
      if (ProgressChanged == null)
        return;
      ProgressChanged(this, args);
    }
    internal void RecalculateDisposals(int disposalRequestLibId, DataContextAsync context)
    {
      foreach (DisposalRequest _drx in this)
        _drx.RecalculateDisposals(disposalRequestLibId, context);
    }
    #endregion

    #region private

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      base.OnPropertyChanged(e);
      RaisePropertyChanged(e);
    }
    private void RaisePropertyChanged(PropertyChangedEventArgs e)
    {
      PropertyChangedEventHandler _pc = PropertyChanged;
      if (_pc != null)
        _pc(this, e);
    }
    private int m_DisposalRequestLibId;
    private class RequestsQueue : Queue<CraeteRequestBase>
    {

      #region internal
      internal RequestsQueue(DisposalRequestObservable parent, DataContextAsync context)
      {
        m_Parent = parent;
        m_DataContext = context;
      }
      internal void DoAsync(IEnumerable<IGrouping<string, CustomsWarehouseDisposal>> requests)
      {
        foreach (IGrouping<string, CustomsWarehouseDisposal> _grx in requests)
          AddRequest2Queue(_grx);
        DoAsync();
      }
      #endregion

      #region private
      private class CraeteRequest : CraeteRequestBase
      {
        public CraeteRequest(RequestsQueue requestsQueue, IGrouping<string, CustomsWarehouseDisposal> grouping) :
          base(grouping)
        {
          m_RequestsQueue = requestsQueue;
        }
        private RequestsQueue m_RequestsQueue = null;
        protected override void DoNext()
        {
          m_RequestsQueue.DoAsync();
        }
        protected override DisposalRequestObservable Parent
        {
          get { return m_RequestsQueue.m_Parent; }
        }
        protected override DataContextAsync DataContext
        {
          get { return m_RequestsQueue.m_DataContext; }
        }
      }
      private void DoAsync()
      {
        if (this.Count == 0)
          return;
        CraeteRequestBase _rqs = this.Dequeue();
        _rqs.DoAsync();
      }
      private void AddRequest2Queue(IGrouping<string, CustomsWarehouseDisposal> group)
      {
        CraeteRequestBase _creator = new CraeteRequest(this, group);
        this.Enqueue(_creator);
      }
      private DataContextAsync m_DataContext = null;
      private DisposalRequestObservable m_Parent;
      #endregion

    }
    private abstract class CraeteRequestBase
    {
      internal CraeteRequestBase(IGrouping<string, CustomsWarehouseDisposal> grouping)
      {
        m_BatchGroup = grouping;
      }
      internal void DoAsync()
      {
        DataContext.GetListCompleted += context_GetListCompleted;
        DataContext.GetListAsync<CustomsWarehouse>(CommonDefinition.CustomsWarehouseTitle,
                                                    CommonDefinition.GetCAMLSelectedID(m_BatchGroup.Key, CommonDefinition.FieldBatch, CommonDefinition.CAMLTypeText));
      }
      protected abstract DataContextAsync DataContext { get; }
      protected abstract DisposalRequestObservable Parent { get; }
      protected abstract void DoNext();
      private IGrouping<string, CustomsWarehouseDisposal> m_BatchGroup = null;
      private void context_GetListCompleted(object siurce, GetListAsyncCompletedEventArgs e)
      {
        try
        {
          DataContext.GetListCompleted -= context_GetListCompleted;
          if (e.Cancelled)
            return;
          if (e.Error != null)
            Parent.OnProgressChanged(new ProgressChangedEventArgs(0, String.Format("Exception {0} at m_DataContext.GetListAsync", e.Error.Message)));
          DisposalRequest _Dr = DisposalRequest.Create(e.Result<CustomsWarehouse>(), m_BatchGroup, (x, y) => Parent.RaisePropertyChanged(y));
          Parent.Add(_Dr);
          _Dr.AutoCalculation = true;
        }
        catch (Exception _ex)
        {
          Parent.OnProgressChanged(new ProgressChangedEventArgs(0, String.Format("Exception {0} at context_GetListCompleted", _ex)));
        }
        DoNext();
      }
    }//class CraeteRequest
    #endregion

  }
}
