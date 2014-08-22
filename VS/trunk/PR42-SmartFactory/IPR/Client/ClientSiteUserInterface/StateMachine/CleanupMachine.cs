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
    public CleanupMachine() { }

    #region BackgroundWorkerMachine
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      success = false;
      SetEventMask(Events.Cancel);
      RunAsync();
    }
    public override void Next()
    {
      base.Next();
      if (success)
        Context.EnterState<FinishedMachine>();
      else
        SetEventMask(Events.Cancel | Events.Next | Events.Previous);
    }
    protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      DataManagement.CleanupContent.Go(Properties.Settings.Default.SiteURL, ViewModel.MainWindowModel.GetConnectionString(), ReportProgress);
    }
    protected override void RunWorkerCompleted(object result)
    {
      success = true;
      Next();
    }
    public override void OnException(Exception exception)
    {
      Context.Exception(exception);
      Context.EnterState<FinishedMachine>();
    }
    public override void OnCancellation()
    {
      Context.EnterState<FinishedMachine>();
    }
    #endregion

    public override string ToString()
    {
      return "Cleanup";
    }

    private bool success = false;

  }
}
