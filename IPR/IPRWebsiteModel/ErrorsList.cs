using System.Collections.Generic;
using CAS.SmartFactory.Customs;

namespace CAS.SmartFactory.IPR.WebsiteModel
{
  /// <summary>
  /// Errors List
  /// </summary>
  public class ErrorsList: List<Warnning>
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
    public new void Add( Warnning message )
    {
      this.Add( message, message.Fatal );
    }
    /// <summary>
    /// Adds the specified warnnings.
    /// </summary>
    /// <param name="warnnings">The warnnings.</param>
    /// <param name="fatal">if set to <c>true</c> [fatal].</param>
    public void Add( List<string> warnnings, bool fatal )
    {
      foreach ( string _wrn in warnnings )
        Add( new Warnning( _wrn, fatal ) );
    }
    /// <summary>
    /// Adds the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="fatal">if set to <c>true</c> it is fata error.</param>
    public void Add( Warnning message, bool fatal )
    {
      base.Add( message );
      Fatal |= fatal;
    }
    /// <summary>
    /// Adds the specified warnnings.
    /// </summary>
    /// <param name="warnnings">The warnnings.</param>
    public void Add( List<Warnning> warnnings )
    {
      foreach ( Warnning _item in warnnings )
        this.Add( warnnings );
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
