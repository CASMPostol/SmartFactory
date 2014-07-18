//<summary>
//  Title   : class ArchivingMachine 
//  System  : Microsoft Visual C#
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

using System.ComponentModel;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class ArchivingMachine : BackgroundWorkerVizardMachine
  {
    #region creator
    internal ArchivingMachine(StateMachineContext context)
      : base(context)
    {
      m_Me = this;
    }
    #endregion

    internal static ArchivingMachine Get()
    {
      return m_Me;
    }

    #region BackgroundWorkerMachine implementation
    internal override void OnEnteringState()
    {
      base.OnEnteringState();
      SetEventMask(Events.Cancel);
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
      Context.Machine = FinishedMachine.Get();
    }
    internal override string State
    {
      get { return "Archiving"; }
    }
    #endregion

    private static ArchivingMachine m_Me;

  }
}
