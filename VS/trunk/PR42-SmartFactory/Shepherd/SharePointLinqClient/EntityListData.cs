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
            GetValues( (ITrackOriginalValues)_entity, _entity.GetType(), ( na, val ) => _newListItem[ na ] = val );
            _newListItem.Update();
            break;
          case EntityState.ToBeUpdated:
            if ( item.Value == null )
              throw new ArgumentNullException( "VAlue", "Inconsistent content of association table, ToBeUpdated should has associated ListItem" );
            GetValues( (ITrackOriginalValues)_entity, _entity.GetType(), ( na, val ) => item.Value[ na ] = val );
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
            item.Value.Update();
            break;
          case EntityState.ToBeUpdated:
            if ( item.Value == null )
              throw new ArgumentNullException( "Value", "Inconsistent content of association table while excuting SubmitedChanges, ToBeUpdated/ToBeInserted should has associated ListItem" );
            AssignValues( _entity, _entity.GetType(), name => item.Value[ name ] );
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
    private static void GetValues( ITrackOriginalValues entity, Type type, Method<string, object> assign )
    {
      Dictionary<string, MemberInfo> _mmbrs = GetMembers( type );
      foreach ( MemberInfo _ax in from _memeberidx in _mmbrs where _memeberidx.Value.MemberType == MemberTypes.Property select _memeberidx.Value )
        foreach ( var _cax in _ax.GetCustomAttributes( false ) )
          if ( _cax is ColumnAttribute )
          {
            if ( !entity.OriginalValues.ContainsKey( _ax.Name ) )
              continue;
            ColumnAttribute _ca = _cax as ColumnAttribute;
            if ( _ca.ReadOnly )
              throw new InvalidOperationException( "Readonly value cannot be changed" );
            PropertyInfo _info = _ax as PropertyInfo;
            object value = _info.GetValue( entity, null );
            assign( _ca.Name, value );
            break;
          }
          else if ( _cax is AssociationAttribute )
          {
            AssociationAttribute _aa = _cax as AssociationAttribute;
            //TODO development
            break;
          }
      if ( type.BaseType != typeof( Object ) )
        GetValues( entity, type.BaseType, assign );
    }
    internal protected static void AssignValues( object entity, Type type, Func<string, object> getValue )
    {
      Dictionary<string, MemberInfo> _mmbrs = GetMembers( type );
      foreach ( MemberInfo _ax in from _pidx in _mmbrs where _pidx.Value.MemberType == MemberTypes.Property select _pidx.Value )
      {
        foreach ( var _cax in _ax.GetCustomAttributes( false ) )
        {
          if ( _cax is ColumnAttribute )
          {
            ColumnAttribute _ca = _cax as ColumnAttribute;
            FieldInfo _strg = _mmbrs[ _ca.Storage ] as FieldInfo;
            object value = getValue( _ca.Name );
            _strg.SetValue( entity, value );
          }
          else if ( _cax is AssociationAttribute )
          {
            AssociationAttribute _aa = _cax as AssociationAttribute;
            //TODO development
          }
        }
      }
      if ( type.BaseType != typeof( Object ) )
        AssignValues( entity, type.BaseType, getValue );
    }
    private static Dictionary<string, MemberInfo> GetMembers( Type type )
    {
      BindingFlags _flgs = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic;
      Dictionary<string, MemberInfo> _mmbrs = ( from _midx in type.GetMembers( _flgs )
                                                where _midx.MemberType == MemberTypes.Field || _midx.MemberType == MemberTypes.Property
                                                select _midx ).ToDictionary<MemberInfo, string>( _mi => _mi.Name );
      return _mmbrs;
    }
    private delegate void Method<T1, T2>( T1 arg1, T2 arg2 );
    internal protected SPCList m_list = default( SPCList );
  }
}
