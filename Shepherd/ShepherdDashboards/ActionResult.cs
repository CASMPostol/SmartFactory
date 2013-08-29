//<summary>
//  Title   : ActionResult
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

namespace CAS.SmartFactory.Shepherd.Dashboards
{
  /// <summary>
  /// Action Result
  /// </summary>
  public class ActionResult: List<string>
  {
    #region public
    /// <summary>
    /// Gets a value indicating whether this <see cref="ActionResult"/> is valid.
    /// </summary>
    /// <value>
    ///   <c>true</c> if no error encountered.; otherwise, <c>false</c>.
    /// </value>
    internal bool Valid { get { return this.Count == 0; } }
    internal void AddException( Exception _excptn )
    {
      string _msg = String.Format( "ReportExceptionTemplate".GetShepherdLocalizedString(), _excptn.Message );
      base.Add( GlobalDefinitions.ErrorMessage( _msg ) );
    }
    public void AddLabel( string _source )
    {
      string _msg = _source + "MustBeProvided".GetShepherdLocalizedString();
      base.Add( GlobalDefinitions.ErrorMessage( _msg ) );
    }
    public void AddMessage( string _message )
    {
      base.Add( GlobalDefinitions.ErrorMessage( _message ) );
    }
    #endregion
  }

}
