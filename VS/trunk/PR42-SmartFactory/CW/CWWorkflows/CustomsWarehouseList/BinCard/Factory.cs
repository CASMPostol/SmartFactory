//<summary>
//  Title   : BinCard Factory class
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
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.BinCard;
using CAS.SmartFactory.CW.WebsiteModel.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.BinCard
{
  /// <summary>
  /// BinCard Factory class
  /// </summary>
  internal static class Factory
  {
    internal static BinCardContentType CreateContent( CustomsWarehouse item )
    {

      string _at = "BinCardContentType";
      BinCardContentType _ret = new BinCardContentType()
      {
        Batch = item.Batch,
        NetWeight = item.CW_Quantity.ValueOrException<double>(m_Source, _at, "NetWeight"),
        NetWeightSpecified = true,
        PackageQuantity = item.CW_PackageUnits.ValueOrException<double>(m_Source, _at, "PackageQuantity"),
        PackageQuantitySpecified = true,
        PzNo = item.CW_PzNo,
        SAD = item.DocumentNo,
        SADDate = item.CustomsDebtDate.ValueOrException<DateTime>(m_Source, _at, "SADDate"),
        SKU = item.SKU,
        TobaccoName = item.TobaccoName,
        TobaccoType = item.Grade
      };
      return _ret;
    }
    private const double c_doubleDefault = -1;
    private const string m_Source = "CreateContent";
  }
}
