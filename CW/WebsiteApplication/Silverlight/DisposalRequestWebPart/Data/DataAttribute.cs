﻿using System;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{  
  /// <summary>
  /// Provides two optional properties commonly used by attributes on properties (of entity classes) that are mapped to list fields (columns) or list properties.
  /// </summary>
  [AttributeUsage( AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false )]
  public abstract class DataAttribute: Attribute
  { 
    /// <summary>
    ///  Initializes a new instance of the Microsoft.SharePoint.Linq.DataAttribute class.
    /// </summary>
    protected DataAttribute(){}   
    /// <summary>
    /// Gets or sets the internal name of the list field (column) or list property.
    /// </summary>
    /// <value>
    /// A System.String that identifies the name of the field or property.
    /// </value>
    public string Name { get; set; }  
    /// <summary>
    /// Gets or sets a value that indicates whether the column on the list is read-only.
    /// </summary>
    /// <value>
    ///    true, if the column is read-only; otherwise, false.
    /// </value>
    public bool ReadOnly { get; set; }    
    /// <summary>
    /// Gets or sets the field member of the entity class that stores the value of the property to which the attribute is applied.
    /// </summary>
    /// <value>
    /// A System.String that represents the name of the field member.
    /// </value>
    public string Storage { get; set; }
  }
}
