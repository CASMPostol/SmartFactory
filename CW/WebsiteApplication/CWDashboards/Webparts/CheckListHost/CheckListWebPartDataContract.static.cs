//<summary>
//  Title   : Name of Application
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
using CAS.SharePoint.Serialization;
using CAS.SmartFactory.CW.WebsiteModel.Linq;

namespace CAS.SmartFactory.CW.Dashboards.Webparts.CheckListHost
{
  /// <summary>
  /// partial class CheckListWebPartDataContract
  /// </summary>
  public partial class CheckListWebPartDataContract
  {
    /// <summary>
    /// Deserializes the specified serialized object.
    /// </summary>
    /// <param name="serializedObject">The serialized object.</param>
    public static CheckListWebPartDataContract Deserialize(string serializedObject)
    {
      return JsonSerializer.Deserialize<CheckListWebPartDataContract>(serializedObject);
    }
    /// <summary>
    /// Serializes this instance.
    /// </summary>
    /// <returns><see cref="string"/> as serialized this object.</returns>
    public string Serialize()
    {
      return JsonSerializer.Serialize<CheckListWebPartDataContract>(this);
    }
    internal static CheckListWebPartDataContract GetCheckListWebPartDataContract(WebsiteModel.Linq.Entities entities, WebsiteModel.Linq.DisposalRequestLib disposalRequestLib)
    {
      List<DisposalDescription> _dda = new List<DisposalDescription>();
      foreach (CustomsWarehouseDisposal _cwdx in disposalRequestLib.CustomsWarehouseDisposal(entities, false))
      {
        CustomsWarehouse _cw = _cwdx.CWL_CWDisposal2CustomsWarehouseID;
        if (_cw == null)
          throw new ArgumentNullException("CWL_CWDisposal2CustomsWarehouseID", "CustomsWarehouseDisposal is not connected to CustomsWarehouse");
        DisposalDescription _new = new DisposalDescription
        {
          OGLDate = _cw.CustomsDebtDate.GetValueOrDefault(),
          OGLNumber = _cw.DocumentNo,
          PackageToClear = Convert.ToInt32(_cw.AccountBalance.Value / _cw.CW_MassPerPackage.Value)
        };
        _dda.Add(_new);
      }
      CheckListWebPartDataContract _ret = new CheckListWebPartDataContract
      {
        Today = DateTime.Today,
        DisposalsList = _dda.ToArray()
      };
      return _ret;
    }
  }
}
