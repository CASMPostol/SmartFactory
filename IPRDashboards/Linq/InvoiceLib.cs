﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class InvoiceLib
  {
    internal static GenericStateMachineEngine.ActionResult PrepareConsignment( EntitiesDataContext entitiesDataContext, List<CigaretteExportForm> _consignment )
    {
      
      return GenericStateMachineEngine.ActionResult.Exception( new NotImplementedException(), "InvoiceLib.PrepareConsignment" );
    }
  }
  internal abstract class Ingredient
  {
    internal Ingredient( double quantity )
    {
      IngredientQuantity = quantity;
    }
    internal double IngredientQuantity { get; private set; }
  }
  internal class IPRIngredient: Ingredient
  {
    public IPRIngredient( Disposal disposal )
      : base( disposal.SettledQuantity.Value )
    {
      this.disposal = disposal;
    }
    internal IPR IPRAccount { get { return disposal.IPRID; } }
    internal bool ClosingEntry { get { return disposal.ClearingType.Value == ClearingType.TotalWindingUp; } }
    private Disposal disposal;
  }
  internal class RegularIngredient: Ingredient
  {
    public RegularIngredient( double quantity, string sku, string batch )
      : base( quantity )
    {
      IngredientSKU = sku;
      IngredientBatch = batch;
    }
    public string IngredientSKU { get; private set; }
    public string IngredientBatch { get; private set; }
  }
  internal class CigaretteExportForm: List<Ingredient>
  {
    internal CigaretteExportForm( Batch batch, InvoiceContent invoice, double portion )
    {
      if ( batch == null )
        throw new ArgumentNullException( "Batch cannot be null" );
      if ( batch.SKULookup == null )
        throw new ArgumentNullException( "SKU in batch cannot be null" );
      if ( batch.SKULookup.FormatLookup == null )
        throw new ArgumentNullException( "Format in SKU cannot be null" );
      if ( invoice == null )
        throw new ArgumentNullException( "Invoice cannot be null" );
      ProductBatch = batch;
      ProductInvoice = invoice;
      Portion = portion;
      DustKg = batch.DustKg.Value * portion;
      SHMentholKg = batch.SHMentholKg.Value * portion;
      WasteKg = batch.WasteKg.Value + portion;
    }
    internal type SKU<type>()
      where type: SKUCommonPart
    {
      return (type)ProductBatch.SKULookup;
    }
    internal Format ProductFormat { get { return ProductBatch.SKULookup.FormatLookup; } }
    internal Batch ProductBatch { get; private set; }
    internal InvoiceContent ProductInvoice { get; private set; }
    internal double DustKg { get; private set; }
    internal double SHMentholKg { get; private set; }
    internal double WasteKg { get; private set; }
    internal double TotalDSWKg { get { return DustKg + SHMentholKg + WasteKg; } }
    internal double Portion { get; private set; }
    internal ProductType Product { get { return ProductBatch.ProductType.Value; } }
  }
}
