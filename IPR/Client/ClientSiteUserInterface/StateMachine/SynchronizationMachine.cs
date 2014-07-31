//<summary>
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class SynchronizationMachine : BackgroundWorkerMachine<ViewModel.MainWindowModel>
  {
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      SetEventMask(Events.Cancel);
      base.RunAsync();
    }
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      e.Cancel = false;
      DataManagement.SynchronizationContent.SynchronizationSettings _setting;
      _setting.SiteURL = Properties.Settings.Default.SiteURL;
      _setting.ConnectionString = ViewModel.MainWindowModel.GetConnectionString();
      ReportProgress(this, new ProgressChangedEventArgs(1, String.Format("Connection string {0}", _setting.ConnectionString)));
      DataManagement.SynchronizationContent.Go(_setting, ReportProgress);
    }
    protected override void RunWorkerCompleted(object result)
    {
      Context.EnterState<FinishedMachine>();
    }
    public override void OnException(Exception exception)
    {
      Context.Exception(exception);
      Context.EnterState<FinishedMachine>();
    }
    public override string ToString()
    {
      return " Synchronization";
    }
  }
}
