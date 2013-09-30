//<summary>
//  Title   : class DisposalRequestObservable
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.SharePoint.Client;
using System.Linq;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.Entities
{
  /// <summary>
  /// class DisposalRequestObservable
  /// </summary>
  public class DisposalRequestObservable : ObservableCollection<DisposalRequest>
  {
    internal void GetDataContext(EntityList<CustomsWarehouseDisposalRowData> _list)
    {
      IEnumerable<IGrouping<string, CustomsWarehouseDisposalRowData>> _requests = _list.ToList<CustomsWarehouseDisposalRowData>().GroupBy<CustomsWarehouseDisposalRowData, string>(x => x.Batch);
      foreach (IGrouping<string, CustomsWarehouseDisposalRowData> _grx in _requests)
      {
        DisposalRequest _oc = new DisposalRequest();
        foreach (CustomsWarehouseDisposalRowData _cwdrdx in _grx)
        {
          _oc.GetDataContext(_cwdrdx);
        }
        //TODO get data from _grx
        this.Add(_oc);
      }
    }
    private static List<DisposalRequest> GetDisposalRequestList(EntityList<CustomsWarehouseDisposalRowData> _list)
    {
      throw new NotImplementedException();
    }
  }
}
