//<summary>
//  Title   : ArchivingOperationLogs
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
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Client.DataManagement.Linq2SQL
{
  /// <summary>
  /// Class ArchivingOperationLogs - enumerates all operations available for the website.
  /// </summary>
  public partial class ArchivingOperationLogs
  {
    /// <summary>
    /// Operation Name enumerator. The names are used to report operation done for the content.
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
