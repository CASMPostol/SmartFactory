//<summary>
//  Title   : class CleanupMachin
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
using System.Net;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  /// <summary>
  /// Cleanup state for the machine <see cref="ViewModel.MainWindowModel"/>
  /// </summary>
  internal class CleanupMachine : BackgroundWorkerMachine<ViewModel.MainWindowModel>
  {

    #region BackgroundWorkerMachine
    /// <summary>
    /// Called when entering the state.
    /// </summary>
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      if (Context.DoCleanupIsChecked || Context.DoSynchronizationIsChecked || Context.DoArchivingIsChecked)
      {
        SetEventMask(Events.Cancel);
        Context.ButtonNextTitle = Properties.Resources.ButtonInactive;
        Context.ButtonGoBackwardTitle = Properties.Resources.ButtonInactive;
        RunAsync();
      }
      else
      {
        this.ReportProgress(this, new ProgressChangedEventArgs(0, "Cleanup skipped because is not selected by the user."));
        Next();
        return;
      }
    }
    public override void Next()
    {
      base.Next();
      if (Success)
        Context.EnterState<SynchronizationMachine>();
      else
        Context.EnterState<FinishedMachine>();
    }
    protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      DataManagement.CleanupContent.Go(Properties.Settings.Default.SiteURL, ViewModel.MainWindowModel.GetConnectionString(), ReportProgress);
    }
    protected override void RunWorkerCompleted(object result)
    {
      Next();
    }
    public override void OnCancellation()
    {
      Next();
    }
    #endregion
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
      return "Cleanup";
    }
  }
}
