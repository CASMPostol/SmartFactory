//<summary>
//  Title   : partial class IE529ZwolnienieTowarOplata
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>
      
using System;

namespace CAS.SmartFactory.Customs.Messages.ECS
{
  /// <summary>
  /// partial class IE529ZwolnienieTowarOplata
  /// </summary>
  public partial class IE529ZwolnienieTowarOplata : DutiesDescription
  {
    /// <summary>
    /// Gets the type of the duty.
    /// </summary>
    /// <returns></returns>
    public override string GetDutyType()
    {
      return this.Typ;
    }
    /// <summary>
    /// Gets the amount.
    /// </summary>
    /// <returns></returns>
    public override double? GetAmount()
    {
      return Convert.ToDouble(Kwota);
    }
  }
}
