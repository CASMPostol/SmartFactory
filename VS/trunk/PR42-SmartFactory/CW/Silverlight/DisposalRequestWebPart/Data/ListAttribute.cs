using System;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{    
  /// <summary>
  /// Specifies that a property of a Microsoft.SharePoint.Linq.DataContext object represents a Windows SharePoint Services "14" list.
  /// </summary>
  [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false )]
  public sealed class ListAttribute: Attribute
  {
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.ListAttribute class
    /// </summary>
    public ListAttribute() { }   
    /// <summary>
    /// Gets or sets the name of the list.
    /// </summary>
    /// <value>
    /// A System.String that represents the name of the list.
    /// </value>
    public string Name { get; set; }
  }
}
