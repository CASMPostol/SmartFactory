﻿//<summary>
//  Title   : class SynchronizationMachine
//  System  : Microsoft VisulaStudio 2013 / C#
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
using System;
using System.ComponentModel;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class SynchronizationMachine : BackgroundWorkerMachine<ViewModel.MainWindowModel>
  {

    #region BackgroundWorkerMachine implementation
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      Context.ButtonNextTitle = Properties.Resources.ButtonInactive;
      Context.ButtonGoBackwardTitle = Properties.Resources.ButtonInactive;
      if (Context.DoSynchronizationIsChecked || Context.DoArchivingIsChecked)
      {
        SetEventMask(Events.Cancel);
        RunAsync();
      }
      else
      {
        this.ReportProgress(this, new ProgressChangedEventArgs(0, "Synchronization skipped because is not selected by the user."));
        Next();
        return;
      }
    }
    public override void Next()
    {
      base.Next();
      Context.EnterState<ArchivingMachine>();
    }
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      e.Cancel = false;
      DataManagement.SynchronizationContent.SynchronizationSettings _setting;
      _setting.SiteURL = Properties.Settings.Default.SiteURL;
      _setting.ConnectionString = ViewModel.MainWindowModel.GetConnectionString();
      _setting.RowLimit = Context.RowLimit;
      ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("Connection string {0}", _setting.ConnectionString)));
      DataManagement.SynchronizationContent.Go(_setting, ReportProgress);
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
      return " Synchronization";
    }
    #endregion

  }
}
