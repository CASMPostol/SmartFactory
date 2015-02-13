//_______________________________________________________________
//  Title   : BalanceBatchWrapper
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate:  $
//  $Rev: $
//  $LastChangedBy: $
//  $URL: $
//  $Id:  $
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Balance
{
  /// <summary>
  /// Struct BalanceBatchWrapper - to keep relationship between <see cref="BalanceBatch"/> and <see cref="BalanceIPR"/>
  /// </summary>
  public struct BalanceBatchWrapper
  {
    /// <summary>
    /// The batch to be wrapped
    /// </summary>
    public BalanceBatch batch;
    /// <summary>
    /// The ipr collection aggregated by the <see cref="BalanceBatch"/>
    /// </summary>
    public IEnumerable<BalanceIPR> iprCollection;
  }
}
