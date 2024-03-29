﻿//<summary>
//  Title   : public partial class CustomsWarehouse
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

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  public partial class CustomsWarehouse
  {

    internal double Quantity(int packages)
    {
      return this.CW_MassPerPackage.Value * packages;
    }
    internal double PackageWeight()
    {
      return this.CW_PackageUnits.Value == 0 ? 0 : (this.GrossMass.Value - this.CW_Quantity.Value) / this.CW_PackageUnits.Value;
    }
    internal int Packages(double quantity)
    {
      return Packages(quantity, this.CW_MassPerPackage.Value);
    }
    internal static int Packages(double quantity, double massPerPackage)
    {
      return Convert.ToInt32(Math.Round(quantity / massPerPackage + 0.499999, 0));
    }
    /// <summary>
    /// Compares two instances of the <see cref="CustomsWarehouse " /> type and returns an integer that indicates 
    /// whether the first instance is earlier than, the same as, or later than the second instance.
    /// </summary>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>
    /// A signed integer that indicates the relative values of x and y, as shown in the following table.
    /// Value Condition Less than 0 x is less than y.
    /// 0 x equals y.
    /// Greater than 0 x is greater than y.
    /// </returns>
    internal static int CompareCustomsWarehouse(CustomsWarehouse x, CustomsWarehouse y)
    {
      int _ret = DateTime.Compare(x.CustomsDebtDate.Value, y.CustomsDebtDate.Value);
      if (_ret == 0)
        _ret = String.Compare(x.DocumentNo, y.DocumentNo);
      return _ret;
    }
  }
}
