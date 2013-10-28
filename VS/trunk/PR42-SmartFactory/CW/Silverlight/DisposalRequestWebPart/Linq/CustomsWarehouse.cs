
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
      int _TdspsePackages = Math.Min(Linq.CustomsWarehouseDisposal.Packages( this.TobaccoNotAllocated.Value, this.CW_MassPerPackage.Value ), packagesToDispose );
      if ( _TdspsePackages == 0 )
        return null;
      double _Tdspsekg = this.CW_MassPerPackage.Value * _TdspsePackages;
      CustomsWarehouseDisposal _NewDisposal = Linq.CustomsWarehouseDisposal.Create( disposalRequestLibId, _TdspsePackages, _Tdspsekg, PackageWeight(), this );
      this.TobaccoNotAllocated -= _Tdspsekg;
      packagesToDispose -= _TdspsePackages;
      return _NewDisposal;
    }
    internal double PackageWeight()
    {
      return this.CW_PackageUnits.Value == 0 ? 0 : ( this.GrossMass.Value - this.CW_Quantity.Value ) / this.CW_PackageUnits.Value;
    }

  }
}
