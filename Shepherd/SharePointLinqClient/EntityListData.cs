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
    internal void SubmitChanges()
    {
      foreach ( var item in m_EntitieAssociations )
      {
        ITrackEntityState _entity = GetEntity( item.Key );
        switch ( _entity.EntityState )
        {
          case EntityState.Unchanged:
            break;
          case EntityState.ToBeInserted:
            if ( item.Value != null )
              throw new ApplicationException( "Inconsistent content of association table, ToBeInserted should not has associated ListItem" );
            ListItem _newListItem = m_list.AddItem( new ListItemCreationInformation() );
            GetValues( _entity, _entity.GetType(), ( na, val ) => _newListItem[ na ] = val );
            _newListItem.Update();
            break;
          case EntityState.ToBeUpdated:
            break;
          case EntityState.ToBeRecycled:
            break;
          case EntityState.ToBeDeleted:
            break;
          case EntityState.Deleted:
            break;
          default:
            break;
        }
      }
    }
    protected abstract ITrackEntityState GetEntity( int key );

    protected Dictionary<int, ListItem> m_EntitieAssociations = new Dictionary<int, ListItem>();
    private static void GetValues( object entity, Type type, Method<string, object> assign )
    {
      Dictionary<string, MemberInfo> _mmbrs = GetMembers( type );
      foreach ( MemberInfo _ax in from _pidx in _mmbrs where _pidx.Value.MemberType == MemberTypes.Property select _pidx.Value )
        foreach ( var _cax in _ax.GetCustomAttributes( false ) )
          if ( _cax is ColumnAttribute )
          {
            ColumnAttribute _ca = _cax as ColumnAttribute;
            if ( _ca.ReadOnly )
              continue;
            FieldInfo _strg = _mmbrs[ _ca.Storage ] as FieldInfo;
            object value = _strg.GetValue( entity );
            assign( _ca.Name, value );
          }
          else if ( _cax is AssociationAttribute )
          {
            AssociationAttribute _aa = _cax as AssociationAttribute;
            //TODO development
          }
      if ( type.BaseType != typeof( Object ) )
        GetValues( entity, type.BaseType, assign );
    }
    protected static Dictionary<string, MemberInfo> GetMembers( Type type )
    {
      BindingFlags _flgs = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Public | BindingFlags.NonPublic;
      Dictionary<string, MemberInfo> _mmbrs = ( from _midx in type.GetMembers( _flgs )
                                                where _midx.MemberType == MemberTypes.Field || _midx.MemberType == MemberTypes.Property
                                                select _midx ).ToDictionary<MemberInfo, string>( _mi => _mi.Name );
      return _mmbrs;
    }
    private delegate void Method<T1, T2>( T1 arg1, T2 arg2 );
    protected SPCList m_list = default( SPCList );

  }
}
