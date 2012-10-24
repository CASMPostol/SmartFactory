using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using ExportedProductType = CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.ProductType;

namespace CAS.SmartFactory.IPR.Dashboards.Clearance
{

  /// <summary>
  /// Batch Extension
  /// </summary>
  internal static class FinishedGoodsFormFactory
  {
    #region public

    /// <summary>
    /// Exports the specified _this.
    /// </summary>
    /// <param name="_this">The _this.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="productInvoice">The product invoice.</param>
    /// <param name="consignment">The consignment.</param>
    /// <param name="invoiceNumber">The invoice number.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="masterDocumentNo">The master document no.</param>
    /// <param name="_position">The _position.</param>
    /// <exception cref="ApplicationError">Batch.Export</exception>
    internal static void Do
      (
        Batch _this,
        Entities edc,
        InvoiceContent productInvoice,
        List<CigaretteExportForm> consignment,
        string invoiceNumber,
        string procedure,
        Clearence clearence,
        string masterDocumentNo,
        ref int _position
      )
    {
      string _at = "beginning";
      try
      {
        bool closingBatch = _this.FGQuantityAvailable == productInvoice.Quantity.Value;
        _at = "FGQuantityAvailable";
        _this.FGQuantityAvailable -= productInvoice.Quantity.Value;
        double _portion = productInvoice.Quantity.Value / _this.FGQuantity.Value;
        List<Ingredient> _ingredients = new List<Ingredient>();
        _at = "foreach";
        foreach ( Material _materialIdx in _this.Material )
          _materialIdx.Export( edc, _ingredients, closingBatch, invoiceNumber, procedure, clearence, _portion );
        _at = "CutfillerCoefficient";
        CutfillerCoefficient _cc = ( from _ccx in edc.CutfillerCoefficient orderby _ccx.Identyfikator descending select _ccx ).First();
        _at = "_exportConsignment";
        CigaretteExportForm _exportConsignment =
          CigaretteExportForm( _cc, _this, productInvoice, _portion, _ingredients, masterDocumentNo, ref _position, clearence.ProcedureCode );
        consignment.Add( _exportConsignment );
      }
      catch ( ApplicationError _ar )
      {
        throw _ar;
      }
      catch ( Exception _ex )
      {
        string _tmpl = "Cannot proceed with export of Batch: {0} because of error: {1}.";
        throw new ApplicationError( "Batch.Export", _at, String.Format( _tmpl, _this.Batch0, _ex.Message ), _ex );
      }
    }
    /// <summary>
    /// Exports the possible.
    /// </summary>
    /// <param name="_this">The _this.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns></returns>
    internal static ActionResult IsPossible( this Batch _this, double? quantity )
    {
      ActionResult _result = new ActionResult();
      if ( !quantity.HasValue )
      {
        string _message = String.Format( Resources.NotValidValue.GetLocalizedString(), "Quantity" );
        _result.AddMessage( "ExportPossible", _message );
        return _result;
      }
      else if ( _this.FGQuantityAvailable.Value < quantity.Value )
      {
        string _message = String.Format( Resources.QuantityIsUnavailable.GetLocalizedString(), _this.FGQuantityAvailable.Value );
        _result.AddMessage( "ExportPossible", _message );
        return _result;
      }
      return _result;
    }

    #endregion

    #region private

