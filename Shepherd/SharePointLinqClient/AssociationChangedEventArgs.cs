using System;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Provides data for the OnChanged event of Microsoft.SharePoint.Linq.EntityRef<TEntity>,
  //     Microsoft.SharePoint.Linq.EntitySet<TEntity>, or Microsoft.SharePoint.Linq.LookupList<T>.
  //
  // Type parameters:
  //   T:
  //     The type of the the entity involved in the change.
  public class AssociationChangedEventArgs<T>: EventArgs
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.AssociationChangedEventArgs<T>
    //     class.
    //
    // Parameters:
    //   item:
    //     The entity involved in the change.
    //
    //   state:
    //     An object that specifies the type of change.
    public AssociationChangedEventArgs( T item, AssociationChangedState state ) { throw new NotImplementedException(); }

    // Summary:
    //     Gets the entity involved in the change.
    //
    // Returns:
    //     A T that represents the entity involved in the change.
    public T Item { get { throw new NotImplementedException(); } }
    //
    // Summary:
    //     Gets the type of change.
    //
    // Returns:
    //     An Microsoft.SharePoint.Linq.AssociationChangedState that specifies the type
    //     of change.
    public AssociationChangedState State { get { throw new NotImplementedException(); } }

  }
}
