using System;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Specifies that a property of a Microsoft.SharePoint.Linq.DataContext object
  //     represents a Windows SharePoint Services "14" list.
  [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false )]
  public sealed class ListAttribute: Attribute
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.ListAttribute
    //     class.
    public ListAttribute() { }

    // Summary:
    //     Gets or sets the name of the list.
    //
    // Returns:
    //     A System.String that represents the name of the list.
    public string Name { get; set; }
  }
}
