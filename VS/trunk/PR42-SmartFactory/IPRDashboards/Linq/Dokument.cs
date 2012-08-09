using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebCommonDefinitions = CAS.SharePoint.Web.CommonDefinitions;
using CAS.SharePoint;
using Microsoft.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Dokument
  {
    internal static int PrepareConsignment
      ( SPWeb site, List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm> _consignment, string fileName )
    {
      string _stt = "Starting";
      try
      {
        CigaretteExportFormCollection _cefc = new CigaretteExportFormCollection( _consignment );
        SPDocumentLibrary _lib = (SPDocumentLibrary)site.Lists[ CommonDefinitions.IPRSADConsignmentLibraryTitle ];
        _stt = "SPDocumentLibrary";
        _stt = "AddDocument2Collection";
        SPFile _docFile = CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportFormCollection.AddDocument2Collection( _lib.RootFolder.Files, fileName);
        return _docFile.Item.ID;
      }
      catch ( Exception ex )
      {
        throw new ApplicationError( "InvoiceLib.PrepareConsignment" , _stt ,String.Format("Cannot finish the operation because of error {0}", ex.Message), ex);
      }
    }
  }
  internal class IPRIngredient: CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.IPRIngredient
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
  internal class RegularIngredient: CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.RegularIngredient
  {
    public RegularIngredient( string batch, string sku, double quantity )
      : base( batch, sku, quantity )
    { }
  }
  internal class CigaretteExportForm: CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm
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
  internal class CigaretteExportFormCollection: CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportFormCollection
  {
    public CigaretteExportFormCollection( List<CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.CigaretteExportForm> cigaretteExportForms )
    {
      this.CigaretteExportForms = cigaretteExportForms.ToArray();
    }
  }
}
