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
    public string Batch { get; set; }
    internal static PagedCollectionView GetDataContext(ListItemCollection itemsCollection)
    {
      ObservableCollection<CustomsWarehouseDisposalRowData> _oc = new ObservableCollection<CustomsWarehouseDisposalRowData>();
      // Generate some task data and add it to the task list.
      foreach (ListItem _lix in itemsCollection)
        _oc.Add(new CustomsWarehouseDisposalRowData(_lix));
      PagedCollectionView _return = new PagedCollectionView(_oc);
      //if (_return.CanGroup == true)
      //{
      //  // Group tasks by 
      //  _return.GroupDescriptions.Add(new PropertyGroupDescription("Batch"));
      //}
      if (_return.CanSort == true)
      {
        // By default, sort by ProjectName.
        _return.SortDescriptions.Add(new SortDescription("Batch", ListSortDirection.Ascending));
      }
      return _return;
    }
  }
}
