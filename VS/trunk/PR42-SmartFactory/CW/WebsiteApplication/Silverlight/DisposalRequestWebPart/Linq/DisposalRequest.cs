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

using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using CAS.Common.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq
{
  /// <summary>
  /// DisposalRequest class 
  /// </summary>
  public class DisposalRequest : Element
  {

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
          this.OnPropertyChanging("DeclaredNetMass", this._declaredNetMass);
          this._declaredNetMass = value;
          this.OnPropertyChanged("DeclaredNetMass");
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
          this.OnPropertyChanging("CustomsProcedure", this.b_customsProcedure);
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
    /// <summary>
    /// Gets or sets a value indicating whether this item is read only.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this item is read only; otherwise, <c>false</c>.
    /// </value>
    public bool ReadOnly
    {
      get
      {
        return b_ReadOnly;
      }
      set
      {
        if ((value != this.b_ReadOnly))
        {
          this.OnPropertyChanging("CustomsProcedure", this.b_ReadOnly);
          this.b_ReadOnly = value;
          this.OnPropertyChanged("CustomsProcedure");
        }
      }
    } 
    #endregion

    #region internal
    internal bool AutoCalculation { get; set; }
    /// <summary>
    /// Creates an instance of <see cref="DisposalRequest"/> using the collection of <see cref="CustomsWarehouse"/> and existing <see cref="CustomsWarehouseDisposal"/> entries.
    /// </summary>
    /// <param name="listOfAccounts">The list of <see cref="CustomsWarehouse"/> with the same batch.</param>
    /// <param name="groupOfDisposals">The group of existing <see cref="CustomsWarehouseDisposal"/> for the selected Batch.</param>
    internal static DisposalRequest Create(List<CustomsWarehouse> listOfAccounts, IGrouping<string, CustomsWarehouseDisposal> groupOfDisposals)
    {
      CustomsWarehouseDisposal _firstDisposal = groupOfDisposals.First<CustomsWarehouseDisposal>();
      CustomsWarehouse _firstAccount = _firstDisposal.CWL_CWDisposal2CustomsWarehouseID;
      DisposalRequest _newRequest = CreateDisposalRequest(_firstAccount, _firstDisposal.SKUDescription, _firstDisposal.CustomsProcedure);
      listOfAccounts.Sort(new Comparison<CustomsWarehouse>(CustomsWarehouse.CompareCustomsWarehouse));
      List<DisposalRequestDetails> _newCollection = new List<DisposalRequestDetails>();
      int _sequenceNumber = 0;
      List<CustomsWarehouse> _copylistOfAccounts = new List<CustomsWarehouse>(listOfAccounts);
      foreach (CustomsWarehouseDisposal _cwdx in groupOfDisposals)
      {
        DisposalRequestDetails _newDisposalRequestDetails = new DisposalRequestDetails(_newRequest, _cwdx, ref _sequenceNumber);
        _copylistOfAccounts.Remove(_cwdx.CWL_CWDisposal2CustomsWarehouseID);
        _newCollection.Add(_newDisposalRequestDetails);
        _newRequest.GetDataContext(_newDisposalRequestDetails);
      }
      foreach (CustomsWarehouse _cwx in _copylistOfAccounts)
      {
        DisposalRequestDetails _newDisposalRequestDetails = new DisposalRequestDetails(_newRequest, _cwx, ref _sequenceNumber);
        _newCollection.Add(_newDisposalRequestDetails);
      }
      _newRequest.RemainingOnStock = _newCollection.Sum(x => x.RemainingOnStock);
      _newRequest.UpdateOnInit(_newCollection);
      return _newRequest;
    }
    /// <summary>
    /// Creates an instance of <see cref="DisposalRequest"/> using the collection of <see cref="CustomsWarehouse"/> when there are no disposals.
    /// </summary>
    /// <param name="listOfAccounts">The list of <see cref="CustomsWarehouse"/> with the same batch.</param>
    /// <param name="toDispose">To dispose.</param>
    /// <param name="customsProcedure">The customs procedure.</param>
    internal static DisposalRequest Create(List<CustomsWarehouse> listOfAccounts, double toDispose, string customsProcedure)
    {
      CustomsWarehouse _fcw = listOfAccounts.First<CustomsWarehouse>();
      DisposalRequest _ret = DisposalRequest.CreateDisposalRequest(_fcw, "N/A", customsProcedure);
      listOfAccounts.Sort(new Comparison<CustomsWarehouse>(CustomsWarehouse.CompareCustomsWarehouse));
      ObservableCollection<DisposalRequestDetails> _newCollection = new ObservableCollection<DisposalRequestDetails>();
      int _sequenceNumber = 0;
      foreach (CustomsWarehouse _cwx in listOfAccounts)
      {
        DisposalRequestDetails _newDisposalRequestDetails = new DisposalRequestDetails(_ret, _cwx, ref _sequenceNumber);
        _newCollection.Add(_newDisposalRequestDetails);
      }
      _ret.TotalStock = listOfAccounts.Sum(x => x.TobaccoNotAllocated.Value);
      _ret.AddedKg = toDispose;
      _ret.UpdateOnChange();
      return _ret;
    }
    /// <summary>
    /// Recalculates the disposals before committing changes to the website.
    /// </summary>
    /// <param name="disposalRequestLibId">The disposal request library identifier.</param>
    /// <param name="context">The context.</param>
    /// <exception cref="System.ArgumentOutOfRangeException">toDispose;Cannot dispose - tobacco not available.</exception>
    internal void RecalculateDisposals(int disposalRequestLibId, DataContextAsync context)
    {
      List<CustomsWarehouseDisposal> _2Delete = new List<CustomsWarehouseDisposal>();
      List<CustomsWarehouseDisposal> _2Insert = new List<CustomsWarehouseDisposal>();
      foreach (DisposalRequestDetails _dslRqstDtlItem in Items)
        _dslRqstDtlItem.UpdateDisposal(disposalRequestLibId, _2Delete, _2Insert);
      EntityList<CustomsWarehouseDisposal> _Entity = context.GetList<CustomsWarehouseDisposal>(CommonDefinition.CustomsWarehouseDisposalTitle);
      _Entity.InsertAllOnSubmit(_2Insert);
      // _Entity.DeleteAllOnSubmit(_2Delete); TODO to be implemented
    }
    internal void EndOfBatch()
    {
      this.AddedKg += this.RemainingOnStock;
    }
    internal void EndOfOgl()
    {
      double _AddedKg = 0;
      foreach (DisposalRequestDetails _cwix in Items)
        _AddedKg += _cwix.EndOfOGL();
      this.AddedKg = _AddedKg;
    }
    internal void GoDown(int sequenceNumber)
    {
      Dictionary<int, DisposalRequestDetails> _dctnry = Items.ToDictionary(x => x.SequenceNumber);
      DisposalRequestDetails _current = _dctnry[sequenceNumber];
      DisposalRequestDetails _next = _dctnry[sequenceNumber + 1];
      _dctnry.Remove(sequenceNumber);
      _dctnry.Remove(sequenceNumber + 1);
      _current.SequenceNumber += 1;
      _next.SequenceNumber -= 1;
      _dctnry.Add(_current.SequenceNumber, _current);
      _dctnry.Add(_next.SequenceNumber, _next);
      RecreateDisposals(_dctnry.Values);
    }

    internal void GoUp(int sequenceNumber)
    {
      Dictionary<int, DisposalRequestDetails> _dctnry = Items.ToDictionary(x => x.SequenceNumber);
      DisposalRequestDetails _prvs = _dctnry[sequenceNumber - 1];
      DisposalRequestDetails _current = _dctnry[sequenceNumber];
      _dctnry.Remove(sequenceNumber);
      _dctnry.Remove(sequenceNumber - 1);
      _current.SequenceNumber -= 1;
      _prvs.SequenceNumber += 1;
      _dctnry.Add(_current.SequenceNumber, _current);
      _dctnry.Add(_prvs.SequenceNumber, _prvs);
      RecreateDisposals(_dctnry.Values);
    }
    internal bool IsBottom(int sequenceNumber)
    {
      return Items.Count == 0 || sequenceNumber == Items.Count - 1;
    }
    internal bool IsTop(int sequenceNumber)
    {
      return sequenceNumber == 0; ;
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
    private int _packagesToClear;
    private string b_customsProcedure = String.Empty;
    private ObservableCollection<DisposalRequestDetails> b_Items;
    private bool b_ReadOnly;
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
    private void GetDataContext(DisposalRequestDetails rowData)
    {
      DeclaredNetMass += rowData.DeclaredNetMass;
      AddedKg += rowData.AddedKg;
    }
    private void UpdateOnInit(IEnumerable<DisposalRequestDetails> items)
    {
      bool _ac = AutoCalculation;
      Items = new ObservableCollection<DisposalRequestDetails>(items); 
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
      RecalculateDisposals(Items);
    }
    /// <summary>
    /// Recalculates the disposals after changing the sequence of accounts to be used for disposing the request item.
    /// </summary>
    /// <param name="items">The list of <see cref="DisposalRequestDetails"/> after changing the order of this lists items.</param>
    private void RecreateDisposals(IEnumerable<DisposalRequestDetails> items)
    {
      ObservableCollection<DisposalRequestDetails> _newItems = new ObservableCollection<DisposalRequestDetails>(items);
      RecalculateDisposals(_newItems);
      Items = _newItems;
    }
    private void RecalculateDisposals(IEnumerable<DisposalRequestDetails> items)
    {
      int _packages = this.PackagesToDispose;
      double _declared = this.DeclaredNetMass;
      foreach (DisposalRequestDetails _drx in items)
        _drx.Update(ref _packages, ref _declared);
      Debug.Assert(_packages == 0, String.Format("Cannot dispose {0} packages - tobacco not available.", _packages));
    }
    private static DisposalRequest CreateDisposalRequest(CustomsWarehouse account, string skuDescription, string customsProcedure)
    {
      return new DisposalRequest()
      {
        AddedKg = 0,
        DeclaredNetMass = 0,
        CustomsProcedure = customsProcedure,
        Batch = account.Batch,
        MassPerPackage = account.CW_MassPerPackage.Value,
        PackagesToDispose = 0,
        QuantityyToClearSum = 0,
        QuantityyToClearSumRounded = 0,
        RemainingOnStock = 0,
        SKUDescription = skuDescription,
        Title = "Title TBD",
        TotalStock = 0,
        Units = account.Units,
        SKU = account.SKU,
      };
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="DisposalRequest"/> class.
    /// </summary>
    private DisposalRequest()
    {
      AutoCalculation = false;
      ReadOnly = true;
      Items = new ObservableCollection<DisposalRequestDetails>();
    }
    #endregion

  }
}
