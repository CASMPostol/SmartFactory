using System;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{   
  /// <summary>
  /// Provides data for the OnChanged event of Microsoft.SharePoint.Linq.EntityRef, Microsoft.SharePoint.Linq.EntitySet, or Microsoft.SharePoint.Linq.LookupList.
  /// </summary>
  /// <typeparam name="TEntity">The type of the the entity involved in the change.</typeparam>
  public class AssociationChangedEventArgs<TEntity>: EventArgs
  {    
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.AssociationChangedEventArgs class.
    /// </summary>
    /// <param name="item">The entity involved in the change.</param>
    /// <param name="state">An object that specifies the type of change.</param>
    public AssociationChangedEventArgs( TEntity item, AssociationChangedState state )
    {
      m_Entity = item;
      m_State = state;
    }   
    /// <summary>
    /// Gets the entity involved in the change.
    /// </summary>
    /// <value>
    /// A T that represents the entity involved in the change.
    /// </value>
    public TEntity Item { get { return m_Entity; } }   
    /// <summary>
    /// Gets the type of change.
    /// </summary>
    /// <value>
    /// An Microsoft.SharePoint.Linq.AssociationChangedState that specifies the type of change.
    /// </value>
    public AssociationChangedState State { get { return m_State; } }

    private AssociationChangedState m_State = default( AssociationChangedState );
    private TEntity m_Entity = default( TEntity );
  }
}
