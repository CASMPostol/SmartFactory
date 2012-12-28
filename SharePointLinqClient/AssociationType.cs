using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SharePoint.Linq
{
  public enum AssociationType
  {
    // Summary:
    //     Unspecified or undefined relationship.
    None = 0,
    //
    // Summary:
    //     Forward lookup with a single value.
    Single = 1,
    //
    // Summary:
    //     Forward lookup with multiple values.
    Multi = 2,
    //
    // Summary:
    //     Reverse lookup with multiple values.
    Backward = 3,
  }
  public enum AssociationChangedState
  {
    None = 0,
    //
    // Summary:
    //     A child entity is added.
    Added = 1,
    //
    // Summary:
    //     A child entity is removed.
    Removed = 2,
  }
}
