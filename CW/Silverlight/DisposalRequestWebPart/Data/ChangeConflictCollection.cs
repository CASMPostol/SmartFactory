using System.Collections;
using System.Collections.Generic;
namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// Represents a collection of Microsoft.SharePoint.Linq.ObjectChangeConflict objects.
  /// </summary>
  public sealed class ChangeConflictCollection: List<ObjectChangeConflict>, IEnumerable<ObjectChangeConflict>, ICollection, IEnumerable
  {
    /// <summary>
    /// Resolves all the concurrency conflicts in the collection.
    /// </summary>
    public void ResolveAll() { }
    /// <summary>
    /// Resolves all the concurrency conflicts in the collection using the specified refresh mode.
    /// </summary>
    /// <param name="refreshMode">A value that specifies how to resolve the conflict.</param>
    public void ResolveAll( RefreshMode refreshMode ) { }
    /// <summary>
    /// Resolves all the concurrency conflicts in the collection using the specified refresh mode and the specified resolution of deleted items.
    /// </summary>
    /// <param name="refreshMode">A value that specifies how to resolve the conflict.</param>
    /// <param name="autoResolveDeletes">true to treat the conflict as resolved if the list item being changed has been deleted from the database; false to throw InvalidOperationException if the list item being changed has been deleted from the database</param>
    public void ResolveAll( RefreshMode refreshMode, bool autoResolveDeletes ) { }
  }
}
