using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.SharePoint.Client;
using SPCList = Microsoft.SharePoint.Client.List;

namespace Microsoft.SharePoint.Linq
{
  public abstract class EntityListData
  {

    internal void SubmitingChanges()
    {
      Dictionary<int, ListItem> _newEntitieAssociations = new Dictionary<int, ListItem>();
      foreach ( var item in m_EntitieAssociations )
      {
        ITrackEntityState _entity = GetEntity( item.Key );
        ListItem _newListItem = item.Value;
        switch ( _entity.EntityState )
        {
          case EntityState.ToBeInserted:
            if ( item.Value != null )
              throw new ApplicationException( "Inconsistent content of association table, ToBeInserted should not has associated ListItem" );
            _newListItem = m_list.AddItem( new ListItemCreationInformation() );
            GetValuesFromEntity( (ITrackOriginalValues)_entity, ( na, val ) => _newListItem[ na ] = val );
            _newListItem.Update();
            break;
          case EntityState.ToBeUpdated:
            if ( item.Value == null )
              throw new ArgumentNullException( "VAlue", "Inconsistent content of association table, ToBeUpdated should has associated ListItem" );
            GetValuesFromEntity( (ITrackOriginalValues)_entity, ( na, val ) => item.Value[ na ] = val );
            item.Value.Update();
            break;
          case EntityState.Unchanged:
          case EntityState.ToBeRecycled:
          case EntityState.ToBeDeleted:
          case EntityState.Deleted:
            break;
        }
        _newEntitieAssociations.Add( item.Key, _newListItem );
      }
      m_EntitieAssociations = _newEntitieAssociations;
    }
    internal void SubmitedChanges()
    {
      foreach ( var item in m_EntitieAssociations )
      {
        ITrackEntityState _entity = GetEntity( item.Key );
        switch ( _entity.EntityState )
        {
          case EntityState.ToBeInserted:
            if ( item.Value == null )
              throw new ArgumentNullException( "Value", "Inconsistent content of association table while excuting SubmitedChanges, ToBeUpdated/ToBeInserted should has associated ListItem" );
            _entity.EntityState = EntityState.ToBeUpdated;
            m_DataContext.m_ClientContext.Load( item.Value );
            break;
          case EntityState.ToBeUpdated:
            if ( item.Value == null )
              throw new ArgumentNullException( "Value", "Inconsistent content of association table while excuting SubmitedChanges, ToBeUpdated/ToBeInserted should has associated ListItem" );
            AssignValues2Entity( _entity, item.Value.FieldValues );
            _entity.EntityState = EntityState.Unchanged;
            break;
          case EntityState.Unchanged:
          case EntityState.ToBeRecycled:
          case EntityState.ToBeDeleted:
          case EntityState.Deleted:
            break;
        }
      }
    }
    internal protected abstract ITrackEntityState GetEntity( int key );
    internal protected Dictionary<int, ListItem> m_EntitieAssociations = new Dictionary<int, ListItem>();
    private void GetValuesFromEntity( ITrackOriginalValues entity, Method<string, object> assign )
    {
      Dictionary<string, StorageItem> _storageDic = m_StorageDescription.ToDictionary<StorageItem, string>( key => key.PropertyName );
      foreach ( var item in entity.OriginalValues )
      {
        StorageItem _storageItem = _storageDic[ item.Key ];
        object _value = _storageItem.Storage.GetValue( entity );
        if ( _storageItem.Association )
        {
          AssociationAttribute _attr = (AssociationAttribute)_storageItem.Description;
          if ( _attr.MultivalueType != AssociationType.Single )
            throw new ApplicationException( "Unexpected MultivalueType in the GetValuesFromEntity" );
          _value = ( (DataContext.IAssociationAttribute)_value ).Lookup;
        }
        assign( item.Key, _value );
      }
    }
    protected internal class StorageItem
    {
      internal StorageItem( string propertyName, bool association, DataAttribute description, FieldInfo storage )
      {
        PropertyName = propertyName;
        Association = association;
        Description = description;
        Storage = storage;
      }
      internal string PropertyName { get; private set; }
      internal bool Association { get; private set; }
      internal DataAttribute Description { get; private set; }
      internal FieldInfo Storage { get; private set; }
    }
    protected internal List<StorageItem> m_StorageDescription = new List<StorageItem>();
    protected internal void AssignValues2Entity<TEntity>( TEntity _newEntity, Dictionary<string, object> values )
      where TEntity: class
    {
      Dictionary<string, StorageItem> _storageDic = m_StorageDescription.ToDictionary<StorageItem, string>( key => key.Description.Name );
      foreach ( KeyValuePair<string, object> _item in values )
      {
        StorageItem _storage = _storageDic[ _item.Key ];
        if ( _storage.Association )
        {
          DataContext.IAssociationAttribute _itemRef = (DataContext.IAssociationAttribute)_storage.Storage.GetValue( _newEntity );
          _itemRef.Lookup = (FieldLookupValue)_item.Value;
        }
        else
          _storage.Storage.SetValue( _newEntity, _item.Value );
      }
    }

    protected internal static Dictionary<string, MemberInfo> GetMembers( Type type )
    {
      BindingFlags _flgs = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic;
      Dictionary<string, MemberInfo> _mmbrs = ( from _midx in type.GetMembers( _flgs )
                                                where _midx.MemberType == MemberTypes.Field || _midx.MemberType == MemberTypes.Property
                                                select _midx ).ToDictionary<MemberInfo, string>( _mi => _mi.Name );
      return _mmbrs;
    }
    private delegate void Method<T1, T2>( T1 arg1, T2 arg2 );
    protected internal SPCList m_list = default( SPCList );
    protected internal DataContext m_DataContext = default( DataContext );
  }
}
