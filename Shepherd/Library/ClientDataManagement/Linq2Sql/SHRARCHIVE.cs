//<summary>
//  Title   : SHRARCHIVE
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Data.SqlClient;

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
    internal static SHRARCHIVE Connect2SQL(string connectionString, Action<string> progressChanged)
    {
      progressChanged(String.Format("Attempt to connect to SQL at: {0}", connectionString));
      System.Data.IDbConnection _connection = new SqlConnection(connectionString);
      SHRARCHIVE _entities = new SHRARCHIVE(_connection);
      if (_entities.DatabaseExists())
        progressChanged("The specified database exists.");
      else
        progressChanged("The specified database does not exist.");
      return _entities;
    }

  }
}
