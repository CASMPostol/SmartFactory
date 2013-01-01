using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    List<TEntity>, DataContext.IAssociationAttribute, DataContext.IRegister,
    ICollection<TEntity>, IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IList, ICollection, IEnumerable, ICloneable
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
    public void Assign( IEnumerable<TEntity> entities ) { this.Assign( entities ); }
    //
    // Summary:
    //     Creates a shallow copy of the Microsoft.SharePoint.Linq.EntitySet<TEntity>.
    //
    // Returns:
    //     A System.Object (castable Microsoft.SharePoint.Linq.EntitySet<TEntity>) whose
    //     property values refer to the same objects as the property values of this
    //     Microsoft.SharePoint.Linq.EntitySet<TEntity>.
    public object Clone() { return this.Clone(); }
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
    public bool Contains( object value ) { return this.Contains( value ); }
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
    public void CopyTo( Array array, int index ) { this.CopyTo( array, index ); }
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
    public int IndexOf( object value ) { return this.IndexOf( value ); }
    //
    // Summary:
    //     Removes the specified object from the Microsoft.SharePoint.Linq.EntitySet<TEntity>.
    //
    // Parameters:
    //   value:
    //     The object that is removed.
    public void Remove( object value ) { this.Remove( value ); }

    #region IQueryable Members
    /// <summary>
    /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
    /// </summary>
    /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
    public Type ElementType
    {
      get { return typeof( TEntity ); }
    }
    /// <summary>
    /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
    /// </summary>
    /// <returns>The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of <see cref="T:System.Linq.IQueryable" />.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public System.Linq.Expressions.Expression Expression
    {
      get { throw new NotImplementedException(); }
    }
    /// <summary>
    /// Gets the query provider that is associated with this data source.
    /// </summary>
    /// <returns>The <see cref="T:System.Linq.IQueryProvider" /> that is associated with this data source.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public IQueryProvider Provider
    {
      get { throw new NotImplementedException(); }
    }
    #endregion

    #region IRegister Members
    void DataContext.IRegister.RegisterInContext( DataContext dataContext, AssociationAttribute associationAttribute )
    {
      m_DataContext = dataContext;
      m_AssociationAttribute = associationAttribute;
      m_InContext = true;
    }
    #endregion

    #region IAssociationAttribute Members
    FieldLookupValue DataContext.IAssociationAttribute.Lookup { get; set; }
    #endregion

    #region private
    private bool m_InContext = false;
    private DataContext m_DataContext = default( DataContext );
    private AssociationAttribute m_AssociationAttribute = default( AssociationAttribute );
    #endregion

  }
}
