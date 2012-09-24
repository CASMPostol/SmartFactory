using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using ExportedProductType = CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.ProductType;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Batch
  {
    internal void Export
      (
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
        bool closingBatch = this.FGQuantityAvailable == productInvoice.Quantity.Value;
        _at = "FGQuantityAvailable";
        this.FGQuantityAvailable -= productInvoice.Quantity.Value;
        double _portion = productInvoice.Quantity.Value / this.FGQuantity.Value;
        List<Ingredient> _ingredients = new List<Ingredient>();
        _at = "foreach";
        foreach ( Material _materialIdx in this.Material )
          _materialIdx.Export( edc, _ingredients, closingBatch, invoiceNumber, procedure, clearence, _portion );
        _at = "CutfillerCoefficient";
        CutfillerCoefficient _cc = ( from _ccx in edc.CutfillerCoefficient orderby _ccx.Identyfikator descending select _ccx ).First();
        _at = "_exportConsignment";
        CigaretteExportForm _exportConsignment =
          CigaretteExportFormFactory.CigaretteExportForm( _cc, this, productInvoice, _portion, _ingredients, masterDocumentNo, ref _position, clearence.ProcedureCode );
        consignment.Add( _exportConsignment );
      }
      catch ( ApplicationError _ar )
      {
        throw _ar;
      }
      catch ( Exception _ex )
      {
        string _tmpl = "Cannot proceed with export of Batch: {0} because of error: {1}.";
        throw new ApplicationError( "Batch.Export", _at, String.Format(_tmpl, this.Batch0, _ex.Message), _ex );
      }
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
