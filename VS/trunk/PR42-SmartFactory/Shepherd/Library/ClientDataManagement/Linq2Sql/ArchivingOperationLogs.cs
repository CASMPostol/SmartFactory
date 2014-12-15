using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq2SQL
{
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
    /// <summary>
    /// Gets the recent actions.
    /// </summary>
    /// <param name="entities">The entities <see cref="SHRARCHIVE"/>.</param>
    /// <param name="operation">The operation <see cref="OperationName"/>.</param>
    /// <returns>An instance of <see cref="ArchivingOperationLogs"/> or null in not found .</returns>
    public static ArchivingOperationLogs GetRecentActions(SHRARCHIVE entities, OperationName operation)
    {
      ArchivingOperationLogs _recentActions = entities.ArchivingOperationLogs.Where<ArchivingOperationLogs>(x => x.Operation.Contains(operation.ToString())).OrderByDescending<ArchivingOperationLogs, DateTime>(x => x.Date).FirstOrDefault();
      return _recentActions;
    }
  }
}
