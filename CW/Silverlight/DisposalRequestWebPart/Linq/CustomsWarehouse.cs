
//<summary>
//  Title   : public partial class CustomsWarehouse
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

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  public partial class CustomsWarehouse
  {

    internal CustomsWarehouseDisposal CreateDisposal( int disposalRequestLibId, ref int packagesToDispose )
    {
      int _TdspsePackages = Math.Min( CustomsWarehouseDisposal.Packages(this.TobaccoNotAllocated.Value, this.CW_MassPerPackage.Value), packagesToDispose);
      double _Tdspsekg = this.CW_MassPerPackage.Value * _TdspsePackages;
      CustomsWarehouseDisposal _NewDisposal = new CustomsWarehouseDisposal()
      {
        CustomsStatus = CustomsStatus.NotStarted,
        CW_AddedKg = _Tdspsekg,
        CW_DeclaredNetMass = 0,
        CW_SettledNetMass = _Tdspsekg,
        CW_PackageToClear = _TdspsePackages,
        CWL_CWDisposal2CustomsWarehouseID = this,
        Title = "TBD",
        SKUDescription = "N/A",
        DisposalRequestId = disposalRequestLibId 
      };
      packagesToDispose -= _TdspsePackages;
      return _NewDisposal;
    }

  }
}
