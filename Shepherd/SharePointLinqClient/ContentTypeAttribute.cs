using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Specifies that the class maps to a Windows SharePoint Services "14" content
  //     type.
  [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
  public sealed class ContentTypeAttribute: Attribute
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.ContentTypeAttribute
    //     class.
    public ContentTypeAttribute() { }

    // Summary:
    //     Gets or sets the ID of the content type.
    //
    // Returns:
    //     A System.String that represents the ID of the content type.
    public string Id { get; set; }
    //
    // Summary:
    //     Gets or sets the name of the list to which the content type is scoped if,
    //     and only if, it is a list-scoped content type.
    //
    // Returns:
    //     A System.String that represents the name of the list.
    public string List { get; set; }
    //
    // Summary:
    //     Gets or sets the name of the content type.
    //
    // Returns:
    //     A System.String that represents the name of the content type.
    public string Name { get; set; }
  }
}
