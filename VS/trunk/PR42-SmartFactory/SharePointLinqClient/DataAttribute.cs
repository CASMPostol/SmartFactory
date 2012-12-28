using System;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Provides two optional properties commonly used by attributes on properties
  //     (of entity classes) that are mapped to list fields (columns) or list properties.
  [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false )]
  public abstract class DataAttribute: Attribute
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.DataAttribute
    //     class.
    protected DataAttribute(){}

    // Summary:
    //     Gets or sets the internal name of the list field (column) or list property.
    //
    // Returns:
    //     A System.String that identifies the name of the field or property.
    public string Name { get; set; }
    //
    // Summary:
    //     Gets or sets a value that indicates whether the column on the list is read-only.
    //
    // Returns:
    //     true, if the column is read-only; otherwise, false.
    public bool ReadOnly { get; set; }
    //
    // Summary:
    //     Gets or sets the field member of the entity class that stores the value of
    //     the property to which the attribute is applied.
    //
    // Returns:
    //     A System.String that represents the name of the field member.
    public string Storage { get; set; }
  }
}
