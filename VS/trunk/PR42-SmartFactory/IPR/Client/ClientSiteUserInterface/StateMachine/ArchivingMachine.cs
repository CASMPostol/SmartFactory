﻿//<summary>
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

using CAS.SharePoint.ViewModel.Wizard;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class ArchivingMachine : BackgroundWorkerMachine<ViewModel.MainWindowModel>
  {

    #region creator
    public ArchivingMachine() { }
    #endregion

    #region BackgroundWorkerMachine implementation
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      SetEventMask(Events.Cancel);
      RunAsync();
    }
    public override void Next()
    {
      base.Next();
      Context.EnterState<FinishedMachine>();
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
    protected override void RunWorkerCompleted(object result)
    {
      Next();
    }
    public override string ToString()
    {
      return "Archiving";
    }
    public override void OnCancellation()
    {
      Next();
    }
    #endregion

  }
}