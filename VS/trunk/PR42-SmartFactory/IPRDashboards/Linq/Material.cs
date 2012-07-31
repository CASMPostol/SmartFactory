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
      foreach ( Disposal _disposal in TobaccoList )
      {
      }
    }
    internal void Export( ExportConsignment _batchAnalysis, ref double maxIncrement )
    {
      if ( this.ProductType.Value == Linq.IPR.ProductType.IPRTobacco )
      {
        double _quantity = this.TobaccoQuantityKg.Value * _batchAnalysis.Portion;
        IPR _ca = null;
        bool _closing = false;
        foreach ( Disposal _disposal in TobaccoList )
        {
          _disposal.Export( ref _quantity, _batchAnalysis, ref maxIncrement);
        }
      }
      else if ( this.ProductType.Value == Linq.IPR.ProductType.Tobacco )
      {
      }
    }
    public List<Disposal> TobaccoList
    {
      get
      {
        return (
              from _didx in this.Disposal
              let _ipr = _didx.IPRID
              where _didx.CustomsStatus.Value == CustomsStatus.NotStarted && _didx.DisposalStatus.Value == DisposalStatus.TobaccoInCigaretesWarehouse
              orderby _ipr.Identyfikator descending
              select _didx
            ).ToList();
      }
    }
  }
}
