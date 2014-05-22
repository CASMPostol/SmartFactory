using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using Microsoft.SharePoint;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ClearenceWebPart
{
  /// <summary>
  /// Selection of compensation material.
  /// </summary>
  public partial class Selection
  {
    internal partial class SelectionTableRowWraper
    {

      #region ctor
      internal SelectionTableRowWraper( Disposal _dspslx )
      {
        InitializeDisposal( _dspslx.Disposal2IPRIndex );
        Disposal = true;
        Quantity = _dspslx.SettledQuantity.Value;
        Status = _dspslx.DisposalStatus.Value.ToString();
        Created = _dspslx.Created.Value;
        ID = ( -_dspslx.Id.Value ).ToString();
      }
      internal SelectionTableRowWraper( IPRClass _iprx )
      {
        InitializeDisposal( _iprx );
        Disposal = false;
        Quantity = _iprx.TobaccoNotAllocated.Value;
        Status = "IPR Material";
        Created = _iprx.CustomsDebtDate.Value;
        ID = _iprx.Id.Value.ToString();
      }
      #endregion

      #region public
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
      internal int Id
      {
        get { return Math.Abs( ID.String2Int().Value ); }
      }
      #endregion

      #region private
      private void InitializeDisposal( IPRClass _ipr )
      {
        DocumentNo = _ipr.DocumentNo;
        DebtDate = _ipr.CustomsDebtDate.Value;
        ValidTo = _ipr.OGLValidTo.Value;
        SKU = _ipr.SKU;
        Batch = _ipr.Batch;
        UnitPrice = _ipr.IPRUnitPrice.Value;
        Currency = _ipr.Currency;
      }
      #endregion

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
        if ( sourceRow.RowState != DataRowState.Detached )
          sourceRow.AcceptChanges();
      }
      internal void NewSelectionTableRow( SelectionTableRowWraper _rowx )
      {
        SelectionTableRow _nr = this.NewSelectionTableRow();
        _nr.Batch = _rowx.Batch.Trim();
        _nr.Created = _rowx.Created.Date.ToString( "d",  CultureInfo.CurrentUICulture );
        _nr.Currency = _rowx.Currency;
        _nr.DocumentNo = _rowx.DocumentNo.Trim();
        _nr.DebtDate = _rowx.DebtDate.Date.ToString( "d", CultureInfo.CurrentUICulture );
        _nr.Disposal = _rowx.Disposal;
        _nr.ID = _rowx.ID.ToString();
        _nr.SKU = _rowx.SKU.Trim();
        _nr.Quantity = _rowx.Quantity;
        _nr.Status = _rowx.Status;
        _nr.UnitPrice = _rowx.UnitPrice;
        _nr.ValidTo = _rowx.ValidTo.Date.ToString( "d", CultureInfo.CurrentUICulture );
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
      internal int Id
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
