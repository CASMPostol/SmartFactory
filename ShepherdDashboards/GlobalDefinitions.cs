using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  public class GlobalDefinitions
  {
    public enum Roles
    {
      InboundOwner,
      OutboundOwner,
      Coordinator,
      Supervisor,
      Operator,
      Vendor,
      Forwarder,
      Escort,
      Guard, 
      None,
    }
    internal const string NumberOfTimeSLotsFormat = "<font size=\"1\" color=\"#ff0000\"> [{0}]</font>";
    public delegate void UpdateToolStripEvent(object obj, ProgressChangedEventArgs progres);
  }
}
