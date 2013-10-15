//<summary>
//  Title   : class DisposalRequestObservable
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.SharePoint.Client;
using System.Linq;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  /// <summary>
  /// class DisposalRequestObservable
  /// </summary>
  public class DisposalRequestObservable: ObservableCollection<DisposalRequest>
  {
    internal void GetDataContext( List<CustomsWarehouseDisposal> _list )
    {
      IEnumerable<IGrouping<string, CustomsWarehouseDisposal>> _requests = _list.ToList<CustomsWarehouseDisposal>().GroupBy<CustomsWarehouseDisposal, string>( x => x.CWL_CWDisposal2CustomsWarehouseID.Batch );
      foreach ( IGrouping<string, CustomsWarehouseDisposal> _grx in _requests )
      {
        CustomsWarehouseDisposal _first = _grx.First<CustomsWarehouseDisposal>();
        CustomsWarehouse _cw = _first.CWL_CWDisposal2CustomsWarehouseID != null ? _first.CWL_CWDisposal2CustomsWarehouseID : new CustomsWarehouse() { Units = "N/A", SKU = "N/A", CW_MassPerPackage = 0 };
        DisposalRequest _oc = new DisposalRequest()
        {
          AddedKg = 0,
          DeclaredNetMass = 0,
          Batch = _grx.Key,
          MassPerPackage = _cw.CW_MassPerPackage.Value,
          PackagesToClear = 0,
          QuantityyToClearSum = 0,
          QuantityyToClearSumRounded = 0,
          RemainingOnStock = 0,
          RemainingPackages = 0,
          SKUDescription = _first.SKUDescription,
          Title = "Title TBD",
          TotalStock = 0,
          Units = _first.CWL_CWDisposal2CustomsWarehouseID.Units,
          SKU = _first.CWL_CWDisposal2CustomsWarehouseID.SKU,
        };
        foreach ( CustomsWarehouseDisposal _cwdrdx in _grx )
          _oc.GetDataContext( _cwdrdx );
        _oc.Update();
        this.Add( _oc );
        _oc.AutoCalculation = true;
      }
    }
  }
}
