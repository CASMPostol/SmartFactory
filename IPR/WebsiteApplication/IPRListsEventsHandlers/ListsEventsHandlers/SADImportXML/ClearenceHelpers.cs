//<summary>
//  Title   : static class ClearenceHelpers
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

using CAS.SharePoint.Logging;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Account;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.CWInterconnection;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers
{
  internal static class ClearenceHelpers
  {

    #region public
    internal static void DeclarationProcessing(string webUrl, int sadDocumentTypeId, CustomsDocument.DocumentType documentType, ref string comments, List<Warnning> warnings, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering ClearenceHelpers.DeclarationProcessing", 40, TraceSeverity.Verbose);
      comments = "Clearance association error";
      switch (documentType)
      {
        case CustomsDocument.DocumentType.SAD:
        case CustomsDocument.DocumentType.PZC:
          SADPZCProcessing(webUrl, documentType, sadDocumentTypeId, ref comments, warnings, trace);
          break;
        case CustomsDocument.DocumentType.IE529:
          IE529Processing(webUrl, sadDocumentTypeId, ref comments, trace);
          break;
        case CustomsDocument.DocumentType.CLNE:
          CLNEProcessing(webUrl, sadDocumentTypeId, ref comments, warnings, trace);
          break;
      }//switch (_documentType
    }
    #endregion

    #region private
    private static void IE529Processing(string webUrl, int sadDocumentTypeId, ref string comments, NamedTraceLogger.TraceAction trace)
    {
      comments = "Reexport of goods failed";
      trace("Entering ClearenceHelpers.IE529Processing", 61, TraceSeverity.Verbose);
      using (Entities _entities = new Entities(webUrl))
      {
        SADDocumentType _sad = Element.GetAtIndex<SADDocumentType>(_entities.SADDocument, sadDocumentTypeId);
        foreach (SADGood _gdx in _sad.SADGood(_entities))
          IPRClearThroughCustoms(_entities, _gdx, trace);
        comments = "Reexport of goods";
        trace("ClearenceHelpers.IE529Processing SubmitChanges", 61, TraceSeverity.Verbose);
        _entities.SubmitChanges();
      }
    }
    private static void CLNEProcessing(string webUrl, int sadDocumentTypeId, ref string comments, List<Warnning> warnings, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering ClearenceHelpers.CLNEProcessing", 71, TraceSeverity.Verbose);
      List<CWAccountData> _tasksList = new List<CWAccountData>();
      using (Entities _entities = new Entities(webUrl))
      {
        SADDocumentType _sad = Element.GetAtIndex<SADDocumentType>(_entities.SADDocument, sadDocumentTypeId);
        IQueryable<Clearence> _clrncs = Clearence.GetClearence(_entities, _sad.ReferenceNumber);
        if ((from _cx in _clrncs where _cx.Clearence2SadGoodID == null select _cx).Any<Clearence>())
        {
          string _error = String.Format("SAD with reference number: {0} must be imported first", _sad.ReferenceNumber);
          throw new InputDataValidationException("CLNE message cannot be processed before SAD", "DeclarationProcessing", _error, true);
        }
        foreach (Clearence _cx in _clrncs)
        {
          _cx.DocumentNo = _sad.DocumentNumber;
          switch (_cx.ClearenceProcedure.Value.RequestedProcedure())
          {
            case CustomsProcedureCodes.FreeCirculation:
              _cx.FinishClearingThroughCustoms(_entities, trace);
              break;
            case CustomsProcedureCodes.InwardProcessing:
              CreateIPRAccount(_entities, _cx, CustomsDocument.DocumentType.SAD, out comments, warnings, trace);
              break;
            case CustomsProcedureCodes.CustomsWarehousingProcedure:
              throw new NotImplementedException("CLNEProcessing - CustomsWarehousingProcedure"); //TODO http://casas:11227/sites/awt/Lists/RequirementsList/_cts/Requirements/displayifs.aspx?List=e1cf335a
            //comments = "CW account creation error";
            //CWAccountData _accountData = new CWAccountData(_cx.Id.Value);
            //_accountData.GetAccountData(_entities, _cx, ImportXMLCommon.Convert2MessageType(CustomsDocument.DocumentType.SAD), progressChange);
            //CreateCWAccount(_accountData, webUrl, out comments);
            //break;
            case CustomsProcedureCodes.ReExport:
            case CustomsProcedureCodes.NoProcedure:
            default:
              throw new IPRDataConsistencyException("CLNEProcessing", "Unexpected procedure code for CLNE message", null, c_wrongProcedure);
          }
        }
        _entities.SubmitChanges();
      }
      trace("ClearenceHelpers.CLNEProcessing at CreateCWAccount", 109, TraceSeverity.Verbose);
      foreach (CWAccountData _accountData in _tasksList)
        CreateCWAccount(_accountData, webUrl, out comments);
    }
    private static void SADPZCProcessing(string webUrl, CustomsDocument.DocumentType messageType, int sadDocumentTypeId, ref string comments, List<Warnning> warnings, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering ClearenceHelpers.SADPZCProcessing", 71, TraceSeverity.Verbose);
      List<CommonClearanceData> _tasksList = new List<CommonClearanceData>();
      using (Entities entities = new Entities(webUrl))
      {
        SADDocumentType sad = Element.GetAtIndex<SADDocumentType>(entities.SADDocument, sadDocumentTypeId);
        foreach (SADGood _sgx in sad.SADGood(entities))
        {
          switch (_sgx.SPProcedure.RequestedProcedure())
          {
            case CustomsProcedureCodes.FreeCirculation:
              if (messageType == CustomsDocument.DocumentType.SAD)
              {
                comments = "Document added";
                continue;
              }
              if (_sgx.SPProcedure.PreviousProcedure() == CustomsProcedureCodes.CustomsWarehousingProcedure)
                _tasksList.Add(CWPrepareClearance(entities, _sgx)); //Procedure 4071
              else if (_sgx.SPProcedure.PreviousProcedure() == CustomsProcedureCodes.InwardProcessing)
                IPRClearThroughCustoms(entities, _sgx, trace); //Procedure 4051
              else
              {
                string _msg = string.Format("Unexpected previous procedure code {1} for the {0} message", messageType, _sgx.SPProcedure.PreviousProcedure());
                trace("IPRDataConsistencyException at ClearenceHelpers.SADPZCProcessing: ", 140, TraceSeverity.Verbose);
                throw new IPRDataConsistencyException("SADPZCProcessing.FreeCirculation", _msg, null, c_wrongProcedure);
              }
              break;
            case CustomsProcedureCodes.InwardProcessing:
              {
                if (messageType == CustomsDocument.DocumentType.SAD)
                {
                  comments = "Document added";
                  continue;
                }
                if (_sgx.SPProcedure.PreviousProcedure() == CustomsProcedureCodes.CustomsWarehousingProcedure)
                  _tasksList.Add(CWPrepareClearance(entities, _sgx)); //Procedure 5171
                // Procedure 5100 or 5171
                Clearence _newClearance = Clearence.CreataClearence(entities, "InwardProcessing", ClearenceProcedure._5171, _sgx);
                CreateIPRAccount(entities, _newClearance, CustomsDocument.DocumentType.PZC, out comments, warnings, trace);
                break;
              }
            case CustomsProcedureCodes.CustomsWarehousingProcedure:
              Clearence _newWarehousinClearance = Clearence.CreataClearence(entities, "CustomsWarehousingProcedure", ClearenceProcedure._7100, _sgx);
              if (messageType == CustomsDocument.DocumentType.PZC)
              {
                comments = "CW account creation error";
                CWAccountData _accountData = new CWAccountData(_newWarehousinClearance.Id.Value);
                _accountData.GetAccountData(entities, _newWarehousinClearance, ImportXMLCommon.Convert2MessageType(CustomsDocument.DocumentType.SAD), warnings, trace);
                _tasksList.Add(_accountData);
              }
              else
                comments = "Document added";
              break;
            case CustomsProcedureCodes.NoProcedure:
            case CustomsProcedureCodes.ReExport:
            default:
              throw new IPRDataConsistencyException("SADPZCProcessing.RequestedProcedure", string.Format("Unexpected procedure code for the {0} message", messageType), null, c_wrongProcedure);
          }//switch ( _sgx.Procedure.RequestedProcedure() )
        }//foreach ( SADGood _sgx in sad.SADGood )
        entities.SubmitChanges();
      } //using ( Entities entities
      foreach (CommonClearanceData _accountData in _tasksList)
      {
        if (_accountData is CWAccountData)
          CreateCWAccount((CWAccountData)_accountData, webUrl, out comments);
        else if (_accountData is CWClearanceData)
          CWClearThroughCustoms((CWClearanceData)_accountData, webUrl, out comments);
      }
    }
    /// <summary>
    /// Creates the IPR account.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="clearance">The clearance.</param>
    /// <param name="messageType">Type of the _message.</param>
    /// <param name="comments">The _comments.</param>
    /// <param name="warnings">The list of warnings.</param>
    /// <param name="trace">The trace action.</param>
    private static void CreateIPRAccount(Entities entities, Clearence clearance, CustomsDocument.DocumentType messageType, out string comments, List<Warnning> warnings, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering ClearenceHelpers.CreateIPRAccount", 169, TraceSeverity.Verbose);
      comments = "IPR account creation error";
      string _referenceNumber = String.Empty;
      SADDocumentType declaration = clearance.Clearence2SadGoodID.SADDocumentIndex;
      _referenceNumber = declaration.ReferenceNumber;
      if (WebsiteModel.Linq.IPR.RecordExist(entities, clearance.DocumentNo))
      {
        string _msg = "IPR record with the same SAD document number: {0} exist";
        _msg = String.Format(_msg, clearance.DocumentNo);
        trace("Exception at ClearenceHelpers.CreateIPRAccount: " + _msg, 199, TraceSeverity.Verbose);
        throw GenericStateMachineEngine.ActionResult.NotValidated(_msg);
      }
      comments = "Inconsistent or incomplete data to create IPR account";
      IPRAccountData _iprdata = new IPRAccountData(clearance.Id.Value);
      _iprdata.GetAccountData(entities, clearance, ImportXMLCommon.Convert2MessageType(messageType), warnings, trace);
      comments = "Consent lookup filed";
      IPRClass _ipr = new IPRClass(entities, _iprdata, clearance, declaration);
      entities.IPR.InsertOnSubmit(_ipr);
      clearance.SPStatus = true;
      trace("ClearenceHelpers.CreateIPRAccount at SubmitChanges", 209, TraceSeverity.Verbose);
      entities.SubmitChanges();
      _ipr.UpdateTitle();
      comments = "IPR account created";
      trace("ClearenceHelpers.Create - IPRAccount comments", 213, TraceSeverity.Verbose);
    }
    /// <summary>
    /// Creates the CW account.
    /// </summary>
    /// <param name="_accountData">The _account data.</param>
    /// <param name="requestUrl">The request URL.</param>
    /// <param name="comments">The comments.</param>
    /// <exception cref="InputDataValidationException">Create CW Account Failed;CreateCWAccount</exception>
    /// <exception cref="IPRDataConsistencyException">IPR account creation error</exception>
    private static void CreateCWAccount(CWAccountData _accountData, string requestUrl, out string comments)
    {
      List<Warnning> _lw = new List<Warnning>();
      _accountData.CallService(requestUrl, _lw);
      if (_lw.Count == 0)
      {
        comments = "CW account created";
        return;
      }
      ErrorsList _el = new ErrorsList();
      _el.AddRange(_lw);
      throw new InputDataValidationException("Create CW Account Failed", "CreateCWAccount", _el);
    }
    /// <summary>
    /// Clear through customs according 4071 procedure.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="good">The good.</param>
    /// <exception cref="InputDataValidationException">Create CW Account Failed;CreateCWAccount</exception>
    private static CWClearanceData CWPrepareClearance(Entities entities, SADGood good)
    {
      Clearence _clearance = GetClearanceIds(entities, good, Settings.GetParameter(entities, SettingsEntry.RequiredDocumentSADTemplateDocumentNamePattern)).First<Clearence>();
      _clearance.FinishClearingThroughCustoms(good);
      return new CWClearanceData(_clearance.Id.Value);
    }
    private static void CWClearThroughCustoms(CWClearanceData _accountData, string webUrl, out string comments)
    {
      List<Warnning> _lw = new List<Warnning>();
      _accountData.CallService(webUrl, _lw);
      if (_lw.Count == 0)
      {
        comments = "CW account created";
        return;
      }
      ErrorsList _el = new ErrorsList();
      _el.AddRange(_lw);
      throw new InputDataValidationException("Create CW Account Failed", "CreateCWAccount", _el);
    }
    /// <summary>
    /// Clear through customs according procedure 4051.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="good">The good description form.</param>
    /// <param name="trace">The trace action.</param>
    /// <exception cref="InputDataValidationException">SAD Required Documents;clear through customs fatal error; true</exception>
    private static void IPRClearThroughCustoms(Entities entities, SADGood good, NamedTraceLogger.TraceAction trace)
    {
      foreach (Clearence _clearance in GetClearanceIds(entities, good, Settings.GetParameter(entities, SettingsEntry.RequiredDocumentFinishedGoodExportConsignmentPattern)))
        _clearance.FinishClearingThroughCustoms(entities, good, trace);
    }
    private static List<Clearence> GetClearanceIds(Entities entities, SADGood good, string pattern)
    {
      List<Clearence> _ret = new List<Clearence>();
      foreach (SADRequiredDocuments _rdx in good.SADRequiredDocuments(entities))
      {
        if (_rdx.Code != XMLResources.RequiredDocumentConsignmentCode)
          continue;
        int? _cleranceInt = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber(_rdx.Number, pattern);
        if (!_cleranceInt.HasValue)
          continue;
        if (_cleranceInt.HasValue)
        {
          Clearence _clearance = Element.GetAtIndex<Clearence>(entities.Clearence, _cleranceInt.Value);
          _ret.Add(_clearance);
        }
        else
        {
          string _template = "Cannot find clearance for the required document code={0} for customs document = {1}/ref={2}";
          throw new InputDataValidationException(
            String.Format(_template, _rdx.Number, good.SADDocumentIndex.DocumentNumber, good.SADDocumentIndex.ReferenceNumber),
            "SAD Required Documents",
            "clear through customs fatal error", true);
        }
      }// foreach
      if (_ret.Count == 0)
      {
        string _template = "Cannot find required document code={0} for customs document = {1}/ref={2}";
        throw new InputDataValidationException(
          String.Format(_template, XMLResources.RequiredDocumentConsignmentCode, good.SADDocumentIndex.DocumentNumber, good.SADDocumentIndex.ReferenceNumber),
          "SAD Required Documents",
          "clear through customs fatal error", true);
      }
      return _ret;
    }
    /// <summary>
    /// Get requested customs procedure code
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>
    /// Requested procedure code <see cref="CustomsProcedureCodes" /> - first two chars of the box 37
    /// </returns>
    private static CustomsProcedureCodes RequestedProcedure(this ClearenceProcedure value)
    {
      CustomsProcedureCodes _ret = default(CustomsProcedureCodes);
      switch (value)
      {
        case ClearenceProcedure._3151:
        case ClearenceProcedure._3171:
          _ret = CustomsProcedureCodes.NoProcedure;
          break;
        case ClearenceProcedure._4051:
        case ClearenceProcedure._4071:
          _ret = CustomsProcedureCodes.FreeCirculation;
          break;
        case ClearenceProcedure._5100:
        case ClearenceProcedure._5171:
          _ret = CustomsProcedureCodes.InwardProcessing;
          break;
        case ClearenceProcedure._7100:
        case ClearenceProcedure._7171:
          _ret = CustomsProcedureCodes.CustomsWarehousingProcedure;
          break;
      }
      return _ret;
    }
    private static CustomsProcedureCodes PreviousProcedure(this string cpc)
    {
      return Convert2CustomsProcedureCodes(cpc.Substring(2, 2));
    }
    private static CustomsProcedureCodes RequestedProcedure(this string cpc)
    {
      return Convert2CustomsProcedureCodes(cpc.Remove(2));
    }
    private static CustomsProcedureCodes Convert2CustomsProcedureCodes(string cpc)
    {
      switch (cpc)
      {
        case "00":
          return CustomsProcedureCodes.NoProcedure;
        case "31":
          return CustomsProcedureCodes.ReExport;
        case "40":
          return CustomsProcedureCodes.FreeCirculation;
        case "51":
          return CustomsProcedureCodes.InwardProcessing;
        case "71":
          return CustomsProcedureCodes.CustomsWarehousingProcedure;
        default:
          throw new CustomsDataException("Extensions.RequestedProcedure", "Unsupported requested procedure");
      }
    }
    private const string c_wrongProcedure = "Wrong customs procedure";
    #endregion

  }
}
