//<summary>
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
      Warehouse _wrh = (from _wrhx in edc.Warehouse where _wrhx.SPProcedure == cwd.CustomsProcedure select _wrhx).FirstOrDefault();
      string _warehouseName = _wrh == null ? "N/A" : _wrh.WarehouseName;
      CustomsWarehouse _cw = cwd.CWL_CWDisposal2CustomsWarehouseID;
      if (_cw == null)
        throw new ArgumentNullException("CWL_CWDisposal2CustomsWarehouseID", "CustomsWarehouseDisposal is not connected to CustomsWarehouse");
      ExitSheeDataContract _esdc = new ExitSheeDataContract
      {
        Grade = cwd.CWL_CWDisposal2CustomsWarehouseID.Grade,
        OGLNumber = _cw.DocumentNo,
        PackageToClear = Convert.ToInt32(cwd.CW_PackageToClear.GetValueOrDefault()),
        RemainingPackage = Convert.ToInt32(cwd.CW_RemainingPackage.GetValueOrDefault()),
        RemainingQuantity = cwd.RemainingQuantity.GetValueOrDefault(),
        SAD = cwd.SADDocumentNo,
        SettledNetMass = cwd.CW_SettledNetMass.GetValueOrDefault(),
        WarehouseName = _warehouseName
      };
      return _esdc;
    }

  }
}
