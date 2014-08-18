//<summary>
//  Title   : class EntityRef<TEntity>
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
using System.ComponentModel;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  ///  Provides for deferred loading and relationship maintenance for the singleton side of a one-to-many relationship.
  /// </summary>
  /// <typeparam name="TEntity"> The type of the entity on the singleton side of the relationship.</typeparam>
  public class EntityRef<TEntity> : IEntityRef
    where TEntity : class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.EntityRef class.
    /// </summary>
    public EntityRef() { }
    #endregion

    #region public
    /// <summary>
    /// Raised after a change to this <see cref="EntityRef{TEntity}"/> object.
    /// </summary>
    public event EventHandler OnChanged;
    /// <summary>
    /// Raised before a change to this  <see cref="EntityRef{TEntity}"/> object.
    /// </summary>
    public event EventHandler OnChanging;
    /// <summary>
    /// Raised when the <see cref="EntityRef{TEntity}"/> object is synchronized with the entity it represents.
    /// </summary>
    public event EventHandler<AssociationChangedEventArgs<TEntity>> OnSync;
    /// <summary>
    /// Creates a shallow copy of the Microsoft.SharePoint.Linq.EntityRef.
    /// </summary>
    /// <returns>
    /// A <see cref="Object"/> (<see cref="EntityRef"/>) whose property values refer to the same objects as the property values of this <see cref="EntityRef"/>EntityRef.
    ///</returns>
    public object Clone()
    {
      return MemberwiseClone();
    }
    /// <summary>
    /// Returns the entity that is wrapped by this Microsoft.SharePoint.Linq.EntityRef object.
    /// </summary>
    /// <returns>A System.Object that represents the entity that is stored in a private field
    ///     of this Microsoft.SharePoint.Linq.EntityRef object.</returns>
    public TEntity GetEntity()
    {
      return m_Lookup;
    }
    #endregion

    #region IEntityRef Members
    /// <summary>
    /// Sets the entity to which this <see cref="Microsoft.SharePoint.Linq.EntityRef<TEntity>"/> refers.
    /// </summary>
    /// <param name="entity">The entity to which the <see cref="Microsoft.SharePoint.Linq.EntityRef<TEntity>"/> is being pointed.</param>
    /// <exception cref="System.InvalidOperationException">The object is not registered in the context.</exception>
    public void SetEntity(Object entity)
    {
      if (entity == m_Lookup)
        return;
      if (OnChanging != null)
        OnChanging(this, new EventArgs());
      if (OnSync != null && m_Lookup != null)
        OnSync(this, new AssociationChangedEventArgs<TEntity>(m_Lookup, AssociationChangedState.Removed));
      m_Lookup = (TEntity)entity;
      if (OnSync != null && m_Lookup != null)
        OnSync(this, new AssociationChangedEventArgs<TEntity>(m_Lookup, AssociationChangedState.Added));
      if (OnChanged != null)
        OnChanged(this, new EventArgs());
    }
    FieldLookupValue IEntityRef.GetLookup(DataContext dataContext, string listName)
    {
      return dataContext.GetFieldLookupValue(listName, m_Lookup);
    }
    void IEntityRef.SetLookup(FieldLookupValue value, DataContext dataContext, string listName)
    {
      if (value == null)
        return;
      Object _entity = dataContext.GetFieldLookupValue<TEntity>(listName, value);
      SetEntity(_entity);
    }
    #endregion

    #region private
    private TEntity m_Lookup = default(TEntity);
    #endregion

  }
}
