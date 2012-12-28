using System;

namespace Microsoft.SharePoint.Linq
{
  // Summary:
  //     Maps a member of an enumeration to a System.String choice value for a Choice
  //     or MultiChoice type field on a Windows SharePoint Services "14" list.
  [AttributeUsage( AttributeTargets.Field, AllowMultiple = false )]
  public sealed class ChoiceAttribute: DataAttribute
  {
    // Summary:
    //     Initializes a new instance of the Microsoft.SharePoint.Linq.ChoiceAttribute
    //     class.
    public ChoiceAttribute() { }

    // Summary:
    //     Gets or sets the choice value that is mapped to the enumeration value.
    //
    // Returns:
    //     An System.String that represents a choice value that is mapped to the enumeration
    //     value.
    public string Value { get; set; }
  }
}
