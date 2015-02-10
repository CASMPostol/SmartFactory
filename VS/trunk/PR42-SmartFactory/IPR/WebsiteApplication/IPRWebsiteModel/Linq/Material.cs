//<summary>
//  Title   : Name of Application
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

using CAS.SmartFactory.Customs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{

  /// <summary>
  /// Material Entity 
  /// </summary>
  public sealed partial class Material : IComparable<Material>
  {
    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="Material"/> class.
    /// </summary>
    /// <param name="edc">The <see cref="Entities"/> instance.</param>
    /// <param name="product">The product.</param>
    /// <param name="batch">The batch.</param>
    /// <param name="sku">The sku.</param>
    /// <param name="storLoc">The store location.</param>
    /// <param name="skuDescription">The sku description.</param>
    /// <param name="units">The units.</param>
    /// <param name="fgQuantity">The finished good quantity.</param>
    /// <param name="tobaccoQuantity">The tobacco quantity.</param>
    /// <param name="productID">The product ID.</param>
    public Material
      (Entities edc, Entities.ProductDescription product, string batch, string sku, string storLoc, string skuDescription, string units, decimal fgQuantity, decimal
        tobaccoQuantity, string productID)
      : this()
    {
      Archival = false; 
      Batch = batch;
      Material2BatchIndex = null;
      SKU = sku;
      StorLoc = storLoc;
      SKUDescription = skuDescription;
      Title = skuDescription;
      Units = units;
      FGQuantity = Convert.ToDouble(fgQuantity);
      TobaccoQuantity = Convert.ToDouble(tobaccoQuantity).Round2Decimals();
      ProductID = productID;
      ProductType = product.productType;
      Overuse = 0;
      Dust = 0;
      SHMenthol = 0;
      Waste = 0;
      Tobacco = 0;
      Disposed = 0;
    }
    #endregion

    #region public
    /// <summary>
    /// Ratios 
    /// </summary>
    internal struct Ratios
    {
      /// <summary>
      /// The dust ratio
      /// </summary>
      public double dustRatio;
      /// <summary>
      /// The sh menthol ratio
      /// </summary>
      public double shMentholRatio;
      /// <summary>
      /// The waste ratio
      /// </summary>
      public double wasteRatio;
    }
    /// <summary>
    /// Disposals the analysis.
    /// </summary>
    /// <param name="entities">The <see cref="Entities"/>.</param>
    /// <param name="ratios">The ratios.</param>
    /// <param name="overusageRatios">The over-usage.</param>
    internal void CalculateOveruse(Entities entities, Ratios ratios, double overusageRatios)
    {
      if (overusageRatios > 0)
        this[DisposalEnum.OverusageInKg] = (TobaccoQuantityDec * Convert.ToDecimal(overusageRatios)).Round2Decimals();
      else
        this[DisposalEnum.OverusageInKg] = 0;
    }
    internal void CalculateCompensationComponents(Ratios ratios)
    {
      decimal material = TobaccoQuantityDec - this[DisposalEnum.OverusageInKg];
      decimal dust = this[DisposalEnum.Dust] = (material * Convert.ToDecimal(ratios.dustRatio)).Round2Decimals();
      decimal shMenthol = this[DisposalEnum.SHMenthol] = (material * Convert.ToDecimal(ratios.shMentholRatio)).Round2Decimals();
      decimal waste = this[DisposalEnum.Waste] = (material * Convert.ToDecimal(ratios.wasteRatio)).Round2Decimals();
      this[DisposalEnum.TobaccoInCigaretess] = material - shMenthol - waste - dust;
    }
    internal decimal RemoveOveruseIfPossible(List<Material> _2Add, Ratios ratios)
    {
      decimal _ret = 0;
      if (this[DisposalEnum.OverusageInKg] > Settings.MinimalOveruse)
        _2Add.Add(this);
      else
      {
        _ret = this[DisposalEnum.OverusageInKg];
        this[DisposalEnum.OverusageInKg] = 0;
      }
      return _ret;
    }
    internal void IncreaseOveruse(decimal _AddingCff, Ratios ratios)
    {
      decimal overuseInKg = (TobaccoQuantityDec * _AddingCff).Round2Decimals();
      this[DisposalEnum.OverusageInKg] += overuseInKg;
      decimal material = TobaccoQuantityDec - this[DisposalEnum.OverusageInKg];
      if (material < 0)
        throw new CAS.SmartFactory.IPR.WebsiteModel.IPRDataConsistencyException("Material.IncreaseOveruse", "There is not enough tobacco to correct overuse", null, "Operation has been aborted");
    }
    internal void AdjustTobaccoQuantity(ref decimal totalQuantity, ProgressChangedEventHandler progressChanged)
    {
      decimal _available = Accounts2Dispose.Sum(y => y.TobaccoNotAllocatedDec) + Disposed;
      if (Math.Abs(_available - TobaccoQuantityDec) > Settings.MinimalOveruse)
        return;
      totalQuantity += _available - TobaccoQuantityDec;
      decimal _startingTobaccoQuantity = this.TobaccoQuantityDec;
      this.TobaccoQuantityDec = _available;
      Warnning _warnning = null;
      if (Accounts2Dispose.Count > 0)
      {
        string _tmpl = "Adjusted TobaccoQuantity of material: batch {0} IPR: {1}";
        _warnning = new Warnning(String.Format(_tmpl, this.Batch, String.Concat(Accounts2Dispose.Select<IPR, string>(x => x.DocumentNo).ToArray<string>())), false);
      }
      else
      {
        string _tmpl = "Cleared TobaccoQuantity of {0} for the material: batch {1} because no account to adjust found.";
        _warnning = new Warnning(String.Format(_tmpl, _startingTobaccoQuantity, this.Batch), false);
      }
      progressChanged(this, new ProgressChangedEventArgs(1, _warnning));
    }
    /// <summary>
    /// Gets the <see cref="System.Decimal" /> at the specified index.
    /// </summary>
    /// <value>
    /// The <see cref="System.Decimal" />.
    /// </value>
    /// <param name="index">The index.</param>
    /// <returns></returns>
    public decimal this[DisposalEnum index]
    {
      get
      {
        double _ret = default(double);
        switch (index)
        {
          case DisposalEnum.Dust:
            _ret = this.Dust.Value;
            break;
          case DisposalEnum.SHMenthol:
            _ret = this.SHMenthol.Value;
            break;
          case DisposalEnum.Waste:
            _ret = this.Waste.Value;
            break;
          case DisposalEnum.OverusageInKg:
            _ret = this.Overuse.Value;
            break;
          case DisposalEnum.TobaccoInCigaretess:
            _ret = this.Tobacco.Value;
            break;
          default:
            throw new ArgumentException("Index out of range at Material.Indexer", "index");
        }
        return Convert.ToDecimal(_ret);
      }
      set
      {
        double _val = Convert.ToDouble(value);
        switch (index)
        {
          case DisposalEnum.Dust:
            this.Dust = _val;
            break;
          case DisposalEnum.SHMenthol:
            this.SHMenthol = _val;
            break;
          case DisposalEnum.Waste:
            this.Waste = _val;
            break;
          case DisposalEnum.OverusageInKg:
            this.Overuse = _val;
            break;
          case DisposalEnum.TobaccoInCigaretess:
            this.Tobacco = _val;
            break;
          default:
            throw new ArgumentException("Index out of range at Material.Indexer", "index");
        }
      }
    }
    /// <summary>
    /// Get portion of calculated tobacco quantity.
    /// </summary>
    /// <param name="invoice">The invoice.</param>
    /// <returns>
    /// Returns portion of calculated tobacco quantity. i.e. after removing compensation wast, dust, SHMethol, etc.
    /// </returns>
    public decimal CalculatedQuantity(InvoiceContent invoice)
    {
      return (this.Tobacco.Value * invoice.Quantity.Value / this.Material2BatchIndex.FGQuantity.Value).Round2Double();
    }
    /// <summary>
    /// Gets the list of disposals.
    /// </summary>
    /// <returns></returns>
    private IEnumerable<Disposal> GetListOfDisposals(Entities edc)
    {
      Linq.DisposalStatus status = this.Material2BatchIndex.ProductType.Value == Linq.ProductType.Cigarette ? DisposalStatus.TobaccoInCigaretes : DisposalStatus.TobaccoInCutfiller;
      return from _didx in Disposal(edc, false)
             let _ipr = _didx.Disposal2IPRIndex
             where _didx.CustomsStatus.Value == CustomsStatus.NotStarted && _didx.DisposalStatus.Value == status
             orderby _ipr.Id ascending
             select _didx;
    }
    /// <summary>
    /// Exports the specified entities.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="closingBatch">if set to <c>true</c> [closing batch].</param>
    /// <param name="invoiceContent">Content of the invoice.</param>
    /// <param name="disposals">The disposals.</param>
    /// <param name="sadConsignmentNumber">The sad consignment number.</param>
    /// <exception cref="CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException">internal error: it is impossible to mark as exported the material;Material export;false</exception>
    /// <exception cref="CAS">internal error: it is impossible to mark as exported the material;Material export</exception>
    public void Export(Entities entities, bool closingBatch, InvoiceContent invoiceContent, List<Disposal> disposals, int sadConsignmentNumber)
    {
      decimal _quantity = this.CalculatedQuantity(invoiceContent);
      foreach (Disposal _disposalX in this.GetListOfDisposals(entities))
      {
        if (!closingBatch && _quantity == 0)
          break;
        _disposalX.Export(entities, ref _quantity, closingBatch, invoiceContent, sadConsignmentNumber, _disposal => _disposal.Disposal2IPRIndex.RecalculateLastStarted(entities, _disposal));
        disposals.Add(_disposalX);
      }
      if (closingBatch || _quantity == 0)
        return;
      string _error = String.Format(
        "There are {0} kg of material {1}/Id={2} that cannot be found for invoice {3}/Content Id={4}.",
        _quantity, this.Batch, this.Id, invoiceContent.InvoiceIndex.BillDoc, invoiceContent.Id.Value);
      throw new CAS.SmartFactory.IPR.WebsiteModel.InputDataValidationException("internal error: it is impossible to mark as exported the material", "Material export", _error, false);
    }
    /// <summary>
    /// Returns a <see cref="System.String" /> that represents this instance.
    /// </summary>
    /// <returns>
    /// A <see cref="System.String" /> that represents this instance.
    /// </returns>
    public override string ToString()
    {
      return GetKey();
    }
    /// <summary>
    /// Gets the list of IPR accounts - candidates to dispose material.
    /// </summary>
    /// <param name="entities">Provides LINQ (Language Integrated Query) access to, and change tracking for, the lists and document libraries of a Windows SharePoint Services "14" Web site.</param>
    public void GetListOfIPRAccounts(Entities entities)
    {
      myVarAccounts2Dispose = (from IPR _iprx in entities.IPR
                               where _iprx.Batch == this.Batch && !_iprx.AccountClosed.Value && _iprx.TobaccoNotAllocated.Value > 0
                               orderby _iprx.CustomsDebtDate.Value ascending, _iprx.DocumentNo ascending
                               select _iprx).ToList<IPR>();
    }
    internal bool IsTobacco
    {
      get
      {
        return ProductType.Value == Linq.ProductType.IPRTobacco || ProductType.Value == Linq.ProductType.Tobacco;
      }
    }
    /// <summary>
    /// Gets the key.
    /// </summary>
    /// <returns></returns>
    internal string GetKey()
    {
      return String.Format(m_keyForam, SKU, Batch, this.StorLoc);
    }
    #endregion

    #region internal
    /// <summary>
    /// Accounts2s the dispose.
    /// </summary>
    /// <value>
    /// The list of IPR accounts candidates to dispose.
    /// </value>
    /// <exception cref="CAS.SharePoint.ApplicationError">Material;Accounts2Dispose;myVarAccounts2Dispose is null;null</exception>
    internal List<IPR> Accounts2Dispose
    {
      get
      {
        if (myVarAccounts2Dispose == null)
          throw new CAS.SharePoint.ApplicationError("Material", "Accounts2Dispose", "myVarAccounts2Dispose is null", null);
        return myVarAccounts2Dispose;
      }
    }
    /// <summary>
    /// Gets the tobacco total.
    /// </summary>
    /// <value>
    /// The tobacco total.
    /// </value>
    internal decimal TobaccoQuantityDec
    {
      get { return Convert.ToDecimal(this.TobaccoQuantity.GetValueOrDefault(0)); }
      set { this.TobaccoQuantity = Convert.ToDouble(value); }
    }
    internal void ReplaceByExistingOne(List<Material> oldMaterials, List<Material> newMaterials, Dictionary<string, Material> parentsMaterials, Batch parent)
    {
      Material _old = null;
      if (!parentsMaterials.TryGetValue(this.GetKey(), out _old))
      {
        Debug.Assert(this.Material2BatchIndex == null, "Material2BatchIndex must be null for new materials");
        this.Material2BatchIndex = parent;
        newMaterials.Add(this);
        return;
      }
      Debug.Assert(this != _old, "this cannot be the same as _old");
      Debug.Assert(_old.Material2BatchIndex == parent, "Material2BatchIndex must be equal parent for old materials");
      Debug.Assert(((Microsoft.SharePoint.Linq.ITrackEntityState)_old).EntityState != Microsoft.SharePoint.Linq.EntityState.ToBeInserted, "EntityState is in wrong state: ToBeInserted");
      oldMaterials.Add(_old);
      _old.FGQuantity = this.FGQuantity;
      _old.Disposed = _old.TobaccoQuantityDec;
      _old.TobaccoQuantity = this.TobaccoQuantity;
      _old.myVarAccounts2Dispose = this.Accounts2Dispose;
    }
    internal void UpdateDisposals(Entities edc, ProgressChangedEventHandler progressChanged)
    {
      string _parentBatch = this.Material2BatchIndex.Batch0;
      if (this.ProductType.Value != Linq.ProductType.IPRTobacco)
        return;
      IEnumerable<Disposal> _allDisposals = this.Disposal(edc, true);
      foreach (Linq.DisposalEnum _kind in Enum.GetValues(typeof(Linq.DisposalEnum)))
      {
        try
        {
          if (_kind == DisposalEnum.Tobacco || _kind == DisposalEnum.Cartons)
            continue;
          decimal _toDispose = this[_kind];
          List<Disposal> _disposalsOfKind = _allDisposals.Where<Disposal>(x => x.DisposalStatus.Value == Entities.GetDisposalStatus(_kind)).ToList<Disposal>();
          if (_disposalsOfKind.Count<Disposal>() > 0)
          {
            _toDispose -= _disposalsOfKind.Sum<Disposal>(x => x.SettledQuantityDec);
            _disposalsOfKind = _disposalsOfKind.Where(v => v.CustomsStatus.Value == CustomsStatus.NotStarted).ToList<Disposal>();
            foreach (Linq.Disposal _dx in _disposalsOfKind)
              _dx.Adjust(edc, ref _toDispose);
            if (_toDispose == 0)
              continue;
            if (_toDispose <= 0)
            {
              string _toDisposeMessage = "_toDispose < 0 and is of {3} kg: Tobacco batch: {0}, fg batch: {1}, disposal: {2}";
              throw new IPRDataConsistencyException("Material.UpdateDisposals", String.Format(_toDisposeMessage, this.Batch, _parentBatch, _kind, _toDispose), null, "IPR calculation error");
            }
          }
          progressChanged(this, new ProgressChangedEventArgs(1, String.Format("AddDisposal {0}, batch {1}", _kind, this.Batch)));
          if (((_kind == DisposalEnum.SHMenthol) || (_kind == DisposalEnum.OverusageInKg)) && this[_kind] <= 0)
            continue;
          AddNewDisposals(edc, _kind, ref _toDispose);
          if (_toDispose <= 0)
            continue;
          string _mssg = "Cannot find IPR account to dispose the tobacco of {3} kg: Tobacco batch: {0}, fg batch: {1}, disposal: {2}";
          throw new IPRDataConsistencyException("Material.UpdateDisposals", String.Format(_mssg, this.Batch, _parentBatch, _kind, _toDispose), null, "IPR unrecognized account");
        }
        catch (IPRDataConsistencyException _ex)
        {
          _ex.Add2Log(edc);
        }
      }
    }
    internal void AddNewDisposals(Entities edc, Linq.DisposalEnum _kind, ref decimal _toDispose)
    {
      foreach (IPR _iprx in this.Accounts2Dispose)
      {
        if (_iprx.TobaccoNotAllocated <= 0)
          continue;
        _iprx.AddDisposal(edc, _kind, ref _toDispose, this);
        if (_toDispose <= 0)
          return;
      }
    }
    internal void AddNewDisposals(Entities edc, Linq.DisposalEnum _kind, ref decimal _toDispose, InvoiceContent invoiceContent)
    {
      foreach (IPR _iprx in this.Accounts2Dispose)
      {
        if (_iprx.TobaccoNotAllocated <= 0)
          continue;
        _iprx.AddDisposal(edc, _kind, ref _toDispose, this, invoiceContent);
        if (_toDispose <= 0)
          break;
      }
    }
    internal void GetInventory(Balance.StockDictionary balanceStock, Balance.StockDictionary.StockValueKey key, double portion)
    {
      switch (this.ProductType.Value)
      {
        case Linq.ProductType.Cutfiller:
        case Linq.ProductType.Cigarette:
        case Linq.ProductType.Tobacco:
        case Linq.ProductType.Other:
          break;
        case Linq.ProductType.IPRTobacco:
          balanceStock.Sum(Convert.ToDecimal(this.TobaccoQuantity * portion), this.Batch, key);
          break;
      }
    }
    #endregion

    #region private
    IEnumerable<Disposal> m_Disposal = null;
    private decimal Disposed { get; set; }
    private const string m_keyForam = "{0}:{1}:{2}";
    private List<IPR> myVarAccounts2Dispose = null;
    private IEnumerable<Disposal> Disposal(Entities edc, bool emptyListIfNew)
    {
      if (!Id.HasValue)
        return emptyListIfNew ? new Disposal[] { } : null;
      if (m_Disposal == null)
        m_Disposal = this.Id.HasValue ? from _dslx in edc.Disposal let _id = _dslx.Disposal2MaterialIndex.Id.Value where _id == this.Id.Value select _dslx : null;
      return m_Disposal;
    }
    #endregion

    #region IComparable<Material> Members
    /// <summary>
    /// Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>
    /// A 32-bit signed integer that indicates the relative order of the objects being compared. The return value has the following meanings: Value Meaning Less than zero This object is less than the other parameter.Zero This object is equal to other. Greater than zero This object is greater than other.
    /// </returns>
    public int CompareTo(Material other)
    {
      if (other == null)
        return 1;
      return TobaccoQuantityDec.CompareTo(other.TobaccoQuantityDec);
    }
    #endregion

  }
}
