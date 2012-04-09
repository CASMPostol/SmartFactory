using System;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  public partial class DelayedShippingVendorTemplate : IEmailGrnerator
  {
    internal string TruckTitle { get; set; }

    #region IEmailGrnerator
    public DateTime StartTime { get; set; }
    public string ShippingTitle { get; set; }
    public string PartnerTitle { get; set; }
    public string Subject { get; set; }
    #endregion
  }
}
