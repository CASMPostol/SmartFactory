//<summary>
//  Title   : class DisposalRequest
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  /// <summary>
  /// DisposalRequest class 
  /// </summary>
  public class DisposalRequest : Element
  {

    /// <summary>
    /// Initializes a new instance of the <see cref="DisposalRequest"/> class.
    /// </summary>
    public DisposalRequest()
    {
      Disposals = new ObservableCollection<CustomsWarehouseDisposal>();
      Items = new ObservableCollection<DisposalRequestDetails>();
      AutoCalculation = false;
    }

    #region public properties
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
    public int PackagesToDispose
    {
      get
      {
        return this._packagesToClear;
      }
      set
      {
        if ((value != this._packagesToClear))
        {
          this.OnPropertyChanging("PackagesToDispose", this._packagesToClear);
          this._packagesToClear = value;
          this.OnPropertyChanged("PackagesToDispose");
        }
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
      get { return b_customsProcedure; }
      set
      {
        if ((value != this.b_customsProcedure))
        {
          this.OnPropertyChanging("CustomsProcedure", this._packagesToClear);
          this.b_customsProcedure = value;
          this.OnPropertyChanged("CustomsProcedure");
        }
      }
    }
    /// <summary>
    /// Gets or sets the items.
    /// </summary>
    /// <value>
    /// The items.
    /// </value>
    public ObservableCollection<DisposalRequestDetails> Items
    {
      get
      {
        return b_Items;
      }
      set
      {
        if ((value != this.b_Items))
        {
          this.OnPropertyChanging("Items", this._packagesToClear);
          this.b_Items = value;
          this.OnPropertyChanged("Items");
        }
      }
    }

    #endregion

    #region internal
    internal static DisposalRequest DefaultDisposalRequestnew(string skuDescription, CustomsWarehouse cw)
    {
      return new DisposalRequest()
      {
        AddedKg = 0,
        DeclaredNetMass = 0,
        CustomsProcedure = "N/A",
        Batch = cw.Batch,
        MassPerPackage = cw.CW_MassPerPackage.Value,
        PackagesToDispose = 0,
        QuantityyToClearSum = 0,
        QuantityyToClearSumRounded = 0,
        RemainingOnStock = 0,
        RemainingPackages = 0,
        SKUDescription = skuDescription,
        Title = "Title TBD",
        TotalStock = 0,
        Units = cw.Units,
        SKU = cw.SKU,
      };
    }
    internal ObservableCollection<CustomsWarehouseDisposal> Disposals
    {
      get { return b_Disposals; }
      set { b_Disposals = value; }
    }
    internal bool AutoCalculation { get; set; }
    /// <summary>
    /// Updates this instance using the collection <paramref name="groupOfDisposals"/> of selected  <see cref="CustomsWarehouseDisposal"/>.
    /// </summary>
    /// <param name="listOfAccounts">The list.</param>
    /// <param name="groupOfDisposals">The group.</param>
    internal void GetDataContext(List<CustomsWarehouse> listOfAccounts, IGrouping<string, CustomsWarehouseDisposal> groupOfDisposals)
    {
      listOfAccounts.Sort(new Comparison<CustomsWarehouse>(CustomsWarehouse.CompareCustomsWarehouse));
      CustomsProcedure = groupOfDisposals.First<CustomsWarehouseDisposal>().CustomsProcedure;
      m_ListOfCustomsWarehouse = listOfAccounts;
      RemainingOnStock = m_ListOfCustomsWarehouse.Sum(x => x.TobaccoNotAllocated.Value);
      ObservableCollection<DisposalRequestDetails> _newCollection = new ObservableCollection<DisposalRequestDetails>();
      int _sequenceNumber = 0;
      List<CustomsWarehouse> _copylistOfAccounts = new List<CustomsWarehouse>(listOfAccounts);
      foreach (CustomsWarehouseDisposal _cwdrdx in groupOfDisposals)
      {
        DisposalRequestDetails _newDisposalRequestDetails = DisposalRequestDetails.Create4Disposal(_cwdrdx, _sequenceNumber ++);
        _copylistOfAccounts.Remove(_cwdrdx.CWL_CWDisposal2CustomsWarehouseID);
        _newCollection.Add(_newDisposalRequestDetails);
        GetDataContext(_cwdrdx);
      }
      foreach (CustomsWarehouse _cwx in _copylistOfAccounts)
      {
        DisposalRequestDetails _newDisposalRequestDetails = DisposalRequestDetails.Create4Account(_cwx, _sequenceNumber ++);
        _newCollection.Add(_newDisposalRequestDetails);
      }
      UpdateOnInit();
      Items = _newCollection;
    }
    /// <summary>
    /// Gets the data context.
    /// </summary>
    /// <param name="list">The list.</param>
    /// <param name="toDispose">To dispose.</param>
    /// <param name="customsProcedure">The customs procedure.</param>
    internal void GetDataContext(List<CustomsWarehouse> list, double toDispose, string customsProcedure)
    {
      b_customsProcedure = customsProcedure;
      list.Sort(new Comparison<CustomsWarehouse>(CustomsWarehouse.CompareCustomsWarehouse));
      m_ListOfCustomsWarehouse = list;
      TotalStock = m_ListOfCustomsWarehouse.Sum(x => x.TobaccoNotAllocated.Value);
      AddedKg = toDispose;
      UpdateOnChange();
    }
    internal void RecalculateDisposals(int disposalRequestLibId, DataContextAsync context)
    {
      List<CustomsWarehouse> _CWListCopy = new List<CustomsWarehouse>(m_ListOfCustomsWarehouse);
      List<CustomsWarehouseDisposal> _2Delete = new List<CustomsWarehouseDisposal>();
      int _packagesToDispose = PackagesToDispose;
      foreach (CustomsWarehouseDisposal _cwItem in Disposals)
        if (_packagesToDispose > 0)
          _cwItem.DisposeMaterial(ref _packagesToDispose, _CWListCopy);
        else
        {
          _cwItem.DeleteDisposal();
          _2Delete.Add(_cwItem);
        };
      EntityList<CustomsWarehouseDisposal> _Entity = context.GetList<CustomsWarehouseDisposal>(CommonDefinition.CustomsWarehouseDisposalTitle);
      if (_packagesToDispose > 0)
      {
        int _cwx = 0;
        while (_packagesToDispose > 0)
        {
          if (_cwx >= _CWListCopy.Count)
            throw new ArgumentOutOfRangeException("toDispose", "Cannot dispose - tobacco not available.");
          CustomsWarehouseDisposal _newDisposal = _CWListCopy[_cwx++].CreateDisposal(disposalRequestLibId, ref _packagesToDispose, CustomsProcedure);
          if (_newDisposal == null)
            continue;
          Disposals.Add(_newDisposal);
          _Entity.InsertOnSubmit(_newDisposal);
        }
      }
      //else
      //  _Entity.DeleteAllOnSubmit( _2Delete );
    }
    internal void EndOfBatch()
    {
      this.AddedKg += this.RemainingOnStock;
    }
    internal void EndOfOgl()
    {
      this.AddedKg = 0;
      foreach (CustomsWarehouseDisposal _cwix in Disposals)
        this.AddedKg += _cwix.CW_AddedKg.Value + _cwix.CWL_CWDisposal2CustomsWarehouseID.TobaccoNotAllocated.Value;
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
    private int _packagesToClear;
    private ObservableCollection<CustomsWarehouseDisposal> b_Disposals;
    private string b_customsProcedure = String.Empty;
    private List<CustomsWarehouse> m_ListOfCustomsWarehouse = null;
    private ObservableCollection<DisposalRequestDetails> b_Items;
    #endregion

    protected override void OnPropertyChanged(string propertyName)
    {
      base.OnPropertyChanged(propertyName);
      if (!AutoCalculation)
        return;
      AddedKg = Math.Max(AddedKg, 0);
      AddedKg = Math.Min(AddedKg, TotalStock - DeclaredNetMass);
      UpdateOnChange();
    }
    private void GetDataContext(CustomsWarehouseDisposal rowData)
    {
      DeclaredNetMass += rowData.CW_DeclaredNetMass.Value;
      AddedKg += rowData.CW_AddedKg.Value;
      QuantityyToClearSum += rowData.CW_SettledNetMass.Value;
      Disposals.Add(rowData);
    }
    private void UpdateOnInit()
    {
      bool _ac = AutoCalculation;
      Recalculate();
      TotalStock = RemainingOnStock + QuantityyToClearSumRounded;
      AutoCalculation = _ac;
    }
    private void UpdateOnChange()
    {
      bool _ac = AutoCalculation;
      Recalculate();
      RemainingOnStock = TotalStock - QuantityyToClearSumRounded;
      AutoCalculation = _ac;
    }
    private void Recalculate()
    {
      AutoCalculation = false;
      QuantityyToClearSum = DeclaredNetMass + AddedKg;
      PackagesToDispose = CustomsWarehouse.Packages(QuantityyToClearSum, this.MassPerPackage);
      QuantityyToClearSumRounded = PackagesToDispose * this.MassPerPackage;
    }
    #endregion

  }
}
