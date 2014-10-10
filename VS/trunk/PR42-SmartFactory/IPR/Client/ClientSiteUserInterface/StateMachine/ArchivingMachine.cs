//<summary>
//  Title   : class ArchivingMachine 
//  System  : Microsoft Visual C#
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

using CAS.Common.ViewModel.Wizard;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class ArchivingMachine : BackgroundWorkerMachine<ViewModel.MainWindowModel>
  {

    #region BackgroundWorkerMachine implementation
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      Context.ButtonNextTitle = Properties.Resources.ButtonInactive;
      Context.ButtonGoBackwardTitle = Properties.Resources.ButtonInactive;
      if (Context.DoArchivingIsChecked)
      {
        SetEventMask(Events.Cancel);
        RunAsync();
      }
      else
      {
        this.ReportProgress(this, new ProgressChangedEventArgs(0, "Archive operation skipped because is not selected by the user."));
        Next();
        return;
      }
    }
    public override void Next()
    {
      base.Next();
      Context.EnterState<FinishedMachine>();
    }
    protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      DataManagement.ArchiveContent.ArchiveSettings _settings = new DataManagement.ArchiveContent.ArchiveSettings
      {
        ArchiveBatchDelay = Properties.Settings.Default.ArchiveBatchDelay,
        ArchiveIPRDelay = Properties.Settings.Default.ArchiveIPRDelay,
        ReportsArchivalDelay = Properties.Settings.Default.ReportsArchivalDelay,
        SiteURL = Properties.Settings.Default.SiteURL,
        ConnectionString = ViewModel.MainWindowModel.GetConnectionString() 
      };
      DataManagement.ArchiveContent.Go(_settings, ReportProgress);
    }
    protected override void RunWorkerCompleted(object result)
    {
      Next();
    }
    public override void OnCancellation()
    {
      Next();
    }
    public override string ToString()
    {
      return "Archiving";
    }
    #endregion

  }
}
