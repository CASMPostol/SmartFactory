using System;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Specifies that the property (mapped to a list field) has an association to
  //     another list, such as when the property is mapped to a lookup field.
  [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false )]
  public sealed class AssociationAttribute: DataAttribute
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.AssociationAttribute
    //     class.
    public AssociationAttribute() { }

    // Summary:
    //     Gets or sets the name of the target list.
    //
    // Returns:
    //     A System.String that represents the target list.
    public string List { get; set; }
    //
    // Summary:
    //     Gets or sets the directionality of the association and whether it is single-valued
    //     or multi-valued.
    //
    // Returns:
    //     An Microsoft.SharePoint.Linq.AssociationType that specifies the directionality
    //     of the association and whether it is single-valued or multi-valued.
    public AssociationType MultivalueType { get; set; }
  }
}
