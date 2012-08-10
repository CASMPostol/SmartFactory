using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;

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
    internal static CigaretteExportForm CigaretteExportForm( Batch batch, InvoiceContent invoice, double portion, List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.Ingredient> _ingredients )
    {
      CigaretteExportForm _ret = new CigaretteExportForm();
      if ( batch == null )
        throw new ArgumentNullException( "Batch cannot be null" );
      if ( batch.SKULookup == null )
        throw new ArgumentNullException( "SKU in batch cannot be null" );
      if ( batch.SKULookup.FormatLookup == null )
        throw new ArgumentNullException( "Format in SKU cannot be null" );
      if ( invoice == null )
        throw new ArgumentNullException( "Invoice cannot be null" );
      _ret.DustKg = batch.DustKg.Value * portion;
      _ret.Ingredients = _ingredients.ToArray();
      _ret.Portion = portion;
      switch ( batch.ProductType.Value )
      {
        case ProductType.Cutfiller:
          _ret.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cutfiller;
          break;
        case ProductType.Cigarette:
          _ret.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cigarette;
          break;
        default:
          throw new ApplicationError( "InvoiceLib.CigaretteExportForm", "Product", "Wrong ProductType", null );
      }
      _ret.ProductFormat = batch.SKULookup.FormatLookup.Tytuł;
      _ret.SHMentholKg = batch.SHMentholKg.Value * portion;
      _ret.WasteKg = batch.WasteKg.Value + portion;
      return _ret;
    }
  }
  internal static class CigaretteExportFormCollectionFactory
  {
    internal static CigaretteExportFormCollection CigaretteExportFormCollection( List<CigaretteExportForm> cigaretteExportForms )
    {
      CigaretteExportFormCollection _ret = new CigaretteExportFormCollection()
      {
        CigaretteExportForms = cigaretteExportForms.ToArray()
      };
      return _ret;
    }
  }
}

