using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SharePoint;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class IPR
  {
    internal ClearingType GetClearingType()
    {
      return this.TobaccoNotAllocated == 0 &&
        (
          from _dec in this.Disposal
          where _dec.CustomsStatus.Value == CustomsStatus.NotStarted
          select _dec
        ).Count() == 1 ? ClearingType.TotalWindingUp : ClearingType.PartialWindingUp;
    }

    internal double CalculateDutyAndVAT( double p )
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }

    internal double CalculateVATPerSettledAmount( double p )
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }

    internal double CalculateTobaccoValue( double p )
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }

    internal void Withdraw( double p )
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }

    internal void RevertWithdraw( double? nullable )
    {
      //TODO NotImplementedException
      throw new NotImplementedException();
    }
  }
}
