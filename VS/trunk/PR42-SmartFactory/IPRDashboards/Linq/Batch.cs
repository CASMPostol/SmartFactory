using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.Linq.IPR.DocumentsFactory;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;
using ExportedProductType = CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm.ProductType;

namespace CAS.SmartFactory.Linq.IPR
{
  /// <summary>
  /// Batch Extension
  /// </summary>
  public static class BatchExtension
  {
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
    public static void Export
      (
        this Batch _this,
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
          CigaretteExportFormFactory.CigaretteExportForm( _cc, _this, productInvoice, _portion, _ingredients, masterDocumentNo, ref _position, clearence.ProcedureCode );
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
    public static ActionResult ExportPossible( this Batch _this, double? quantity )
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
    private static ExportedProductType Product(CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType productType )
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
  }
}
