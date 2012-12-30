using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.SharePoint.Client;
using SPCList = Microsoft.SharePoint.Client.List;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  /// Represents a Windows SharePoint Services "14" list that can be queried with Language Integrated Query (LINQ).
  /// </summary>
  /// <typeparam name="TEntity">The content type of the list items.</typeparam>
  public sealed class EntityList<TEntity>: EntityListData, IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IEnumerable
    where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {
    #region ctor
    internal EntityList( DataContext dataContext, string listName )
    {
      this.m_DataContext = dataContext;
      this.m_ListName = listName;
      // Prepare a reference to the list of "DevLeap Contacts"
      m_list = dataContext.m_RootWeb.Lists.GetByTitle( listName );
      dataContext.m_ClientContext.Load( m_list );
      // Execute the prepared commands against the target ClientContext
      dataContext.m_ClientContext.ExecuteQuery();
      // Prepare a query for all items in the list
      CamlQuery query = new CamlQuery();
      query.ViewXml = "<View/>";
      m_allItems = m_list.GetItems( query );
      dataContext.m_ClientContext.Load( m_allItems );
      // Execute the prepared command against the target ClientContext
      dataContext.m_ClientContext.ExecuteQuery();
      foreach ( ListItem _listItemx in m_allItems )
      {
        TEntity _newEntity = new TEntity();
        Associate( _newEntity, _listItemx );
        AssignValues2Entity( _newEntity, _newEntity.GetType(), name => _listItemx.FieldValues.ContainsKey( name ) ? _listItemx[ name ] : null );
        _newEntity.PropertyChanging += _newEntity_PropertyChanging;
        _newEntity.PropertyChanged += _newEntity_PropertyChanged;
        _newEntity.EntityState = EntityState.Unchanged;
      }
    }
    #endregion

    #region public
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
    /// <summary>
    /// Marks the specified entity for insertion into the list on the next call of <see cref="Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges"/>.
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
    public void InsertOnSubmit( TEntity entity )
    {
      if ( !m_DataContext.ObjectTrackingEnabled )
        throw new InvalidOperationException( "Object tracking is not enabled for the DataContext object" );
      if ( entity == null )
        throw new ArgumentNullException( "entity", "entity is null." );
      Associate( entity, null );
      entity.EntityState = EntityState.ToBeInserted;
    }
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
    #endregion

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
      return m_allListItems.GetEnumerator();
    }
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return m_allListItems.GetEnumerator();
    }
    #endregion

    #region private
    private List<TEntity> m_allListItems = new List<TEntity>();
    private string m_ListName = String.Empty;
    private ListItemCollection m_allItems = default( ListItemCollection );
    private List<TEntity> m_allItemsEntities = new List<TEntity>();
    private void _newEntity_PropertyChanged( object sender, PropertyChangedEventArgs e )
    {
      ITrackEntityState _entity = sender as ITrackEntityState;
      if ( _entity == null )
        throw new ArgumentNullException( "sender", "PropertyChanged must be called from ITrackEntityState" );
      _entity.EntityState = EntityState.ToBeUpdated;
    }
    private void _newEntity_PropertyChanging( object sender, PropertyChangingEventArgs e )
    {
      //Do nothing
    }
    /// <summary>
    /// Gets the entity.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    internal protected override ITrackEntityState GetEntity( int key )
    {
      return m_allListItems[ key ];
    }
    private void Associate( TEntity entity, ListItem listItem )
    {
      m_EntitieAssociations.Add( m_allListItems.Count, listItem );
      m_allListItems.Add( entity );
    }
    #endregion
  }
}
