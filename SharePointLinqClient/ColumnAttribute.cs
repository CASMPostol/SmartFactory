using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Specifies that the property is mapped to a field (column) in a Windows SharePoint
  //     Services "14" list.
  [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false )]
  public sealed class ColumnAttribute: DataAttribute
  {
    // Summary:
    //     Initializes an instance of the Microsoft.SharePoint.Linq.ColumnAttribute
    //     class.
    public ColumnAttribute() { }

    // Summary:
    //     Gets or sets the type of the field.
    //
    // Returns:
    //     A case-sensitive System.String that represents the type of the data in the
    //     field.
    public string FieldType { get; set; }
    public bool IsCalculated { get; set; }
    //
    // Summary:
    //     Gets or sets a value that indicates whether the property represents the Id
    //     of the list item that provides the value for the property that represents
    //     the looked-up value.
    //
    // Returns:
    //     true, if the property represents the Id of the same item in the lookup target
    //     list that provides the value for the Lookup field; false otherwise.
    public bool IsLookupId { get; set; }
    //
    // Summary:
    //     Gets or sets a value that indicates whether this field is a lookup field
    //     value.
    //
    // Returns:
    //     true, if the field is a lookup field; false otherwise.
    public bool IsLookupValue { get; set; }
    //
    // Summary:
    //     Gets or sets a value that indicates whether the field (column) is required
    //     to have a value.
    //
    // Returns:
    //     true, if the field must have a value; false otherwise.
    public bool Required { get; set; }
  }
}
