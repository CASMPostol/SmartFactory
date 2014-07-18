//<summary>
//  Title   : class BackgroundWorkerVizardMachine
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using CAS.SmartFactory.IPR.Client.DataManagement;
using System;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal abstract class BackgroundWorkerVizardMachine : AbstractMachine, IDisposable
  {
    public BackgroundWorkerVizardMachine(StateMachineContext context)
      : base(context)
    {
      m_BackgroundWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
      m_BackgroundWorker.DoWork += BackgroundWorker_DoWork;
      m_BackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
      m_BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
    }
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      m_BackgroundWorker.Dispose();
    }

    #region AbstractMachine implementation
    internal virtual void RunAsync()
    {
      if (m_BackgroundWorker.IsBusy)
        throw new ApplicationException("Background worker is busy");
      SetEventMask(Events.Cancel);
      m_BackgroundWorker.RunWorkerAsync();
    }
    public override void Cancel()
    {
      if (!m_BackgroundWorker.CancellationPending)
        m_BackgroundWorker.CancelAsync();
    }
    #endregion

    #region private
    protected abstract void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e);
    protected virtual void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      if (e.Cancelled)
        Context.Close();
      else if (e.Error != null)
        Context.Exception(e.Error);
      else
        this.RunWorkerCompleted();
    }
    protected abstract void RunWorkerCompleted();
    protected virtual void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      Context.ProgressChang(this, (EntitiesChangedEventArgs.EntitiesState)e.UserState);
    }
    protected bool ReportProgress(object source, EntitiesChangedEventArgs progress)
    {
      m_BackgroundWorker.ReportProgress(progress.ProgressPercentage, progress.UserState);
      return m_BackgroundWorker.CancellationPending;
    }
    private BackgroundWorker m_BackgroundWorker;
    #endregion
  }
}
