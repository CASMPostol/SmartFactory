using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using CAS.SmartFactory.xml.DocumentsFactory.Disposals;

namespace CAS.SmartFactory.Linq.IPR
{
  /// <summary>
  /// Disposal Extension
  /// </summary>
  public static class DisposalExtension
  {
    internal static MaterialRecord[] GetListOfMaterials( IEnumerable<Disposal> _disposals, ref double _subTotal )
    {
      List<MaterialRecord> _dustRecord = new List<MaterialRecord>();
      foreach ( Disposal _dx in _disposals )
      {
        _subTotal += _dx.SettledQuantity.Value;
        MaterialRecord _newRecord = new MaterialRecord()
        {
          Qantity = _dx.SettledQuantity.GetValueOrDefault( 0 ),
          Date = _dx.Disposal2IPRIndex.CustomsDebtDate.GetValueOrNull(),
          CustomDocumentNo = _dx.Disposal2IPRIndex.DocumentNo,
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
    /// <summary>
    /// Exports the specified _this.
    /// </summary>
    /// <param name="_this">The _this.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="_quantity">The _quantity.</param>
    /// <param name="ingredient">The ingredient.</param>
    /// <param name="closingBatch">if set to <c>true</c> [closing batch].</param>
    /// <param name="invoiceNoumber">The invoice noumber.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="clearence">The clearence.</param>
    /// <exception cref="ApplicationError">Disposal.Export</exception>
    public static void Export(this Disposal _this, Entities edc, ref double _quantity, List<Ingredient> ingredient, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence )
    {
      string _at = "startting";
      try
      {
        CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType _clearingType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp;
        if ( !closingBatch && _quantity < _this.SettledQuantity )
        {
          _at = "_newDisposal";
          Disposal _newDisposal = new Disposal()
          {
            Disposal2BatchIndex = _this.Disposal2BatchIndex,
            Disposal2ClearenceIndex = null,
            ClearingType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp,
            CustomsStatus = CustomsStatus.NotStarted,
            CustomsProcedure = "N/A",
            DisposalStatus = _this.DisposalStatus,
            InvoiceNo = "N/A",
            IPRDocumentNo = "N/A", // [pr4-3432] Disposal IPRDocumentNo - clarify  http://itrserver/Bugs/BugDetail.aspx?bid=3432
            Disposal2IPRIndex = _this.Disposal2IPRIndex,
            // CompensationGood = _this.CompensationGood, //TODO [pr4-3585] Wrong value for CompensationGood http://itrserver/Bugs/BugDetail.aspx?bid=3585
            JSOXCustomsSummaryIndex = null,
            Disposal2MaterialIndex = _this.Disposal2MaterialIndex,
            No = int.MaxValue,
            SADDate = CAS.SharePoint.Extensions.SPMinimum,
            SADDocumentNo = "N/A",
            SettledQuantity = _this.SettledQuantity - _quantity
          };
          _at = "SetUpCalculatedColumns";
          _newDisposal.SetUpCalculatedColumns( CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp );
          _this.SettledQuantity = _quantity;
          _quantity = 0;
          _at = "InsertOnSubmit";
          edc.Disposal.InsertOnSubmit( _newDisposal );
          _at = "SubmitChanges #1";
          edc.SubmitChanges();
        }
        else
        {
          _clearingType = _this.Disposal2IPRIndex.GetClearingType();
          _quantity -= _this.SettledQuantity.Value;
        }
        _at = "StartClearance";
        _this.StartClearance( _clearingType, invoiceNoumber, procedure, clearence );
        _at = "new IPRIngredient";
        IPRIngredient _ingredient = IPRIngredientFactory.IPRIngredient( _this );
        ingredient.Add( _ingredient );
        _at = "SubmitChanges #2";
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        string _tmpl = "Cannot proceed with export of disposal: {0} because of error: {1}.";
        throw new ApplicationError( "Disposal.Export", _at, String.Format( _tmpl, _this.Identyfikator, ex.Message ), ex );
      }
    }
  }
}
