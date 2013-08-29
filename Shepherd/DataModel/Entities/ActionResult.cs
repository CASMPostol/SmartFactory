//<summary>
//  Title   : class ActionResult
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.DataModel.Entities
{
  /// <summary>
  /// ActionResult class
  /// </summary>
  public class ActionResult: List<string>
  {
    public ActionResult( string source )
      : base()
    {
      m_Source = source;
    }
    #region public
    internal bool Valid { get { return this.Count == 0; } }
    /// <summary>
    /// Adds the message.
    /// </summary>
    /// <param name="location">The location.</param>
    /// <param name="message">The _message.</param>
    public void AddMessage( string location, string message )
    {
      string _msg = String.Format( "The operation reports at {0} the problem: {1}.", location, message );
      base.Add( message );
    }
    /// <summary>
    /// Adds the exception.
    /// </summary>
    /// <param name="location">The location of the problem.</param>
    /// <param name="excptn">The <see cref="Exception "/>.</param>
    public void AddException( string location, Exception excptn )
    {
      string _msg = String.Format( "The operation interrupted at {0} by the error: {1}.", location, excptn.Message );
      base.Add( _msg );
    }
    /// <summary>
    /// Reports the action result.
    /// </summary>
    /// <param name="EDC">The <see cref="EntitiesDataContext"/> object.</param>
    public void ReportActionResult( EntitiesDataContext EDC )
    {
      if ( this.Count == 0 )
        return;
      CreateAnonsEntry( EDC );
    }
    #endregion

    #region private
    private string m_Source;
    private void CreateAnonsEntry( EntitiesDataContext EDC )
    {
      foreach ( string _msg in this )
      {
        Anons _entry = new Anons() { Title = m_Source, Body = _msg, Expires = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) };
        EDC.EventLogList.InsertOnSubmit( _entry );
      }
    }
    #endregion    

  }
}
