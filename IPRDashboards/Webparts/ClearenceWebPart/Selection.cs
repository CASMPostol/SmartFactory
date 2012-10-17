using System.Data;
namespace CAS.SmartFactory.IPR.Dashboards.Webparts.ClearenceWebPart
{
  /// <summary>
  /// Selection of compensation material.
  /// </summary>
  public partial class Selection
  {
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
    }
  }
}
