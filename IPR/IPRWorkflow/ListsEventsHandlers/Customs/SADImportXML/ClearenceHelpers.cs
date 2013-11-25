﻿//<summary>
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.CWInterconnection;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs.SADImportXML
{
  internal static class ClearenceHelpers
  {

    #region public
    internal static void DeclarationProcessing(string webUrl, int sadDocumentTypeId, CustomsDocument.DocumentType documentType, ref string comments, ProgressChangedEventHandler ProgressChange)
    {
      comments = "Clearance association error";
      switch (documentType)
      {
        case CustomsDocument.DocumentType.SAD:
        case CustomsDocument.DocumentType.PZC:
          SADPZCProcessing(webUrl, documentType, sadDocumentTypeId, ref comments, ProgressChange);
          break;
        case CustomsDocument.DocumentType.IE529:
          IE529Processing(webUrl, sadDocumentTypeId, ref comments);
          break;
        case CustomsDocument.DocumentType.CLNE:
          CLNEProcessing(webUrl, sadDocumentTypeId, ref comments, ProgressChange);
          break;
      }//switch (_documentType
    }
    #endregion

    #region private
    private static void IE529Processing(string webUrl, int sadDocumentTypeId, ref string comments)
    {
      comments = "Reexport of goods failed";
      using (Entities _entities = new Entities(webUrl))
      {
        SADDocumentType _sad = Element.GetAtIndex<SADDocumentType>(_entities.SADDocument, sadDocumentTypeId);
        foreach (SADGood _gdx in _sad.SADGood)
          IPRClearThroughCustoms(_entities, _gdx);
        comments = "Reexport of goods";
        _entities.SubmitChanges();
      }
    }
    private static void CLNEProcessing(string webUrl, int sadDocumentTypeId, ref string comments, ProgressChangedEventHandler progressChange)
    {
      List<CWAccountData> _tasksList = new List<CWAccountData>();
      using (Entities _entities = new Entities(webUrl))
      {
        SADDocumentType _sad = Element.GetAtIndex<SADDocumentType>(_entities.SADDocument, sadDocumentTypeId);
        IQueryable<Clearence> _clrncs = Clearence.GetClearence(_entities, _sad.ReferenceNumber);
        if ((from _cx in _clrncs where _cx.Clearence2SadGoodID == null select _cx).Any<Clearence>())
        {
          string _error = String.Format("SAD with reference number: {0} must be imported first", _sad.ReferenceNumber);
          throw new InputDataValidationException("CLNE message cannot be processed befor SAD", "DeclarationProcessing", _error, true);
        }
        foreach (Clearence _cx in _clrncs)
        {
          _cx.DocumentNo = _sad.DocumentNumber;
          switch (_cx.ClearenceProcedure.Value.RequestedProcedure())
          {
            case CustomsProcedureCodes.FreeCirculation:
              _cx.FinishClearingThroughCustoms(_entities);
              break;
            case CustomsProcedureCodes.InwardProcessing:
              CreateIPRAccount(_entities, _cx, CustomsDocument.DocumentType.SAD, out comments, progressChange);
              break;
            case CustomsProcedureCodes.CustomsWarehousingProcedure:
              throw new NotImplementedException("CLNEProcessing - CustomsWarehousingProcedure"); //TODO http://casas:11227/sites/awt/Lists/RequirementsList/_cts/Requirements/displayifs.aspx?List=e1cf335a
              comments = "CW account creation error";
              CWAccountData _accountData = new CWAccountData(_cx.Id.Value);
              _accountData.GetAccountData(_entities, _cx, ImportXMLCommon.Convert2MessageType(CustomsDocument.DocumentType.SAD), progressChange);
              CreateCWAccount(_accountData, webUrl, out comments);
              break;
            case CustomsProcedureCodes.ReExport:
            case CustomsProcedureCodes.NoProcedure:
            default:
              throw new IPRDataConsistencyException("CLNEProcessing", "Unexpected procedure code for CLNE message", null, _wrongProcedure);
          }
        }
        _entities.SubmitChanges();
      }
      foreach (CWAccountData _accountData in _tasksList)
        CreateCWAccount(_accountData, webUrl, out comments);
    }
    private static void SADPZCProcessing(string webUrl, CustomsDocument.DocumentType messageType, int sadDocumentTypeId, ref string comments, ProgressChangedEventHandler ProgressChange)
    {
      List<CWAccountData> _tasksList = new List<CWAccountData>();
      using (Entities entities = new Entities(webUrl))
      {
        SADDocumentType sad = Element.GetAtIndex<SADDocumentType>(entities.SADDocument, sadDocumentTypeId);
        foreach (SADGood _sgx in sad.SADGood)
        {
          switch (_sgx.Procedure.RequestedProcedure())
          {
            case CustomsProcedureCodes.FreeCirculation:
              if (messageType == CustomsDocument.DocumentType.SAD)
              {
                comments = "Document added";
                continue;
              }
              if (_sgx.Procedure.PreviousProcedure() == CustomsProcedureCodes.CustomsWarehousingProcedure)
                CWClearThroughCustoms(entities, _sgx, out comments); //Procedure 4071
              else if (_sgx.Procedure.PreviousProcedure() == CustomsProcedureCodes.InwardProcessing)
                IPRClearThroughCustoms(entities, _sgx); //Procedure 4051
              else
                throw new IPRDataConsistencyException
                  ("SADPZCProcessing.FreeCirculation", string.Format("Unexpected previous procedure code {1}for the {0} message", messageType, _sgx.Procedure.PreviousProcedure()), null, _wrongProcedure);
              break;
            case CustomsProcedureCodes.InwardProcessing:
              {
                if (messageType == CustomsDocument.DocumentType.SAD)
                {
                  comments = "Document added";
                  continue;
                }
                if (_sgx.Procedure.PreviousProcedure() == CustomsProcedureCodes.CustomsWarehousingProcedure)
                  // TODO Procedure 5171 
                  ;
                else if (_sgx.Procedure.PreviousProcedure() == CustomsProcedureCodes.NoProcedure)
                {
                  // Procedure 5100 
                  Clearence _newClearance = Clearence.CreataClearence(entities, "InwardProcessing", ClearenceProcedure._5171, _sgx);
                  CreateIPRAccount(entities, _newClearance, CustomsDocument.DocumentType.PZC, out comments, ProgressChange);
                }
                break;
              }
            case CustomsProcedureCodes.CustomsWarehousingProcedure:
              Clearence _newWarehousinClearance = Clearence.CreataClearence(entities, "CustomsWarehousingProcedure", ClearenceProcedure._7100, _sgx);
              if (messageType == CustomsDocument.DocumentType.PZC)
              {
                comments = "CW account creation error";
                CWAccountData _accountData = new CWAccountData(_newWarehousinClearance.Id.Value);
                _accountData.GetAccountData(entities, _newWarehousinClearance, ImportXMLCommon.Convert2MessageType(CustomsDocument.DocumentType.SAD), ProgressChange);
                _tasksList.Add(_accountData);
              }
              else
                comments = "Document added";
              break;
            case CustomsProcedureCodes.NoProcedure:
            case CustomsProcedureCodes.ReExport:
            default:
              throw new IPRDataConsistencyException("SADPZCProcessing.RequestedProcedure", string.Format("Unexpected procedure code for the {0} message", messageType), null, _wrongProcedure);
          }//switch ( _sgx.Procedure.RequestedProcedure() )
        }//foreach ( SADGood _sgx in sad.SADGood )
        entities.SubmitChanges();
      } //using ( Entities entities
      foreach (CWAccountData _accountData in _tasksList)
        CreateCWAccount(_accountData, webUrl, out comments);
    }
    /// <summary>
    /// Creates the IPR account.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="messageType">Type of the _message.</param>
    /// <param name="comments">The _comments.</param>
    /// <param name="ProgressChange">Represents the method that will handle an event.</param>
    private static void CreateIPRAccount(Entities entities, Clearence clearence, CustomsDocument.DocumentType messageType, out string comments, ProgressChangedEventHandler ProgressChange)
    {
      ProgressChange(null, new ProgressChangedEventArgs(1, "CreateIPRAccount.Starting"));
      comments = "IPR account creation error";
      string _referenceNumber = String.Empty;
      SADDocumentType declaration = clearence.Clearence2SadGoodID.SADDocumentIndex;
      _referenceNumber = declaration.ReferenceNumber;
      if (WebsiteModel.Linq.IPR.RecordExist(entities, clearence.DocumentNo))
      {
        string _msg = "IPR record with the same SAD document number: {0} exist";
        throw GenericStateMachineEngine.ActionResult.NotValidated(String.Format(_msg, clearence.DocumentNo));
      }
      ProgressChange(null, new ProgressChangedEventArgs(1, "CreateIPRAccount.newIPRData"));
      comments = "Inconsistent or incomplete data to create IPR account";
      IPRAccountData _iprdata = new IPRAccountData(clearence.Id.Value);
      _iprdata.GetAccountData(entities, clearence, ImportXMLCommon.Convert2MessageType(messageType), ProgressChange);
      comments = "Consent lookup filed";
      ProgressChange(null, new ProgressChangedEventArgs(1, "CreateIPRAccount.newIPRClass"));
      IPRClass _ipr = new IPRClass(entities, _iprdata, clearence, declaration);
      entities.IPR.InsertOnSubmit(_ipr);
      clearence.Status = true;
      ProgressChange(null, new ProgressChangedEventArgs(1, "CreateIPRAccount.SubmitChanges"));
      entities.SubmitChanges();
      _ipr.UpdateTitle();
      comments = "IPR account created";
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
    /// Clear through customs according 4071 procudure.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="good">The good.</param>
    /// <param name="comments">The comments.</param>
    /// <exception cref="InputDataValidationException">Create CW Account Failed;CreateCWAccount</exception>
    private static void CWClearThroughCustoms(Entities entities, SADGood good, out string comments)
    {
      List<Warnning> _lw = new List<Warnning>();
      Clearence _clearance = GetClearanceId(entities, good, Settings.GetParameter(entities, SettingsEntry.RequiredDocumentSADTemplateDocumentNamePattern));
      CWClearanceData _ClearanceData = new CWClearanceData(_clearance.Id.Value);
      _ClearanceData.CallService(entities.Web, _lw);
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
    /// <param name="good">The good.</param>
    /// <exception cref="InputDataValidationException">SAD Required Documents;clear through castoms fatal error;true</exception>
    private static void IPRClearThroughCustoms(Entities entities, SADGood good)
    {
      Clearence _clearance = GetClearanceId(entities, good, Settings.GetParameter(entities, SettingsEntry.RequiredDocumentFinishedGoodExportConsignmentPattern));
      _clearance.FinishClearingThroughCustoms(entities, good);
    }
    private static Clearence GetClearanceId(Entities entities, SADGood good, string pattern)
    {
      int? _cleranceInt = new Nullable<int>();
      foreach (SADRequiredDocuments _rdx in good.SADRequiredDocuments)
      {
        if (_rdx.Code != XMLResources.RequiredDocumentConsignmentCode)
          continue;
        _cleranceInt = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber(_rdx.Number, pattern);
        if (_cleranceInt.HasValue)
          break;
      }// foreach 
      Clearence _clearance = null;
      if (_cleranceInt.HasValue)
        _clearance = Element.GetAtIndex<Clearence>(entities.Clearence, _cleranceInt.Value);
      else
      {
        string _template = "Cannot find required document code={0} for customs document = {1}/ref={2}";
        throw new InputDataValidationException(
          String.Format(_template, XMLResources.RequiredDocumentConsignmentCode, good.SADDocumentIndex.DocumentNumber, good.SADDocumentIndex.ReferenceNumber),
          "SAD Required Documents",
          "clear through castoms fatal error", true);
      }
      return _clearance;
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
    private const string _wrongProcedure = "Wrong customs procedure";
    #endregion

  }
}
