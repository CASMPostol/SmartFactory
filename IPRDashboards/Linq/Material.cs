using System;
using System.Linq;
using CAS.SmartFactory.Linq.IPR;
using Microsoft.SharePoint.Linq;
using System.Collections.Generic;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Material
  {
    internal void ExportPossible( double p, double? nullable, ActionResult _result )
    {
      if ( this.ProductType.Value != Linq.IPR.ProductType.IPRTobacco )
        return;
      //foreach ( var item in this. )
      //{

      //}
    }
    internal void Export( ExportConsignment _batchAnalysis, double portion )
    {
      if ( this.ProductType.Value == Linq.IPR.ProductType.IPRTobacco )
      {
        double _quantity = this.TobaccoQuantityKg.Value * portion;
        IPR _ca = null;
        bool _closing = false;
        List<Disposal> _tobacco =
          (
            from _didx in this._disposals
            let _ipr = _didx.IPRID
            where _didx.CustomsStatus.Value == CustomsStatus.NotStarted && _didx.DisposalStatus.Value == DisposalStatus.TobaccoInCigaretesWarehouse
            orderby _ipr.Identyfikator
            select _didx
          ).ToList();
        foreach ( Disposal _disposal in _tobacco )
        {
          double _exported = _disposal.Export( _quantity );
          IPRIngredient _Ingredient = new IPRIngredient( _quantity, _ca, _closing );

        }
      }
      else if ( this.ProductType.Value == Linq.IPR.ProductType.Tobacco )
      {
      }
    }
    private EntitySet<Disposal> _disposals = null;
  }
}
