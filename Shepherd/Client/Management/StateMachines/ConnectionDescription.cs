//<summary>
//  Title   : ConnectionDescription
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

namespace CAS.SmartFactory.Shepherd.Client.Management.StateMachines
{

  /// <summary>
  /// Class ConnectionDescription - information to get access to SharePoint and SQL Database.
  /// </summary>
  public class ConnectionDescription
  {
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionDescription"/> class.
    /// </summary>
    /// <param name="sharePointServerURL">The SharePoint server website URL.</param>
    /// <param name="databaseName">Name of the SQL database.</param>
    /// <param name="sqlServer">The SQL server name.</param>
    public ConnectionDescription(string sharePointServerURL, string databaseName, string sqlServer)
    {
      SharePointServerURL = sharePointServerURL;
      DatabaseName = databaseName;
      SQLServer = sqlServer;
    }
    /// <summary>
    /// Gets the URL of the SharePoint application website.
    /// </summary>
    /// <value>The URL.</value>
    internal string SharePointServerURL { get; private set; }
    /// <summary>
    /// Gets the name of the SQL database containing backup of SharePoint application data content.
    /// </summary>
    /// <value>The name of the database.</value>
    internal string DatabaseName { get; private set; }
    /// <summary>
    /// Gets the SQL server part of the connection string.
    /// </summary>
    /// <value>The SQL server address.</value>
    internal string SQLServer { get; private set; }

  }
}
