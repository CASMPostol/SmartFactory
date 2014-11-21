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
      List<ArrayOfAccountsAccountsArray> _AccountsColection = CreateArrayOfAccountsAccountsArray(_groups, _ConsentsList);
      AccountsReportContentWithStylesheet _ret = new AccountsReportContentWithStylesheet()
        {
          AccountsColection = _AccountsColection.ToArray(),
          DocumentDate = DateTime.Today,
          DocumentName = documentName,
          ReportDate = DateTime.Today,
          ConsentsCollection = _ConsentsList.Select(x => new ArrayOfConsentsConsentsArray() { Consent = x.Value.Title, ConsentDate = x.Value.ConsentDate.Value.Date }).ToArray()
        };
      return _ret;
    }
    #endregion

    #region private
    private static List<ArrayOfAccountsAccountsArray> CreateArrayOfAccountsAccountsArray(IQueryable<IGrouping<string, CustomsWarehouse>> groups, Dictionary<string, Consent> consentsList)
    {
      List<ArrayOfAccountsAccountsArray> _ret = new List<ArrayOfAccountsAccountsArray>();
      foreach (IGrouping<string, CustomsWarehouse> _grx in groups)
      {
        decimal _TotalNetMass;
        decimal _TotalValue;
        List<ArrayOfAccountsDetailsDetailsOfOneAccount> _AccountsDetails = CreateDetails(_grx, out _TotalNetMass, out _TotalValue, consentsList);
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
    private static List<ArrayOfAccountsDetailsDetailsOfOneAccount> CreateDetails(Entities edc, IGrouping<string, CustomsWarehouse> group, out decimal totalNetMass, out decimal totalValue, Dictionary<string, Consent> consentsList)
    {
      totalNetMass = 0;
      totalValue = 0;
      List<ArrayOfAccountsDetailsDetailsOfOneAccount> _ret = new List<ArrayOfAccountsDetailsDetailsOfOneAccount>();
      int _No = 1;
      foreach (CustomsWarehouse _cwx in group.OrderBy<CustomsWarehouse, string>(x => x.DocumentNo))
      {
        if (_cwx.CWL_CW2ConsentTitle == null)
          throw new ArgumentNullException("CWL_CW2ConsentTitle", "Incosistent data content: in AccountsReportContentWithStylesheet.CreateDetails the account must be associated with a consent.");
        if (!consentsList.ContainsKey(_cwx.CWL_CW2ConsentTitle.Title))
          consentsList.Add(_cwx.CWL_CW2ConsentTitle.Title, _cwx.CWL_CW2ConsentTitle);
        List<CustomsWarehouseDisposal> _last = (from _cwdx in _cwx.CustomsWarehouseDisposal(edc, false) //TODO mp
                                                where _cwdx.CustomsStatus.Value == CustomsStatus.Finished
                                                orderby _cwdx.SPNo.Value descending
                                                select _cwdx).ToList<CustomsWarehouseDisposal>();
        decimal _Value = 0;
        decimal _mass = 0;
        if (_last.Count > 0)
        {
          _Value = Convert.ToDecimal(_last[0].CW_RemainingTobaccoValue.GetValueOrDefault());
          _mass = Convert.ToDecimal(_last[0].RemainingQuantity.GetValueOrDefault(-1));
        }
        else
        {
          _Value = Convert.ToDecimal(_cwx.Value.GetValueOrDefault());
          _mass = Convert.ToDecimal(_cwx.CW_Quantity.GetValueOrDefault(-1));
        }
        totalValue += _Value;
        totalNetMass += _mass;
        ArrayOfAccountsDetailsDetailsOfOneAccount _newitem = new ArrayOfAccountsDetailsDetailsOfOneAccount()
        {
          Batch = _cwx.Batch,
          CNTarrifCode = _cwx.CWL_CW2PCNID.ProductCodeNumber,
          Currency = _cwx.Currency,
          CustomsDebtDate = _cwx.CustomsDebtDate.GetValueOrNull(),
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
