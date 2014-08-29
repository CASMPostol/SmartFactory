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

using System;
using System.ComponentModel;
using System.Linq;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL
{

  /// <summary>
  /// ActivitiesLogs table contains logs for each operation.
  /// </summary>
  public partial class ArchivingOperationLogs
  {
    /// <summary>
    /// Operation Name 
    /// </summary>
    public enum OperationName
    {

      /// <summary>
      /// The cleanup operation name
      /// </summary>
      Cleanup,
      /// <summary>
      /// The synchronization operation name
      /// </summary>
      Synchronization,
      /// <summary>
      /// The archiving operation name
      /// </summary>
      Archiving
    }
    internal static void UpdateActivitiesLogs(IPRDEV sqlEntities, OperationName operation, Action<object, ProgressChangedEventArgs> progressChanged)
    {
      Linq2SQL.ArchivingOperationLogs _logs = new ArchivingOperationLogs()
      {
        Date = DateTime.Now,
        Operation = operation.ToString(),
        UserName = Extensions.UserName()
      };
      sqlEntities.ArchivingOperationLogs.InsertOnSubmit(_logs);
      sqlEntities.SubmitChanges();
      progressChanged(1, new ProgressChangedEventArgs(1, "Updated ActivitiesLogs"));
    }
    /// <summary>
    /// Gets the recent actions.
    /// </summary>
    /// <param name="entities">The _entities.</param>
    /// <param name="operation">The operation.</param>
    /// <returns></returns>
    public static ArchivingOperationLogs GetRecentActions(IPRDEV entities, OperationName operation)
    {
      ArchivingOperationLogs _recentActions = entities.ArchivingOperationLogs.Where<ArchivingOperationLogs>(x => x.Operation.Contains(operation.ToString())).OrderByDescending<ArchivingOperationLogs, DateTime>(x => x.Date).FirstOrDefault();
      return _recentActions;
    }

  }
}
