﻿using System;

namespace CAS.SmartFactory.xml.ECS.IE529
{
  public partial class IE529ZwolnienieTowarOplata : DutiesDescription
  {
    public override string GetDutyType()
    {
      return this.Typ;
    }
    public override double? GetAmount()
    {
      return Convert.ToDouble(Kwota);
    }
  }
}
