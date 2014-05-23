namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// Enables participation in the object tracking system of the LINQ to SharePoint provider.
  /// </summary>
  public interface ITrackEntityState
  {
    /// <summary>
    /// Gets or sets a value that indicates the changed status of the entity.
    /// </summary>
    /// <value>
    /// An <see cref="Microsoft.SharePoint.Linq.EntityState"/> value that indicates the changed status of an entity.
    /// </value>
    EntityState EntityState { get; set; }
  }
}
