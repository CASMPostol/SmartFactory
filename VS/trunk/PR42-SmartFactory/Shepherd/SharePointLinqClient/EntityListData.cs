using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.SharePoint.Client;
using SPCList = Microsoft.SharePoint.Client.List;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  /// Entity List Data
  /// </summary>
  public abstract class EntityListData
  {
    public EntityListData( Type tEntity )
    {
      CreateStorageDescription( tEntity );
      Unchaged = true;
    }
    internal void SubmitingChanges()
    {
      if ( Unchaged )
      {
        Debug.Assert( ( from _x in m_EntitieAssociations where _x.Key.EntityState != EntityState.Unchanged select _x ).Any(), "Wrong value of Unchanged in the SubmitingChanges" );
        return;
      }
      SubmitingChanges4Parents();
      Dictionary<ITrackEntityState, ListItem> _newEntitieAssociations = new Dictionary<ITrackEntityState, ListItem>();
      foreach ( var item in m_EntitieAssociations )
      {
        ITrackEntityState _entity = item.Key;
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
        _entity.EntityState = EntityState.Unchanged;
        _newEntitieAssociations.Add( item.Key, _newListItem );
      }
      m_EntitieAssociations = _newEntitieAssociations;
      m_DataContext.ExecuteQuery();
      Unchaged = true;
    }
    private void SubmitingChanges4Parents()
    {
      foreach ( StorageItem _si in m_StorageDescription )
      {
        if ( !_si.IsLookup )
          continue;
        m_DataContext.SubmitChanges( ( (AssociationAttribute)_si.Description ).List );
      };
    }
    private class EntityEqualityComparer: IEqualityComparer<ITrackEntityState>
    {
      #region IEqualityComparer<object> Members
      public bool Equals( ITrackEntityState x, ITrackEntityState y )
      {
        return x == y;
      }
      public int GetHashCode( ITrackEntityState obj )
      {
        return obj.GetHashCode();
      }
      #endregion
    }
    protected internal class StorageItem
    {
      internal StorageItem( string propertyName, DataAttribute description, FieldInfo storage )
      {
        PropertyName = propertyName;
        Association = description is AssociationAttribute;
        Description = description;
        Storage = storage;
      }
      internal string PropertyName { get; private set; }
      internal bool Association { get; private set; }
      internal DataAttribute Description { get; private set; }
      internal FieldInfo Storage { get; private set; }
      internal bool IsLookup
      {
        get
        {
          if ( !Association )
            return false;
          return ( (AssociationAttribute)Description ).MultivalueType == AssociationType.Single;
        }
      }
    }
    protected internal List<StorageItem> m_StorageDescription = new List<StorageItem>();
    internal protected Dictionary<ITrackEntityState, ListItem> m_EntitieAssociations = new Dictionary<ITrackEntityState, ListItem>( new EntityEqualityComparer() );
    private delegate void Method<T1, T2>( T1 arg1, T2 arg2 );
    protected internal SPCList m_list = default( SPCList );
    protected internal DataContext m_DataContext = default( DataContext );
    private void GetValuesFromEntity( ITrackOriginalValues entity, Method<string, object> assign )
    {
      Dictionary<string, StorageItem> _storageDic = m_StorageDescription.ToDictionary<StorageItem, string>( key => key.PropertyName );
      foreach ( var item in entity.OriginalValues )
      {
        StorageItem _storage = _storageDic[ item.Key ];
        object _value = _storage.Storage.GetValue( entity );
        if ( _storage.Association )
        {
          Debug.Assert( _storage.IsLookup, "Unexpected MultivalueType in the GetValuesFromEntity. Expected is lookup, but the filde is reverse lookup" );
          _value = ( (DataContext.IAssociationAttribute)_value ).Lookup;
        }
        else if ( ( (ColumnAttribute)_storage.Description ).FieldType.Contains( "Choice" ) )
        {
          Dictionary<string, string> _values = new Dictionary<string, string>();
          Type _type = GetEnumValues( _storage, _values, true );
          _value = _values[ _value.ToString() ];
        }
        assign( _storage.Description.Name, _value );
      }
    }
    protected internal void AssignValues2Entity<TEntity>( TEntity _newEntity, Dictionary<string, object> values )
      where TEntity: class
    {
      Dictionary<string, StorageItem> _storageDic = m_StorageDescription.ToDictionary<StorageItem, string>( key => key.Description.Name );
      foreach ( KeyValuePair<string, object> _item in values )
      {
        if ( !_storageDic.ContainsKey( _item.Key ) )
          continue;
        StorageItem _storage = _storageDic[ _item.Key ];
        if ( _storage.Association )
        {
          Debug.Assert( _storage.IsLookup, "Unexpected assignment to reverse lookup" );
          if ( _item.Value == null )
            continue;
          DataContext.IAssociationAttribute _itemRef = (DataContext.IAssociationAttribute)_storage.Storage.GetValue( _newEntity );
          _itemRef.Lookup = (FieldLookupValue)_item.Value;
        }
        else if ( ( (ColumnAttribute)_storage.Description ).IsLookupId )
        {
          if ( _item.Value == null )
            continue;
          _storage.Storage.SetValue( _newEntity, ( (Client.FieldUserValue)_item.Value ).LookupId );
        }
        else if ( ( (ColumnAttribute)_storage.Description ).FieldType.Contains( "Choice" ) )
        {
          Dictionary<string, string> _values = new Dictionary<string, string>();
          Type _type = GetEnumValues( _storage, _values, false );
          object _enumValue = Enum.Parse( _type, _values[ (string)_item.Value ], true );
          _storage.Storage.SetValue( _newEntity, _enumValue );
        }
        else
          _storage.Storage.SetValue( _newEntity, _item.Value );
      }
    }
    private static Type GetEnumValues( StorageItem _storage, Dictionary<string, string> _values, bool fildNameIsKey )
    {
      Type[] _types = _storage.Storage.FieldType.GetGenericArguments();
      if ( _types.Length != 1 )
        throw new ApplicationException( "Unexpected type in the AssignValues2Entity" );
      FieldInfo[] _fields = _types[ 0 ].GetFields();
      foreach ( FieldInfo _fld in _fields )
      {
        object[] _attrbts = _fld.GetCustomAttributes( false );
        foreach ( Attribute _attr in _attrbts )
        {
          ChoiceAttribute _ca = _attr as ChoiceAttribute;
          if ( _ca == null )
            continue;
          if ( fildNameIsKey )
            _values.Add( _fld.Name, _ca.Value );
          else
            _values.Add( _ca.Value, _fld.Name );
        }
      }
      return _types[ 0 ];
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
    /// <summary>
    /// Gets or sets a value indicating whether this <see cref="EntityListData" /> is unchaged.
    /// </summary>
    /// <value>
    ///   <c>true</c> if unchaged; otherwise, <c>false</c>.
    /// </value>
    protected internal bool Unchaged { get; set; }
  }
}
