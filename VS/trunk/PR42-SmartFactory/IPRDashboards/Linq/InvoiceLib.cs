using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint.Web;
using CAS.SharePoint;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class InvoiceLib
  {
    internal static GenericStateMachineEngine.ActionResult PrepareConsignment( EntitiesDataContext entitiesDataContext, List<CigaretteExportForm> _consignment )
    {
      return GenericStateMachineEngine.ActionResult.Exception( new NotImplementedException(), "InvoiceLib.PrepareConsignment" );
    }
  }

  public class IPRIngredient: CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.IPRIngredient
  {
    public IPRIngredient( Disposal disposal )
      : base( disposal.IPRID.Batch, disposal.IPRID.SKU, disposal.SettledQuantity.Value )
    {
      IPR _ipr = disposal.IPRID;
      this.Currency = _ipr.Currency;
      this.Date = _ipr.CustomsDebtDate.Value;
      this.DocumentNoumber = _ipr.DocumentNo;
      this.Duty = disposal.DutyPerSettledAmount.Value;
      switch ( disposal.ClearingType.Value )
      {
        case ClearingType.PartialWindingUp:
          this.ItemClearingType = xml.DocumentsFactory.CigaretteExportForm.ClearingType.PartialWindingUp;
          break;
        case ClearingType.TotalWindingUp:
          this.ItemClearingType = xml.DocumentsFactory.CigaretteExportForm.ClearingType.PartialWindingUp;
          break;
        default:
          throw new ApplicationError( "InvoiceLib.IPRIngredient", "ItemClearingType", "Wrong Clearing Type", null );
      }
      this.TobaccoUnitPrice = _ipr.UnitPrice.Value;
      this.TobaccoValue = disposal.DutyPerSettledAmount.Value;
      this.VAT = disposal.VATPerSettledAmount.Value;
    }
  }
  public class RegularIngredient: CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.RegularIngredient
  {
    public RegularIngredient( string batch, string sku, double quantity )
      : base( batch, sku, quantity )
    { }
  }
  public class CigaretteExportForm: CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm
  {
    internal CigaretteExportForm( Batch batch, InvoiceContent invoice, double portion, List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.Ingredient> _ingredients )
    {
      if ( batch == null )
        throw new ArgumentNullException( "Batch cannot be null" );
      if ( batch.SKULookup == null )
        throw new ArgumentNullException( "SKU in batch cannot be null" );
      if ( batch.SKULookup.FormatLookup == null )
        throw new ArgumentNullException( "Format in SKU cannot be null" );
      if ( invoice == null )
        throw new ArgumentNullException( "Invoice cannot be null" );
      this.DustKg = batch.DustKg.Value * portion;
      this.Ingredients = _ingredients.ToArray();
      this.Portion = portion;
      switch ( batch.ProductType.Value )
      {
        case ProductType.Cutfiller:
          this.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cutfiller;
         break;
        case ProductType.Cigarette:
          this.Product = xml.DocumentsFactory.CigaretteExportForm.ProductType.Cigarette;
          break;
        default:
          throw new ApplicationError( "InvoiceLib.CigaretteExportForm", "Product", "Wrong ProductType", null );
      }
      this.ProductFormat = batch.SKULookup.FormatLookup.Tytuł;
      this.SHMentholKg = batch.SHMentholKg.Value * portion;
      this.WasteKg = batch.WasteKg.Value + portion;
    }
  }
  public class CigaretteExportFormCollection: CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportFormCollection
  {
    public CigaretteExportFormCollection(List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm> cigaretteExportForms)
    {
      this.CigaretteExportForms = cigaretteExportForms.ToArray();
    }
  }
}
