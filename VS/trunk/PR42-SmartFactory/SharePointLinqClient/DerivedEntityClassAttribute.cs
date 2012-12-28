using System;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Identifies a class that derives from the class to which the attribute is
  //     applied.
  [AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
  public sealed class DerivedEntityClassAttribute: Attribute
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.DerivedEntityClassAttribute
    //     class.
    public DerivedEntityClassAttribute() { }

    // Summary:
    //     Gets or sets the type of the derived class.
    //
    // Returns:
    //     A System.Type that represents the type of the derived class.
    public Type Type { get; set; }
  }
}
