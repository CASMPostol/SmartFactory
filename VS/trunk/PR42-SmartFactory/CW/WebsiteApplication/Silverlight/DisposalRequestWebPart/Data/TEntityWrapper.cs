//<summary>
//  Title   : class TEntityWrapper<TEntity>
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
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// TEntityWrapper class
  /// </summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  internal class TEntityWrapper<TEntity>: ITrackEntityState
    where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {

    #region creators
    internal TEntityWrapper( DataContext dataContext, ListItem listItem, Dictionary<string, StorageItem> _storageDic, PropertyChangedEventHandler handler )
      : this( dataContext )
    {
      if ( listItem == null )
        throw new ArgumentNullException( "listItem" );
      MyListItem = listItem;
      m_Index = listItem.Id;
      TEntity _newEntity = new TEntity();
      this.TEntityGetter = _newEntity;
      AssignValues2Entity( _storageDic );
      _newEntity.EntityState = EntityState.Unchanged;
      _newEntity.OriginalValues.Clear(); 
      _newEntity.PropertyChanged += handler;
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="TEntityWrapper{TEntity}"/> class.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    /// <param name="entity">The new entity to be inserted.</param>
    /// <param name="handler">The <see cref="PropertyChangedEventHandler"/> handler.</param>
    internal TEntityWrapper( DataContext dataContext, TEntity entity, PropertyChangedEventHandler handler, List list )
      : this( dataContext )
    {
      entity.EntityState = EntityState.ToBeInserted;
      entity.PropertyChanged += handler;
      this.TEntityGetter = entity;
      this.MyListItem = list.AddItem( new ListItemCreationInformation() );
      m_Index = b_indexCounter--;
    }
    #endregion

    #region internal
    /// <summary>
    /// Assigns the values to entity.
    /// </summary>
    /// <param name="storageDictionary">The storage dictionary containing field name and <see cref="StorageItem" /> pairs.</param>
    /// <exception cref="System.NotImplementedException">Only ColumnAttribute is supported.
    /// or
    /// IsLookupId must be true for lookup field.
    /// </exception>
    internal void AssignValues2Entity( Dictionary<string, StorageItem> storageDictionary )
    {
      TEntity _entity = (TEntity)this.TEntityGetter;
      foreach ( KeyValuePair<string, object> _item in MyListItem.FieldValues )
      {
        if ( !storageDictionary.ContainsKey( _item.Key ) )
          continue;
        StorageItem _storage = storageDictionary[ _item.Key ];
        if ( _storage.Association )
        {
          Debug.Assert( _storage.IsLookup, "Unexpected assignment to reverse lookup" );
          IEntityRef _itemRef = (IEntityRef)_storage.Storage.GetValue( _entity );
          _itemRef.SetLookup( _item.Value == null ? null : (FieldLookupValue)_item.Value, m_DataContext, ( (AssociationAttribute)_storage.Description ).List );
        }
        else
        {
          ColumnAttribute _column = _storage.Description as ColumnAttribute;
          if ( _column == null )
            throw new NotImplementedException( "Only ColumnAttribute is supported." );
          if ( _column.FieldType == "Lookup" )
          {
            if ( _item.Value == null )
              continue;
            if ( _column.IsLookupId )
            {
              _storage.Storage.SetValue( _entity, ( (Microsoft.SharePoint.Client.FieldLookupValue)_item.Value ).LookupId );
            }
            else
              throw new NotImplementedException( "IsLookupId must be true for lookup field." );
          }
          else if ( _column.FieldType == "Choice" )
          {
            Dictionary<string, string> _values = new Dictionary<string, string>();
            Type _type = StorageItem.GetEnumValues( _storage, _values, false );
            object _enumValue = Enum.Parse( _type, _values[ (string)_item.Value ], true );
            _storage.Storage.SetValue( _entity, _enumValue );
          }
          else
            _storage.Storage.SetValue( _entity, _item.Value );
        }
      }
    }
    /// <summary>
    /// Gets the values from entity.
    /// </summary>
    /// <param name="entityDictionary">The entity dictionary containing property name <see cref="StorageItem" pairs./>.</param>
    internal void GetValuesFromEntity( Dictionary<string, StorageItem> entityDictionary )
    {
      ITrackOriginalValues _entity = (ITrackOriginalValues)this.TEntityGetter;
      foreach ( var _ovx in _entity.OriginalValues )
      {
        StorageItem _storage = entityDictionary[ _ovx.Key ];
        object _value = _storage.Storage.GetValue( _entity );
        if ( _storage.Association )
        {
          Debug.Assert( _storage.IsLookup, "Unexpected MultivalueType in the GetValuesFromEntity. Expected is lookup, but the filde is reverse lookup" );
          _value = ( (IEntityRef)_value ).GetLookup( m_DataContext, ( (AssociationAttribute)_storage.Description ).List );
        }
        else if ( ( (ColumnAttribute)_storage.Description ).FieldType.Contains( "Choice" ) )
        {
          Dictionary<string, string> _values = new Dictionary<string, string>();
          Type _type = StorageItem.GetEnumValues( _storage, _values, true );
          _value = _values[ _value.ToString() ];
        }
        MyListItem[ _storage.Description.Name ] = _value;
      }
      MyListItem.Update();
      _entity.OriginalValues.Clear();
      EntityState = EntityState.Unchanged;
    }
    internal int Index { get { return m_Index; } }
    internal ListItem MyListItem { get; private set; }
    internal TEntity TEntityGetter { get; private set; }
    #region ITrackEntityState Members
    public EntityState EntityState
    {
      get
      {
        return TEntityGetter.EntityState;
      }
      set
      {
        TEntityGetter.EntityState = value;
      }
    }
    #endregion

    #endregion


    #region private
    private DataContext m_DataContext = default( DataContext );
    private int m_Index = -1;
    private static int b_indexCounter = -1;
    private TEntityWrapper( DataContext dataContext )
    {
      m_DataContext = dataContext;
    }
    #endregion

  }
}
