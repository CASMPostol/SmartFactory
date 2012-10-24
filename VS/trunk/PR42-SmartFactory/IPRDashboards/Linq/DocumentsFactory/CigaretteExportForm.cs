using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;

namespace CAS.SmartFactory.Linq.IPR.DocumentsFactory
{
  //TODO - refctor the - move methods to a single helpper class : http://cas_sp:11225/sites/awt/Lists/TaskList/DispForm.aspx?ID=3272
  internal static class IPRIngredientFactory
  {
    internal static IPRIngredient IPRIngredient( Disposal disposal )
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
  }
  internal static class CigaretteExportFormCollectionFactory
  {
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
  }
}

