using System;
using System.Collections.ObjectModel;


namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data
{  
  /// <summary>
  /// Specifies information about discrepancies between the current client value of one or more fields in a list item and the current database values of the fields.
  /// </summary>
  public sealed class ObjectChangeConflict
  {   
    /// <summary>
    /// Specifies information about discrepancies between the current client value
    ///     of one or more fields in a list item and the current database values of the
    ///     fields.
    /// </summary>
    /// <value>
    ///   true if the list item had been deleted; otherwise false.
    /// </value>
    public bool IsDeleted { get; internal set; }
    /// <summary>
    /// Gets a value indicating whether the discrepancies have been resolved.
    /// </summary>
    /// <value>
    ///    true if the discrepancies are resolved; otherwise false.
    /// </value>
    /// <exception cref="System.NotImplementedException"></exception>
    public bool IsResolved { get { throw new NotImplementedException(); } }   
    /// <summary>
    /// Gets a collection of objects that represent the discrepancies between the
    ///     current values of the list item’s fields and the values they have in the
    ///     database.
    /// </summary>
    /// <value>
    /// A System.Collections.ObjectModel.ReadOnlyCollection &lt; T &gt; of Microsoft.SharePoint.Linq.MemberChangeConflict
    ///     objects that represent the discrepancies.
    /// </value>
    /// <exception cref="System.NotImplementedException"></exception>
    public ReadOnlyCollection<MemberChangeConflict> MemberConflicts { get { throw new NotImplementedException(); } } 
    /// <summary>
    /// Gets the list item (as an System.Object) for which there is one or more discrepancies.
    /// </summary>
    /// <value>
    /// The System.Object for which there is one or more discrepancies.
    /// </value>
    /// <exception cref="System.NotImplementedException"></exception>
    public object Object { get { throw new NotImplementedException(); } }     
    /// <summary>
    /// Resolves the discrepancies by assigning each field and property, for which
    ///     there is a discrepancy, a value that is persisted to the database on the
    ///     next call of Microsoft.SharePoint.Linq.DataContext.SubmitChanges().
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Resolve() { throw new NotImplementedException(); } 
    /// <summary>
    /// Resolves the discrepancies by assigning each field and property, for which
    ///     there is a discrepancy, a value that is persisted to the database on the
    ///     next call of Microsoft.SharePoint.Linq.DataContext.SubmitChanges().
    /// </summary>
    /// <param name="refreshMode">A value that specifies how to resolve the conflict.</param>
    /// <exception cref="System.NotImplementedException"></exception>
    public void Resolve( RefreshMode refreshMode ) { throw new NotImplementedException(); }    
    /// <summary>
    /// Resolves the discrepancies by assigning each field and property, for which
    ///     there is a discrepancy, a value that is persisted to the database on the
    ///     next call of Microsoft.SharePoint.Linq.DataContext.SubmitChanges().
    /// </summary>
    /// <param name="refreshMode">A value that specifies how to resolve the conflict.</param>
    /// <param name="autoResolveDeletes">true to treat the conflict as resolved if the list item being changed has been deleted from the database; false to throw InvalidOperationException if the list item being changed has been deleted from the database</param>
    /// <exception cref="System.NotImplementedException">autoResolveDeletes is false and the list item being updated has been deleted from the database.</exception>
    public void Resolve( RefreshMode refreshMode, bool autoResolveDeletes ) { throw new NotImplementedException(); }
  }
}
