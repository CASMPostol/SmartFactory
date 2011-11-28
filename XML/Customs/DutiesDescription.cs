using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.xml
{
  public abstract class DutiesDescription
  {
    public abstract string GetDutyType();
    public abstract double? GetAmount();
  }
}
