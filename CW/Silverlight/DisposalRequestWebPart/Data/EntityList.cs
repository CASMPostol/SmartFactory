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
  public sealed class EntityList<TEntity>: EntityListData, IOrderedQueryable<TEntity>, IQueryable<TEntity>, IEnumerable<TEntity>, IOrderedQueryable, IQueryable, IEnumerable
    where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {
    #region ctor
    internal EntityList( DataContext dataContext, string listName )
      : base( typeof( TEntity ) )
    {
      this.m_DataContext = dataContext;
      this.m_ListName = listName;
      // Prepare a reference to the list
      m_list = dataContext.m_RootWeb.Lists.GetByTitle( listName );
      dataContext.m_ClientContext.Load( m_list );
      // Execute the prepared commands against the target ClientContext
      dataContext.m_ClientContext.ExecuteQuery();
      // Prepare a query for all items in the list
      CamlQuery query = new CamlQuery();
      query.ViewXml = "<View/>";
      ListItemCollection m_ListItemCollection = m_list.GetItems( query );
      dataContext.m_ClientContext.Load( m_ListItemCollection );
      // Execute the prepared command against the target ClientContext
      dataContext.m_ClientContext.ExecuteQuery();
      foreach ( ListItem _listItemx in m_ListItemCollection )
      {
        TEntity _newEntity = new TEntity();
        m_EntitieAssociations.Add( _newEntity, _listItemx );
        AddMetadata( dataContext, _newEntity );
        AssignValues2Entity( _newEntity, _listItemx.FieldValues );
        _newEntity.PropertyChanging += _newEntity_PropertyChanging;
        _newEntity.PropertyChanged += _newEntity_PropertyChanged;
        _newEntity.EntityState = EntityState.Unchanged;
      }
    }
    #endregion

    #region public    
    /// <summary>
    /// Registers a disconnected or "detached" entity with the object tracking system 
    /// of the Microsoft.SharePoint.Linq.DataContext object associated with the list.
    /// </summary>
    /// <param name="entity">The entity that is registered.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    ///     object.- or -entity is not of the same type as the list items.- or -entity
    ///     has been deleted.- or -There is a problem with the internal ID of entity
    ///     that is used by the object tracking system.</exception>
    public void Attach( TEntity entity ) { throw new NotImplementedException(); }     
    /// <summary>
    /// Marks the specified entities for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entities">The entities to be marked for deletion.</param>
    /// <exception cref="System.ArgumentNullException">At least one member of entities is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    ///     object.- or -At least one member of entities is not of the same type as the
    ///     list items.</exception>
    public void DeleteAllOnSubmit( IEnumerable<TEntity> entities ) { throw new NotImplementedException(); }   
    /// <summary>
    /// Marks the specified entity for deletion on the next call of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entity">The entity to be marked for deletion.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    ///     object.- or -entity is not of the same type as the list items.</exception>
    public void DeleteOnSubmit( TEntity entity ) { throw new NotImplementedException(); }
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
    public void InsertAllOnSubmit( IEnumerable<TEntity> entities )
    {
      foreach ( TEntity _item in entities )
        InsertOnSubmit( _item );
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
    public void InsertOnSubmit( TEntity entity )
    {
      if ( !m_DataContext.ObjectTrackingEnabled )
        throw new InvalidOperationException( "Object tracking is not enabled for the DataContext object" );
      if ( entity == null )
        throw new ArgumentNullException( "entity", "entity is null." );
      m_EntitieAssociations.Add( entity, null );
      AddMetadata( m_DataContext, entity );
      entity.EntityState = EntityState.ToBeInserted;
      entity.PropertyChanging += _newEntity_PropertyChanging;
      entity.PropertyChanged += _newEntity_PropertyChanged;
      Unchaged = false;
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
    public void RecycleAllOnSubmit( IEnumerable<TEntity> entities ) { throw new NotImplementedException(); }    
    /// <summary>
    /// Marks the specified entity to be put in the Recycle Bin on the next call
    /// of Overload:Microsoft.SharePoint.Linq.DataContext.SubmitChanges.
    /// </summary>
    /// <param name="entity">The entity to be recycled.</param>
    /// <exception cref="System.ArgumentNullException">entity is null.</exception>
    /// <exception cref="System.InvalidOperationException">Object tracking is not enabled for the Microsoft.SharePoint.Linq.DataContext
    ///     object.- or -entity is not of the same type as the list items.</exception>
    public void RecycleOnSubmit( TEntity entity ) { throw new NotImplementedException(); }   
    /// <summary>
    /// Gets the subset of the Microsoft.SharePoint.Linq.EntityList<TEntity> that
    ///     consists of all and only the list items that belong to a particular folder,
    ///     with or without the items in subfolders.
    /// </summary>
    /// <param name="folderUrl">The list-relative path to the folder.</param>
    /// <param name="recursive">true to include items in subfolders; false to exclude them.</param>
    /// <returns>An System.Linq.IQueryable<T> object that can be cast to Microsoft.SharePoint.Linq.EntityList<TEntity>.</returns>
    public IQueryable<TEntity> ScopeToFolder( string folderUrl, bool recursive ) { throw new NotImplementedException(); }
    internal FieldLookupValue GetFieldLookupValue( TEntity entity )
    {
      FieldLookupValue _ret = new FieldLookupValue()
      {
        LookupId = entity == null ? -1 : m_EntitieAssociations[ entity ].Id
      };
      Debug.Assert( _ret.LookupId > 0, "Unexpected null reference to existing Entity" );
      return _ret;
    }
    internal TEntity GetFieldLookupValue( FieldLookupValue fieldLookupValue )
    {
      if ( fieldLookupValue.LookupId < 0 )
        return null;
      int _dumyKey = -1;
      Dictionary<int, KeyValuePair<ITrackEntityState, ListItem>> _idDictionary = m_EntitieAssociations.ToDictionary( key => key.Value == null ? _dumyKey-- : key.Value.Id );
      if ( !_idDictionary.ContainsKey( fieldLookupValue.LookupId ) )
        return null;
      return (TEntity)_idDictionary[ fieldLookupValue.LookupId ].Key;
    }
    #endregion

    #region IQueryable Members
    /// <summary>
    /// Gets the type of the element(s) that are returned when the expression tree associated with this instance of <see cref="T:System.Linq.IQueryable" /> is executed.
    /// </summary>
    /// <returns>A <see cref="T:System.Type" /> that represents the type of the element(s) that are returned when the expression tree associated with this object is executed.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public Type ElementType
    {
      get { return typeof( TEntity ); }
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
      return ( from _ix in m_EntitieAssociations select _ix.Key ).Cast<TEntity>().GetEnumerator();
    }
    /// <summary>
    /// Returns an enumerator that iterates through a collection.
    /// </summary>
    /// <returns>
    /// An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
    /// </returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
      return ( from _ix in m_EntitieAssociations select _ix.Key ).Cast<TEntity>().GetEnumerator();
    }
    #endregion

    #region private
    private string m_ListName = String.Empty;
    private void _newEntity_PropertyChanged( object sender, PropertyChangedEventArgs e )
    {
      Unchaged = false;
      ITrackEntityState _entity = sender as ITrackEntityState;
      if ( _entity == null )
        throw new ArgumentNullException( "sender", "PropertyChanged must be called from ITrackEntityState" );
      switch ( _entity.EntityState )
      {
        case EntityState.Unchanged:
          _entity.EntityState = EntityState.ToBeUpdated;
          break;
        case EntityState.ToBeInserted:
        case EntityState.ToBeUpdated:
        case EntityState.ToBeRecycled:
        case EntityState.ToBeDeleted:
        case EntityState.Deleted:
          break;
      }
    }
    private void _newEntity_PropertyChanging( object sender, PropertyChangingEventArgs e )
    {
      //Do nothing
    }
    private void AddMetadata( DataContext dataContext, TEntity _newEntity )
    {
      foreach ( StorageItem _item in from _six in m_StorageDescription where _six.IsLookup select _six )
      {
        AssociationAttribute _ass = (AssociationAttribute)_item.Description;
        DataContext.IRegister _entityRef = (DataContext.IRegister)_item.Storage.GetValue( _newEntity );
        _entityRef.RegisterInContext( dataContext, _ass );
      }
    }
    #endregion
  }
}
