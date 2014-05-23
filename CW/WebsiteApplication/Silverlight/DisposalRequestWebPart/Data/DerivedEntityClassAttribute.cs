using System;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{  
  /// <summary>
  /// Identifies a class that derives from the class to which the attribute is applied.
  /// </summary>
  [AttributeUsage( AttributeTargets.Class, AllowMultiple = true )]
  public sealed class DerivedEntityClassAttribute: Attribute
  {    
    /// <summary>
    /// Initializes a new instance of the Microsoft.SharePoint.Linq.DerivedEntityClassAttribute class.
    /// </summary>
    public DerivedEntityClassAttribute() { }
    /// <summary>
    /// Gets or sets the type of the derived class.
    /// </summary>
    /// <value>
    /// A System.Type that represents the type of the derived class.
    /// </value>
    public Type Type { get; set; }
  }
}
