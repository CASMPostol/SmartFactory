using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Workflows.WorkflowData
{
  public partial class SupplementData2hEscortTemplate : IEmailGrnerator
  {
    public SupplementData2hEscortTemplate()
    {
      PartnerTitle = String.Empty.NotAvailable();
      Subject = String.Empty.NotAvailable();
      ShippingTitle = String.Empty.NotAvailable();
      StartTime = DateTime.MinValue;
    }
    public DateTime StartTime { get; set; }
    public string PartnerTitle { get; set; }
    public string Subject { get; set; }
    public string ShippingTitle{ get; set; }
  }
}
