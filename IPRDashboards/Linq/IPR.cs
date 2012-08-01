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
      return Duty.Value - DisposalsInExportedCigarettes.Sum( val => val.DutyPerSettledAmount.Value );
    }
    internal double GetPriceNotCleared()
    {
      throw new NotImplementedException();
    }
    internal double GetVATNotCleared()
    {
      throw new NotImplementedException();
    }
    public IQueryable<Disposal> DisposalsInExportedCigarettes
    {
      get { return from _dec in this.Disposal where _dec.DisposalStatus.Value == DisposalStatus.TobaccoInCigaretesExported select _dec; }
    }
  }
}
