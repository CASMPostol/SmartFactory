using System;
using System.Collections.Generic;
using CAS.SharePoint;
using CAS.SmartFactory.IPR.Dashboards;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Batch
  {
    internal void Export( double? quantity, List<ExportConsignment> consignment )
    {
      this.FGQuantityAvailable -= quantity.Value;
      double _portion = quantity.Value / this.FGQuantityKUKg.Value;
      foreach ( var _didx in this.Material )
      {

      }
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
  }
}
