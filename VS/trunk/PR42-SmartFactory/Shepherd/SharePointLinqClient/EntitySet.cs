using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Provides for deferred loading and relationship maintenance for the “many”
  //     side of one-to-many and many-to-many relationships
  //
  // Type parameters:
  //   TEntity:
  //     The type of the member of the collection.
  public class EntitySet<TEntity>:
    List<TEntity>, DataContext.IAssociationAttribute, ICollection<TEntity>, IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IList, ICollection, IEnumerable, ICloneable
    where TEntity: class
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.EntitySet<TEntity>
    //     class
    public EntitySet() { }

    // Summary:
    //     Raised after a change to this Microsoft.SharePoint.Linq.EntitySet<TEntity>
    //     object.
    public event EventHandler OnChanged;
    //
    // Summary:
    //     Raised before a change to this Microsoft.SharePoint.Linq.EntitySet<TEntity>
    //     object.
    public event EventHandler OnChanging;
    //
    // Summary:
    //     Raised when the Microsoft.SharePoint.Linq.EntitySet<TEntity> object is synchronized
    //     with the entities that it represents.
    public event EventHandler<AssociationChangedEventArgs<TEntity>> OnSync;

    //
    // Summary:
    //     Replaces the entities currently associated with this Microsoft.SharePoint.Linq.EntitySet<TEntity>
    //     with the specified collection.
    //
    // Parameters:
    //   entities:
    //     The collection of entities with which the current set is replaced.
    public void Assign( IEnumerable<TEntity> entities ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Creates a shallow copy of the Microsoft.SharePoint.Linq.EntitySet<TEntity>.
    //
    // Returns:
    //     A System.Object (castable Microsoft.SharePoint.Linq.EntitySet<TEntity>) whose
    //     property values refer to the same objects as the property values of this
    //     Microsoft.SharePoint.Linq.EntitySet<TEntity>.
    public object Clone() { throw new NotImplementedException(); }
    //
    // Summary:
    //     Indicates whether a specified object is in the Microsoft.SharePoint.Linq.EntitySet<TEntity>.
    //
    // Parameters:
    //   value:
    //     The object whose presence is questioned.
    //
    // Returns:
    //     true, if the object is present; false otherwise..
    public bool Contains( object value ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Copies the members of the Microsoft.SharePoint.Linq.EntitySet<TEntity> to
    //     the specified array beginning at the specified array index.
    //
    // Parameters:
    //   array:
    //     The target array.
    //
    //   index:
    //     The zero-based index in the array at which copying begins.
    public void CopyTo( Array array, int index ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Returns the zero-based index of the first occurrence of the specified object
    //     in the collection.
    //
    // Parameters:
    //   value:
    //     The object whose index is returned.
    //
    // Returns:
    //     A System.Int32 that represents the zero-based index of the specified entity
    //     in the Microsoft.SharePoint.Linq.EntitySet<TEntity>.
    public int IndexOf( object value ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Removes the specified object from the Microsoft.SharePoint.Linq.EntitySet<TEntity>.
    //
    // Parameters:
    //   value:
    //     The object that is removed.
    public void Remove( object value ) { throw new NotImplementedException(); }

    #region IQueryable Members
    public Type ElementType
    {
      get { return typeof( TEntity ); }
    }
    public System.Linq.Expressions.Expression Expression
    {
      get { throw new NotImplementedException(); }
    }
    public IQueryProvider Provider
    {
      get { throw new NotImplementedException(); }
    }
    #endregion

    #region IAssociationAttribute Members
    AssociationAttribute DataContext.IAssociationAttribute.AssociationAttribute { get; set; }
    FieldLookupValue DataContext.IAssociationAttribute.Lookup { get; set; }
    #endregion
  }
}
