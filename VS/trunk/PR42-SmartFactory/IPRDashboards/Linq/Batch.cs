using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Batch
  {
    internal void Export( EntitiesDataContext edc, InvoiceContent productInvoice, List<ExportConsignment> consignment, string invoiceNoumber, string procedure, Clearence clearence )
    {
      bool closingBatch = this.FGQuantityAvailable == productInvoice.Quantity.Value;
      this.FGQuantityAvailable -= productInvoice.Quantity.Value;
      double _portion = closingBatch ? 1 : this.FGQuantityKUKg.Value / productInvoice.Quantity.Value;
      ExportConsignment _batchAnalysis = new ExportConsignment( this, productInvoice, _portion );
      foreach ( Material _didx in this.Material )
        _didx.Export( edc, _batchAnalysis, closingBatch, invoiceNoumber, procedure, clearence );
      consignment.Add( _batchAnalysis );
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
