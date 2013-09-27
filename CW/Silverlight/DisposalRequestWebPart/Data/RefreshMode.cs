
namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// Specifies how the list item changing system of the LINQ to SharePoint provider will respond when it finds that 
  /// a list item has been changed by another process since it was retrieved.
  /// </summary>
  public enum RefreshMode
  {
    // Summary:
    //     
    /// <summary>
    /// Accept every user’s changes with prejudice for the current user. This means: When applied to a Microsoft.SharePoint.Linq.MemberChangeConflict object: 
    /// Keep the current client value if it has changed since originally retrieved; otherwise make it match the current database value.When applied to an Microsoft.SharePoint.Linq.ObjectChangeConflict 
    /// object: Keep the new values for fields that the current version has changed since the original retrieval, even if they conflict with the latest version in the database; 
    /// but all other fields should be changed as needed to match the latest version in the database.
    /// </summary>
    KeepChanges = 0,
    //
    // Summary:
    //     
    /// <summary>
    /// Rollback all other users’ changes. This means: When applied to a Microsoft.SharePoint.Linq.MemberChangeConflict
    /// object: Keep the current client value. (So, if it has changed since originally
    /// retrieved keep the new value; otherwise leave it at the original value.)When
    /// applied to an Microsoft.SharePoint.Linq.ObjectChangeConflict object: Keep
    /// the new values for fields that the current version has changed since the
    /// original retrieval, even if they conflict with the latest version in the
    /// database; but all other fields should remain as they were when originally
    /// retrieved, even if those values no longer match the latest version in the
    /// database.
    /// </summary>
    KeepCurrentValues = 1,
    //
    // Summary:
    //     
    /// <summary>
    /// Give absolute prejudice to the database version. This means: When applied
    /// to a Microsoft.SharePoint.Linq.MemberChangeConflict object: Match the current
    /// database value.When applied to an Microsoft.SharePoint.Linq.ObjectChangeConflict
    /// object: Make all values match the latest version in the database.
    /// </summary>
    OverwriteCurrentValues = 2,
  }
}
