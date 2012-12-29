using System;
using System.Reflection;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Specifies information about a discrepancy between the value of a field (or
  //     list item metadata property) in the database and its value in the current
  //     process, its client value.
  public sealed class MemberChangeConflict
  {
    // Summary:
    //     Gets the value of the field in the current process, the client value.
    //
    // Returns:
    //     A System.Object that represents the value of the field after the most recent
    //     change, if any, made in the current process.
    public object CurrentValue { get { throw new NotImplementedException(); } }
    //
    // Summary:
    //     Gets the value of the field in the content database.
    //
    // Returns:
    //     A System.Object that represents the value of the field in the database.
    public object DatabaseValue { get { throw new NotImplementedException(); } }
    //
    // Summary:
    //     Gets a value that indicates whether the current user has changed field value
    //     since it was last retrieved from the database.
    //
    // Returns:
    //     true, if the value has been changed by the current process; otherwise, false.
    public bool IsModified { get { throw new NotImplementedException(); } }
    //
    // Summary:
    //     Gets a value indicating whether the discrepancy has been resolved.
    //
    // Returns:
    //     true, if the discrepancy is resolved; otherwise, false.
    public bool IsResolved { get { throw new NotImplementedException(); } }
    //
    // Summary:
    //     Gets metadata information about the property of the list item object that
    //     represents the field for which there is a discrepancy.
    //
    // Returns:
    //     A System.Reflection.MemberInfo object that holds information about the property.
    public MemberInfo Member { get { throw new NotImplementedException(); } }
    //
    // Summary:
    //     Gets the value of the field as it was when it was last retrieved from the
    //     database by the current process.
    //
    // Returns:
    //     A System.Object that represents the value the field had in the database that
    //     last time it was retrieved by this current process.
    public object OriginalValue { get { throw new NotImplementedException(); } }

    // Summary:
    //     Resolves the discrepancy by setting client value of the field (or property)
    //     to the specified object.
    //
    // Parameters:
    //   value:
    //     The value to which the field (or property) should be set.
    public void Resolve( object value ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Resolves the discrepancy using the specified refresh mode.
    //
    // Parameters:
    //   refreshMode:
    //     A value that specifies how to resolve the conflict.
    public void Resolve( RefreshMode refreshMode ) { throw new NotImplementedException(); }
  }
}
