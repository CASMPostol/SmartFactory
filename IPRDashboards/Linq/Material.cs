using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;

namespace CAS.SmartFactory.Linq.IPR
{
  /// <summary>
  /// Material Extension
  /// </summary>
  public static class MaterialExtension
  {
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
    public static void Export( this Material material, Entities edc, List<Ingredient> ingredient, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence, double portion )
    {
      string _at = "Beginning";
      try
      {
        if ( material.ProductType.Value == Linq.IPR.ProductType.IPRTobacco )
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
        else if ( material.ProductType.Value == Linq.IPR.ProductType.Tobacco )
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
  }
}
