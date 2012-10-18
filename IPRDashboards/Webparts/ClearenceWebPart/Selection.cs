using System;
using System.Data;
namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ClearenceWebPart
{
  /// <summary>
  /// Selection of compensation material.
  /// </summary>
  public partial class Selection
  {
    internal partial class SelectionTableRowWraper
    {

      public SelectionTableRowWraper( Linq.IPR.Disposal _dspslx )
      {
        DocumentNo = _dspslx.Disposal2IPRIndex.DocumentNo;
        DebtDate = _dspslx.Disposal2IPRIndex.CustomsDebtDate.Value;
        ValidTo = _dspslx.Disposal2IPRIndex.ValidToDate.Value;
        SKU = _dspslx.Disposal2IPRIndex.SKU;
        Batch = _dspslx.Disposal2IPRIndex.Batch;
        UnitPrice = _dspslx.Disposal2IPRIndex.IPRUnitPrice.Value;
        Currency = _dspslx.Disposal2IPRIndex.Currency;
        Quantity = _dspslx.SettledQuantity.Value;
        Status = _dspslx.DisposalStatus.Value.ToString();
        Created = SharePoint.Extensions.SPMinimum;
        ID = _dspslx.Identyfikator.Value.ToString();
      }
      internal string DocumentNo { get; set; }
      internal DateTime DebtDate { get; set; }
      internal DateTime ValidTo { get; set; }
      internal double Quantity { get; set; }
      internal double UnitPrice { get; set; }
      internal string Currency { get; set; }
      internal DateTime Created { get; set; }
      internal string Status { get; set; }
      internal string Batch { get; set; }
      internal string SKU { get; set; }
      internal string ID { get; set; }
    }
    partial class SelectionTableDataTable
    {
      internal void GetRow( Selection.SelectionTableDataTable source, string _id )
      {
        Selection.SelectionTableRow _slctdItem = source.FindByID( _id );
        Selection.SelectionTableRow _oldRow = FindByID( _id );
        if ( _oldRow == null )
          ImportRow( _slctdItem );
        else
          _oldRow.Quantity += _slctdItem.Quantity;
        _slctdItem.Delete();
      }

      internal void NewSelectionTableRow( SelectionTableRowWraper _rowx )
      {
        SelectionTableRow _nr = this.NewSelectionTableRow();
        _nr.Batch = _rowx.Batch.Trim();
        _nr.Created = _rowx.Created;
        _nr.Currency = _rowx.Currency;
        _nr.DocumentNo = _rowx.DocumentNo.Trim();
        _nr.DebtDate = _rowx.DebtDate;
        _nr.ID = _rowx.ID.ToString();
        _nr.SKU = _rowx.SKU.Trim();
        _nr.Quantity = _rowx.Quantity;
        _nr.Status = _rowx.Status;
        _nr.UnitPrice = _rowx.UnitPrice;
        _nr.ValidTo = _rowx.ValidTo;
        AddSelectionTableRow( _nr );
      }
    }
  }
}
