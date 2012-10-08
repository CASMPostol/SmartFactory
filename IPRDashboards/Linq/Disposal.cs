using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Disposal
  {
    internal static MaterialRecord[] GetListOfMaterials( IEnumerable<Disposal> _disposals, string customsReference, ref double _subTotal )
    {
      List<MaterialRecord> _dustRecord = new List<MaterialRecord>();
      foreach ( Disposal _dx in _disposals )
      {
        _subTotal += _dx.SettledQuantity.Value;
        MaterialRecord _newRecord = new MaterialRecord()
        {
          Qantity = _dx.SettledQuantity.GetValueOrDefault( 0 ),
          Date = _dx.Disposal2IPRIndex.CustomsDebtDate.GetValueOrNull(),
          CustomDocumentNo = customsReference,
          FinishedGoodBatch = _dx.Disposal2BatchIndex == null ? String.Empty : _dx.Disposal2BatchIndex.Batch0,
          MaterialBatch = _dx.Disposal2IPRIndex.Batch,
          MaterialSKU = _dx.Disposal2IPRIndex.SKU,
          UnitPrice = _dx.Disposal2IPRIndex.IPRUnitPrice.Value,
          TobaccoValue = _dx.TobaccoValue.Value,
          Currency = _dx.Disposal2IPRIndex.Currency
        };
        _dustRecord.Add( _newRecord );
      }
      return _dustRecord.ToArray();
    }
    internal void Export( Entities edc, ref double _quantity, List<Ingredient> ingredient, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence )
    {
      string _at = "startting";
      try
      {
        ClearingType _clearingType = Linq.IPR.ClearingType.PartialWindingUp;
        if ( !closingBatch && _quantity < this.SettledQuantity )
        {
          _at = "_newDisposal";
          Disposal _newDisposal = new Disposal()
          {
            Disposal2BatchIndex = this.Disposal2BatchIndex,
            ClearenceIndex = null,
            ClearingType = Linq.IPR.ClearingType.PartialWindingUp,
            CustomsStatus = Linq.IPR.CustomsStatus.NotStarted,
            CustomsProcedure = "N/A",
            DisposalStatus = this.DisposalStatus,
            InvoiceNo = "N/A",
            IPRDocumentNo = "N/A", // [pr4-3432] Disposal IPRDocumentNo - clarify  http://itrserver/Bugs/BugDetail.aspx?bid=3432
            Disposal2IPRIndex = this.Disposal2IPRIndex,
            // CompensationGood = this.CompensationGood, //TODO [pr4-3585] Wrong value for CompensationGood http://itrserver/Bugs/BugDetail.aspx?bid=3585
            JSOXCustomsSummaryIndex = null,
            Disposal2MaterialIndex = this.Disposal2MaterialIndex,
            No = int.MaxValue,
            SADDate = CAS.SharePoint.Extensions.SPMinimum,
            SADDocumentNo = "N/A",
            SettledQuantity = this.SettledQuantity - _quantity
          };
          _at = "SetUpCalculatedColumns";
          _newDisposal.SetUpCalculatedColumns( Linq.IPR.ClearingType.PartialWindingUp );
          this.SettledQuantity = _quantity;
          _quantity = 0;
          _at = "InsertOnSubmit";
          edc.Disposal.InsertOnSubmit( _newDisposal );
          _at = "SubmitChanges #1";
          edc.SubmitChanges();
        }
        else
        {
          _clearingType = this.Disposal2IPRIndex.GetClearingType();
          _quantity -= this.SettledQuantity.Value;
        }
        _at = "StartClearance";
        this.StartClearance( _clearingType, invoiceNoumber, procedure, clearence );
        _at = "new IPRIngredient";
        IPRIngredient _ingredient = IPRIngredientFactory.IPRIngredient( this );
        ingredient.Add( _ingredient );
        _at = "SubmitChanges #2";
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        string _tmpl = "Cannot proceed with export of disposal: {0} because of error: {1}.";
        throw new ApplicationError( "Disposal.Export", _at, String.Format( _tmpl, this.Identyfikator, ex.Message ), ex );
      }
    }
    private void StartClearance( ClearingType clearingType, string invoiceNoumber, string procedure, Clearence clearence )
    {
      this.ClearenceIndex = clearence;
      this.CustomsStatus = Linq.IPR.CustomsStatus.Started;
      this.ClearingType = clearingType;
      this.CustomsProcedure = procedure;
      this.InvoiceNo = invoiceNoumber;
      this.SetUpCalculatedColumns( ClearingType.Value );
    }
  }
}
