using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Material
  {
    internal void Export( EntitiesDataContext edc, ExportConsignment _batchAnalysis, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence )
    {
      double _quantity = this.TobaccoQuantityKg.Value * _batchAnalysis.Portion;
      if ( this.ProductType.Value == Linq.IPR.ProductType.IPRTobacco )
        foreach ( Disposal _disposal in GetListOfDisposals(  ) )
          _disposal.Export( edc, ref _quantity, _batchAnalysis, closingBatch, invoiceNoumber, procedure, clearence );
      else if ( this.ProductType.Value == Linq.IPR.ProductType.Tobacco )
      {
        RegularIngredient _ri = new RegularIngredient( _quantity, this.SKU, this.Batch );
        _batchAnalysis.Add( _ri );
      }
    }
    private List<Disposal> GetListOfDisposals( )
    {
      Linq.IPR.DisposalStatus status = this.BatchLookup.ProductType.Value == Linq.IPR.ProductType.Cigarette ? DisposalStatus.TobaccoInCigaretesWarehouse : DisposalStatus.TobaccoInCutfillerWarehouse;
      return
        (
            from _didx in this.Disposal
            let _ipr = _didx.IPRID
            where _didx.CustomsStatus.Value == CustomsStatus.NotStarted && _didx.DisposalStatus.Value == status
            orderby _ipr.Identyfikator ascending
            select _didx
        ).ToList();
    }
  }
}
