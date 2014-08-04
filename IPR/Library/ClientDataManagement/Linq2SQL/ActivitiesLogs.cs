//<summary>
//  Title   : partial class ActivitiesLogs
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

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL
{
  /// <summary>
  /// ActivitiesLogs table contains logs for each operation.
  /// </summary>
  public partial class ActivitiesLogs
  {

    /// <summary>
    /// The cleanup operation name
    /// </summary>
    public const string CleanupOperationName = "Cleanup";
    /// <summary>
    /// The synchronization operation name
    /// </summary>
    public const string SynchronizationOperationName = "Synchronization";
    /// <summary>
    /// The archiving operation name
    /// </summary>
    public const string ArchivingOperationName = "Archiving";

  }
}
