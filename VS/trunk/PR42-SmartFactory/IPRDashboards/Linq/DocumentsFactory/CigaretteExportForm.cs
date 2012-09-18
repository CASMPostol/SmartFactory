using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using CAS.SmartFactory.IPR.Dashboards;

namespace CAS.SmartFactory.Linq.IPR.DocumentsFactory
{
  internal class IPRIngredientFactory
  {
    internal static IPRIngredient IPRIngredient( Disposal disposal )
    {
      IPRIngredient _ret = new IPRIngredient( disposal.IPRID.Batch, disposal.IPRID.SKU, disposal.SettledQuantity.Value );
      IPR _ipr = disposal.IPRID;
      _ret.Currency = _ipr.Currency;
      _ret.Date = _ipr.CustomsDebtDate.Value;
      _ret.DocumentNoumber = _ipr.DocumentNo;
      _ret.Duty = disposal.DutyPerSettledAmount.Value;
      switch ( disposal.ClearingType.Value )
      {
        case ClearingType.PartialWindingUp:
          _ret.ItemClearingType = xml.DocumentsFactory.CigaretteExportForm.ClearingType.PartialWindingUp;
          break;
        case ClearingType.TotalWindingUp:
          _ret.ItemClearingType = xml.DocumentsFactory.CigaretteExportForm.ClearingType.PartialWindingUp;
          break;
        default:
          throw new ApplicationError( "InvoiceLib.IPRIngredient", "ItemClearingType", "Wrong Clearing Type", null );
      }
      _ret.TobaccoUnitPrice = _ipr.UnitPrice.Value;
      _ret.TobaccoValue = disposal.TobaccoValue.Value;
      _ret.VAT = disposal.VATPerSettledAmount.Value;
      return _ret;
    }
  }
  internal static class RegularIngredientFactory
  {
    public static RegularIngredient RegularIngredient( string batch, string sku, double quantity )
    {
      return new RegularIngredient( batch, sku, quantity );
    }
  }
  internal static class CigaretteExportFormFactory
  {
    internal static CigaretteExportForm CigaretteExportForm
      ( CutfillerCoefficient cc, Batch batch, InvoiceContent invoice, double portion, List<Ingredient> _ingredients, string masretDocumentNo, ref int subdocumentNo, string customsProcedure, string invoiceNumber )
    {
      CigaretteExportForm _ret = new CigaretteExportForm();
      if ( batch == null )
        throw new ArgumentNullException( "Batch cannot be null" );
      if ( batch.SKULookup == null )
        throw new ArgumentNullException( "SKU in batch cannot be null" );
      if ( invoice == null )
        throw new ArgumentNullException( "Invoice cannot be null" );
      _ret.DocumentNo = String.Format( GlobalDefinitions.CigaretteExportFormNamePatern, masretDocumentNo, subdocumentNo++ );
      _ret.DustKg = ( batch.DustKg.Value * portion ).RountMass();
      CountExportFormTotals( _ingredients, _ret );
      _ret.Portion = portion;
      _ret.CustomsProcedure = customsProcedure;
      _ret.FinishedGoodBatch = batch.Batch0;
      _ret.FinishedGoodQantity = invoice.Quantity.GetValueOrDefault( 0 );
      _ret.FinishedGoodUnit = invoice.Units.GetLocalizedString();
      _ret.FinishedGoodSKU = batch.SKULookup.SKU;
      _ret.FinishedGoodSKUDescription = batch.SKULookup.Title();
      _ret.InvoiceNo = invoiceNumber;
      _ret.MaterialTotal = ( batch.TobaccoKg.Value * portion ).RountMass();
      _ret.ProductFormat = batch.SKULookup.FormatLookup.Title();
      _ret.CTFUsageMin = cc.CFTProductivityRateMin.Value * 1000;
      _ret.CTFUsageMax = cc.CFTProductivityRateMax.Value * 1000;
      _ret.CTFUsagePerUnitMin = cc.CFTProductivityRateMin.Value;
      _ret.CTFUsagePerUnitMax = cc.CFTProductivityRateMax.Value;
      _ret.CTFUsagePer1MFinishedGoodsMin = batch.UsageLookup.CTFUsageMin.Value;
      _ret.CTFUsagePer1MFinishedGoodsMax = batch.UsageLookup.CTFUsageMax.Value;
      _ret.WasteCoefficient = batch.WasteCooeficiency.Value + batch.DustCooeficiency.Value;
      switch ( batch.ProductType.Value )
      {
        case ProductType.Cutfiller:
          _ret.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cutfiller;
          break;
        case ProductType.Cigarette:
          SKUCigarette _skuCigarette = batch.SKULookup as SKUCigarette;
          _ret.BrandDescription = _skuCigarette.Brand;
          _ret.FamilyDescription = _skuCigarette.Family;
          _ret.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cigarette;
          break;
        default:
          throw new ApplicationError( "InvoiceLib.CigaretteExportForm", "Product", "Wrong ProductType", null );
      }
      _ret.SHMentholKg = ( batch.SHMentholKg.Value * portion ).RountMass();
      _ret.WasteKg = ( batch.WasteKg.Value * portion ).RountMass();
      _ret.IPRRestMaterialQantityTotal = _ret.DustKg + _ret.SHMentholKg + _ret.WasteKg;
      _ret.TobaccoTotal = ( _ret.IPTMaterialQuantityTotal + _ret.RegularMaterialQuantityTotal + _ret.IPRRestMaterialQantityTotal ).RountMass();
      return _ret;
    }
    private static void CountExportFormTotals( List<Ingredient> _ingredients, CigaretteExportForm _ret )
    {
      _ret.Ingredients = _ingredients.ToArray();
      _ret.IPTMaterialQuantityTotal = 0;
      _ret.IPTMaterialValueTotal = 0;
      _ret.IPTMaterialDutyTotal = 0;
      _ret.IPTMaterialVATTotal = 0;
      _ret.RegularMaterialQuantityTotal = 0;
      foreach ( Ingredient _item in _ingredients )
      {
        if ( _item is IPRIngredient )
        {
          IPRIngredient _iprItem = (IPRIngredient)_item;
          _ret.IPTMaterialQuantityTotal += _iprItem.TobaccoQuantity;
          _ret.IPTMaterialValueTotal += _iprItem.TobaccoValue;
          _ret.IPTMaterialDutyTotal += _iprItem.Duty;
          _ret.IPTMaterialVATTotal += _iprItem.VAT;
        }
        else
        {
          RegularIngredient _rglrItem = (RegularIngredient)_item;
          _ret.RegularMaterialQuantityTotal += _rglrItem.TobaccoQuantity;
        }
      }
      _ret.IPTMaterialQuantityTotal = _ret.IPTMaterialQuantityTotal.RoundCurrency();
      _ret.IPTMaterialValueTotal = _ret.IPTMaterialValueTotal.RountMass();
      _ret.IPTMaterialDutyTotal = _ret.IPTMaterialDutyTotal.RountMass();
      _ret.IPTMaterialVATTotal = _ret.IPTMaterialVATTotal.RountMass();
      _ret.RegularMaterialQuantityTotal = _ret.RegularMaterialQuantityTotal.RountMass();
    }
  }
  internal static class CigaretteExportFormCollectionFactory
  {
    internal static CigaretteExportFormCollection CigaretteExportFormCollection( List<CigaretteExportForm> cigaretteExportForms, string documentNo )
    {
      //TODO [pr4-3719] Export: Association of the SAD documents - unique naming convention http://itrserver/Bugs/BugDetail.aspx?bid=3719
      CigaretteExportFormCollection _ret = new CigaretteExportFormCollection()
      {
        CigaretteExportForms = cigaretteExportForms.ToArray(),
        DocumentNo = documentNo,
        DocumentDate = DateTime.Now.Date
      };
      return _ret;
    }
  }
}

