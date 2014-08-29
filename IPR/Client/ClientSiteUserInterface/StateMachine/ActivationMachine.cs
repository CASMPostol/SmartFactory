//<summary>
//  Title   : class ActivationMachine
//  System  : Microsoft Visual C# .NET 2012
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
  internal class ActivationMachine : BackgroundWorkerMachine<ViewModel.MainWindowModel>
  {

    #region creator
    internal ActivationMachine() { }
    #endregion

    #region AbstractMachine
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      SetEventMask(Events.Cancel);
      base.RunAsync();
    }
    public override string ToString()
    {
      return "Updating";
    }
    #endregion

    #region BackgroundWorkerMachine implementation
    public override void Next()
    {
      base.Next();
      Context.EnterState<ArchivingMachine>();
    }
    protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      DataManagement.Activate180.Activate.Go(Properties.Settings.Default.SiteURL, ReportProgress);
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

  }
}
