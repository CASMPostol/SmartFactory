//<summary>
//  Title   : EntitySet class
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// Provides for deferred loading and relationship maintenance for the “many” side of one-to-many and many-to-many relationships
  /// </summary>
  /// <typeparam name="TEntity">The type of the member of the collection.</typeparam>
  public class EntitySet<TEntity> :
    List<TEntity>, ICollection<TEntity>, IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IList, ICollection, IEnumerable//, ICloneable
    where TEntity : class
  {
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.EntitySet class
    /// </summary>
    public EntitySet() { }
    /// <summary>
    /// Raised after a change to this Microsoft.SharePoint.Linq.EntitySet object.
    /// </summary>
    public event EventHandler OnChanged;
    /// <summary>
    /// Raised before a change to this Microsoft.SharePoint.Linq.EntitySet object.
    /// </summary>
    public event EventHandler OnChanging;
    /// <summary>
    /// Raised when the Microsoft.SharePoint.Linq.EntitySet object is synchronized with the entities that it represents.
    /// </summary>
    public event EventHandler<AssociationChangedEventArgs<TEntity>> OnSync;
    /// <summary>
    /// Replaces the entities currently associated with this Microsoft.SharePoint.Linq.EntitySet with the specified collection.
    /// </summary>
    /// <param name="entities">The collection of entities with which the current set is replaced.</param>
    public void Assign(IEnumerable<TEntity> entities) { base.AddRange(entities); }
    /// <summary>
    /// Creates a shallow copy of the Microsoft.SharePoint.Linq.EntitySet.
    /// </summary>
    /// <returns>
    /// A System.Object (castable Microsoft.SharePoint.Linq.EntitySet) whose property values refer to the same objects as the property values of this Microsoft.SharePoint.Linq.EntitySet.
    /// </returns>
    public object Clone() { return this.Clone(); }
    /// <summary>
    /// Indicates whether a specified entity is in the <see cref="Microsoft.SharePoint.Linq.EntitySet{TEntity}"/>
    /// </summary>
    /// <param name="value">The <see cref="T:System.Object" />The object whose presence is questioned.</param>
    /// <returns>
    /// Indicates whether a specified object is in the Microsoft.SharePoint.Linq.EntitySet.
    /// </returns>
    public new bool Contains(TEntity value) { return base.Contains(value); }
    /// <summary>
    /// Copies the members of the Microsoft.SharePoint.Linq.EntitySet to the specified array beginning at the specified array index.
    /// </summary>
    /// <param name="array">The target array.</param>
    /// <param name="index">The zero-based index in the array at which copying begins.</param>
    public void CopyTo(Array array, int index)
    {
      TEntity[] _array = new TEntity[100];
      CopyTo(_array, index);
      _array.CopyTo(array, index);
    }
    /// <summary>
    /// Returns the zero-based index of the first occurrence of the specified object in the collection.
    /// </summary>
    /// <param name="value">The object whose index is returned.</param>
    /// <returns>
    ///  A System.Int32 that represents the zero-based index of the specified entity in the Microsoft.SharePoint.Linq.EntitySet
    /// </returns>
    public int IndexOf(object value) { return base.IndexOf((TEntity)value); }
    /// <summary>
    /// Removes the specified object from the Microsoft.SharePoint.Linq.EntitySet<TEntity>.
    /// </summary>
    /// <param name="value">The object that is removed.</param>
    public void Remove(object value) { base.Remove((TEntity)value); }

    #region IQueryable Members
    /// <summary>
    /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
    /// </summary>
    /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
    public Type ElementType
    {
      get { return typeof(TEntity); }
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

  }
}
