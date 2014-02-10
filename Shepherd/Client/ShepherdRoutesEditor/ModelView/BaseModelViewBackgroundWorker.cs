//<summary>
//  Title   : class BaseModelView
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAS.SharePoint.ComponentModel;

namespace CAS.SmartFactory.Shepherd.RouteEditor.ModelView
{
  internal abstract class BaseModelViewBackgroundWorker : INotifyPropertyChanged, IDisposable
  {

    #region creator
    public BaseModelViewBackgroundWorker()
    {
      m_BackgroundWorker.DoWork += m_BackgroundWorker_DoWork;
      m_BackgroundWorker.ProgressChanged += m_BackgroundWorker_ProgressChanged;
      m_BackgroundWorker.RunWorkerCompleted += m_BackgroundWorker_RunWorkerCompleted;
    }
    #endregion

    #region UI properties
    public bool NotBusy
    {
      get
      {
        return b_NotBusy;
      }
      set
      {
        PropertyChanged.RaiseHandler<bool>(value, ref b_NotBusy, "NotBusy", this);
      }
    }
    #endregion

    protected abstract Action<Action<ProgressChangedEventArgs>> DoWorkEventHandler { get; }
    protected abstract RunWorkerCompletedEventHandler CompletedEventHandler { get; }
    protected abstract ProgressChangedEventHandler ProgressChangedEventHandler { get; }
    protected void StartBackgroundWorker()
    {
      NotBusy = false;
      m_BackgroundWorker.RunWorkerAsync();
    }
    protected void StartBackgroundWorker(object argument)
    {
      NotBusy = false;
      m_BackgroundWorker.RunWorkerAsync(argument);
    }

    #region INotifyPropertyChanged Members
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion

    #region IDisposable Members
    public void Dispose()
    {
      if (m_Disposed)
        return;
      if (m_BackgroundWorker == null)
        m_BackgroundWorker.Dispose();
      m_BackgroundWorker = null;
      m_Disposed = true;
    }
    #endregion

    private bool b_NotBusy = true;
    private bool m_Disposed = false;
    private System.ComponentModel.BackgroundWorker m_BackgroundWorker = new BackgroundWorker()
      {
        WorkerReportsProgress = true,
        WorkerSupportsCancellation = false
      };
    private void m_BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (CompletedEventHandler != null)
        CompletedEventHandler(sender, e);
      NotBusy = true;
    }
    private void m_BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (ProgressChangedEventHandler != null)
        ProgressChangedEventHandler(sender, e);
    }
    private void m_BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      e.Cancel = false;
      e.Result = false;
      if (DoWorkEventHandler == null)
        return;
      BackgroundWorker _mbw = (BackgroundWorker)sender;
      DoWorkEventHandler(x => { _mbw.ReportProgress(0, x); });
    }
    private void CheckDisposed()
    {
      if (m_Disposed)
        throw new ObjectDisposedException(typeof(BaseModelViewBackgroundWorker).Name);
    }

  }
}
