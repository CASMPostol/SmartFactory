using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class SupplementData2hEscortTemplate : IEmailGrnerator
  {
    public string Title { get; set; }
    public DateTime StartTime { get; set; }
    public string PartnerTitle { get; set; }
    public string Subject { get; set; }
    public string ShippingTitle{ get; set; }
  }
}
