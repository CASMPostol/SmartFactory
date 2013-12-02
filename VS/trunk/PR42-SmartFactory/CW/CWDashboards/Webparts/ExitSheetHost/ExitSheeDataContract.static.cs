﻿//<summary>
//  Title   : class ExitSheeDataContract
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
using System.Linq;
using CAS.SharePoint.Serialization;
using CAS.SmartFactory.CW.WebsiteModel.Linq;

namespace CAS.SmartFactory.CW.Dashboards.Webparts.ExitSheetHost
{

  /// <summary>
  /// class ExitSheeDataContract
  /// </summary>
  partial class ExitSheeDataContract
  {
    /// <summary>
    /// Deserializes the specified serialized object.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    public static ExitSheeDataContract Deserialize(string serializedObject)
    {
      return JsonSerializer.Deserialize<ExitSheeDataContract>(serializedObject);
    }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns><see cref="string"/> as serialized this object.</returns>
    public string Serialize()
    {
      return JsonSerializer.Serialize<ExitSheeDataContract>(this);
    }
    internal static ExitSheeDataContract GetExitSheeDataContract(Entities edc, WebsiteModel.Linq.CustomsWarehouseDisposal cwd)
    {
      Warehouse _wrh = (from _wrhx in edc.Warehouse where _wrhx.Procedure == cwd.CustomsProcedure select _wrhx).FirstOrDefault();
      string _warehouseName = _wrh == null ? "N/A" : _wrh.WarehouseName;
      ExitSheeDataContract _esdc = new ExitSheeDataContract
      {
        Batch = cwd.CWL_CWDisposal2CustomsWarehouseID.Batch,
        Grade = cwd.CWL_CWDisposal2CustomsWarehouseID.Grade,
        OGLDate = cwd.SADDate.GetValueOrDefault(),
        OGLNumber = cwd.SADDocumentNo,
        PackageToClear = Convert.ToInt32(cwd.CW_PackageToClear.GetValueOrDefault()),
        RemainingPackage = Convert.ToInt32(cwd.CW_RemainingPackage.GetValueOrDefault()),
        RemainingQuantity = cwd.RemainingQuantity.GetValueOrDefault(),
        SAD = cwd.CWL_CWDisposal2ClearanceID == null ? "N/A" : cwd.CWL_CWDisposal2ClearanceID.DocumentNo,
        SettledNetMass = cwd.CW_SettledNetMass.GetValueOrDefault(),
        SKU = cwd.CWL_CWDisposal2CustomsWarehouseID.SKU,
        TobaccoName = cwd.CWL_CWDisposal2CustomsWarehouseID.TobaccoName,
        WarehouseName = _warehouseName
      };
      return _esdc;
    }

  }
}
