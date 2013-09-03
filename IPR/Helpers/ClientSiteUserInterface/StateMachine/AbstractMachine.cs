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
using CAS.SmartFactory.IPR.Client.FeatureActivation;

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
  internal abstract class AbstractMachine: IAbstractMachineEvents
  {

    #region constructor
    public AbstractMachine( StateMachineContext context )
    {
      m_Context = context;
    }
    #endregion

    internal virtual void EnteringState()
    {
      m_Context.SetupUserInterface( GetEventMask() );
    }
    internal abstract Events GetEventMask();
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
      throw new ApplicationException();
    }
    #endregion

    #region private

    #region state classes
    internal abstract class BackgroundWorkerMachine: AbstractMachine
    {
      public BackgroundWorkerMachine( StateMachineContext context )
        : base( context )
      {
        m_BackgroundWorker = new BackgroundWorker() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
        m_BackgroundWorker.DoWork += BackgroundWorker_DoWork;
        m_BackgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
        m_BackgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
      }

      #region AbstractMachine implementation
      public override void RunAsync()
      {
        if ( m_BackgroundWorker.IsBusy )
          return;
        m_BackgroundWorker.RunWorkerAsync();
      }
      public override void Cancel()
      {
        if ( !m_BackgroundWorker.CancellationPending )
          m_BackgroundWorker.CancelAsync();
      }
      internal override Events GetEventMask()
      {
        return Events.Cancel;
      }
      #endregion

      protected abstract void BackgroundWorker_DoWork( object sender, DoWorkEventArgs e );
      protected virtual void BackgroundWorker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
      {
        if ( e.Cancelled )
          m_Context.Close();
        else if ( e.Error != null )
          m_Context.Exception( e.Error );
        else
          this.RunWorkerCompleted();
      }
      protected abstract void RunWorkerCompleted();
      protected virtual void BackgroundWorker_ProgressChanged( object sender, ProgressChangedEventArgs e )
      {
        m_Context.ProgressChang( this, (EntitiesChangedEventArgs.EntitiesState)e.UserState );
      }
      protected bool ReportProgress( object source, EntitiesChangedEventArgs progress )
      {
        m_BackgroundWorker.ReportProgress( progress.ProgressPercentage, progress.UserState );
        return m_BackgroundWorker.CancellationPending;
      }
      private BackgroundWorker m_BackgroundWorker;

    }
    internal class SetupDataDialogMachine: AbstractMachine
    {
      public SetupDataDialogMachine( StateMachineContext context )
        : base( context )
      { }
      internal override void EnteringState()
      {
        base.EnteringState();
        m_Context.SetSiteURL( Properties.Settings.Default.SiteURL );
      }
      internal override Events GetEventMask()
      {
        return Events.Cancel | Events.Next;
      }
      public override void Next()
      {
        Properties.Settings.Default.SiteURL = m_Context.GetSiteUrl();
        Properties.Settings.Default.Save();
        m_Context.AssignStateMachine( ProcessState.Activation );
        m_Context.Machine.RunAsync();
      }
      public override void Cancel()
      {
        m_Context.Close();
      }
    }
    internal class ActivationMachine: BackgroundWorkerMachine
    {
      #region creator
      internal ActivationMachine( StateMachineContext context )
        : base( context )
      { }
      #endregion

      internal override Events GetEventMask()
      {
        return base.GetEventMask();
      }
      #region BackgroundWorkerMachine implementation
      public override void RunAsync()
      {
        m_Context.SetupUserInterface( Events.Cancel );
        m_Context.DisplayStatusURL( Properties.Settings.Default.SiteURL + "(Activating)");
        m_Context.ProgressChang( this, new EntitiesChangedEventArgs( 1, "Feture activation in progress - it could take several minutes.", null ).UserState );
        base.RunAsync();
      }
      protected override void BackgroundWorker_DoWork( object sender, DoWorkEventArgs e )
      {

        FeatureActivation.Activate180.Activate.Go( Properties.Settings.Default.SiteURL, ReportProgress );
      }
      protected override void RunWorkerCompleted()
      {
        m_Context.AssignStateMachine( ProcessState.Archiving );
        m_Context.Machine.RunAsync();
      }
      #endregion

    }
    internal class ArchivingMachine: BackgroundWorkerMachine
    {
      #region creator
      internal ArchivingMachine( StateMachineContext context )
        : base( context )
      { }
      #endregion

      internal override Events GetEventMask()
      {
        return base.GetEventMask();
      }

      #region BackgroundWorkerMachine implementation
      public override void RunAsync()
      {
        m_Context.SetupUserInterface( Events.Cancel );
        m_Context.DisplayStatusURL( Properties.Settings.Default.SiteURL + "(Archiving)" );
        m_Context.ProgressChang( this, new EntitiesChangedEventArgs( 1, "Data archival in progress - it could take several minutes.", null ).UserState );
        base.RunAsync();
      }
      protected override void BackgroundWorker_DoWork( object sender, DoWorkEventArgs e )
      {
        FeatureActivation.Archival.Archive.Go( Properties.Settings.Default.SiteURL, ReportProgress );
      }
      protected override void RunWorkerCompleted()
      {
        m_Context.AssignStateMachine( ProcessState.Finisched );
      }
      #endregion
    }
    internal class FinishedMachine: AbstractMachine
    {
      internal FinishedMachine( StateMachineContext context )
        : base( context )
      { }
      internal override void EnteringState()
      { }
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
