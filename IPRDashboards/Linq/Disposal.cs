using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Disposal
  {

    internal double Export( double _quantity )
    {
      //TODO  new NotImplementedException();
      return 0;
    }

    internal void Export( EntitiesDataContext edc, ref double _quantity, ExportConsignment _batchAnalysis, ref double maxIncrement, string invoiceNoumber, string procedure, Clearence clearence )
    {
      double _q2Export = _quantity;
      bool _closing = false;
      if ( this.SettledQuantity.Value < _quantity )
      {
        _q2Export = this.SettledQuantity.Value;
        _quantity -= _q2Export;
        this.StartClearance( Linq.IPR.ClearingType.TotalWindingUp, invoiceNoumber, procedure, clearence );
      }
      else
      {
        _closing = this.SettledQuantity.Value - _quantity <= Math.Min( maxIncrement, 1 );
        if ( _closing )
        {
          _q2Export = this.SettledQuantity.Value;
          _quantity -= _q2Export;
          this.CustomsStatus = Linq.IPR.CustomsStatus.Started;
        }
        else
        {
          _q2Export = _quantity;
          _quantity = 0;
          Disposal _newDisposal = new Disposal()
          {
            BatchLookup = this.BatchLookup,
            ClearingType = this.ClearingType,
            CompensationGood = this.CompensationGood,
            DisposalStatus = this.DisposalStatus,
            IPRDocumentNo = this.IPRDocumentNo,
            IPRID = this.IPRID,
            JSOXCustomsSummaryListLookup = this.JSOXCustomsSummaryListLookup,
            MaterialLookup = this.MaterialLookup,
            SettledQuantity = _q2Export,
          };
          edc.Disposal.InsertOnSubmit( _newDisposal );
          edc.SubmitChanges();
        }
      }
      IPRIngredient _ingredient = new IPRIngredient( _quantity, this.IPRID, _closing );
      _batchAnalysis.Add( _ingredient );
      throw new NotImplementedException();
    }

    private void StartClearance( ClearingType clearingType, string invoiceNoumber, string procedure, Clearence clearence )
    {
      double _portion = this.IPRID.NetMass.Value / this.SettledQuantity.Value;
      this.ClearenceListLookup = clearence;
      this.CustomsStatus = Linq.IPR.CustomsStatus.Started;
      this.ClearingType = clearingType;
      this.CustomsProcedure = procedure;
      this.InvoiceNo = invoiceNoumber;
      this.DutyAndVAT = this.IPRID.Duty.Value;
      if ( ClearingType.Value == Linq.IPR.ClearingType.PartialWindingUp )
      {
        this.DutyPerSettledAmount = Math.Round( this.IPRID.Duty.Value * _portion, 2 );
        this.TobaccoValue = Math.Round( this.IPRID.UnitPrice.Value * _portion, 2 );
        this.VATPerSettledAmount = Math.Round( this.IPRID.VAT.Value * _portion, 2 );
      }
      else
      {
        this.DutyPerSettledAmount = this.IPRID.Duty.Value - this.IPRID.GetDutyNotCleared();
        this.TobaccoValue = this.IPRID.UnitPrice.Value - this.IPRID.GetPriceNotCleared();
        this.VATPerSettledAmount = Math.Round( this.IPRID.VAT.Value - this.IPRID.GetVATNotCleared() );
      }
    }

    private void StartClearance( ClearingType clearingType )
    {
      throw new NotImplementedException();
    }
  }
}
