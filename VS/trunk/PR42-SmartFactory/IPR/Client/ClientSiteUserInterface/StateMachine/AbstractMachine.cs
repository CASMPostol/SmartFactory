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
using System.ComponentModel;
using CAS.SmartFactory.IPR.Client.DataManagement;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  [Flags]
  internal enum Events
  {
    Previous = 0x1,
    Next = 0x2,
    Cancel = 0x4,
    RunAsync = 0x8
  }
  internal enum ProcessState
  {
    SetupDataDialog,
    Activation,
    Archiving,
    Finisched,
  }
  internal abstract class AbstractMachine : IAbstractMachineEvents
  {

    #region constructor
    public AbstractMachine(StateMachineContext context)
    {
      m_Context = context;
    }
    #endregion

    internal event EventHandler Entered;
    internal event EventHandler Exiting;
    internal virtual void OnEnteringState()
    {
      if (Entered != null)
        Entered(this, new EventArgs());
      m_Context.SetupUserInterface(GetEventMask());
    }
    internal virtual void OnExitingState()
    {
      if (Exiting != null)
        Exiting(this, new EventArgs());
    }
    internal abstract Events GetEventMask();
    internal static void CreateStates(StateMachineContext stateMachineContext)
    {
      new AbstractMachine.SetupDataDialogMachine(stateMachineContext);
      new AbstractMachine.ActivationMachine(stateMachineContext);
      new AbstractMachine.ArchivingMachine(stateMachineContext);
      new AbstractMachine.FinishedMachine(stateMachineContext);
    }

    #region IAbstractMachineEvents Members
    public virtual void Previous()
    {
      throw new ApplicationException();
    }
    public virtual void Next()
    {
      throw new ApplicationException();
    }
    public virtual void Cancel()
    {
      m_Context.Close();
    }
    public virtual void RunAsync()
    {
    }
    #endregion

    #region private

    #region state classes
    internal abstract class BackgroundWorkerMachine : AbstractMachine
    {
      public BackgroundWorkerMachine(StateMachineContext context)
        : base(context)
      {
        m_BackgroundWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
        m_BackgroundWorker.DoWork += BackgroundWorker_DoWork;
        m_BackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
        m_BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
      }

      #region AbstractMachine implementation
      public override void RunAsync()
      {
        if (m_BackgroundWorker.IsBusy)
          return;
        m_BackgroundWorker.RunWorkerAsync();
      }
      public override void Cancel()
      {
        if (!m_BackgroundWorker.CancellationPending)
          m_BackgroundWorker.CancelAsync();
      }
      internal override Events GetEventMask()
      {
        return Events.Cancel;
      }
      #endregion

      #region private
      protected abstract void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e);
      protected virtual void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
        if (e.Cancelled)
          m_Context.Close();
        else if (e.Error != null)
          m_Context.Exception(e.Error);
        else
          this.RunWorkerCompleted();
      }
      protected abstract void RunWorkerCompleted();
      protected virtual void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
        m_Context.ProgressChang(this, (EntitiesChangedEventArgs.EntitiesState)e.UserState);
      }
      protected bool ReportProgress(object source, EntitiesChangedEventArgs progress)
      {
        m_BackgroundWorker.ReportProgress(progress.ProgressPercentage, progress.UserState);
        return m_BackgroundWorker.CancellationPending;
      }
      private BackgroundWorker m_BackgroundWorker;
      #endregion

    }
    internal class SetupDataDialogMachine : AbstractMachine
    {
      public SetupDataDialogMachine(StateMachineContext context)
        : base(context)
      {
        m_Me = this;
      }
      internal static SetupDataDialogMachine Get()
      {
        return m_Me;
      }
      internal override Events GetEventMask()
      {
        return Events.Cancel | Events.Next;
      }

      #region IAbstractMachineEvents Members
      public override void Next()
      {
        m_Context.AssignStateMachine(ProcessState.Activation);
        m_Context.Machine.RunAsync();
      }
      public override void Cancel()
      {
        m_Context.Close();
      }
      #endregion
      private static SetupDataDialogMachine m_Me;

    }
    internal class ActivationMachine : BackgroundWorkerMachine
    {
      #region creator
      internal ActivationMachine(StateMachineContext context)
        : base(context)
      {
        m_Me = this;
      }
      #endregion

      internal static ActivationMachine Get()
      {
        return m_Me;
      }
      internal override Events GetEventMask()
      {
        return base.GetEventMask();
      }

      #region BackgroundWorkerMachine implementation
      public override void RunAsync()
      {
        m_Context.SetupUserInterface(Events.Cancel);
        base.RunAsync();
      }
      protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
      {
        DataManagement.Activate180.Activate.Go(Properties.Settings.Default.SiteURL, Properties.Settings.Default.DoActivate1800, ReportProgress);
      }
      protected override void RunWorkerCompleted()
      {
        m_Context.AssignStateMachine(ProcessState.Archiving);
        m_Context.Machine.RunAsync();
      }
      #endregion

      private static ActivationMachine m_Me;
    }
    internal class ArchivingMachine : BackgroundWorkerMachine
    {
      #region creator
      internal ArchivingMachine(StateMachineContext context)
        : base(context)
      {
        m_Me = this;
      }
      #endregion

      internal override Events GetEventMask()
      {
        return base.GetEventMask();
      }

      #region BackgroundWorkerMachine implementation
      public override void RunAsync()
      {
        m_Context.SetupUserInterface(Events.Cancel);
        base.RunAsync();
      }
      protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
      {
        DataManagement.Archival.Archive.ArchiveSettings _settings = new DataManagement.Archival.Archive.ArchiveSettings
        {
          ArchiveBatchDelay = Properties.Settings.Default.ArchiveBatchDelay,
          ArchiveIPRDelay = Properties.Settings.Default.ArchiveIPRDelay,
          DoArchiveBatch = Properties.Settings.Default.DoArchiveBatch,
          DoArchiveIPR = Properties.Settings.Default.DoArchiveIPR,
          SiteURL = Properties.Settings.Default.SiteURL
        };
        DataManagement.Archival.Archive.Go(_settings, ReportProgress);
      }
      protected override void RunWorkerCompleted()
      {
        m_Context.AssignStateMachine(ProcessState.Finisched);
      }
      #endregion

      private static ArchivingMachine m_Me;
      internal static ArchivingMachine Get()
      {
        return m_Me;
      }

    }
    internal class FinishedMachine : AbstractMachine
    {
      internal FinishedMachine(StateMachineContext context)
        : base(context)
      {
        m_Me = this;
      }
      private static FinishedMachine m_Me;
      internal static FinishedMachine Get()
      {
        return m_Me;
      }
      public override void RunAsync()
      {
        m_Context.WriteLine("All operation have been finished, press >> to exit the program");
        base.RunAsync();
      }
      public override void Next()
      {
        m_Context.Close();
      }
      internal override Events GetEventMask()
      {
        return Events.Next;
      }
    }
    #endregion

    #region vars
    private StateMachineContext m_Context = null;
    #endregion

    #endregion


  }
}
