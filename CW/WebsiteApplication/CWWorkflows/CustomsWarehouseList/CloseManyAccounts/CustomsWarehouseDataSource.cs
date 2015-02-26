//_______________________________________________________________
//  Title   : Name of Application
//  System  : Microsoft VisualStudio 2013 / C#
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2015, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//_______________________________________________________________

using System;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseList.CloseManyAccounts
{
  /// <summary>
  /// Class CustomsWarehouseDataSource.
  /// </summary>
  public class CustomsWarehouseDataSource
  {

    #region public API
    /// <summary>
    /// Gets or sets a value indicating whether this instance is selected.
    /// </summary>
    /// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
    public bool IsSelected
    {
      get { return _isSelected; }
      set { _isSelected = value; }
    }
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>The identifier.</value>
    public int Id
    {
      get { return this._id; }
      set { this._id = value; }
    }
    /// <summary>
    /// Gets or sets the title.
    /// </summary>
    /// <value>The title.</value>
    public string Title
    {
      get { return this._title; }
      set { this._title = value; }
    }
    /// <summary>
    /// Gets or sets the document no.
    /// </summary>
    /// <value>The document no.</value>
    public string DocumentNo
    {
      get { return this._documentNo; }
      set { this._documentNo = value; }
    }
    /// <summary>
    /// Gets or sets the customs debt date.
    /// </summary>
    /// <value>The customs debt date.</value>
    public System.DateTime CustomsDebtDate
    {
      get { return this._customsDebtDate; }
      set { this._customsDebtDate = value; }
    }
    /// <summary>
    /// Gets or sets the grade.
    /// </summary>
    /// <value>The grade.</value>
    public string Grade
    {
      get { return this._grade; }
      set { this._grade = value; }
    }
    /// <summary>
    /// Gets or sets the sku.
    /// </summary>
    /// <value>The sku.</value>
    public string SKU
    {
      get { return this._sKU; }
      set { this._sKU = value; }
    }
    /// <summary>
    /// Gets or sets the batch.
    /// </summary>
    /// <value>The batch.</value>
    public string Batch
    {
      get { return this._batch; }
      set { this._batch = value; }
    }
    /// <summary>
    /// Gets or sets the net mass.
    /// </summary>
    /// <value>The net mass.</value>
    public double NetMass
    {
      get { return this._netMass; }
      set { this._netMass = value; }
    }
    /// <summary>
    /// Gets or sets the account balance.
    /// </summary>
    /// <value>The account balance.</value>
    public double AccountBalance
    {
      get { return this._accountBalance; }
      set { this._accountBalance = value; }
    }
    /// <summary>
    /// Gets or sets the valid to date.
    /// </summary>
    /// <value>The valid to date.</value>
    public System.DateTime ValidToDate
    {
      get { return this._validToDate; }
      set { this._validToDate = value; }
    }
    /// <summary>
    /// Gets or sets the closing date.
    /// </summary>
    /// <value>The closing date.</value>
    public System.DateTime ClosingDate
    {
      get { return this._closingDate; }
      set { this._closingDate = value; }
    }
    #endregion

    #region private
    private int _id;
    private string _title;
    private string _documentNo;
    private DateTime _customsDebtDate;
    private bool _isSelected = true;
    private string _grade;
    private string _sKU;
    private string _batch;
    private double _netMass;
    private double _accountBalance;
    private DateTime _validToDate;
    private DateTime _closingDate;
    #endregion

  }
}
