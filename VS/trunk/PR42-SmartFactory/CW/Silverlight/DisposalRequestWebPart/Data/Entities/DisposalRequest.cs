//<summary>
//  Title   : class DisposalRequest
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using Microsoft.SharePoint.Client;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data.Entities
{
  /// <summary>
  /// DisposalRequest class 
  /// </summary>
  public class DisposalRequest : BindingEditableObject
  {

    public DisposalRequest()
    {
      Disposals = new ObservableCollection<CustomsWarehouseDisposalRowData>();
    }

    //public DisposalRequest(System.Linq.IGrouping<string, CustomsWarehouseDisposalRowData> _grx)
    //{
    //  // TODO: Complete member initialization
    //  this._grx = _grx;
    //}
    #region public
    /// <summary>
    /// Gets or sets the SKU.
    /// </summary>
    /// <value>
    /// The SKU.
    /// </value>
    public String SKU
    {
      get
      {
        return this._sKU;
      }
      set
      {
        if ((value != this._sKU))
        {
          this.OnPropertyChanging("SKU", this._sKU);
          this._sKU = value;
          this.OnPropertyChanged("SKU");
        }
      }
    }
    /// <summary>
    /// Gets or sets the sku description.
    /// </summary>
    /// <value>
    /// The sku description.
    /// </value>
    public String SKUDescription
    {
      get
      {
        return this._sKUDescription;
      }
      set
      {
        if ((value != this._sKUDescription))
        {
          this.OnPropertyChanging("SKUDescription", this._sKUDescription);
          this._sKUDescription = value;
          this.OnPropertyChanged("SKUDescription");
        }
      }
    }
    /// <summary>
    /// Gets or sets the batch.
    /// </summary>
    /// <value>
    /// The batch.
    /// </value>
    public String Batch
    {
      get
      {
        return this._Batch;
      }
      set
      {
        if ((value != this._Batch))
        {
          this.OnPropertyChanging("Batch", this._Batch);
          this._Batch = value;
          this.OnPropertyChanged("Batch");
        }
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
        return this._totalStock;
      }
      set
      {
        if ((value != this._totalStock))
        {
          this.OnPropertyChanging("TotalStock", this._totalStock);
          this._totalStock = value;
          this.OnPropertyChanged("TotalStock");
        }
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
        return this._massPerPackage;
      }
      set
      {
        if ((value != this._massPerPackage))
        {
          this.OnPropertyChanging("MassPerPackage", this._massPerPackage);
          this._massPerPackage = value;
          this.OnPropertyChanged("MassPerPackage");
        }
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
        return this._declaredNetMass;
      }
      set
      {
        if ((value != this._declaredNetMass))
        {
          this.OnPropertyChanging("CW_SettledNetMass", this._declaredNetMass);
          this._declaredNetMass = value;
          this.OnPropertyChanged("CW_SettledNetMass");
        }
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
        return this._addedKg;
      }
      set
      {
        if ((value != this._addedKg))
        {
          this.OnPropertyChanging("AddedKg", this._addedKg);
          this._addedKg = value;
          this.OnPropertyChanged("AddedKg");
        }
      }
    }
    /// <summary>
    /// Gets or sets the quantityy automatic clear sum.
    /// </summary>
    /// <value>
    /// The quantityy automatic clear sum.
    /// </value>
    public double QuantityyToClearSum
    {
      get
      {
        return this._quantityyToClearSum;
      }
      set
      {
        if ((value != this._quantityyToClearSum))
        {
          this.OnPropertyChanging("QuantityyToClearSum", this._quantityyToClearSum);
          this._quantityyToClearSum = value;
          this.OnPropertyChanged("QuantityyToClearSum");
        }
      }
    }
    /// <summary>
    /// Gets or sets the quantityy automatic clear sum rounded.
    /// </summary>
    /// <value>
    /// The quantityy automatic clear sum rounded.
    /// </value>
    public double QuantityyToClearSumRounded
    {
      get
      {
        return this._quantityyToClearSumRounded;
      }
      set
      {
        if ((value != this._quantityyToClearSumRounded))
        {
          this.OnPropertyChanging("QuantityyToClearSumRounded", this._quantityyToClearSumRounded);
          this._quantityyToClearSumRounded = value;
          this.OnPropertyChanged("QuantityyToClearSumRounded");
        }
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
        return this._remainingOnStock;
      }
      set
      {
        if ((value != this._remainingOnStock))
        {
          this.OnPropertyChanging("RemainingOnStock", this._remainingOnStock);
          this._remainingOnStock = value;
          this.OnPropertyChanged("RemainingOnStock");
        }
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
        return this._units;
      }
      set
      {
        if ((value != this._units))
        {
          this.OnPropertyChanging("Units", this._units);
          this._units = value;
          this.OnPropertyChanged("Units");
        }
      }
    }
    /// <summary>
    /// Gets or sets the remaining packages.
    /// </summary>
    /// <value>
    /// The remaining packages.
    /// </value>
    public double RemainingPackages
    {
      get
      {
        return this._remainingPackages;
      }
      set
      {
        if ((value != this._remainingPackages))
        {
          this.OnPropertyChanging("RemainingPackages", this._remainingPackages);
          this._remainingPackages = value;
          this.OnPropertyChanged("RemainingPackages");
        }
      }
    }
    /// <summary>
    /// Gets or sets the packages automatic clear.
    /// </summary>
    /// <value>
    /// The packages automatic clear.
    /// </value>
    public double PackagesToClear
    {
      get
      {
        return this._packagesToClear;
      }
      set
      {
        if ((value != this._packagesToClear))
        {
          this.OnPropertyChanging("PackagesToClear", this._packagesToClear);
          this._packagesToClear = value;
          this.OnPropertyChanged("PackagesToClear");
        }
      }
    }
    #endregion

    #region internal
    internal ObservableCollection<CustomsWarehouseDisposalRowData> Disposals
    {
      get { return b_Disposals; }
      set { b_Disposals = value; }
    }
    internal void GetDataContext(CustomsWarehouseDisposalRowData rowData)
    {
      DeclaredNetMass += rowData.CW_DeclaredNetMass.Value;
      AddedKg += rowData.CW_AddedKg.Value;
      QuantityyToClearSum += rowData.CW_SettledNetMass.Value;
      QuantityyToClearSumRounded = QuantityyToClearSum;
      PackagesToClear = Math.Round(QuantityyToClearSumRounded / MassPerPackage, 0);
      Update();
      Disposals.Add(rowData);
    }
    #endregion

    #region private

    #region backing fields
    private string _sKU;
    private string _sKUDescription;
    private string _Batch;
    private double _totalStock;
    private double _massPerPackage;
    private double _declaredNetMass;
    private double _addedKg;
    private double _quantityyToClearSum;
    private double _quantityyToClearSumRounded;
    private double _remainingOnStock;
    private string _units;
    private double _remainingPackages;
    private double _packagesToClear;
    private ObservableCollection<CustomsWarehouseDisposalRowData> b_Disposals;
    #endregion

    private void Update()
    {

    }

    #endregion

  }
}
