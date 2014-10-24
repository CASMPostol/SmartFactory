//<summary>
//  Title   : DisposalRequestDetails
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  /// <summary>
  /// DisposalRequest details view model
  /// </summary>
  public class DisposalRequestDetails : CAS.Common.ComponentModel.PropertyChangedBase
  {

    #region public UI properties
    /// <summary>
    /// Gets or sets the SKU.
    /// </summary>
    /// <value>
    /// The SKU.
    /// </value>
    public string SKU
    {
      get
      {
        return b_SKU;
      }
      set
      {
        RaiseHandler<string>(value, ref b_SKU, "SKU", this);
      }
    }
    /// <summary>
    /// Gets or sets the SKU description.
    /// </summary>
    /// <value>
    /// The SKU description.
    /// </value>
    public string SKUDescription
    {
      get
      {
        return b_SKUDescription;
      }
      set
      {
        RaiseHandler<string>(value, ref b_SKUDescription, "SKUDescription", this);
      }
    }
    /// <summary>
    /// Gets or sets the batch.
    /// </summary>
    /// <value>
    /// The batch.
    /// </value>
    public string Batch
    {
      get
      {
        return b_Batch;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Batch, "Batch", this);
      }
    }
    /// <summary>
    /// Gets or sets the total stock.
    /// </summary>
    /// <value>
    /// The total stock.
    /// </value>
    public double TotalStock
    {
      get
      {
        return b_TotalStock;
      }
      set
      {
        RaiseHandler<double>(value, ref b_TotalStock, "TotalStock", this);
      }
    }
    /// <summary>
    /// Gets or sets the mass per package.
    /// </summary>
    /// <value>
    /// The mass per package.
    /// </value>
    public double MassPerPackage
    {
      get
      {
        return b_MassPerPackage;
      }
      set
      {
        RaiseHandler<double>(value, ref b_MassPerPackage, "MassPerPackage", this);
      }
    }
    /// <summary>
    /// Gets or sets the declared net mass.
    /// </summary>
    /// <value>
    /// The declared net mass.
    /// </value>
    public double DeclaredNetMass
    {
      get
      {
        return b_DeclaredNetMass;
      }
      set
      {
        RaiseHandler<double>(value, ref b_DeclaredNetMass, "DeclaredNetMass", this);
      }
    }
    /// <summary>
    /// Gets or sets the added kg.
    /// </summary>
    /// <value>
    /// The added kg.
    /// </value>
    public double AddedKg
    {
      get
      {
        return b_AddedKg;
      }
      set
      {
        RaiseHandler<double>(value, ref b_AddedKg, "AddedKg", this);
      }
    }
    /// <summary>
    /// Gets or sets the quantity automatic clear sum.
    /// </summary>
    /// <value>
    /// The quantity automatic clear sum.
    /// </value>
    public double QuantityyToClearSum
    {
      get
      {
        return b_QuantityyToClearSum;
      }
      set
      {
        RaiseHandler<double>(value, ref b_QuantityyToClearSum, "QuantityyToClearSum", this);
      }
    }
    /// <summary>
    /// Gets or sets the quantity automatic clear sum rounded.
    /// </summary>
    /// <value>
    /// The quantity automatic clear sum rounded.
    /// </value>
    public double QuantityyToClearSumRounded
    {
      get
      {
        return b_QuantityyToClearSumRounded;
      }
      set
      {
        RaiseHandler<double>(value, ref b_QuantityyToClearSumRounded, "QuantityyToClearSumRounded", this);
      }
    }
    /// <summary>
    /// Gets or sets the remaining configuration stock.
    /// </summary>
    /// <value>
    /// The remaining configuration stock.
    /// </value>
    public double RemainingOnStock
    {
      get
      {
        return b_RemainingOnStock;
      }
      set
      {
        RaiseHandler<double>(value, ref b_RemainingOnStock, "RemainingOnStock", this);
      }
    }
    /// <summary>
    /// Gets or sets the units.
    /// </summary>
    /// <value>
    /// The units.
    /// </value>
    public string Units
    {
      get
      {
        return b_Units;
      }
      set
      {
        RaiseHandler<string>(value, ref b_Units, "Units", this);
      }
    }
    /// <summary>
    /// Gets or sets the packages automatic clear.
    /// </summary>
    /// <value>
    /// The packages automatic clear.
    /// </value>
    public int PackagesToDispose
    {
      get
      {
        return b_PackagesToDispose;
      }
      set
      {
        RaiseHandler<int>(value, ref b_PackagesToDispose, "PackagesToDispose", this);
      }
    }
    /// <summary>
    /// Gets or sets the customs procedure.
    /// </summary>
    /// <value>
    /// The customs procedure.
    /// </value>
    public string CustomsProcedure
    {
      get
      {
        return b_CustomsProcedure;
      }
      set
      {
        RaiseHandler<string>(value, ref b_CustomsProcedure, "CustomsProcedure", this);
      }
    }
    /// <summary>
    /// Gets or sets the customs document number.
    /// </summary>
    /// <value>
    /// The customs document number.
    /// </value>
    public string DocumentNumber
    {
      get
      {
        return b_DocumentNumber;
      }
      set
      {
        RaiseHandler<string>(value, ref b_DocumentNumber, "DocumentNumber", this);
      }
    }
    /// <summary>
    /// Gets or sets the sequence number.
    /// </summary>
    /// <value>
    /// The sequence number.
    /// </value>
    public int SequenceNumber
    {
      get
      {
        return b_SequenceNumber;
      }
      set
      {
        RaiseHandler<int>(value, ref b_SequenceNumber, "SequenceNumber", this);
      }
    }
    /// <summary>
    /// Gets or sets the Button Up handler.
    /// </summary>
    /// <value>
    /// The Button Up handler.
    /// </value>
    public ICommandWithUpdate MoveUp
    {
      get
      {
        return b_MoveUp;
      }
      set
      {
        RaiseHandler<ICommandWithUpdate>(value, ref b_MoveUp, "MoveUp", this);
      }
    }
    /// <summary>
    /// Gets or sets the Button Down.
    /// </summary>
    /// <value>
    /// The Button Down.
    /// </value>
    public ICommandWithUpdate MoveDown
    {
      get
      {
        return b_MoveDown;
      }
      set
      {
        RaiseHandler<ICommandWithUpdate>(value, ref b_MoveDown, "MoveDown", this);
      }
    }
    #endregion

    #region internal
    /// <summary>
    /// Initializes a new instance of the <see cref="DisposalRequestDetails" /> class. Used to add new position of the requests list.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="account">The account without disposal created for the relevant request.</param>
    /// <param name="sequenceNumber">The sequence number that will be incremented each time new instance is created.</param>
    internal DisposalRequestDetails(DisposalRequest parent, CustomsWarehouse account, ref int sequenceNumber)
    {
      m_Parent = parent;
      m_Disposal = null;
      m_Account = account;
      AddedKg = 0;
      Batch = account.Batch;
      CustomsProcedure = string.Empty;
      DeclaredNetMass = 0;
      DocumentNumber = account.DocumentNo;
      MassPerPackage = account.CW_MassPerPackage.Value;
      PackagesToDispose = 0;
      QuantityyToClearSum = 0;
      QuantityyToClearSumRounded = 0;
      RemainingOnStock = account.TobaccoNotAllocated.Value;
      SequenceNumber = sequenceNumber++;
      SKU = account.SKU;
      SKUDescription = string.Empty;
      TotalStock = account.TobaccoNotAllocated.Value;
      Units = account.Units;
      MoveDown = new SynchronousCommandBase<Nullable<int>>(x => parent.GoDown(SequenceNumber), y => !parent.IsBottomActive(SequenceNumber));
      MoveUp = new SynchronousCommandBase<Nullable<int>>(x => parent.GoUp(SequenceNumber), y => !parent.IsTopActive(SequenceNumber));
    }
    /// <summary>
    /// Creates the instance of <see cref="DisposalRequestDetails" /> to be used as a wrapper of <see cref="CustomsWarehouseDisposal" />.
    /// </summary>
    /// <param name="parent">The parent.</param>
    /// <param name="disposal">The disposal to be wrapped.</param>
    /// <param name="sequenceNumber">The sequence number that will be incremented each time new instance is created.</param>
    internal DisposalRequestDetails(DisposalRequest parent, CustomsWarehouseDisposal disposal, ref  int sequenceNumber)
      : this(parent, disposal.CWL_CWDisposal2CustomsWarehouseID, ref sequenceNumber)
    {
      AddedKg = disposal.CW_AddedKg.Value;
      CustomsProcedure = disposal.CustomsProcedure;
      DeclaredNetMass = disposal.CW_DeclaredNetMass.Value;
      SKUDescription = disposal.SKUDescription;
      m_Disposal = disposal;
      QuantityyToClearSum = AddedKg + DeclaredNetMass;
      PackagesToDispose = m_Account.Packages(QuantityyToClearSum);
      QuantityyToClearSumRounded = PackagesToDispose * MassPerPackage;
      TotalStock = disposal.CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated.Value + QuantityyToClearSumRounded;
    }
    /// <summary>
    /// Updates this instance for specified packages to dispose.
    /// </summary>
    /// <param name="packagesToDispose">The packages to dispose.</param>
    /// <param name="declared">The totally declared amount of goods.</param>
    internal void Update(ref int packagesToDispose, ref double declared)
    {
      this.PackagesToDispose = Math.Min(this.m_Account.Packages(TotalStock), packagesToDispose);
      packagesToDispose -= this.PackagesToDispose;
      Debug.Assert(packagesToDispose >= 0, "packagesToDispose < 0");
      this.QuantityyToClearSumRounded = this.m_Account.Quantity(this.PackagesToDispose);
      this.QuantityyToClearSum = this.QuantityyToClearSumRounded;
      this.DeclaredNetMass = Math.Min(declared, this.QuantityyToClearSumRounded);
      declared -= this.DeclaredNetMass;
      this.AddedKg = this.QuantityyToClearSumRounded - this.DeclaredNetMass;
      Debug.Assert(this.AddedKg >= 0, "CW_AddedKg < 0");
      this.RemainingOnStock = TotalStock - AddedKg - DeclaredNetMass;
      Debug.Assert(this.RemainingOnStock >= 0, "CW_AddedKg < 0");
    }
    internal void UpdateDisposal(int disposalRequestLibId, List<CustomsWarehouseDisposal> list2Delete, List<CustomsWarehouseDisposal> list2Insert)
    {
      if (QuantityyToClearSumRounded > 0)
        if (m_Disposal == null)
        {
          m_Disposal = CustomsWarehouseDisposal.Create(this, disposalRequestLibId, m_Account, CustomsProcedure);
          list2Insert.Add(m_Disposal);
        }
        else
          m_Disposal.Update(this);
      else if (m_Disposal != null)
      {
        m_Disposal.DeleteDisposal();
        list2Delete.Add(m_Disposal);
      }
    }
    internal double EndOfOGL()
    {
      double _ret = 0.0;
      if (this.m_Disposal != null)
        _ret = TotalStock - m_Disposal.CW_DeclaredNetMass.Value;
      return _ret;
    }
    #endregion

    #region backing fields
    private string b_SKU;
    private string b_SKUDescription;
    private string b_Batch;
    private double b_TotalStock;
    private double b_MassPerPackage;
    private double b_DeclaredNetMass;
    private double b_AddedKg;
    private double b_QuantityyToClearSum;
    private double b_QuantityyToClearSumRounded;
    private double b_RemainingOnStock;
    private string b_Units;
    private int b_PackagesToDispose;
    private string b_CustomsProcedure;
    private string b_DocumentNumber;
    private int b_SequenceNumber;
    private ICommandWithUpdate b_MoveDown;
    private ICommandWithUpdate b_MoveUp;
    #endregion

    #region private
    private CustomsWarehouseDisposal m_Disposal = null;
    private CustomsWarehouse m_Account = null;
    private DisposalRequest m_Parent = null;
    #endregion
  }
}
