using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class IPR
  {
    internal double GetDutyNotCleared()
    {
      return this.Duty.Value - ( from _dec in this.Disposal where _dec.DutyPerSettledAmount.HasValue select new { val = _dec.DutyPerSettledAmount.Value } ).Sum( itm => itm.val );
    }
    internal double GetPriceNotCleared()
    {
      return this.UnitPrice.Value - ( from _dec in this.Disposal where _dec.TobaccoValue.HasValue select new { val = _dec.TobaccoValue.Value } ).Sum( itm => itm.val );
    }
    internal double GetVATNotCleared()
    {
      return this.VAT.Value - ( from _dec in this.Disposal where _dec.VATPerSettledAmount.HasValue select new { val = _dec.VATPerSettledAmount.Value } ).Sum( itm => itm.val );
    }
    internal ClearingType GetClearingType()
    {
      return this.TobaccoNotAllocated == 0 &&
        (
          from _dec in this.Disposal
          where _dec.CustomsStatus.Value == CustomsStatus.NotStarted
          select _dec
        ).Count() == 1 ? ClearingType.TotalWindingUp : ClearingType.PartialWindingUp;
    }
    internal void CalcualteDutyAndVat( Disposal disposal, ClearingType clearingType )
    {
      double _portion = NetMass.Value / disposal.SettledQuantity.Value;
      if ( clearingType == Linq.IPR.ClearingType.PartialWindingUp )
      {
        disposal.DutyPerSettledAmount = ( Duty.Value * _portion ).RoundCurrency();
        disposal.VATPerSettledAmount = ( VAT.Value * _portion ).RoundCurrency();
        disposal.TobaccoValue = ( UnitPrice.Value * _portion ).RoundCurrency();
      }
      else
      {
        disposal.DutyPerSettledAmount = GetDutyNotCleared();
        disposal.VATPerSettledAmount = GetVATNotCleared();
        disposal.TobaccoValue = GetPriceNotCleared();
      }
      disposal.DutyAndVAT = disposal.DutyPerSettledAmount.Value + disposal.VATPerSettledAmount.Value;
    }
  }
}
