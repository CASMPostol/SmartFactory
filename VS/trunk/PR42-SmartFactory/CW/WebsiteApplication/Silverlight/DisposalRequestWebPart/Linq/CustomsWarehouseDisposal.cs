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
using System.Diagnostics;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  /// <summary>
  /// Entity representing an item on the list CustomsWarehouseDisposal
  /// </summary>
  public partial class CustomsWarehouseDisposal
  {

    #region internal
    /// <summary>
    /// Updates the disposal using user entered data.
    /// </summary>
    /// <param name="disposalRequestDetails">The disposal request details <see cref="DisposalRequestDetails"/>.</param>
    internal void Update(DisposalRequestDetails disposalRequestDetails)
    {
      if (this.CustomsStatus.Value != Linq.CustomsStatus.NotStarted)
        return;
      Recalculate(disposalRequestDetails);
    }
    internal static CustomsWarehouseDisposal Create(DisposalRequestDetails disposalRequestDetails, int disposalRequestLibId, CustomsWarehouse account, string customsProcedure)
    {
      Debug.Assert(account != null, "Disposal have to has account assigned.");
      CustomsWarehouseDisposal _newItem = new CustomsWarehouseDisposal()
      {
        Archival = false,
        CNIDId = account.CNIDId,
        ClearingType = Linq.ClearingType.PartialWindingUp,
        CustomsStatus = Linq.CustomsStatus.NotStarted,
        CustomsProcedure = customsProcedure,
        CW_AddedKg = 0, //Assigned in Recalculate
        CW_DeclaredNetMass = 0,
        CW_SettledNetMass = 0, //Assigned in Recalculate
        CW_SettledGrossMass = 0, //Assigned in Recalculate
        CW_PackageToClear = 0, //Assigned in Recalculate
        TobaccoValue = 0, //Assigned in Recalculate
        DisposalRequestId = disposalRequestLibId,
        CWL_CWDisposal2CustomsWarehouseID = account,
        SKUDescription = "N/A",
        Title = "ToDo",
        SADDate = Extensions.SPMinimum,
      };
      _newItem.Recalculate(disposalRequestDetails);
      _newItem.Title = String.Format("CW-{0:D4}-{1}", DateTime.Today.Year, "XXXXXX"); //TODO Id.Value);
      return _newItem;
    }
    internal void DeleteDisposal()
    {
      CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated += this.CW_SettledNetMass.Value;
      CW_PackageToClear = 0;
      CW_SettledNetMass = 0;
      CW_SettledGrossMass = 0;
      CW_AddedKg = 0;
      CW_DeclaredNetMass = 0;
      TobaccoValue = 0;
    }
    #endregion

    #region private
    private void Recalculate(DisposalRequestDetails disposalRequestDetails)
    {
      CW_PackageToClear = disposalRequestDetails.PackagesToDispose;
      double _diff = disposalRequestDetails.QuantityyToClearSumRounded - CW_SettledGrossMass.Value;
      CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated -= _diff;
      CW_SettledNetMass = disposalRequestDetails.QuantityyToClearSumRounded;
      CW_SettledGrossMass = (CW_PackageToClear.Value * CWL_CWDisposal2CustomsWarehouseID.PackageWeight() + CW_SettledNetMass.Value).RoundValue();
      CW_AddedKg = disposalRequestDetails.AddedKg;
      double _Portion = CW_SettledNetMass.Value / CWL_CWDisposal2CustomsWarehouseID.CW_Quantity.Value;
      TobaccoValue = (_Portion * CWL_CWDisposal2CustomsWarehouseID.Value.Value).RoundValue();
    }
    #endregion

  }
}