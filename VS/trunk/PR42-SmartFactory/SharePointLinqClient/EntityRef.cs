using System;

namespace Microsoft.SharePoint.Linq
{
  public class EntityRef<TEntity> where TEntity: class
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     class.
    public EntityRef() { }

    // Summary:
    //     Raised after a change to this Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     object.
    public event EventHandler OnChanged;
    //
    // Summary:
    //     Raised before a change to this Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     object.
    public event EventHandler OnChanging;
    //
    // Summary:
    //     Raised when the Microsoft.SharePoint.Linq.EntityRef<TEntity> object is synchronized
    //     with the entity it represents.
    public event EventHandler<AssociationChangedEventArgs<TEntity>> OnSync;

    // Summary:
    //     Creates a shallow copy of the Microsoft.SharePoint.Linq.EntityRef<TEntity>.
    //
    // Returns:
    //     A System.Object (castable Microsoft.SharePoint.Linq.EntityRef<TEntity>) whose
    //     property values refer to the same objects as the property values of this
    //     Microsoft.SharePoint.Linq.EntityRef<TEntity>.
    public object Clone() { throw new NotImplementedException(); }
    //
    // Summary:
    //     Returns the entity that is wrapped by this Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     object.
    //
    // Returns:
    //     A System.Object that represents the entity that is stored in a private field
    //     of this Microsoft.SharePoint.Linq.EntityRef<TEntity> object.
    public TEntity GetEntity() { throw new NotImplementedException(); }
    //
    // Summary:
    //     Sets the entity to which this Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     refers.
    //
    // Parameters:
    //   entity:
    //     The entity to which the Microsoft.SharePoint.Linq.EntityRef<TEntity> is being
    //     pointed.
    public void SetEntity( TEntity entity ) { throw new NotImplementedException(); }
  }
}
