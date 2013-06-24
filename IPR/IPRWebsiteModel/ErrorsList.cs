using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.WebsiteModel
{
  /// <summary>
  /// Errors List
  /// </summary>
  public class ErrorsList: List<string>
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorsList" /> class.
    /// </summary>
    public ErrorsList()
    {
      Fatal = false;
    }
    /// <summary>
    /// Adds the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    public new void Add(string message)
    {
      this.Add(message, true);
    }
    /// <summary>
    /// Adds the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="fatal">if set to <c>true</c> it is fata error.</param>
    public void Add( string message, bool fatal )
    {
      base.Add( message );
      Fatal |= fatal;
    }
    /// <summary>
    /// Gets or sets a value indicating whether this collection contains fatal errors.
    /// </summary>
    /// <value>
    ///   <c>true</c> if fatal; otherwise, <c>false</c>.
    /// </value>
    public bool Fatal { get; set; }
  }
}
