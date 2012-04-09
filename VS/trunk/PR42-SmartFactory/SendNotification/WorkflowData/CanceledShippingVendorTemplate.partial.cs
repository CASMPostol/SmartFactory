using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class CanceledShippingVendorTemplate : IEmailGrnerator
  {
    #region IEmailGrnerator
    public DateTime StartTime { get; set; }
    public string PartnerTitle { get; set; }
    public string Subject { get; set; }
    public string ShippingTitle { get; set; }
    #endregion
  }
}
