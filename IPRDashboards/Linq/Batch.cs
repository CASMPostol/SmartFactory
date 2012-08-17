﻿using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using ExportedProductType = CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.ProductType;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Batch
  {
    internal void Export
      (
        EntitiesDataContext edc,
        InvoiceContent productInvoice,
        List<CigaretteExportForm> consignment,
        string invoiceNoumber,
        string procedure,
        Clearence clearence,
        string masterDocumentNo,
        ref int _position
      )
    {
      bool closingBatch = this.FGQuantityAvailable == productInvoice.Quantity.Value;
      this.FGQuantityAvailable -= productInvoice.Quantity.Value;
      double _portion = productInvoice.Quantity.Value / this.FGQuantityKUKg.Value;
      List<Ingredient> _ingredients = new List<Ingredient>();
      foreach ( Material _materialIdx in this.Material )
        _materialIdx.Export( edc, _ingredients, closingBatch, invoiceNoumber, procedure, clearence, _portion );
      CigaretteExportForm _exportConsignment = CigaretteExportFormFactory.CigaretteExportForm( this, productInvoice, _portion, _ingredients, masterDocumentNo, ref _position, clearence.ProcedureCode );
      consignment.Add( _exportConsignment );
    }
    internal ActionResult ExportPossible( double? quantity )
    {
      ActionResult _result = new ActionResult();
      if ( !quantity.HasValue )
      {
        string _message = String.Format( Resources.NotValidValue.GetLocalizedString(), "Quantity" );
        _result.AddMessage( "ExportPossible", _message );
        return _result;
      }
      else if ( this.FGQuantityAvailable.Value < quantity.Value )
      {
        string _message = String.Format( Resources.QuantityIsUnavailable.GetLocalizedString(), this.FGQuantityAvailable.Value );
        _result.AddMessage( "ExportPossible", _message );
        return _result;
      }
      return _result;
    }
    private ExportedProductType Product
    {
      get
      {
        switch ( ProductType.Value )
        {
          case Linq.IPR.ProductType.Cutfiller:
            return ExportedProductType.Cutfiller;
          case Linq.IPR.ProductType.Cigarette:
            return ExportedProductType.Cigarette;
          default:
            string _prdct = String.Format( "Product: {0}", ProductType.Value );
            throw new ApplicationError( "Batch.Product", _prdct, "Wrong product of the expoerted goods", null );
        };
      }
    }
  }
}
