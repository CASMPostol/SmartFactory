using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
      return ( from _dec in this.Disposal where _dec.CustomsStatus.Value == CustomsStatus.NotStarted select _dec ).Any() ? ClearingType.TotalWindingUp : ClearingType.PartialWindingUp;
    }
  }
}
