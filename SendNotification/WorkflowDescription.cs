using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.SendNotification
{
  internal struct WorkflowDescription
  {
    internal Guid WorkflowId { get; private set; }
    internal string Name { get; private set; }
    internal string Description { get; private set; }
    public WorkflowDescription(Guid _wrkflwId, string _name, string _description)
      : this()
    {
      WorkflowId = _wrkflwId;
      Name = _name;
      Description = _description;
    }
  }
}
