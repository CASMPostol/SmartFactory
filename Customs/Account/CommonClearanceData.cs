//<summary>
//  Title   : class CommonClearanceData
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

namespace CAS.SmartFactory.Customs.Account
{
  /// <summary>
  /// Common Clearance Data
  /// </summary>
  public class CommonClearanceData
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CommonClearanceData"/> class.
    /// </summary>
    /// <param name="clearenceLookup">The clearence lookup.</param>
    public CommonClearanceData(int clearenceLookup)
    {
      ClearenceLookup = clearenceLookup;
    }
    /// <summary>
    /// Gets or sets the clearence lookup.
    /// </summary>
    /// <value>
    /// The clearence lookup.
    /// </value>
    public int ClearenceLookup { get; private set; }

  }
}
