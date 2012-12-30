using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Records the changed state of an entity (usually a list item; but possibly
  //     a detached entity).
  public enum EntityState
  {
    // Summary:
    //     The entity is not changed.
    Unchanged = 0,
    //
    // Summary:
    //     The entity will be inserted into a list.
    ToBeInserted = 1,
    //
    // Summary:
    //     The entity will be updated.
    ToBeUpdated = 2,
    //
    // Summary:
    //     The entity will be recycled.
    ToBeRecycled = 3,
    //
    // Summary:
    //     The entity will be deleted.
    ToBeDeleted = 4,
    //
    // Summary:
    //     The entity has been deleted or recycled.
    Deleted = 5,
  }
}
