using System;
using System.Collections.Generic;
using System.Linq;
using CAS.SharePoint;
using CAS.SmartFactory.xml.DocumentsFactory.CigaretteExportForm;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Material
  {
    internal void Export( Entities edc, List<Ingredient> ingredient, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence, double portion )
    {
      double _quantity = ( this.TobaccoQuantity.Value * portion ).RountMass();
      if ( this.ProductType.Value == Linq.IPR.ProductType.IPRTobacco )
      {
        try
        {
          foreach ( Disposal _disposal in GetListOfDisposals() )
          {
            if ( _quantity == 0 )
              break;
            _disposal.Export( edc, ref _quantity, ingredient, closingBatch, invoiceNoumber, procedure, clearence );
          }
          string _template = "It is imposible the find the material {0} of {1} kg for invoice {2} on any IPR account";
          Anons.Assert( edc, _quantity == 0, "Material.Export", string.Format( _template, this.Batch, _quantity, invoiceNoumber ) );
        }
        catch ( ApplicationError _ae )
        {
          throw _ae;
        }
        catch ( Exception _ex )
        {
          string _tmpl = "Cannot proceed with export of Material: {0} because of error: {1}.";
          throw new ApplicationError( "Material.Export", "", String.Format( _tmpl, this.Material2BatchIndex.Title, _ex.Message ), _ex );
        }
      }
      else if ( this.ProductType.Value == Linq.IPR.ProductType.Tobacco )
      {
        RegularIngredient _ri = new RegularIngredient( this.Batch, this.SKU, _quantity );
        ingredient.Add( _ri );
      }
    }
    private List<Disposal> GetListOfDisposals()
    {
      Linq.IPR.DisposalStatus status = this.Material2BatchIndex.ProductType.Value == Linq.IPR.ProductType.Cigarette ? DisposalStatus.TobaccoInCigaretes : DisposalStatus.TobaccoInCutfiller;
      return
        (
            from _didx in this.Disposal
            let _ipr = _didx.Disposal2IPRIndex
            where _didx.CustomsStatus.Value == CustomsStatus.NotStarted && _didx.DisposalStatus.Value == status
            orderby _ipr.Identyfikator ascending
            select _didx
        ).ToList();
    }
  }
}
