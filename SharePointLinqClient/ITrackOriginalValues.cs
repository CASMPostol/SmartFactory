using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SharePoint.Linq
{
  public interface ITrackOriginalValues
  {
    IDictionary<string, object> OriginalValues { get; }
  }
}
