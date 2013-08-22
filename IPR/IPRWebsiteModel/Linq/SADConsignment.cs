//<summary>
//  Title   : Name of Application
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
      

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// SADConsignment entity partial class
  /// </summary>
  public partial class SADConsignment
  {
    /// <summary>
    /// returns document number.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="sadConsignmentNumber">The sad consignment number.</param>
    /// <returns></returns>
    internal static string DocumentNumber( Entities entities, int sadConsignmentNumber )
    {
      return Settings.DocumentNumber( entities, sadConsignmentNumber );
    }
  }
}
