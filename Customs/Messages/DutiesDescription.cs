//<summary>
//  Title   : public abstract class DutiesDescription
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

namespace CAS.SmartFactory.Customs.Messages
{
  /// <summary>
  /// public abstract class DutiesDescription
  /// </summary>
  public abstract class DutiesDescription
  {
    /// <summary>
    /// Gets the type of the duty.
    /// </summary>
    /// <returns></returns>
    public abstract string GetDutyType();
    /// <summary>
    /// Gets the amount.
    /// </summary>
    /// <returns></returns>
    public abstract double? GetAmount();
  }
}
