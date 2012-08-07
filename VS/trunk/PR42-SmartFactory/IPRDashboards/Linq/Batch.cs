using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Batch
  {
    internal void Export( EntitiesDataContext edc, InvoiceContent productInvoice, List<CigaretteExportForm> consignment, string invoiceNoumber, string procedure, Clearence clearence )
    {
      bool closingBatch = this.FGQuantityAvailable == productInvoice.Quantity.Value;
      this.FGQuantityAvailable -= productInvoice.Quantity.Value;
      double _portion = productInvoice.Quantity.Value / this.FGQuantityKUKg.Value;
      CigaretteExportForm _exportConsignment = new CigaretteExportForm( this, productInvoice, _portion );
      foreach ( Material _materialIdx in this.Material )
        _materialIdx.Export( edc, _exportConsignment, closingBatch, invoiceNoumber, procedure, clearence );
      consignment.Add( _exportConsignment );
    }
    internal ActionResult ExportPossible( double? quantity )
    {
      ActionResult _result = new ActionResult();
      if ( !quantity.HasValue )
      {
        string _message = String.Format( Resources.NotValidValue.GetLocalizedString( GlobalDefinitions.RootResourceFileName ), "Quantity" );
        _result.AddMessage( "ExportPossible", _message );
        return _result;
      }
      else if ( this.FGQuantityAvailable.Value < quantity.Value )
      {
        string _message = String.Format( Resources.QuantityIsUnavailable.GetLocalizedString( GlobalDefinitions.RootResourceFileName ), this.FGQuantityAvailable.Value );
        _result.AddMessage( "ExportPossible", _message );
        return _result;
      }
      return _result;
    }
  }
}
