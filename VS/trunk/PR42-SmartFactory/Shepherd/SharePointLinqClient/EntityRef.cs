using System;
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
  public class EntityRef<TEntity>: DataContext.IAssociationAttribute where TEntity: class
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
    public TEntity GetEntity() { return m_Lookup; }
    //
    // Summary:
    //     Sets the entity to which this Microsoft.SharePoint.Linq.EntityRef<TEntity>
    //     refers.
    //
    // Parameters:
    //   entity:
    //     The entity to which the Microsoft.SharePoint.Linq.EntityRef<TEntity> is being
    //     pointed.
    public void SetEntity( TEntity entity )
    {
      if ( entity == m_Lookup )
        return;
      if ( OnChanging != null )
        OnChanging( this, new EventArgs() );
      //TODO remove old and add new.
      m_FieldLookupValue = DataContext.GetFieldLookupValue( entity );
      if ( OnSync != null )

        OnSync( this, new AssociationChangedEventArgs<TEntity>( entity, AssociationChangedState.Added ) ); //TODO 
      if ( OnChanged != null )
        OnChanged( this, new EventArgs() );
    }

    #region IAssociationAttribute Members
    AssociationAttribute DataContext.IAssociationAttribute.AssociationAttribute { get; set; }
    FieldLookupValue DataContext.IAssociationAttribute.Lookup { get { return m_FieldLookupValue; } set { m_FieldLookupValue = value; } }
    #endregion

    private TEntity m_Lookup = default( TEntity );
    private FieldLookupValue m_FieldLookupValue = default( FieldLookupValue );

  }
}
