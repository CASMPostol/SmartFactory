using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.SharePoint.Client;
using SPCList = Microsoft.SharePoint.Client.List;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  internal class EntityListItemsCollection<TEntity>: EntityListData, IEntityListItemsCollection
    where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {
    internal EntityListItemsCollection( DataContext dataContext, string listName )
    {
      CreateStorageDescription( typeof( TEntity ) );
      // TODO: Complete member initialization
      // Prepare a reference to the list
      m_list = dataContext.m_RootWeb.Lists.GetByTitle( listName );
      m_DataContext.m_ClientContext.Load( m_list );
      // Execute the prepared commands against the target ClientContext
      m_DataContext.m_ClientContext.ExecuteQuery();
      this.m_DataContext = dataContext;
    }

    #region IEntityListItemsCollection Members
    public void SubmitingChanges()
    {
      if ( m_Unchaged )
      {
        Debug.Assert( !( from _x in m_Collection where _x.Value.TEntityGetter.EntityState != EntityState.Unchanged select _x ).Any(), "Wrong value of Unchanged in the SubmitingChanges - expected false" );
        return;
      }
      Debug.Assert( ( from _x in m_Collection where _x.Value.TEntityGetter.EntityState != EntityState.Unchanged select _x ).Any(), "Wrong value of Unchanged in the SubmitingChanges - expected true" );
      SubmitingChanges4Parents();
      int _cntrNew = 0;
      int _cntrUpdt = 0;
      Dictionary<string, StorageItem> _entityDictionary = m_StorageDescription.ToDictionary<StorageItem, string>( storageItem => storageItem.PropertyName );
      foreach ( var _itemX in m_Collection.Values )
      {
        ITrackEntityState _entity = _itemX.TEntityGetter;
        ListItem _listItem = _itemX.MyListItem;
        switch ( _entity.EntityState )
        {
          case EntityState.ToBeInserted:
            Debug.Assert( _listItem == null, "Value is not null: Inconsistent content of association table, ToBeInserted should not has associated ListItem" );
            _listItem = m_list.AddItem( new ListItemCreationInformation() );
            _itemX.GetValuesFromEntity( _entityDictionary );
            _listItem.Update();
            m_DataContext.ReloadNewListItem( _listItem );
            _cntrNew++;
            break;
          case EntityState.ToBeUpdated:
            Debug.Assert( _listItem != null, "VAlue", "Inconsistent content of association table, ToBeUpdated should has associated ListItem" );
            _itemX.GetValuesFromEntity( _entityDictionary );
            _listItem.Update();
            m_DataContext.ExecuteQuery();
            _cntrUpdt++;
            break;
          case EntityState.Unchanged:
          case EntityState.ToBeRecycled:
          case EntityState.ToBeDeleted:
          case EntityState.Deleted:
            break;
        }
        _entity.EntityState = EntityState.Unchanged;
      }
      //TODO Add trace 
      m_Unchaged = true;
    }


    #endregion

    #region internal
    internal FieldLookupValue GetFieldLookupValue( TEntity entity )
    {
      Dictionary<TEntity, TEntityWrapper<TEntity>> m_EntitieAssociations = m_Collection.ToDictionary( key => key.Value.TEntityGetter, val => val.Value, new EqualityComparer() );
      FieldLookupValue _ret = new FieldLookupValue()
      {
        LookupId = entity == null ? -1 : m_EntitieAssociations[ entity ].Index
      };
      Debug.Assert( _ret.LookupId > 0, "Unexpected null reference to existing Entity" );
      return _ret;
    }
    internal TEntity GetFieldLookupValue( FieldLookupValue fieldLookupValue )
    {
      if ( fieldLookupValue.LookupId < 0 )
        return null;
      if ( !this.ContainsKey( fieldLookupValue.LookupId ) )
        return null; //TODO read ListItem
      return m_Collection[ fieldLookupValue.LookupId ].TEntityGetter;
    }
    internal EntityList<TEntity> GetList()
    {
      return new EntityList<TEntity>( m_DataContext, this );
    }
    internal ListItemCollection GetItems( CamlQuery query )
    {
      return m_list.GetItems( query );
    }
    internal void Add( DataContext dataContext, TEntity entity )
    {
      TEntityWrapper<TEntity> _wrpr = new TEntityWrapper<TEntity>( m_DataContext, entity, PropertyChanged );
      entity.PropertyChanged += PropertyChanged;
      m_Collection.Add( _wrpr.Index, _wrpr );
    }
    internal TEntity Add( DataContext dataContext, ListItem listItem )
    {
      Dictionary<string, StorageItem> _storageDic = m_StorageDescription.ToDictionary<StorageItem, string>( storageItem => storageItem.Description.Name );
      TEntityWrapper<TEntity> _erp = new TEntityWrapper<TEntity>( m_DataContext, listItem, _storageDic, PropertyChanged );
      TEntity _newEntity = _erp.TEntityGetter;
      m_Collection.Add( _erp.Index, _erp );
      return _newEntity;
    }
    internal TEntity this[ int index ]
    {
      get { return m_Collection[ index ].TEntityGetter; }
    }
    internal bool ContainsKey( int index )
    {
      return m_Collection.ContainsKey( index );
    }
    #endregion

    #region private

    #region IEqualityComparer<object> Members
    class EqualityComparer: IEqualityComparer<TEntity>
    {
      public bool Equals( TEntity x, TEntity y )
      {
        return Object.ReferenceEquals( x, y );
      }
      public int GetHashCode( TEntity obj )
      {
        return obj.GetHashCode();
      }
    }
    #endregion


    private bool m_Unchaged = true;
    private Dictionary<int, TEntityWrapper<TEntity>> m_Collection = new Dictionary<int, TEntityWrapper<TEntity>>();
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
            continue; //TODOD should be implemented
          StorageItem _newStorageItem = new StorageItem( _ax.Name, _dataAttribute, _mmbrs[ _dataAttribute.Storage ] as FieldInfo );
          m_StorageDescription.Add( _newStorageItem );
        }
      }
      if ( type.BaseType != typeof( Object ) )
        CreateStorageDescription( type.BaseType );
    }
    private static Dictionary<string, MemberInfo> GetMembers( Type type )
    {
      BindingFlags _flgs = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic;
      Dictionary<string, MemberInfo> _mmbrs = ( from _midx in type.GetMembers( _flgs )
                                                where _midx.MemberType == MemberTypes.Field || _midx.MemberType == MemberTypes.Property
                                                select _midx ).ToDictionary<MemberInfo, string>( _mi => _mi.Name );
      return _mmbrs;
    }
    private SPCList m_list = default( SPCList );
    private DataContext m_DataContext = default( DataContext );
    protected List<StorageItem> m_StorageDescription = new List<StorageItem>();
    private void SubmitingChanges4Parents()
    {
      foreach ( StorageItem _si in m_StorageDescription )
      {
        if ( !_si.IsLookup )
          continue;
        m_DataContext.SubmitChanges( ( (AssociationAttribute)_si.Description ).List );
      };
    }
    private void PropertyChanged( object sender, PropertyChangedEventArgs e )
    {
      m_Unchaged = false;
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


    #endregion


  }
}
