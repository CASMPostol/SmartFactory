using System;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class DelayedShippingVendorTemplate : IEmailGrnerator
  {
    internal string TruckTitle { get; set; }
    internal string Title { get; set; }
    internal DateTime StartTime { get; set; }

    #region IEmailGrnerator
    public string PartnerTitle { get; set; }
    public string Subject { get; set; }
    #endregion
  }
}
