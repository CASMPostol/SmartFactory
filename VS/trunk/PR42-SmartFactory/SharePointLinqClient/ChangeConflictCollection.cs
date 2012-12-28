using System.Collections;
using System.Collections.Generic;
namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Represents a collection of Microsoft.SharePoint.Linq.ObjectChangeConflict
  //     objects.
  public sealed class ChangeConflictCollection: List<ObjectChangeConflict>, IEnumerable<ObjectChangeConflict>, ICollection, IEnumerable
  {
    //
    // Summary:
    //     Resolves all the concurrency conflicts in the collection.
    public void ResolveAll() { }
    //
    // Summary:
    //     Resolves all the concurrency conflicts in the collection using the specified
    //     refresh mode.
    //
    // Parameters:
    //   refreshMode:
    //     A value that specifies how to resolve the conflict.
    public void ResolveAll( RefreshMode refreshMode ) { }
    //
    // Summary:
    //     Resolves all the concurrency conflicts in the collection using the specified
    //     refresh mode and the specified resolution of deleted items.
    //
    // Parameters:
    //   refreshMode:
    //     A value that specifies how to resolve the conflict.
    //
    //   autoResolveDeletes:
    //     true to treat the conflict as resolved if the list item being changed has
    //     been deleted from the database; false to throw InvalidOperationException
    //     if the list item being changed has been deleted from the database
    //
    // Exceptions:
    //   System.InvalidOperationException:
    //     autoResolveDeletes is false and the list item being updated has been deleted
    //     from the database.
    public void ResolveAll( RefreshMode refreshMode, bool autoResolveDeletes ) { }
  }
}
