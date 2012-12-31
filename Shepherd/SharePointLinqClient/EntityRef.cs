﻿using System;
using System.ComponentModel;
using Microsoft.SharePoint.Client;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Provides for deferred loading and relationship maintenance for the singleton
  //     side of a one-to-many relationship.
  //
  // Type parameters:
  //   TEntity:
  //     The type of the entity on the singleton side of the relationship.
  public class EntityRef<TEntity>: DataContext.IAssociationAttribute, DataContext.IRegister
    where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     class.
    public EntityRef() { }

    // Summary:
    //     Raised after a change to this Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     object.
    public event EventHandler OnChanged;
    //
    // Summary:
    //     Raised before a change to this Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     object.
    public event EventHandler OnChanging;
    //
    // Summary:
    //     Raised when the Microsoft.SharePoint.Linq.EntityRef<TEntity> object is synchronized
    //     with the entity it represents.
    public event EventHandler<AssociationChangedEventArgs<TEntity>> OnSync;

    // Summary:
    //     Creates a shallow copy of the Microsoft.SharePoint.Linq.EntityRef<TEntity>.
    //
    // Returns:
    //     A System.Object (castable Microsoft.SharePoint.Linq.EntityRef<TEntity>) whose
    //     property values refer to the same objects as the property values of this
    //     Microsoft.SharePoint.Linq.EntityRef<TEntity>.
    public object Clone() { throw new NotImplementedException(); }
    //
    // Summary:
    //     Returns the entity that is wrapped by this Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     object.
    //
    // Returns:
    //     A System.Object that represents the entity that is stored in a private field
    //     of this Microsoft.SharePoint.Linq.EntityRef<TEntity> object.
    public TEntity GetEntity()
    {
      return m_Lookup;
    }
    /// <summary>
    /// Sets the entity to which this <see cref="Microsoft.SharePoint.Linq.EntityRef<TEntity>"/> refers.
    /// </summary>
    /// <param name="entity">The entity to which the <see cref="Microsoft.SharePoint.Linq.EntityRef<TEntity>"/> is being pointed.</param>
    /// <exception cref="System.InvalidOperationException">The object is not registered in the context.</exception>
    public void SetEntity( TEntity entity )
    {
      if ( !m_InContext )
        throw new InvalidOperationException( "The object is not registered in the context." );
      if ( entity == m_Lookup )
        return;
      if ( OnChanging != null )
        OnChanging( this, new EventArgs() );
      m_FieldLookupValue = m_DataContext.GetFieldLookupValue<TEntity>( m_AssociationAttribute.List, entity );
      if ( OnSync != null && m_Lookup != null )
        OnSync( this, new AssociationChangedEventArgs<TEntity>( entity, AssociationChangedState.Removed ) );
      m_Lookup = entity;
      if ( OnSync != null && m_Lookup != null )
        OnSync( this, new AssociationChangedEventArgs<TEntity>( entity, AssociationChangedState.Added ) );
      if ( OnChanged != null )
        OnChanged( this, new EventArgs() );
    }

    #region IAssociationAttribute Members
    FieldLookupValue DataContext.IAssociationAttribute.Lookup { get { return m_FieldLookupValue; } set { m_FieldLookupValue = value; } }
    #endregion

    #region IRegister Members
    public void RegisterInContext( DataContext dataContext, AssociationAttribute associationAttribute )
    {
      m_DataContext = dataContext;
      m_AssociationAttribute = associationAttribute;
      m_InContext = true;
    }
    #endregion

    #region private
    private bool m_InContext = false;
    private TEntity m_Lookup = default( TEntity );
    private FieldLookupValue m_FieldLookupValue = default( FieldLookupValue );
    private DataContext m_DataContext = default( DataContext );
    private AssociationAttribute m_AssociationAttribute = default( AssociationAttribute );
    #endregion

  }
}
