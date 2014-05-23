using System;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// Specifies that the property is mapped to a field (column) in a Windows SharePoint Services "14" list.
  /// </summary>
  [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false )]
  public sealed class ColumnAttribute: DataAttribute
  {
    /// <summary>
    /// Initializes an instance of the Microsoft.SharePoint.Linq.ColumnAttribute class.
    /// </summary>
    public ColumnAttribute() { }   
    /// <summary>
    /// Gets or sets the type of the field.
    /// </summary>
    /// <value>
    /// A case-sensitive System.String that represents the type of the data in the field.
    /// </value>
    public string FieldType { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether the column is calculated.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the column is calculated; otherwise, <c>false</c>.
    /// </value>
    public bool IsCalculated { get; set; }    
    /// <summary>
    /// Gets or sets a value that indicates whether the property represents the Id of the list item that provides the value for the property that represents the looked-up value.
    /// </summary>
    /// <value>
    /// true, if the property represents the Id of the same item in the lookup target list that provides the value for the Lookup field; false otherwise.
    /// </value>
    public bool IsLookupId { get; set; }
    /// <summary>
    /// Gets or sets a value that indicates whether this field is a lookup field value.
    /// </summary>
    /// <value>
    ///   true, if the field is a lookup field; false otherwise.
    /// </value>
    public bool IsLookupValue { get; set; }
    /// <summary>
    ///  Gets or sets a value that indicates whether the field (column) is required to have a value.
    /// </summary>
    /// <value>
    ///    true, if the field must have a value; false otherwise.
    /// </value>
    public bool Required { get; set; }
  }
}
