//<summary>
//  Title   : class AccountsReportContentWithStylesheet 
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
using System.Linq;
using CAS.SharePoint;
using CAS.SharePoint.DocumentsFactory;
using CAS.SmartFactory.CW.Interoperability.DocumentsFactory.AccountsReport;
using CAS.SmartFactory.CW.WebsiteModel.Linq;

namespace CAS.SmartFactory.CW.Workflows.CustomsWarehouseReport
{
  /// <summary>
  /// class AccountsReportContentWithStylesheet 
  /// </summary>
  public class AccountsReportContentWithStylesheet : AccountsReportContent, IStylesheetNameProvider
  {

    #region public
    internal static AccountsReportContentWithStylesheet Create(Entities entities, string documentName)
    {

      Dictionary<string, Consent> _ConsentsList = new Dictionary<string, Consent>();
      IQueryable<IGrouping<string, CustomsWarehouse>> _groups = from _grx in entities.CustomsWarehouse
                                                                where !_grx.Archival.Value && !_grx.AccountClosed.Value && _grx.AccountBalance.Value > 0
                                                                let _curr = _grx.Currency.Trim().ToUpper()
                                                                group _grx by _curr;
      List<ArrayOfAccountsAccountsArray> _AccountsColection = CreateArrayOfAccountsAccountsArray(_groups);
      AccountsReportContentWithStylesheet _ret = new AccountsReportContentWithStylesheet()
        {
          AccountsColection = _AccountsColection.ToArray(),
          Consents = String.Join(",", _ConsentsList.Select(x => String.Format("{0}(1:d)", x.Value.Title, x.Value.ConsentDate.Value)).ToArray<string>()),
          DocumentDate = DateTime.Today,
          DocumentName = documentName,
          ReportDate = DateTime.Today,
        };
      return _ret;
    }
    #endregion

    #region private
    private static List<ArrayOfAccountsAccountsArray> CreateArrayOfAccountsAccountsArray(IQueryable<IGrouping<string, CustomsWarehouse>> groups)
    {
      List<ArrayOfAccountsAccountsArray> _ret = new List<ArrayOfAccountsAccountsArray>();
      foreach (IGrouping<string, CustomsWarehouse> _grx in groups)
      {
        decimal _TotalNetMass;
        decimal _TotalValue;
        List<ArrayOfAccountsDetailsDetailsOfOneAccount> _AccountsDetails = CreateDetails(_grx, out _TotalNetMass, out _TotalValue);
        ArrayOfAccountsAccountsArray _newItem = new ArrayOfAccountsAccountsArray()
        {
          AccountsDetails = _AccountsDetails.ToArray(),
          TotalCurrency = _grx.Key,
          TotalNetMass = Convert.ToDouble(_TotalNetMass),
          TotalValue = Convert.ToDouble(_TotalValue)
        };
        _ret.Add(_newItem);
      }
      return _ret;
    }
    private static List<ArrayOfAccountsDetailsDetailsOfOneAccount> CreateDetails(IGrouping<string, CustomsWarehouse> group, out decimal totalNetMass, out decimal totalValue)
    {
      totalNetMass = 0;
      totalValue = 0;
      List<ArrayOfAccountsDetailsDetailsOfOneAccount> _ret = new List<ArrayOfAccountsDetailsDetailsOfOneAccount>();
      int _No = 0;
      foreach (CustomsWarehouse _cwx in group)
      {
        decimal _Value = Convert.ToDecimal(_cwx.Value.GetValueOrDefault(-1));
        totalValue += _Value;
        decimal _mass = Convert.ToDecimal(_cwx.AccountBalance.GetValueOrDefault(-1));
        totalNetMass += totalNetMass;
        ArrayOfAccountsDetailsDetailsOfOneAccount _newitem = new ArrayOfAccountsDetailsDetailsOfOneAccount()
        {
          Batch = _cwx.Batch,
          CNTarrifCode = _cwx.CWL_CW2PCNID.ProductCodeNumber,
          Currency = _cwx.Currency,
          CustomsDebtDate = _cwx.CustomsDebtDate.GetValueOrDefault(Extensions.SPMinimum),
          DocumentNo = _cwx.DocumentNo,
          Grade = _cwx.Grade,
          NetMass = Convert.ToDouble(_mass),
          No = _No++,
          SKU = _cwx.SKU,
          TobaccoName = _cwx.TobaccoName,
          Value = Convert.ToDouble(_Value)
        };
        _ret.Add(_newitem);
      }
      _ret.Sort((x, y) => x.No.CompareTo(y.No));
      return _ret;
    }
    #endregion

  }
}
