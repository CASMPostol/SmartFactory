
//<summary>
//  Title   : public partial class CustomsWarehouse
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
using System;
using System.Collections.Generic;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  public partial class CustomsWarehouse
  {

    internal void CreateDisposal( int disposalRequestId, List<CustomsWarehouseDisposal> _newDisposals, ref double toDispose )
    {
      double _Tdspse = Math.Min( this.TobaccoNotAllocated.Value, toDispose );
      CustomsWarehouseDisposal _new = new CustomsWarehouseDisposal()
      {
        CustomsStatus = CustomsStatus.NotStarted,
        CW_AddedKg = _Tdspse,
        CWL_CWDisposal2CustomsWarehouseID = this,
        CW_SettledNetMass = 0,
        Title = "TBD",
        SKUDescription = "N/A",
        DisposalRequestId = disposalRequestId
      };
      _newDisposals.Add( _new );
      toDispose -= _Tdspse;
    }
  }
}
