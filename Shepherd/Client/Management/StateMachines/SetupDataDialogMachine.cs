
using CAS.Common.ViewModel.Wizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{
  internal class SetupDataDialogMachine : BackgroundWorkerMachine<ShellViewModel>
  {

    public SetupDataDialogMachine()
    {

    }
    #region BackgroundWorkerMachine
    protected override void BackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
    {
      throw new NotImplementedException();
    }

    protected override void RunWorkerCompleted(object result)
    {
      throw new NotImplementedException();
    }
    public override void OnEnteringState()
    {
      base.OnEnteringState();
      this.Context.ActivateView(new Controls.SetupPanel());
    }
    #endregion

  }
}
