using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SharePoint.Linq
{
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
