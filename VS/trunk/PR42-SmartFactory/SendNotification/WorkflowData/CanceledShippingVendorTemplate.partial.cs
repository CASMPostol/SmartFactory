using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class CanceledShippingVendorTemplate : IEmailGrnerator
  {
    internal string Title { get; set; }
    internal DateTime StartTime { get; set; }

    #region IEmailGrnerator
    public string PartnerTitle { get; set; }
    public string Subject { get; set; }
    #endregion
  }
}
