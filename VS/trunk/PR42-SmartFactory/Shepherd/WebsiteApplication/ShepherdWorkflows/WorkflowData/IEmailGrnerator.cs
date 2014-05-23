using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification.WorkflowData
{
  internal interface IEmailGrnerator
  {
    string PartnerTitle { get; set; }
    string Subject { get; set; }
    DateTime StartTime { get; set; }
    string ShippingTitle{ get; set; }
    string TransformText();
  }
}
