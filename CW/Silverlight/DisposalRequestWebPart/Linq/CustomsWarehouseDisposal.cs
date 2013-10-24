///<summary>
//  Title   : public partial class CustomsWarehouseDisposal
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
///</summary>

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  public partial class CustomsWarehouseDisposal
  {
    internal void DisposeMaterial( ref int packagesToDispose, List<CustomsWarehouse> listCopy )
    {
      double _Available = this.CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated.Value + this.CW_SettledNetMass.Value;
      int _2DisposePackages = Math.Min( Packages( _Available ), packagesToDispose );
      this.CW_PackageToClear = _2DisposePackages;
      this.CW_SettledNetMass = Quantity( _2DisposePackages );
      this.CW_AddedKg = this.CW_SettledNetMass - this.CW_DeclaredNetMass;
      Debug.Assert( this.CW_AddedKg >= 0, "CW_AddedKg <= 0" );
      listCopy.Remove( this.CWL_CWDisposal2CustomsWarehouseID );
      packagesToDispose -= _2DisposePackages;
    }
    internal double Quantity( int packages )
    {
      return this.CWL_CWDisposal2CustomsWarehouseID.CW_MassPerPackage.Value * packages;
    }
    internal int Packages( double quantity )
    {
      return Packages( quantity, this.CWL_CWDisposal2CustomsWarehouseID.CW_MassPerPackage.Value );
    }
    internal static int Packages( double quantityyToClearSum, double massPerPackage )
    {
      return Convert.ToInt32( Math.Round( quantityyToClearSum / massPerPackage + 0.499999, 0 ) );
    }
    internal void DeleteDisposal()
    {
      this.CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated += this.CW_SettledNetMass;
    }
  }
}
