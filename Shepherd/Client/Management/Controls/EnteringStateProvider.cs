using CAS.SmartFactory.Shepherd.Client.Management.StateMachines;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  [Export]
  public class EnteringStateProvider
  {
    internal void ActivateEnteringState(ShellViewModel shellViewModel)
    {
      shellViewModel.EnterState<SetupDataDialogMachine>(); ;
    }
  }
}
