//<summary>
//  Title   : class ActionResult
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
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
    #region public
    internal bool Valid { get { return this.Count == 0; } }
    /// <summary>
    /// Adds the exception.
    /// </summary>
    /// <param name="src">The _SRC.</param>
    /// <param name="excptn">The _excptn.</param>
    public void AddException( string src, Exception excptn )
    {
      string _msg = String.Format( "The operation interrupted at {0} by the error: {1}.", src, excptn.Message );
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
    private void CreateAnonsEntry( EntitiesDataContext EDC )
    {
      foreach ( string _msg in this )
      {
        Anons _entry = new Anons() { Tytuł = "ReportActionResult", Treść = _msg, Wygasa = DateTime.Now + new TimeSpan( 2, 0, 0, 0 ) };
        EDC.EventLogList.InsertOnSubmit( _entry );
      }
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
