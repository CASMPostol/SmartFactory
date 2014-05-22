using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Action Result
  /// </summary>
  internal class ActionResult: List<string>
  {
    #region public
    /// <summary>
    /// Gets a value indicating whether this <see cref="ActionResult" /> is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if valid; otherwise, <c>false</c>.
    /// </value>
    public bool Valid { get { return this.Count == 0; } }
    /// <summary>
    /// Reports the action result.
    /// </summary>
    /// <param name="url">The _url.</param>
    public void ReportActionResult( string url )
    {
      if ( this.Count == 0 )
        return;
      try
      {
        using ( Entities _edc = new Entities( url ) )
        {
          foreach ( string _msg in this )
          {
            ActivityLogCT _entry = new ActivityLogCT() { Title = "ReportActionResult", Body = _msg, Expires = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) };
            _edc.ActivityLog.InsertOnSubmit( _entry );
          }
          _edc.SubmitChanges();
        }
      }
      catch ( Exception ) { }
    }
    /// <summary>
    /// Adds the message.
    /// </summary>
    /// <param name="_src">The _SRC.</param>
    /// <param name="_message">The _message.</param>
    public void AddMessage( string _src, string _message )
    {
      string _msg = String.Format( "The operation reports at {0} the problem: {1}.", _src, _message );
      base.Add( _message );
    }
    #endregion
  }
}
