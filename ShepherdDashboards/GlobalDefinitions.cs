using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
  }
}
