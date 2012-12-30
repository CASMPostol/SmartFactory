using System.Collections.Generic;

namespace Microsoft.SharePoint.Linq
{
  /// <summary>
  ///  Enables implementing classes to store a dictionary of changed properties and their original values.
  /// </summary>
  public interface ITrackOriginalValues
  {
    /// <summary>
    /// Gets a dictionary of the changed properties of the entity object and the values they had when they 
    /// were last retrieved from the database.
    /// </summary>
    /// <value>
    /// A <see cref="System.Collections.Generic.IDictionary<TKey,TValue> "/> that itemizes the names of changed 
    /// properties and their original values.
    /// </value>
    IDictionary<string, object> OriginalValues { get; }
  }
}
