using System;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// Specifies that the property maps to a field (column) that was removed from the parent content type.
  /// </summary>
  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
  public sealed class RemovedColumnAttribute: Attribute
  {
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.RemovedColumnAttribute class.
    /// </summary>
    public RemovedColumnAttribute() { }
  }
}
