using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ClearenceWebPart
{
  [DataObject()]
  public class SelectionDataObject
  {
    private Selection m_Selection = null;
    public SelectionDataObject( Selection selection )
    {
      this.m_Selection = selection;
    }
    [System.ComponentModel.DataObjectMethodAttribute( System.ComponentModel.DataObjectMethodType.Select, true )]
    public Selection.SelectionTableDataTable GetItems()
    {
      return m_Selection.SelectionTable;
    }
    [System.ComponentModel.DataObjectMethodAttribute( System.ComponentModel.DataObjectMethodType.Select, false )]
    public Selection.SelectionTableDataTable GetItemByItemID( int ID )
    {
      Selection.SelectionTableDataTable _newTAble = new Selection.SelectionTableDataTable();
      foreach ( var item in from _itmx in m_Selection.SelectionTable where _itmx.ID == ID select _itmx )
        _newTAble.ImportRow( item );
      return _newTAble;
    }
    [System.ComponentModel.DataObjectMethodAttribute( System.ComponentModel.DataObjectMethodType.Update, true )]
    public bool UpdateItem( string DocumentNo, DateTime DebtDate, DateTime ValidTo, double Quantity, int original_ID )
    {
      Selection.SelectionTableRow _row = m_Selection.SelectionTable.FindByID( original_ID );
      if ( Quantity > _row.Quantity )
        throw SharePoint.Web.GenericStateMachineEngine.ActionResult.NotValidated( "You cannot withdraw more than there is on the stock." );
      _row.Quantity -= Quantity;
      return true;
    }
  }
}
