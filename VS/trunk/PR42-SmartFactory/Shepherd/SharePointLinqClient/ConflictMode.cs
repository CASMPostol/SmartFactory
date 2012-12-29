
namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Specifies when an attempt to submit changes to a list should be stopped and
  //     rolled back.
  public enum ConflictMode
  {
    // Summary:
    //     Attempt all changes and, when done, if there have been any concurrency conflicts,
    //     throw a Microsoft.SharePoint.Linq.ChangeConflictException exception, populate
    //     Microsoft.SharePoint.Linq.DataContext.ChangeConflicts, and rollback all changes.
    ContinueOnConflict = 0,
    //
    // Summary:
    //     Throw a Microsoft.SharePoint.Linq.ChangeConflictException exception when
    //     the first concurrency change conflict is found, stop making changes, populate
    //     Microsoft.SharePoint.Linq.DataContext.ChangeConflicts, and rollback all changes
    //     that were made to that point.
    FailOnFirstConflict = 1,
  }
}
