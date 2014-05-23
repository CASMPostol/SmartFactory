using System;

namespace CAS.SmartFactory.Shepherd.Workflows.ShippingStateMachine
{
  [Serializable]
  internal class MailData
  {
    internal ExternalRole Role { get; set; }
    internal EmailType EmailType { get; set; }
    internal int ShippmentID { get; set; }
    internal string URL { get; set; }
  }
  internal enum ExternalRole { Vendor, Forwarder, Escort }
  internal enum EmailType { Delayed, RequestData, Canceled }
}
