//<summary>
//  Title   : class EntityListItemsCollection
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.SharePoint.Client;
using SPCList = Microsoft.SharePoint.Client.List;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  internal class EntityListItemsCollection<TEntity>: IEntityListItemsCollection
    where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {

    #region creator
    internal EntityListItemsCollection( DataContext dataContext, SPCList list )
    {
      this.m_list = list;
      this.m_DataContext = dataContext;
      CreateStorageDescription( typeof( TEntity ) );
    }
    #endregion

    #region IEntityListItemsCollection Members
    public void SubmitingChanges( ProgressChangedEventHandler executeQuery )
    {
      if ( m_Unchaged )
      {
        //TODO Wrong Assertion in SubmitingChanges caused by an exception
        //TODO http://casas:11227/sites/awt/Lists/TaskList/_cts/Tasks/displayifs.aspx?List=72c511b5%2D8b63%2D4dfa%2Dad34%2D133a97eba469&ID=4105
        Debug.Assert( !( from _x in m_Collection where _x.Value.TEntityGetter.EntityState != EntityState.Unchanged select _x ).Any(), "Wrong value of Unchanged in the SubmitingChanges - expected false" );
        return;
      }
      Debug.Assert( ( from _x in m_Collection where _x.Value.TEntityGetter.EntityState != EntityState.Unchanged select _x ).Any(), "Wrong value of Unchanged in the SubmitingChanges - expected true" );
      SubmitingChanges4Parents();
      int _cntrNew = 0;
      int _cntrUpdt = 0;
      Dictionary<string, StorageItem> _entityDictionary = m_StorageDescription.ToDictionary<StorageItem, string>( storageItem => storageItem.PropertyName );
      //ToBeDeleted
      //ToBeInserted
      List<TEntityWrapper<TEntity>> _toBeInsertedCollection = ( from _tewx in m_Collection.Values where _tewx.EntityState == EntityState.ToBeInserted select _tewx ).ToList();
      foreach ( TEntityWrapper<TEntity> _toBeInsertedItem in _toBeInsertedCollection )
      {
        m_Collection.Remove( _toBeInsertedItem.Index );
        _toBeInsertedItem.GetValuesFromEntity( _entityDictionary );
        _cntrNew++;
        if ( _cntrNew % 10 == 0 )
          executeQuery( this, new ProgressChangedEventArgs( 1, String.Format( "Execute query ListItem to be inserted # {0}", _cntrNew ) ) );
      }
      executeQuery( this, new ProgressChangedEventArgs( 1, String.Format( "Execute query ListItem to be inserted # {0}", _cntrNew ) ) );
      _cntrNew = 0;
      foreach ( TEntityWrapper<TEntity> _toBeInsertedItem in _toBeInsertedCollection )
      {
        _toBeInsertedItem.MyListItem.RefreshLoad();
        _cntrNew++;
        if ( _cntrNew % 10 == 0 )
          executeQuery( this, new ProgressChangedEventArgs( 1, String.Format( "Execute query ListItem RefreshLoad # {0}", _cntrNew ) ) );
      }
      executeQuery( this, new ProgressChangedEventArgs( 1, String.Format( "Execute query ListItem RefreshLoad # {0}", _cntrNew ) ) );
      foreach ( TEntityWrapper<TEntity> _toBeInsertedItem in _toBeInsertedCollection )
      {
        _toBeInsertedItem.AssignValues2Entity( ListItemPropertiesDictionary() );
        this.m_Collection.Add( _toBeInsertedItem.Index, _toBeInsertedItem );
      }
      //ToBeUpdated
      List<TEntityWrapper<TEntity>> _toBeUpdatedCollection = ( from _tewx in m_Collection.Values where _tewx.EntityState == EntityState.ToBeUpdated select _tewx ).ToList();
      foreach ( TEntityWrapper<TEntity> _itemX in _toBeUpdatedCollection )
      {
        _itemX.GetValuesFromEntity( _entityDictionary );
        _cntrUpdt++;
        if ( _cntrUpdt % 10 == 0 )
          executeQuery( this, new ProgressChangedEventArgs( 1, String.Format( "Execute query ListItem Update # {0}", _cntrUpdt ) ) );
      }
      executeQuery( this, new ProgressChangedEventArgs( 1, String.Format( "Execute query ListItem Update # {0}", _cntrUpdt ) ) );
      m_Unchaged = true;
    }
    /// <summary>
    /// Gets the field lookup value.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>An <see cref="FieldLookupValue"/> object containing reference to the entity</returns>
    public FieldLookupValue GetFieldLookupValue( Object entity )
    {
      Dictionary<TEntity, TEntityWrapper<TEntity>> m_EntitieAssociations =
        m_Collection.ToDictionary<KeyValuePair<int, TEntityWrapper<TEntity>>, TEntity, TEntityWrapper<TEntity>>( key => key.Value.TEntityGetter, val => val.Value, new EqualityComparer() );
      FieldLookupValue _ret = new FieldLookupValue()
      {
        LookupId = entity == null ? -1 : m_EntitieAssociations[ (TEntity)entity ].Index
      };
      // it is wrong after deleting an entity Debug.Assert( _ret.LookupId > 0, "Unexpected null reference to existing Entity" );
      return _ret;
    }
    public Object GetFieldLookupValue( FieldLookupValue fieldLookupValue )
    {
      if ( fieldLookupValue.LookupId < 0 )
        return null;
      if ( !this.ContainsKey( fieldLookupValue.LookupId ) )
      {
        if ( !m_DataContext.DeferredLoadingEnabled )
          return null;
        m_DataContext.LoadListItem( fieldLookupValue.LookupId, this );
      }
      return m_Collection[ fieldLookupValue.LookupId ].TEntityGetter;
    }
    #endregion

    #region internal
    internal List MyList { get { return m_list; } }
    /// <summary>
    /// Adds new entity to be inserted
    /// </summary>
    /// <param name="entity">The entity.</param>
    internal void Add( TEntity entity )
    {
      TEntityWrapper<TEntity> _wrpr = new TEntityWrapper<TEntity>( m_DataContext, entity, PropertyChanged, MyList );
      m_Collection.Add( _wrpr.Index, _wrpr );
      m_Unchaged = false;
    }
    /// <summary>
    /// Adds an entity for the specified list item.
    /// </summary>
    /// <param name="listItem">The list item.</param>
    /// <param name="dataContext">The data context.</param>
    /// <returns></returns>
    internal TEntity Add( ListItem listItem )
    {
      Dictionary<string, StorageItem> _storageDic = ListItemPropertiesDictionary();
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

    private DataContext m_DataContext = default( DataContext );
    private SPCList m_list = default( SPCList );
    private bool m_Unchaged = true;
    private List<StorageItem> m_StorageDescription = new List<StorageItem>();
    private Dictionary<int, TEntityWrapper<TEntity>> m_Collection = new Dictionary<int, TEntityWrapper<TEntity>>();
    private Dictionary<string, StorageItem> ListItemPropertiesDictionary()
    {
      return m_StorageDescription.ToDictionary<StorageItem, string>( storageItem => storageItem.Description.Name );
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
