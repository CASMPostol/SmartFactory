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
    /// <summary>
    /// Initializes a new instance of the <see cref="WorkflowDescription"/> struct.
    /// </summary>
    /// <param name="_wrkflwId">The identifier.</param>
    /// <param name="_name">The name.</param>
    /// <param name="_description">The description.</param>
    public WorkflowDescription(Guid _wrkflwId, string _name, string _description)
      : this()
    {
      WorkflowId = _wrkflwId;
      Name = _name;
      Description = _description;
    }
  }
}
