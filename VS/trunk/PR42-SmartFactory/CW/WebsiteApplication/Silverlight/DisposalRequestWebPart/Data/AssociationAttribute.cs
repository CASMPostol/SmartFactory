using System;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  /// <summary>
  /// Specifies that the property (mapped to a list field) has an association to another list, such as when the property is mapped to a lookup field.
  /// </summary>
  [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false )]
  public sealed class AssociationAttribute: DataAttribute
  {
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.AssociationAttribute class.
    /// </summary>
    public AssociationAttribute() { }   
    /// <summary>
    /// Gets or sets the name of the target list.
    /// </summary>
    /// <value>
    /// A System.String that represents the target list.
    /// </value>
    public string List { get; set; }   
    /// <summary>
    /// Gets or sets the directionality of the association and whether it is single-valued or multi-valued.
    /// </summary>
    /// <value>
    /// An Microsoft.SharePoint.Linq.AssociationType that specifies the directionality of the association and whether it is single-valued or multi-valued.
    /// </value>
    public AssociationType MultivalueType { get; set; }
  }
}
