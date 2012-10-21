using System;
using System.Collections.Generic;
using System.Data;
using CAS.SharePoint;
namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ClearenceWebPart
{
  /// <summary>
  /// Selection of compensation material.
  /// </summary>
  public partial class Selection
  {
    internal partial class SelectionTableRowWraper
    {
      internal SelectionTableRowWraper( Linq.IPR.Disposal _dspslx )
      {
        Disposal = true;
        DocumentNo = _dspslx.Disposal2IPRIndex.DocumentNo;
        DebtDate = _dspslx.Disposal2IPRIndex.CustomsDebtDate.Value;
        ValidTo = _dspslx.Disposal2IPRIndex.ValidToDate.Value;
        SKU = _dspslx.Disposal2IPRIndex.SKU;
        Batch = _dspslx.Disposal2IPRIndex.Batch;
        UnitPrice = _dspslx.Disposal2IPRIndex.IPRUnitPrice.Value;
        Currency = _dspslx.Disposal2IPRIndex.Currency;
        Quantity = _dspslx.SettledQuantity.Value;
        Status = _dspslx.DisposalStatus.Value.ToString();
        Created = _dspslx.CreatedDate.Value;
        ID = ( -_dspslx.Identyfikator.Value ).ToString();
      }
      internal SelectionTableRowWraper( Linq.IPR.IPR _iprx )
      {
        Disposal = false;
        DocumentNo = _iprx.DocumentNo;
        DebtDate = _iprx.CustomsDebtDate.Value;
        ValidTo = _iprx.OGLValidTo.Value;
        SKU = _iprx.SKU;
        Batch = _iprx.Batch;
        UnitPrice = _iprx.IPRUnitPrice.Value;
        Currency = _iprx.Currency;
        Quantity = _iprx.TobaccoNotAllocated.Value;
        Status = "IPR Material";
        Created = _iprx.CustomsDebtDate.Value;
        ID = _iprx.Identyfikator.Value.ToString();
      }
      internal bool Disposal { get; set; }
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
      internal int Identyfikator
      {
        get { return Math.Abs( ID.String2Int().Value ); }
      }
    }
    /// <summary>
    /// Represents the strongly named DataTable class.
    /// </summary>
    partial class SelectionTableDataTable
    {
      internal void GetRow( Selection.SelectionTableDataTable source, string id )
      {
        GetRow( source.FindByID( id ) );
      }
      internal void GetRow( Selection.SelectionTableRow sourceRow )
      {
        Selection.SelectionTableRow _destinationRow = FindByID( sourceRow.ID );
        if ( _destinationRow == null )
        {
          if ( sourceRow.RowState == DataRowState.Added )
            sourceRow.AcceptChanges();
          else
          {
            sourceRow.AcceptChanges();
            sourceRow.SetAdded();
          }
          ImportRow( sourceRow );
        }
        else
          _destinationRow.Quantity += sourceRow.Quantity;
        sourceRow.Delete();
      }
      internal void NewSelectionTableRow( SelectionTableRowWraper _rowx )
      {
        SelectionTableRow _nr = this.NewSelectionTableRow();
        _nr.Batch = _rowx.Batch.Trim();
        _nr.Created = _rowx.Created;
        _nr.Currency = _rowx.Currency;
        _nr.DocumentNo = _rowx.DocumentNo.Trim();
        _nr.DebtDate = _rowx.DebtDate;
        _nr.Disposal = _rowx.Disposal;
        _nr.ID = _rowx.ID.ToString();
        _nr.SKU = _rowx.SKU.Trim();
        _nr.Quantity = _rowx.Quantity;
        _nr.Status = _rowx.Status;
        _nr.UnitPrice = _rowx.UnitPrice;
        _nr.ValidTo = _rowx.ValidTo;
        AddSelectionTableRow( _nr );
      }
      internal IEnumerable<SelectionTableRow> OnlyDisposals
      {
        get
        {
          return from _dx in this where _dx.Disposal select _dx;
        }
      }
      internal IEnumerable<Selection.SelectionTableRow> OnlyAdded
      {
        get
        {
          return from _dx in this where _dx.RowState == DataRowState.Added select _dx;
        }
      }
      internal void Add( List<Selection.SelectionTableRowWraper> collection )
      {
        Clear();
        foreach ( Selection.SelectionTableRowWraper _rowx in collection )
          NewSelectionTableRow( _rowx );
        AcceptChanges();
      }
    }

    /// <summary>
    ///Represents strongly named DataRow class.
    ///</summary>
    public partial class SelectionTableRow
    {
      internal int Identyfikator
      {
        get { return Math.Abs( ID.String2Int().Value ); }
      }
      internal static bool IsDisposal( string ID )
      {
        return ID.Contains( "-" );
      }
    }
  }
}
