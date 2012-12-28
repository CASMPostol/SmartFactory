using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Represents a Windows SharePoint Services "14" list that can be queried with
  //     Language Integrated Query (LINQ).
  //
  // Type parameters:
  //   TEntity:
  //     The content type of the list items.
  public sealed class EntityList<TEntity>: List<TEntity>, IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IEnumerable
  {
    // Summary:
    //     Registers a disconnected or "detached" entity with the object tracking system
    //     of the Microsoft.SharePoint.Linq.DataContext object associated with the list.
    //
    // Parameters:
    //   entity:
    //     The entity that is registered.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     entity is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -entity is not of the same type as the list items.- or -entity
    //     has been deleted.- or -There is a problem with the internal ID of entity
    //     that is used by the object tracking system.
    public void Attach( TEntity entity ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entities for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entities:
    //     The entities to be marked for deletion.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     At least one member of entities is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -At least one member of entities is not of the same type as the
    //     list items.
    public void DeleteAllOnSubmit( IEnumerable<TEntity> entities ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entity for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entity:
    //     The entity to be marked for deletion.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     entity is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -entity is not of the same type as the list items.
    public void DeleteOnSubmit( TEntity entity ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Returns an enumerator that iterates through the Microsoft.SharePoint.Linq.EntityList<TEntity>.
    //
    // Returns:
    //     An System.Collections.Generic.IEnumerator<T> that can be used to iterate
    //     the list.
    public IEnumerator<TEntity> GetEnumerator() { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entities for insertion into the list on the next call
    //     of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entities:
    //     The entities to be inserted.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     At least one member of entities is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -At least one member of entities is not of the same type as the
    //     list items.- or -At least one member of entities has been deleted.- or -At
    //     least one member of entities has been updated.- or -There is a problem with
    //     the internal ID of at least one member of entities that is used by the object
    //     tracking system.
    public void InsertAllOnSubmit( IEnumerable<TEntity> entities ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entity for insertion into the list on the next call of
    //     Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entity:
    //     The entity to be marked for insertion.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     entity is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -entity is not of the same type as the list items.- or -entity
    //     has been deleted.- or -entity has been updated.- or -There is a problem with
    //     the internal ID of entity that is used by the object tracking system.
    public void InsertOnSubmit( TEntity entity ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entities to be put in the Recycle Bin on the next call
    //     of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entities:
    //     The entities to be recycled.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     At least one member of entities is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -At least one member of entities is not of the same type as the
    //     list items.
    public void RecycleAllOnSubmit( IEnumerable<TEntity> entities ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Marks the specified entity to be put in the Recycle Bin on the next call
    //     of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    //
    // Parameters:
    //   entity:
    //     The entity to be recycled.
    //
    // Exceptions:
    //   System.ArgumentNullException:
    //     entity is null.
    //
    //   System.InvalidOperationException:
    //     Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    //     object.- or -entity is not of the same type as the list items.
    public void RecycleOnSubmit( TEntity entity ) { throw new NotImplementedException(); }
    //
    // Summary:
    //     Gets the subset of the Microsoft.SharePoint.Linq.EntityList<TEntity> that
    //     consists of all and only the list items that belong to a particular folder,
    //     with or without the items in subfolders.
    //
    // Parameters:
    //   listRelativePath:
    //     The list-relative path to the folder.
    //
    //   recursive:
    //     true to include items in subfolders; false to exclude them.
    //
    // Returns:
    //     An System.Linq.IQueryable<T> object that can be cast to Microsoft.SharePoint.Linq.EntityList<TEntity>.
    public IQueryable<TEntity> ScopeToFolder( string folderUrl, bool recursive ) { throw new NotImplementedException(); }

    #region IQueryable Members

    /// <summary>
    /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
    /// </summary>
    /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public Type ElementType
    {
      get { throw new NotImplementedException(); }
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
