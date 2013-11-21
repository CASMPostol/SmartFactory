//<summary>
//  Title   : class InitParam
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

namespace CAS.SmartFactory.CW.Dashboards.Silverlight
{
  /// <summary>
  /// 
  /// </summary>
  internal class InitParam
  {
    #region private
    private string m_Name;
    private string m_Value;
    #endregion

    #region public
    public InitParam( string key, string value )
    {
      // TODO: Complete member initialization
      this.m_Name = key;
      this.m_Value = value;
    }
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents the formated key=value pair.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    /// <exception cref="System.ArgumentNullException">name</exception>
    public override string ToString()
    {
      string _tmplt = "{0}={1}";
      if ( String.IsNullOrEmpty( m_Name ) )
        throw new ArgumentNullException( "name" );
      return String.Format( _tmplt, m_Name, m_Value );
    }

    #endregion
  }
}
