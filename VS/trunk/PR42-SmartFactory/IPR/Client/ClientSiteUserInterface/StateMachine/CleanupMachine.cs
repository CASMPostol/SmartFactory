
using CAS.SharePoint.ViewModel.Wizard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  /// <summary>
  /// Cleanup state for the machine <see cref="ViewModel.MainWindowModel"/>
  /// </summary>
  internal class CleanupMachine : BackgroundWorkerMachine<ViewModel.MainWindowModel>
  {
    public CleanupMachine(ViewModel.MainWindowModel context)
      : base(context)
    { }
    #region BackgroundWorkerMachine
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      SetEventMask(Events.Cancel);
      base.RunAsync();
    }
    protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      while (true)
      {
        try
        {
          DataManagement.CleanupContent.Go(Properties.Settings.Default.SiteURL, ReportProgress);
          break;
        }
        catch (WebException _we)
        {
          ReportProgress(this, new ProgressChangedEventArgs(0, _we.InnerException == null ? _we.Message : _we.InnerException.Message));
        }
        catch (Exception)
        {
          throw;
        }
      }
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
    public override void OnCancellation()
    {

      Context.EnterState<FinishedMachine>();
    }
    #endregion
    public override string ToString()
    {
      return "Cleanup";
    }

  }
}
