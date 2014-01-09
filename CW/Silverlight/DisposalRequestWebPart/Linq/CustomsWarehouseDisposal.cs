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
    internal void DisposeMaterial(ref int packagesToDispose, List<CustomsWarehouse> listCopy)
    {
      if (this.CustomsStatus.Value == Linq.CustomsStatus.NotStarted)
      {
        double _Available = this.CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated.Value + this.CW_SettledNetMass.Value;
        int _2DisposePackages = Math.Min(this.CWL_CWDisposal2CustomsWarehouseID.Packages(_Available), packagesToDispose);
        packagesToDispose -= _2DisposePackages;
        if (CW_PackageToClear != _2DisposePackages)
        {
          this.CW_PackageToClear = _2DisposePackages;
          double _diff = this.CWL_CWDisposal2CustomsWarehouseID.Quantity(_2DisposePackages) - this.CW_SettledNetMass.Value;
          SettledNetMass(this.CW_SettledNetMass.Value + _diff);
          Debug.Assert(this.CW_AddedKg >= 0, "CW_AddedKg <= 0");
        }
      }
      listCopy.Remove(this.CWL_CWDisposal2CustomsWarehouseID);
    }
    internal static CustomsWarehouseDisposal Create(int disposalRequestLibId, int toDisposePackages, double toDisposeKg, double packageWeight, CustomsWarehouse cw, string customsProcedure)
    {
      CustomsWarehouseDisposal _newItem = new CustomsWarehouseDisposal()
      {
        Archival = false,
        CNIDId = cw.CNIDId,
        ClearingType = Linq.ClearingType.PartialWindingUp,
        CustomsStatus = Linq.CustomsStatus.NotStarted,
        CustomsProcedure = customsProcedure,
        CW_AddedKg = 0, //Assigned in SettledNetMass
        CW_DeclaredNetMass = 0,
        CW_SettledNetMass = 0, //Assigned in SettledNetMass
        CW_SettledGrossMass = 0, //Assigned in SettledNetMass
        CW_PackageToClear = toDisposePackages,
        TobaccoValue = 0, //Assigned in SettledNetMass
        DisposalRequestId = disposalRequestLibId,
        CWL_CWDisposal2CustomsWarehouseID = cw,
        SKUDescription = "N/A",
        Title = "ToDo",
        SADDate = Extensions.SPMinimum
      };
      _newItem.SettledNetMass(toDisposeKg);
      _newItem.UpdateTitle(DateTime.Today);
      return _newItem;
    }
    internal void DeleteDisposal()
    {
      this.CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated += this.CW_SettledNetMass.Value;
      this.SettledNetMass(0);
      Debug.Assert(this.CW_DeclaredNetMass.Value == 0, "I expect Value of this.CW_DeclaredNetMass == 0 while deleting.");
      this.CW_DeclaredNetMass = 0;
      this.CW_PackageToClear = 0;
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    internal void UpdateTitle(DateTime dateTime)
    {
      Title = String.Format("CW-{0:D4}-{1:D6}", dateTime.Year, "XXXXXX"); //TODO Id.Value);
    }
    /// <summary>
    /// Assigns value to: CW_SettledNetMass, TobaccoValue, CW_SettledGrossMass, CW_AddedKg.
    /// </summary>
    /// <param name="value">The value.</param>
    private void SettledNetMass(double value)
    {
      this.CW_SettledNetMass = value.RoundValue();
      double _Portion = value / CWL_CWDisposal2CustomsWarehouseID.CW_Quantity.Value;
      this.TobaccoValue = (_Portion * CWL_CWDisposal2CustomsWarehouseID.Value.Value).RoundValue();
      this.CW_SettledGrossMass = (CW_PackageToClear.Value * CWL_CWDisposal2CustomsWarehouseID.PackageWeight() + value).RoundValue();
      this.CW_AddedKg = (value - this.CW_DeclaredNetMass.Value).RoundValue();
    }

  }
}
