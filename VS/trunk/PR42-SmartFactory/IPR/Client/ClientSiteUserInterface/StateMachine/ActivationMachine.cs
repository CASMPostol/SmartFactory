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

using System.ComponentModel;

namespace CAS.SmartFactory.IPR.Client.UserInterface.StateMachine
{
  internal class ActivationMachine : BackgroundWorkerMachine<ViewModel.MainWindowModel>
  {

    #region creator
    internal ActivationMachine(ViewModel.MainWindowModel context)
      : base(context)
    {
      m_Me = this;
    }
    #endregion

    internal static ActivationMachine Get()
    {
      return m_Me;
    }

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

    #region private

    #region BackgroundWorkerMachine implementation
    protected override void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
    {
      int _counteri = 0;
      while (true)
      {
        System.Threading.Thread.Sleep(1000);
        if (ReportProgress(this, new ProgressChangedEventArgs(_counteri, System.String.Format("I am on the {0} round.", _counteri))))
        {
          e.Cancel = true;
          e.Result = null;
          return;
        }
        _counteri++;
      }
      DataManagement.Activate180.Activate.Go(Properties.Settings.Default.SiteURL, Properties.Settings.Default.DoActivate1800, ReportProgress);
    }
    protected override void RunWorkerCompleted()
    {
      Context.Machine = ArchivingMachine.Get();
    }
    public override void OnException(System.Exception exception)
    {
      Context.Exception(exception);
      Context.Machine = FinishedMachine.Get();
    }
    public override void OnCancellation()
    {
      Context.Machine = FinishedMachine.Get();
    }
    #endregion

    private static ActivationMachine m_Me;

    #endregion

  }
}
