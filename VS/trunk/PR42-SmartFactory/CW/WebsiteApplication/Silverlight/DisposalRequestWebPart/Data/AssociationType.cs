namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// Specifies the type of lookup relation between a field (column) in one list and a field in another list.
  /// </summary>
  public enum AssociationType
  {    
    /// <summary>
    /// Unspecified or undefined relationship.
    /// </summary>
    None = 0,    
    /// <summary>
    /// Forward lookup with a single value.
    /// </summary>
    Single = 1,   
    /// <summary>
    /// Forward lookup with multiple values.
    /// </summary>
    Multi = 2,    
    /// <summary>
    /// Reverse lookup with multiple values.
    /// </summary>
    Backward = 3,
  }
  /// <summary>
  /// Specifies how an Microsoft.SharePoint.Linq.EntityRef, Microsoft.SharePoint.Linq.EntitySet, or Microsoft.SharePoint.Linq.LookupList changes.
  /// </summary>
  public enum AssociationChangedState
  {
    /// <summary>
    /// The none
    /// </summary>
    None = 0,    
    /// <summary>
    /// A child entity is added.
    /// </summary>
    Added = 1,
    /// <summary>
    /// A child entity is removed.
    /// </summary>
    Removed = 2,
  }
}
