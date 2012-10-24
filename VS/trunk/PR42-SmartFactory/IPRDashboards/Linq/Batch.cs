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
  internal static class BatchExtension
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
    internal static void Export
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
    internal static ActionResult ExportPossible( this Batch _this, double? quantity )
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
    /// <summary>
    /// Exports the specified material of <see cref="Material"/>.
    /// </summary>
    /// <param name="material">The _this.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="ingredient">The ingredient.</param>
    /// <param name="closingBatch">if set to <c>true</c> [closing batch].</param>
    /// <param name="invoiceNoumber">The invoice noumber.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="portion">The portion.</param>
    /// <exception cref="ApplicationError">Material.Export</exception>
    private static void Export( this Material material, Entities edc, List<Ingredient> ingredient, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence, double portion )
    {
      string _at = "Beginning";
      try
      {
        if ( material.ProductType.Value == CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.IPRTobacco )
        {
          double _quantity = material.DisposedQuantity( portion );
          _at = "GetListOfDisposals";
          foreach ( Disposal _disposal in material.GetListOfDisposals() )
          {
            if ( _quantity == 0 )
              break;
            _at = "_disposal.Export(";
            _disposal.Export( edc, ref _quantity, ingredient, closingBatch, invoiceNoumber, procedure, clearence );
          }
          string _template = "It is imposible the find the material {0} of {1} kg for invoice {2} on any IPR account";
          Anons.Assert( edc, _quantity == 0, "Material.Export", string.Format( _template, material.Batch, _quantity, invoiceNoumber ) );
        }
        else if ( material.ProductType.Value == CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType.Tobacco )
        {
          _at = "RegularIngredient";
          RegularIngredient _ri = new RegularIngredient( material.Batch, material.SKU, material.DisposedQuantity( portion ) );
          ingredient.Add( _ri );
        }
      }
      catch ( ApplicationError _ae )
      {
        throw _ae;
      }
      catch ( Exception _ex )
      {
        string _tmpl = "Cannot proceed with export of Material: {0} because of error: {1}.";
        throw new ApplicationError( "Material.Export", _at, String.Format( _tmpl, material.Material2BatchIndex.Title, _ex.Message ), _ex );
      }
    }
    private static ExportedProductType Product( CAS.SmartFactory.IPR.WebsiteModel.Linq.ProductType productType )
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
    /// <summary>
    /// Exports the specified _this.
    /// </summary>
    /// <param name="_this">The _this.</param>
    /// <param name="edc">The edc.</param>
    /// <param name="_quantity">The _quantity.</param>
    /// <param name="ingredient">The ingredient.</param>
    /// <param name="closingBatch">if set to <c>true</c> [closing batch].</param>
    /// <param name="invoiceNoumber">The invoice noumber.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="clearence">The clearence.</param>
    /// <exception cref="ApplicationError">Disposal.Export</exception>
    private static void Export( this Disposal _this, Entities edc, ref double _quantity, List<Ingredient> ingredient, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence )
    {
      string _at = "startting";
      try
      {
        CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType _clearingType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp;
        if ( !closingBatch && _quantity < _this.SettledQuantity )
        {
          _at = "_newDisposal";
          Disposal _newDisposal = new Disposal()
          {
            Disposal2BatchIndex = _this.Disposal2BatchIndex,
            Disposal2ClearenceIndex = null,
            ClearingType = CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp,
            CustomsStatus = CustomsStatus.NotStarted,
            CustomsProcedure = "N/A",
            DisposalStatus = _this.DisposalStatus,
            InvoiceNo = "N/A",
            IPRDocumentNo = "N/A", // [pr4-3432] Disposal IPRDocumentNo - clarify  http://itrserver/Bugs/BugDetail.aspx?bid=3432
            Disposal2IPRIndex = _this.Disposal2IPRIndex,
            // CompensationGood = _this.CompensationGood, //TODO [pr4-3585] Wrong value for CompensationGood http://itrserver/Bugs/BugDetail.aspx?bid=3585
            JSOXCustomsSummaryIndex = null,
            Disposal2MaterialIndex = _this.Disposal2MaterialIndex,
            No = int.MaxValue,
            SADDate = CAS.SharePoint.Extensions.SPMinimum,
            SADDocumentNo = "N/A",
            SettledQuantity = _this.SettledQuantity - _quantity
          };
          _at = "SetUpCalculatedColumns";
          _newDisposal.SetUpCalculatedColumns( CAS.SmartFactory.IPR.WebsiteModel.Linq.ClearingType.PartialWindingUp );
          _this.SettledQuantity = _quantity;
          _quantity = 0;
          _at = "InsertOnSubmit";
          edc.Disposal.InsertOnSubmit( _newDisposal );
          _at = "SubmitChanges #1";
          edc.SubmitChanges();
        }
        else
        {
          _clearingType = _this.Disposal2IPRIndex.GetClearingType();
          _quantity -= _this.SettledQuantity.Value;
        }
        _at = "StartClearance";
        _this.StartClearance( _clearingType, invoiceNoumber, procedure, clearence );
        _at = "new IPRIngredient";
        IPRIngredient _ingredient = IPRIngredientFactory.IPRIngredient( _this );
        ingredient.Add( _ingredient );
        _at = "SubmitChanges #2";
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        string _tmpl = "Cannot proceed with export of disposal: {0} because of error: {1}.";
        throw new ApplicationError( "Disposal.Export", _at, String.Format( _tmpl, _this.Identyfikator, ex.Message ), ex );
      }
    }
  }
}
