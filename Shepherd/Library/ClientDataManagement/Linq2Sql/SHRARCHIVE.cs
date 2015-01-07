//<summary>
//  Title   : SHRARCHIVE
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq2SQL
{
  /// <summary>
  /// Class SHRARCHIVE - provides connectivity services 
  /// </summary>
  public partial class SHRARCHIVE
  {
    /// <summary>
    /// Connects the clint to the SQL database.
    /// </summary>
    /// <param name="connectionString">The connection string.</param>
    /// <param name="progressChanged">The progress changed.</param>
    /// <returns>Instance of the <see cref="SHRARCHIVE"/>.</returns>
    internal static SHRARCHIVE Connect2SQL(string connectionString, Action<ProgressChangedEventArgs> progressChanged)
    {
      progressChanged(new ProgressChangedEventArgs(1, String.Format("Attempt to connect to SQL at: {0}", connectionString)));
      System.Data.IDbConnection _connection = new SqlConnection(connectionString);
      SHRARCHIVE _entities = new SHRARCHIVE(_connection);
      if (_entities.DatabaseExists())
        progressChanged(new ProgressChangedEventArgs(1, "The specified database can be opened."));
      else
        progressChanged(new ProgressChangedEventArgs(1, "The specified database cannot be opened."));
      return _entities;
    }

  }
}
