using System;
using System.Reflection;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{
  // Summary:
  //     
  /// <summary>
  /// Specifies information about a discrepancy between the value of a field (or
  /// list item metadata property) in the database and its value in the current
  /// process, its client value.
  /// </summary>
  public sealed class MemberChangeConflict
  {   
    /// <summary>
    /// Gets the value of the field in the current process, the client value.
    /// </summary>
    /// <value>
    /// A System.Object that represents the value of the field after the most recent change, if any, made in the current process.
    /// </value>
    /// <exception cref="System.NotImplementedException"></exception>
    public object CurrentValue { get { throw new NotImplementedException(); } }    
    /// <summary>
    /// Gets the value of the field in the content database.
    /// </summary>
    /// <value>
    /// A System.Object that represents the value of the field in the database.
    /// </value>
    /// <exception cref="System.NotImplementedException"></exception>
    public object DatabaseValue { get { throw new NotImplementedException(); } }   
    /// <summary>
    /// Gets a value that indicates whether the current user has changed field value since it was last retrieved from the database.
    /// </summary>
    /// <value>
    ///    true, if the value has been changed by the current process; otherwise, false.
    /// </value>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool IsModified { get { throw new NotImplementedException(); } }    
    /// <summary>
    /// Gets a value indicating whether the discrepancy has been resolved.
    /// </summary>
    /// <value>
    ///  true, if the discrepancy is resolved; otherwise, false.
    /// </value>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool IsResolved { get { throw new NotImplementedException(); } }   
    /// <summary>
    ///  Gets metadata information about the property of the list item object that represents the field for which there is a discrepancy.
    /// </summary>
    /// <value>
    ///  A System.Reflection.MemberInfo object that holds information about the property.
    /// </value>
    /// <exception cref="System.NotImplementedException"></exception>
    public MemberInfo Member { get { throw new NotImplementedException(); } }    
    /// <summary>
    ///  Gets the value of the field as it was when it was last retrieved from the database by the current process.
    /// </summary>
    /// <value>
    /// A System.Object that represents the value the field had in the database that last time it was retrieved by this current process.
    /// </value>
    /// <exception cref="System.NotImplementedException"></exception>
    public object OriginalValue { get { throw new NotImplementedException(); } } 
    /// <summary>
    ///  Resolves the discrepancy by setting client value of the field (or property) to the specified object.
    /// </summary>
    /// <param name="value">The value to which the field (or property) should be set.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Resolve( object value ) { throw new NotImplementedException(); }    
    /// <summary>
    /// Resolves the discrepancy using the specified refresh mode.
    /// </summary>
    /// <param name="refreshMode">A value that specifies how to resolve the conflict.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Resolve( RefreshMode refreshMode ) { throw new NotImplementedException(); }
  }
}
