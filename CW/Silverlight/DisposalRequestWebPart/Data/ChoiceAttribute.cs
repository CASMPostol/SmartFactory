using System;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{   
  /// <summary>
  /// Maps a member of an enumeration to a System.String choice value for a Choice or MultiChoice type field on a Windows SharePoint Services "14" list.
  /// </summary>
  [AttributeUsage( AttributeTargets.Field, AllowMultiple = false )]
  public sealed class ChoiceAttribute: DataAttribute
  {   
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.ChoiceAttribute class.
    /// </summary>
    public ChoiceAttribute() { } 
    /// <summary>
    /// Gets or sets the choice value that is mapped to the enumeration value.
    /// </summary>
    /// <value>
    /// An System.String that represents a choice value that is mapped to the enumeration value.
    /// </value>
    public string Value { get; set; }
  }
}
