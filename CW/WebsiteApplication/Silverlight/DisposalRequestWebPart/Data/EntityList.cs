//<summary>
//  Title   : public sealed class EntityList
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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.SharePoint.Client;
using SPCList = Microsoft.SharePoint.Client.List;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// Represents a Windows SharePoint Services "14" list that can be queried with Language Integrated Query (LINQ).
  /// </summary>
  /// <typeparam name="TEntity">The content type of the list items.</typeparam>
  public sealed class EntityList<TEntity> : IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IEnumerable
    where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {
    #region creator
    internal EntityList(DataContext dataContext, EntityListItemsCollection<TEntity> itemsCollection)
    {
      this.Query = CamlQuery.CreateAllItemsQuery();
      this.m_AllItemsCollection = itemsCollection;
      this.m_DataContext = dataContext;
    }
    #endregion

    #region public
    /// <summary>
    /// Registers a disconnected or "detached" entity with the object tracking system 
    /// of the Microsoft.SharePoint.Linq.DataContext object associated with the list.
    /// </summary>
    /// <param name="entity">The entity that is registered.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">
    ///   Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext object.
    ///- or 
    ///   -entity is not of the same type as the list items.
    /// - or 
    ///   -entity has been deleted.
    /// - or 
    ///   -There is a problem with the internal ID of entity that is used by the object tracking system.
    /// </exception>
    public void Attach(TEntity entity)
    {
      if (!m_DataContext.ObjectTrackingEnabled)
        throw new InvalidOperationException("Object tracking is not enabled for the DataContext object");
      if (entity == null)
        throw new ArgumentNullException("entity", "entity is null.");
      entity.EntityState = EntityState.ToBeInserted;
      //Unchanged = false;
    }
    /// <summary>
    /// Marks the specified entities for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entities">The entities to be marked for deletion.</param>
    /// <exception cref="System.ArgumentNullException">At least one member of entities is null.</exception>
    /// <exception cref="System.InvalidOperationException">
    /// Object tracking is not enabled for the <see cref="DataContext"/> object.
    /// - or -
    /// At least one member of entities is not of the same type as the list items.
    /// </exception>
    public void DeleteAllOnSubmit(IEnumerable<TEntity> entities)
    {
      foreach (TEntity item in entities)
        DeleteOnSubmit(item);
    }
    /// <summary>
    /// Marks the specified entity for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entity">The entity to be marked for deletion.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    ///     object.- or -entity is not of the same type as the list items.</exception>
    public void DeleteOnSubmit(TEntity entity) { throw new NotImplementedException(); }
    /// <summary>
    /// Marks the specified entities for insertion into the list on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges
    /// </summary>
    /// <param name="entities">The entities to be inserted.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the <see cref="Microsoft.SharePoint.Linq.DataContext"/> object.
    /// - or -
    /// entity is not of the same type as the list items.
    /// - or -
    /// entity has been deleted.
    /// - or -
    /// entity has been updated. 
    /// - or -
    /// There is a problem with the internal ID of entity that is used by the object tracking system.
    /// </exception>
    public void InsertAllOnSubmit(IEnumerable<TEntity> entities)
    {
      foreach (TEntity _item in entities)
        InsertOnSubmit(_item);
    }
    /// <summary>
    /// Marks the specified entity for insertion into the list on the next call of <see cref="Microsoft.SharePoint.Linq.DataContext.SubmitChanges()"/>.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the <see cref="Microsoft.SharePoint.Linq.DataContext"/> object.
    /// - or -
    /// entity is not of the same type as the list items.
    /// - or -
    /// entity has been deleted.
    /// - or -
    /// entity has been updated. 
    /// - or -
    /// There is a problem with the internal ID of entity that is used by the object tracking system.
    /// </exception>
    public void InsertOnSubmit(TEntity entity)
    {
      if (!m_DataContext.ObjectTrackingEnabled)
        throw new InvalidOperationException("Object tracking is not enabled for the DataContext object");
      if (entity == null)
        throw new ArgumentNullException("entity", "entity is null.");
      Add(entity);
    }
    /// <summary>
    /// Marks the specified entities to be put in the Recycle Bin on the next call
    /// of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entities">The entities to be recycled.</param>
    /// <exception cref="System.ArgumentNullException">At least one member of entities is null.</exception>
    /// <exception cref=" System.InvalidOperationException">Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    ///     object.- or -At least one member of entities is not of the same type as the
    ///     list items.</exception>
    public void RecycleAllOnSubmit(IEnumerable<TEntity> entities) { throw new NotImplementedException(); }
    /// <summary>
    /// Marks the specified entity to be put in the Recycle Bin on the next call
    /// of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entity">The entity to be recycled.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    ///     object.- or -entity is not of the same type as the list items.</exception>
    public void RecycleOnSubmit(TEntity entity) { throw new NotImplementedException(); }
    /// <summary>
    /// Gets the subset of the Microsoft.SharePoint.Linq.EntityList<TEntity> that
    ///     consists of all and only the list items that belong to a particular folder,
    ///     with or without the items in subfolders.
    /// </summary>
    /// <param name="folderUrl">The list-relative path to the folder.</param>
    /// <param name="recursive">true to include items in subfolders; false to exclude them.</param>
    /// <returns>An System.Linq.IQueryable<T> object that can be cast to Microsoft.SharePoint.Linq.EntityList<TEntity>.</returns>
    public IQueryable<TEntity> ScopeToFolder(string folderUrl, bool recursive) { throw new NotImplementedException(); }

    #region internal
    internal CamlQuery Query { private get; set; }
    #endregion

    #endregion

    #region IQueryable Members
    /// <summary>
    /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
    /// </summary>
    /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public Type ElementType
    {
      get { return typeof(TEntity); }
    }
    /// <summary>
    /// Gets the expression tree that is associated with the instance of <see cref="T:System.Linq.IQueryable" />.
    /// </summary>
    /// <returns>The <see cref="T:System.Linq.Expressions.Expression" /> that is associated with this instance of <see cref="T:System.Linq.IQueryable" />.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public Expression Expression
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

    #region IEnumerable Members
    /// <summary>
    /// Returns an enumerator that iterates through the <see cref="Microsoft.SharePoint.Linq.EntityList<TEntity>"/>.
    /// </summary>
    /// <returns>An <see cref="System.Collections.Generic.IEnumerator<TEntity>"/>  that can be used to iterate the list.</returns>
    public IEnumerator<TEntity> GetEnumerator()
    {
      if (m_2BeExecuted)
        GetListItems();
      return m_LocalItemsCollection.GetEnumerator();
    }
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      if (m_2BeExecuted)
        GetListItems();
      return m_LocalItemsCollection.GetEnumerator();
    }
    #endregion

    #region private
    private DataContext m_DataContext = null;
    private EntityListItemsCollection<TEntity> m_AllItemsCollection = null;
    private List<TEntity> m_LocalItemsCollection = new List<TEntity>();
    private bool m_2BeExecuted = true;
    private void GetListItems()
    {
      ListItemCollection m_ListItemCollection = m_DataContext.GetListItemCollection(m_AllItemsCollection.MyList, Query);
      foreach (ListItem _listItemx in m_ListItemCollection)
        Add(_listItemx);
      m_2BeExecuted = false;
    }
    private void Add(TEntity entity)
    {
      m_AllItemsCollection.Add(entity);
      m_LocalItemsCollection.Add(entity);
    }
    private void Add(ListItem listItem)
    {
      TEntity _newEntity = null;
      if (m_AllItemsCollection.ContainsKey(listItem.Id))
      {
        _newEntity = m_AllItemsCollection[listItem.Id];
        m_LocalItemsCollection.Add(_newEntity);
      }
      else
      {
        _newEntity = m_AllItemsCollection.Add(listItem);
        m_LocalItemsCollection.Add(_newEntity);
      }
    }

    #endregion

  }
}
