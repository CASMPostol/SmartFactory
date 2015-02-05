//<summary>
//  Title   : partial class SADZgloszenieTowarOplata
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

namespace CAS.SmartFactory.Customs.Messages.CELINA.SAD 
{
  /// <summary>
  /// partial class SADZgloszenieTowarOplata
  /// </summary>
  public partial class SADZgloszenieTowarOplata : DutiesDescription
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
      return this.Kwota.ConvertToDouble(this.KwotaSpecified);
    }
  }
}
