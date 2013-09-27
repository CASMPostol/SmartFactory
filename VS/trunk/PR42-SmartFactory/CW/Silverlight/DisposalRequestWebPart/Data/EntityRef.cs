using System;
using System.ComponentModel;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  ///  Provides for deferred loading and relationship maintenance for the singleton side of a one-to-many relationship.
  /// </summary>
  /// <typeparam name="TEntity"> The type of the entity on the singleton side of the relationship.</typeparam>
  public class EntityRef<TEntity>: DataContext.IAssociationAttribute, DataContext.IRegister
    where TEntity: class, ITrackEntityState, ITrackOriginalValues, INotifyPropertyChanged, INotifyPropertyChanging, new()
  {

    #region ctor    
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.EntityRef class.
    /// </summary>
    public EntityRef() { }
    #endregion
    #region public    
    /// <summary>
    /// Raised after a change to this Microsoft.SharePoint.Linq.EntityRef object.
    /// </summary>
    public event EventHandler OnChanged;    
    /// <summary>
    /// Raised before a change to this Microsoft.SharePoint.Linq.EntityRef object.
    /// </summary>
    public event EventHandler OnChanging;    
    /// <summary>
    /// Raised when the Microsoft.SharePoint.Linq.EntityRef object is synchronized with the entity it represents.
    /// </summary>
    public event EventHandler<AssociationChangedEventArgs<TEntity>> OnSync;    
    /// <summary>
    /// Creates a shallow copy of the Microsoft.SharePoint.Linq.EntityRef.
    /// </summary>
    /// <returns>A System.Object (castable Microsoft.SharePoint.Linq.EntityRef) whose
    ///     property values refer to the same objects as the property values of this
    ///     Microsoft.SharePoint.Linq.EntityRef.</returns>
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
    /// <summary>
    /// Sets the entity to which this <see cref="Microsoft.SharePoint.Linq.EntityRef<TEntity>"/> refers.
    /// </summary>
    /// <param name="entity">The entity to which the <see cref="Microsoft.SharePoint.Linq.EntityRef<TEntity>"/> is being pointed.</param>
    /// <exception cref="System.InvalidOperationException">The object is not registered in the context.</exception>
    public void SetEntity( TEntity entity )
    {
      if ( entity == m_Lookup )
        return;
      if ( OnChanging != null )
        OnChanging( this, new EventArgs() );
      if ( OnSync != null && m_Lookup != null )
        OnSync( this, new AssociationChangedEventArgs<TEntity>( entity, AssociationChangedState.Removed ) );
      m_Lookup = entity;
      if ( OnSync != null && m_Lookup != null )
        OnSync( this, new AssociationChangedEventArgs<TEntity>( entity, AssociationChangedState.Added ) );
      if ( OnChanged != null )
        OnChanged( this, new EventArgs() );
    }
    #endregion

    #region IAssociationAttribute Members
    FieldLookupValue DataContext.IAssociationAttribute.Lookup
    {
      get
      {
        return m_DataContext.GetFieldLookupValue<TEntity>( m_AssociationAttribute.List, m_Lookup );
      }
      set
      {
        TEntity _entity = m_DataContext.GetFieldLookupValue<TEntity>( m_AssociationAttribute.List, value );
        SetEntity( _entity );
      }
    }
    #endregion

    #region IRegister Members
    void DataContext.IRegister.RegisterInContext( DataContext dataContext, AssociationAttribute associationAttribute )
    {
      m_DataContext = dataContext;
      m_AssociationAttribute = associationAttribute;
      m_InContext = true;
    }
    #endregion

    #region private
    private bool m_InContext = false;
    private TEntity m_Lookup = default( TEntity );
    private DataContext m_DataContext = default( DataContext );
    private AssociationAttribute m_AssociationAttribute = default( AssociationAttribute );
    #endregion

  }
}
