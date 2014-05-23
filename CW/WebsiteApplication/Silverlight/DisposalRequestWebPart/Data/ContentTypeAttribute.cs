using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{   
  /// <summary>
  /// Specifies that the class maps to a Windows SharePoint Services "14" content type.
  /// </summary>
  [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
  public sealed class ContentTypeAttribute: Attribute
  {    
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.ContentTypeAttribute class.
    /// </summary>
    public ContentTypeAttribute() { }   
    /// <summary>
    /// Gets or sets the ID of the content type.
    /// </summary>
    /// <value>
    /// A System.String that represents the ID of the content type.
    /// </value>
    public string Id { get; set; }   
    /// <summary>
    /// Gets or sets the name of the list to which the content type is scoped if, and only if, it is a list-scoped content type.
    /// </summary>
    /// <value>
    /// A System.String that represents the name of the list.
    /// </value>
    public string List { get; set; }    
    /// <summary>
    /// Gets or sets the name of the content type.
    /// </summary>
    /// <value>
    /// A System.String that represents the name of the content type.
    /// </value>
    public string Name { get; set; }
  }
}
