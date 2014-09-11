//<summary>
//  Title   : public partial class IPRDEV
//  System  : Microsoft VisulaStudio 2013 / C#
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
using System.ComponentModel;
using System.Data.SqlClient;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL
{
  public partial class IPRDEV
  {
    internal static IPRDEV Connect2SQL(string connectionString, ProgressChangedEventHandler progressChanged)
    {
      progressChanged(null, new ProgressChangedEventArgs(1, String.Format("Attempt to connect to SQL at: {0}", connectionString)));
      System.Data.IDbConnection _connection = new SqlConnection(connectionString);
      IPRDEV _entities = new IPRDEV(_connection);
      if (_entities.DatabaseExists())
        progressChanged(null, new ProgressChangedEventArgs(1, "The specified database can be opened."));
      else
        progressChanged(null, new ProgressChangedEventArgs(1, "The specified database cannot be opened."));
      return _entities;
    }

  }
}
