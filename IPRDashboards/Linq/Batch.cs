using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Batch
  {
    internal void Export( EntitiesDataContext edc, InvoiceContent productInvoice, List<ExportConsignment> consignment, string invoiceNoumber, string procedure, Clearence clearence  )
    {
      this.FGQuantityAvailable -= productInvoice.Quantity.Value;
      double _portion = this.FGQuantityKUKg.Value / productInvoice.Quantity.Value;
      ExportConsignment _batchAnalysis = new ExportConsignment( this, productInvoice, _portion );
      double _maxIncrement = this.GetTobaccoQuantityWeCanAdd();
      foreach ( Material _didx in this.Material )
      {
        _didx.Export( edc, _batchAnalysis, ref _maxIncrement, invoiceNoumber, procedure, clearence );
      }
      consignment.Add( _batchAnalysis );
    }

    private double GetTobaccoQuantityWeCanAdd()
    {
      throw new NotImplementedException();
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
      double _portion = quantity.Value / this.FGQuantityKUKg.Value;
      foreach ( Material _didx in this.Material )
      {
        _didx.ExportPossible( quantity.Value, this.MaterialQuantity * _portion, _result );
      }
      return _result;
    }
    internal double TobaccoQuantity( double fgQuantity )
    {
      return this.ProductType.Value == Linq.IPR.ProductType.Cigarette ? this.TobaccoKg.Value / this.FGQuantityKUKg.Value * fgQuantity : fgQuantity;
    }
  }
}
