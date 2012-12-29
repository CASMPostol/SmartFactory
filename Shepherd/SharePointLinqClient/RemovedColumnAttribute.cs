using System;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Specifies that the property maps to a field (column) that was removed from
  //     the parent content type.
  [AttributeUsage( AttributeTargets.Property, AllowMultiple = false )]
  public sealed class RemovedColumnAttribute: Attribute
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.RemovedColumnAttribute
    //     class.
    public RemovedColumnAttribute() { }
  }
}
