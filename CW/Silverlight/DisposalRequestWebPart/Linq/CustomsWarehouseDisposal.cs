﻿///<summary>
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
      if ( this.CustomsStatus.Value == Linq.CustomsStatus.NotStarted )
      {
        double _Available = this.CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated.Value + this.CW_SettledNetMass.Value;
        int _2DisposePackages = Math.Min( Packages( _Available ), packagesToDispose );
        packagesToDispose -= _2DisposePackages;
        if ( CW_PackageToClear != _2DisposePackages )
        {
          this.CW_PackageToClear = _2DisposePackages;
          double _diff = Quantity( _2DisposePackages ) - this.CW_SettledNetMass.Value;
          this.CW_SettledNetMass += _diff;
          this.CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated -= _diff;
          this.CW_AddedKg = this.CW_SettledNetMass - this.CW_DeclaredNetMass;
          Debug.Assert( this.CW_AddedKg >= 0, "CW_AddedKg <= 0" );
          CW_SettledGrossMass = _2DisposePackages * CWL_CWDisposal2CustomsWarehouseID.PackageWeight() + CW_SettledNetMass.Value;
        }
      }
      listCopy.Remove( this.CWL_CWDisposal2CustomsWarehouseID );
    }
    internal static CustomsWarehouseDisposal Create( int disposalRequestLibId, int toDisposePackages, double toDisposeKg, double packageWeight, CustomsWarehouse cw )
    {
      CustomsWarehouseDisposal _newItem = new CustomsWarehouseDisposal()
      {
        CNIDId = cw.CNIDId,
        CustomsStatus = Linq.CustomsStatus.NotStarted,
        Archival = false,
        AccountClosed = false,
        CW_AddedKg = toDisposeKg,
        CW_DeclaredNetMass = 0,
        CW_SettledNetMass = toDisposeKg,
        CW_SettledGrossMass = toDisposePackages * packageWeight + toDisposeKg,
        CW_PackageToClear = toDisposePackages,
        CWL_CWDisposal2CustomsWarehouseID = cw,
        Title = "TBD",
        SKUDescription = "N/A",
        DisposalRequestId = disposalRequestLibId,
      };
      _newItem.UpdateTitle( DateTime.Today );
      return _newItem;
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
      this.CW_SettledGrossMass = 0;
      this.CW_SettledNetMass = 0;
      this.CW_AddedKg = 0;
      Debug.Assert( this.CW_DeclaredNetMass.Value == 0, "I expect Value of this.CW_DeclaredNetMass == 0 while deleting." );
      this.CW_DeclaredNetMass = 0;
      this.CW_PackageToClear = 0;
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    internal void UpdateTitle( DateTime dateTime )
    {
      Title = String.Format( "CW-{0:D4}{1:D6}", dateTime.Year, "XXXXXX" ); //TODO Id.Value);
    }

  }
}