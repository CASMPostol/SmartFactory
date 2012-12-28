using System;
using System.Collections.ObjectModel;


namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Specifies information about discrepancies between the current client value
  //     of one or more fields in a list item and the current database values of the
  //     fields.
  public sealed class ObjectChangeConflict
  {
    // Summary:
    //     Gets a value that indicates whether the list item had been deleted from the
    //     database before the Microsoft.SharePoint.Linq.DataContext.SubmitChanges(Microsoft.SharePoint.Linq.ConflictMode)
    //     method was called.
    //
    // Returns:
    //     true if the list item had been deleted; otherwise false.
    public bool IsDeleted { get; internal set; }
    //
    // Summary:
    //     Gets a value indicating whether the discrepancies have been resolved.
    //
    // Returns:
    //     true if the discrepancies are resolved; otherwise false.
    public bool IsResolved { get { throw new NotImplementedException(); } }
    //
    // Summary:
    //     Gets a collection of objects that represent the discrepancies between the
    //     current values of the list item’s fields and the values they have in the
    //     database.
    //
    // Returns:
    //     A System.Collections.ObjectModel.ReadOnlyCollection<T> of Microsoft.SharePoint.Linq.MemberChangeConflict
    //     objects that represent the discrepancies.
    public ReadOnlyCollection<MemberChangeConflict> MemberConflicts { get { throw new NotImplementedException(); } }
    //
    // Summary:
    //     Gets the list item (as an System.Object) for which there is one or more discrepancies.
    //
    // Returns:
    //     The System.Object for which there is one or more discrepancies.
    public object Object { get { throw new NotImplementedException(); } }

    // Summary:
    //     Resolves the discrepancies by assigning each field and property, for which
    //     there is a discrepancy, a value that is persisted to the database on the
    //     next call of Microsoft.SharePoint.Linq.DataContext.SubmitChanges().
    public void Resolve() { throw new NotImplementedException(); }
    //
    // Summary:
    //     Resolves the discrepancies by assigning each field and property, for which
    //     there is a discrepancy, a value that is persisted to the database on the
    //     next call of Microsoft.SharePoint.Linq.DataContext.SubmitChanges().
    //
    // Parameters:
    //   refreshMode:
    //     A value that specifies how to resolve the conflict.
    public void Resolve( RefreshMode refreshMode ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Resolves the discrepancies by assigning each field and property, for which
    //     there is a discrepancy, a value that is persisted to the database on the
    //     next call of Microsoft.SharePoint.Linq.DataContext.SubmitChanges().
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
    public void Resolve( RefreshMode refreshMode, bool autoResolveDeletes ) { throw new NotImplementedException(); }
  }
}
