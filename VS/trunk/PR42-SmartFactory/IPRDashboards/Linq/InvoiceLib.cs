﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class InvoiceLib
  {
    internal static GenericStateMachineEngine.ActionResult PrepareConsignment( EntitiesDataContext entitiesDataContext, List<ExportConsignment> _consignment )
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
    internal IPRIngredient( double quantity, IPR account )
      : base( quantity )
    {
      IPRAccount = account;
    }
    internal IPR IPRAccount { get; private set; }
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
  internal class ExportConsignment: List<Ingredient>
  {
    internal ExportConsignment( Batch batch, InvoiceContent invoice )
    {
      if (batch == null)
        throw new ArgumentNullException( "Batch cannot be null" );
      if ( batch.SKULookup == null )
        throw new ArgumentNullException( "SKU in batch cannot be null" );
      if ( batch.SKULookup.FormatLookup == null )
        throw new ArgumentNullException( "Format in SKU cannot be null" );
      if (invoice == null)
        throw new ArgumentNullException( "Invoice cannot be null" );
      ProductBatch = batch;
      ProductInvoice = invoice;
    }
    internal type SKU<type>()
      where type: SKUCommonPart
    {
      return (type)ProductBatch.SKULookup;
    }
    internal Format ProductFormat { get { return ProductBatch.SKULookup.FormatLookup; } }
    internal Batch ProductBatch { get; private set; }
    internal InvoiceContent ProductInvoice { get; private set; }
  }
}