    /// <summary>
    /// Exports the specified material of <see cref="Material"/>.
    /// </summary>
    /// <param name="material">The _this.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="ingredient">The ingredient.</param>
    /// <param name="closingBatch">if set to <c>true</c> [closing batch].</param>
    /// <param name="invoiceNoumber">The invoice noumber.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="portion">The portion.</param>
    /// <exception cref="ApplicationError">Material.Export</exception>
    private static void Export( this Material material, Entities edc, List<Ingredient> ingredient, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence, double portion )
    {
      string _at = "Beginning";
      try
      {
        if ( material.ProductType.Value == CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.IPRTobacco )
        {
          double _quantity = material.DisposedQuantity( portion );
          _at = "GetListOfDisposals";
          foreach ( Disposal _disposal in material.GetListOfDisposals() )
          {
            if ( _quantity == 0 )
              break;
            _at = "_disposal.Export(";
            _disposal.Export( edc, ref _quantity, ingredient, closingBatch, invoiceNoumber, procedure, clearence );
          }
          string _template = "It is imposible the find the material {0} of {1} kg for invoice {2} on any IPR account";
          Anons.Assert( edc, _quantity == 0, "Material.Export", string.Format( _template, material.Batch, _quantity, invoiceNoumber ) );
        }
        else if ( material.ProductType.Value == CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Tobacco )
        {
          _at = "RegularIngredient";
          RegularIngredient _ri = new RegularIngredient( material.Batch, material.SKU, material.DisposedQuantity( portion ) );
          ingredient.Add( _ri );
        }
      }
      catch ( ApplicationError _ae )
      {
        throw _ae;
      }
      catch ( Exception _ex )
      {
        string _tmpl = "Cannot proceed with export of Material: {0} because of error: {1}.";
        throw new ApplicationError( "Material.Export", _at, String.Format( _tmpl, material.Material2BatchIndex.Title, _ex.Message ), _ex );
      }
    }
    private static ExportedProductType Product( CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType productType )
    {
      switch ( productType )
      {
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Cutfiller:
          return ExportedProductType.Cutfiller;
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Cigarette:
          return ExportedProductType.Cigarette;
        default:
          string _prdct = String.Format( "Product: {0}", productType );
          throw new ApplicationError( "Batch.Product", _prdct, "Wrong product of the expoerted goods", null );
      };
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
    private static void Export( this Disposal _this, Entities edc, ref double _quantity, List<Ingredient> ingredient, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence )
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
        IPRIngredient _ingredient = IPRIngredient( _this );
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
    //TODO - refctor the - move methods to a single helpper class : http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=3272
    private static IPRIngredient IPRIngredient( Disposal disposal )
    {
      IPRIngredient _ret = new IPRIngredient( disposal.Disposal2IPRIndex.Batch, disposal.Disposal2IPRIndex.SKU, disposal.SettledQuantity.Value );
      CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR _ipr = disposal.Disposal2IPRIndex;
      _ret.Currency = _ipr.Currency;
      _ret.Date = _ipr.CustomsDebtDate.Value;
      _ret.DocumentNoumber = _ipr.DocumentNo;
      _ret.Duty = disposal.DutyPerSettledAmount.Value;
      switch ( disposal.ClearingType.Value )
      {
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp:
          _ret.ItemClearingType = xml.DocumentsFactory.CigaretteExportForm.ClearingType.PartialWindingUp;
          break;
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.TotalWindingUp:
          _ret.ItemClearingType = xml.DocumentsFactory.CigaretteExportForm.ClearingType.PartialWindingUp;
          break;
        default:
          throw new ApplicationError( "InvoiceLib.IPRIngredient", "ItemClearingType", "Wrong Clearing Type", null );
      }
      _ret.TobaccoUnitPrice = _ipr.IPRUnitPrice.Value;
      _ret.TobaccoValue = disposal.TobaccoValue.Value;
      _ret.VAT = disposal.VATPerSettledAmount.Value;
      return _ret;
    }
    //public static RegularIngredient RegularIngredient( string batch, string sku, double quantity )
    //{
    //  return new RegularIngredient( batch, sku, quantity );
    //}
    private static CigaretteExportForm CigaretteExportForm
      ( CutfillerCoefficient cc, Batch batch, InvoiceContent invoice, double portion, List<Ingredient> _ingredients, string masretDocumentNo, ref int subdocumentNo, string customsProcedure )
    {
      CigaretteExportForm _ret = new CigaretteExportForm();
      if ( batch == null )
        throw new ArgumentNullException( "Batch cannot be null" );
      if ( batch.SKUIndex == null )
        throw new ArgumentNullException( "SKU in batch cannot be null" );
      if ( invoice == null )
        throw new ArgumentNullException( "Invoice cannot be null" );
      _ret.DocumentNo = String.Format( GlobalDefinitions.CigaretteExportFormNamePatern, masretDocumentNo, subdocumentNo++ );
      _ret.DustKg = ( batch.Dust.GetValueOrDefault( -1 ) * portion ).RountMass();
      CountExportFormTotals( _ingredients, _ret );
      _ret.Portion = portion;
      _ret.CustomsProcedure = customsProcedure;
      _ret.FinishedGoodBatch = batch.Batch0;
      _ret.FinishedGoodQantity = invoice.Quantity.GetValueOrDefault( 0 );
      _ret.FinishedGoodUnit = invoice.Units.GetLocalizedString();
      _ret.FinishedGoodSKU = batch.SKUIndex.SKU;
      _ret.FinishedGoodSKUDescription = batch.SKUIndex.Title();
      _ret.MaterialTotal = ( batch.Tobacco.GetValueOrDefault( -1 ) * portion ).RountMass();
      _ret.ProductFormat = batch.SKUIndex.FormatIndex.Title();
      //TODO [pr4-3697] Handling versions for selected lists: Batch, IPR
      _ret.CTFUsageMin = cc.CFTProductivityRateMin.GetValueOrDefault( -1 ) * 100;
      _ret.CTFUsageMax = cc.CFTProductivityRateMax.GetValueOrDefault( -1 ) * 100;
      _ret.CTFUsagePerUnitMin = cc.CFTProductivityRateMin.GetValueOrDefault( -1 );
      _ret.CTFUsagePerUnitMax = cc.CFTProductivityRateMax.GetValueOrDefault( -1 );
      _ret.CTFUsagePer1MFinishedGoodsMin = batch.UsageIndex.CTFUsageMin.GetValueOrDefault( -1 );
      _ret.CTFUsagePer1MFinishedGoodsMax = batch.UsageIndex.CTFUsageMax.GetValueOrDefault( -1 );
      _ret.WasteCoefficient = batch.BatchWasteCooeficiency.GetValueOrDefault( -1 ) + batch.BatchDustCooeficiency.GetValueOrDefault( -1 );
      switch ( batch.ProductType.Value )
      {
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Cutfiller:
          _ret.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cutfiller;
          break;
        case CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Cigarette:
          SKUCigarette _skuCigarette = batch.SKUIndex as SKUCigarette;
          _ret.BrandDescription = _skuCigarette.Brand;
          _ret.FamilyDescription = _skuCigarette.Family;
          _ret.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cigarette;
          break;
        default:
          throw new ApplicationError( "InvoiceLib.CigaretteExportForm", "Product", "Wrong ProductType", null );
      }
      _ret.SHMentholKg = ( batch.SHMenthol.GetValueOrDefault( -1 ) * portion ).RountMass();
      _ret.WasteKg = ( batch.Waste.GetValueOrDefault( -1 ) * portion ).RountMass();
      _ret.IPRRestMaterialQantityTotal = _ret.DustKg + _ret.SHMentholKg + _ret.WasteKg;
      _ret.TobaccoTotal = ( _ret.IPTMaterialQuantityTotal + _ret.RegularMaterialQuantityTotal + _ret.IPRRestMaterialQantityTotal ).RountMass();
      return _ret;
    }
    private static void CountExportFormTotals( List<Ingredient> _ingredients, CigaretteExportForm _ret )
    {
      _ret.Ingredients = _ingredients.ToArray();
      _ret.IPTMaterialQuantityTotal = 0;
      _ret.RegularMaterialQuantityTotal = 0;
      _ret.IPTDutyVatTotals = new TotalAmountOfMoney();
      foreach ( Ingredient _item in _ingredients )
      {
        if ( _item is IPRIngredient )
        {
          IPRIngredient _iprItem = (IPRIngredient)_item;
          _ret.IPTMaterialQuantityTotal += _iprItem.TobaccoQuantity;
          _ret.IPTDutyVatTotals.Add( new AmountOfMoney( _iprItem.TobaccoValue, _iprItem.Duty, _iprItem.VAT, _iprItem.Currency ) );
        }
        else
        {
          RegularIngredient _rglrItem = (RegularIngredient)_item;
          _ret.RegularMaterialQuantityTotal += _rglrItem.TobaccoQuantity;
        }
      }
      _ret.IPTMaterialQuantityTotal = _ret.IPTMaterialQuantityTotal.RountMass();
      _ret.IPTDutyVatTotals.AssignTotals();
      _ret.RegularMaterialQuantityTotal = _ret.RegularMaterialQuantityTotal.RountMass();
    }
    internal static CigaretteExportFormCollection CigaretteExportFormCollection( List<CigaretteExportForm> cigaretteExportForms, string documentNo, string invoiceNo )
    {
      CigaretteExportFormCollection _ret = new CigaretteExportFormCollection()
      {
        CigaretteExportForms = cigaretteExportForms.ToArray(),
        DocumentNo = documentNo,
        DocumentDate = DateTime.Now.Date,
        InvoiceNo = invoiceNo,
        NumberOfDocuments = cigaretteExportForms.Count
      };
      return _ret;
    }

    #endregion
  }
}
