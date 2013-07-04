//<summary>
//  Title   : Customs Warehousing Account Factory
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
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
  interface ICWAccountFactory
  {
    
    /// <summary>
    /// Creates the Customs Warehousing account.
    /// </summary>
    /// <param name="accountData">The account data.</param>
    /// <param name="warning">The warnings collection.</param>
    void CreateCWAccount( CommonAccountData accountData, List<Warnning> warning );
  }
}
