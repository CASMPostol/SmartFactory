//<summary>
//  Title   : ConnectionData
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

using CAS.SharePoint.Client.WebReferences;

namespace CAS.SmartFactory.Shepherd.Client.Management.Services
{
  /// <summary>
  /// Class ConnectionData contains recent information gathered during connection to the SharePoint and SQL database
  /// </summary>
  public class ConnectionData
  {
    /// <summary>
    /// Gets this service.
    /// </summary>
    /// <value>The services.</value>
    internal static ConnectionData ThisInstance
    {
      get
      {
        if (m_Singleton == null)
          m_Singleton = new ConnectionData();
        return m_Singleton;
      }
    }
    internal ConnectionDescription ConnectionDescription { get; set; }
    public string CleanupLastRunBy = Properties.Settings.Default.RunByError;
    public string SyncLastRunBy = Properties.Settings.Default.RunByError;
    public string ArchivingLastRunBy = Properties.Settings.Default.RunByError;
    public string CleanupLastRunDate = Properties.Settings.Default.RunDateError;
    public string SyncLastRunDate = Properties.Settings.Default.RunDateError;
    public string ArchivingLastRunDate = Properties.Settings.Default.RunDateError;
    public bool SQLConnected = false;
    public bool SPConnected = false;
    /// <summary>
    /// Prevents a default instance of the <see cref="ConnectionData"/> class from being created.
    /// </summary>
    private ConnectionData() { }
    private static ConnectionData m_Singleton;
  }
}
