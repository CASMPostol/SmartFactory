//<summary>
//  Title   : Customs Warehousing Account Factory
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

using System.Collections.Generic;

namespace CAS.SmartFactory.Customs.Account
{
  /// <summary>
  /// Customs Warehousing Account Factory
  /// </summary>
  public interface ICWAccountFactory
  {

    /// <summary>
    /// Creates the Customs Warehousing account.
    /// </summary>
    /// <param name="accountData">The account data.</param>
    /// <param name="warnings">The warnings collection.</param>
    /// <param name="requestUrl">The The URL of a Windows SharePoint Services "14" Web site.</param>
    void CreateCWAccount(CommonAccountData accountData, List<Warnning> warnings, string requestUrl);


    /// <summary>
    /// Clear through customs.
    /// </summary>
    /// <param name="commonClearanceData">The common clearance data.</param>
    /// <param name="warnings">The warnings.</param>
    /// <param name="requestedUrl">The requested URL.</param>
    void ClearThroughCustoms(CommonClearanceData commonClearanceData, List<Warnning> warnings, string requestedUrl);

  }
}
