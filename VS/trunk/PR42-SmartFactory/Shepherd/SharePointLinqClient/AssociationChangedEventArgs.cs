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
  public class AssociationChangedEventArgs<TEntity>: EventArgs
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
    public AssociationChangedEventArgs( TEntity item, AssociationChangedState state )
    {
      m_Entity = item;
      m_State = state;
    }
    // Summary:
    //     Gets the entity involved in the change.
    //
    // Returns:
    //     A T that represents the entity involved in the change.
    public TEntity Item { get { return m_Entity; } }
    //
    // Summary:
    //     Gets the type of change.
    //
    // Returns:
    //     An Microsoft.SharePoint.Linq.AssociationChangedState that specifies the type
    //     of change.
    public AssociationChangedState State { get { return m_State; } }

    private AssociationChangedState m_State = default( AssociationChangedState );
    private TEntity m_Entity = default( TEntity );
  }
}
