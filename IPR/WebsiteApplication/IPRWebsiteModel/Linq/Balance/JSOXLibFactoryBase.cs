//<summary>
//  Title   : JSOXLibFactory class
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

using CAS.SharePoint.Logging;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Balance
{
  /// <summary>
  /// JSOXLibFactory class
  /// </summary>
  public abstract class JSOXLibFactoryBase
  {

    #region public
    /// <summary>
    /// Updates the JSOX report.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> representing data model.</param>
    /// <param name="previous">The previous report.</param>
    /// <param name="batches">The list of <see cref="BalanceBatchWrapper"/>.</param>
    /// <param name="trace">The trace action.</param>
    /// <returns><c>true</c>if the report is consistent, <c>false</c> otherwise.</returns>
    public bool CreateJSOXReport(Entities edc, JSOXLib previous, List<BalanceBatchWrapper> batches, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering JSOXLibFactoryBase.CreateJSOXReport", 40, TraceSeverity.Verbose);
      this.JSOXList.CreateJSOXReport(previous);
      return UpdateBalanceReport(edc, batches, trace);
    }
    /// <summary>
    /// Updates the balance report.
    /// </summary>
    /// <param name="edc">The <see cref="Entities" /> representing data model.</param>
    /// <param name="batches">The list of <see cref="BalanceBatchWrapper"/>.</param>
    /// <param name="trace">The trace action.</param>
    /// <returns><c>true</c>if the report is consistent, <c>false</c> otherwise.</returns>
    public bool UpdateBalanceReport(Entities edc, List<BalanceBatchWrapper> batches, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering JSOXLibFactoryBase.UpdateBalanceReport", 49, TraceSeverity.Verbose);
      return this.JSOXList.UpdateBalanceReport(edc, GetOutboundQuantity, batches, trace);
    }
    /// <summary>
    /// Gets the outbound quantity.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="parent">The parent.</param>
    /// <param name="start">The start date.</param>
    /// <param name="end">The end date.</param>
    /// <returns></returns>
    public abstract decimal GetOutboundQuantity(Entities entities, JSOXLib parent, out DateTime start, out DateTime end);
    /// <summary>
    /// Gets the JSOX list.
    /// </summary>
    /// <value>
    /// The JSOX list.
    /// </value>
    public JSOXLib JSOXList { get; private set; }
    /// <summary>
    /// Gets or sets a value indicating whether [JSOX library read only].
    /// </summary>
    /// <value>
    /// <c>true</c> if [JSOX library read only]; otherwise, <c>false</c>.
    /// </value>
    public bool JSOXLibraryReadOnly
    {
      get { return this.JSOXList.JSOXLibraryReadOnly.Value; }
      set { JSOXList.JSOXLibraryReadOnly = value; }
    }
    /// <summary>
    /// Gets the id of the associated <see cref="JSOXLib"/> entry.
    /// </summary>
    /// <value>
    /// The associated <see cref="JSOXLib"/> id.
    /// </value>
    public int Id { get { return JSOXList.Id.Value; } }
    #endregion

    #region private
    /// <summary>
    /// Gets the JSOX lib.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> representing data model.</param>
    /// <param name="jsoxLibindex">The jsox library index.</param>
    protected void GetJSOXLib(Entities edc, int jsoxLibindex)
    {
      this.JSOXList = Element.GetAtIndex<JSOXLib>(edc.JSOXLibrary, jsoxLibindex);
    }
    #endregion

  }
}
