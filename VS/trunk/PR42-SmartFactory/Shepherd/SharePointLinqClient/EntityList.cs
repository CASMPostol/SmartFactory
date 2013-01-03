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
      CreateStorageDescription( typeof( TEntity ) );
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
        RegisterEntity( _newEntity, dataContext, _listItemx );
        AssignValues2Entity( _newEntity, _listItemx.FieldValues );
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
      RegisterEntity( entity, m_DataContext, null );
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
    internal FieldLookupValue GetFieldLookupValue( TEntity entity )
    {

      FieldLookupValue _ret = new FieldLookupValue()
      {
        LookupId = entity == null ? -1 : m_EntitieAssociations[ entity ].Id
      };
      return _ret;
    }
    internal TEntity GetFieldLookupValue( FieldLookupValue fieldLookupValue )
    {
      if ( fieldLookupValue.LookupId < 0 )
        return null;
      Dictionary<int, KeyValuePair<ITrackEntityState, ListItem>> _idDictionary = m_EntitieAssociations.ToDictionary( key => key.Value.Id );
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
      ITrackEntityState _entity = sender as ITrackEntityState;
      if ( _entity == null )
        throw new ArgumentNullException( "sender", "PropertyChanged must be called from ITrackEntityState" );
      _entity.EntityState = EntityState.ToBeUpdated;
    }
    private void _newEntity_PropertyChanging( object sender, PropertyChangingEventArgs e )
    {
      //Do nothing
    }
    private void RegisterEntity( TEntity entity, DataContext DataContext, ListItem listItem )
    {
      m_EntitieAssociations.Add( entity, listItem );
      entity.PropertyChanging += _newEntity_PropertyChanging;
      entity.PropertyChanged += _newEntity_PropertyChanged;
      foreach ( StorageItem _item in from _six in m_StorageDescription where _six.Association select _six )
      {
        AssociationAttribute _ass = (AssociationAttribute)_item.Description;
        DataContext.IRegister _ListRef = (DataContext.IRegister)_item.Storage.GetValue( entity );
        _ListRef.RegisterInContext( DataContext, _ass );
      }
    }
    private void CreateStorageDescription( Type type )
    {
      Dictionary<string, MemberInfo> _mmbrs = GetMembers( type );
      foreach ( MemberInfo _ax in from _pidx in _mmbrs where _pidx.Value.MemberType == MemberTypes.Property select _pidx.Value )
      {
        foreach ( Attribute _cax in _ax.GetCustomAttributes( false ) )
        {
          if ( _cax is RemovedColumnAttribute )
            continue;
          DataAttribute _dataAttribute = _cax as DataAttribute;
          if ( _dataAttribute == null )
            continue;
          ColumnAttribute _columnAttribute = _cax as ColumnAttribute;
          if ( _columnAttribute != null && _columnAttribute.IsLookupValue )
            continue;
          StorageItem _newStorageItem = new StorageItem( _ax.Name, _cax is AssociationAttribute, _dataAttribute, _mmbrs[ _dataAttribute.Storage ] as FieldInfo );
          m_StorageDescription.Add( _newStorageItem );
        }
      }
      if ( type.BaseType != typeof( Object ) )
        CreateStorageDescription( type.BaseType );
    }
    protected internal static Dictionary<string, MemberInfo> GetMembers( Type type )
    {
      BindingFlags _flgs = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic;
      Dictionary<string, MemberInfo> _mmbrs = ( from _midx in type.GetMembers( _flgs )
                                                where _midx.MemberType == MemberTypes.Field || _midx.MemberType == MemberTypes.Property
                                                select _midx ).ToDictionary<MemberInfo, string>( _mi => _mi.Name );
      return _mmbrs;
    }
    #endregion
  }
}
