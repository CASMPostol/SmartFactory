using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.Entities
{
  internal partial class CustomsWarehouseDisposalRowData
  {
    private ListItem _lix;

    public CustomsWarehouseDisposalRowData(ListItem _lix)
    {
      // TODO: Complete member initialization
      this._lix = _lix;
    }
    public string SKU
    {
      get { return GetSecondaryLookup<string>("CWL_CWDisposal2CWSKU_sec"); }
    }
    public string Batch
    {
      get { return GetSecondaryLookup<string>("CWL_CWDisposal2CWBatch_sec"); }
    }
    public string MassPerPackage
    {
      get { return GetSecondaryLookup<string>("MassPerPackage"); }
    }
    public string Units
    {
      get { return GetSecondaryLookup<string>("CWL_CWDisposal2CWUnits_sec"); }
    }

    private string GetSecondaryLookup<T>(string p)
    {
      return ((FieldLookupValue)_lix[p]).LookupValue;
    }
  }
}
