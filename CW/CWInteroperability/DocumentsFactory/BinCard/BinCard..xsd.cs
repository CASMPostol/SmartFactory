//<summary>
//  Title   : BinCardContentType custom part
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.CW.Interoperability.DocumentsFactory.BinCard
{
  /// <summary>
  /// BinCardContentType custom part 
  /// </summary>
  public partial class BinCardContentType
  {
    public static string StylesheetNmane { get { return "BinCardStylesheet"; } }
    public static BinCardContentType CreateEmptyContent()
    {
      return new BinCardContentType()
      {
        Batch = String.Empty,
        NetWeight = 0,
        NetWeightSpecified = false,
        PackageQuantity = 0,
        PackageQuantitySpecified = false,
        SAD = String.Empty,
        SADDate = DateTime.Today,
        SKU = String.Empty,
        PzNo = String.Empty,
        TobaccoName = String.Empty,
        TobaccoType = String.Empty
      };
    }
  }
}

