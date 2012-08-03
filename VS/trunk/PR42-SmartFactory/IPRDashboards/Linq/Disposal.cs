using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class Disposal
  {
    internal void Export( EntitiesDataContext edc, ref double _quantity, CigaretteExportForm _batchAnalysis, bool closingBatch, string invoiceNoumber, string procedure, Clearence clearence )
    {
      ClearingType _clearingType = Linq.IPR.ClearingType.PartialWindingUp;
      if ( !closingBatch && _quantity < this.SettledQuantity )
      {
        Disposal _newDisposal = (Disposal)this.MemberwiseClone();
        _newDisposal.SettledQuantity = this.SettledQuantity - _quantity;
        this.SettledQuantity = _quantity;
        _quantity = 0;
        edc.Disposal.InsertOnSubmit( _newDisposal );
        edc.SubmitChanges();
      }
      else
      {
        _clearingType = this.IPRID.GetClearingType();
        _quantity -= this.SettledQuantity.Value;
      }
      this.StartClearance( _clearingType, invoiceNoumber, procedure, clearence );
      IPRIngredient _ingredient = new IPRIngredient( this );
      _batchAnalysis.Add( _ingredient );
      edc.SubmitChanges();
    }
    private void StartClearance( ClearingType clearingType, string invoiceNoumber, string procedure, Clearence clearence )
    {
      double _portion = this.IPRID.NetMass.Value / this.SettledQuantity.Value;
      this.ClearenceListLookup = clearence;
      this.CustomsStatus = Linq.IPR.CustomsStatus.Started;
      this.ClearingType = clearingType;
      this.CustomsProcedure = procedure;
      this.InvoiceNo = invoiceNoumber;
      if ( ClearingType.Value == Linq.IPR.ClearingType.PartialWindingUp )
      {
        this.DutyPerSettledAmount = ( this.IPRID.Duty.Value * _portion ).RoundCurrency();
        this.VATPerSettledAmount = ( this.IPRID.VAT.Value * _portion ).RoundCurrency();
        this.TobaccoValue = ( this.IPRID.UnitPrice.Value * _portion ).RoundCurrency();
      }
      else
      {
        this.DutyPerSettledAmount = this.IPRID.GetDutyNotCleared();
        this.VATPerSettledAmount = this.IPRID.GetVATNotCleared();
        this.TobaccoValue = this.IPRID.GetPriceNotCleared();
      }
      this.DutyAndVAT = this.DutyPerSettledAmount.Value + this.VATPerSettledAmount.Value;
    }
  }
}
