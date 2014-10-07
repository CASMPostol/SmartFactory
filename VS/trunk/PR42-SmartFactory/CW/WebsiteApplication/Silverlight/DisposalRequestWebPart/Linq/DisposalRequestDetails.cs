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
    /// Gets or sets the remaining packages.
    /// </summary>
    /// <value>
    /// The remaining packages.
    /// </value>
    public string RemainingPackages
    {
      get
      {
        return b_RemainingPackages;
      }
      set
      {
        RaiseHandler<string>(value, ref b_RemainingPackages, "RemainingPackages", this);
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
    private string b_RemainingPackages;
    private int b_PackagesToDispose;
    private string b_CustomsProcedure;
    private string b_DocumentNumber;
    private int b_SequenceNumber;
    #endregion

  }
}
