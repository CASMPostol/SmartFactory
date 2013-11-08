using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Account;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Account;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;
using IPRClass = CAS.SmartFactory.IPR.WebsiteModel.Linq.IPR;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs.SADImportXML
{
  internal static class ClearenceHelpers
  {

    #region public
    internal static void DeclarationProcessing(string url, int _sad, CustomsDocument.DocumentType documentType, ref string comments, ProgressChangedEventHandler ProgressChange)
    {
      comments = "Clearance association error";
      switch (documentType)
      {
        case CustomsDocument.DocumentType.SAD:
        case CustomsDocument.DocumentType.PZC:
          SADPZCProcessing(url, documentType, _sad, ref comments, ProgressChange);
          break;
        case CustomsDocument.DocumentType.IE529:
          IE529Processing(url, _sad, ref comments);
          break;
        case CustomsDocument.DocumentType.CLNE:
          CLNEProcessing(url, _sad, ref comments, ProgressChange);
          break;
      }//switch (_documentType
    }
    #endregion

    #region private
    private static void IE529Processing(string WebUrl, int sadDocumentTypeId, ref string _comments)
    {
      _comments = "Reexport of goods failed";
      using (Entities _entities = new Entities(WebUrl))
      {
        SADDocumentType _sad = Element.GetAtIndex<SADDocumentType>(_entities.SADDocument, sadDocumentTypeId);
        foreach (SADGood _gdx in _sad.SADGood)
          ClearThroughCustoms(_entities, _gdx);
        _comments = "Reexport of goods";
        _entities.SubmitChanges();
      }
    }
    private static void CLNEProcessing(string WebUrl, int sadDocumentTypeId, ref string comments, ProgressChangedEventHandler ProgressChange)
    {
      List<CWAccountData> _tasksList = new List<CWAccountData>();
      using (Entities _entities = new Entities(WebUrl))
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
              CreateIPRAccount(_entities, _cx, CustomsDocument.DocumentType.SAD, out comments, ProgressChange);
              break;
            case CustomsProcedureCodes.CustomsWarehousingProcedure:
              comments = "CW account creation error";
              CWAccountData _accountData = new CWAccountData();
              _accountData.GetAccountData(_entities, _cx, ImportXMLCommon.Convert2MessageType(CustomsDocument.DocumentType.SAD), ProgressChange);
              CreateCWAccount(_accountData, WebUrl, out comments);
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
        CreateCWAccount(_accountData, WebUrl, out comments);
    }
    private static void SADPZCProcessing(string WebUrl, CustomsDocument.DocumentType messageType, int sadDocumentTypeId, ref string comments, ProgressChangedEventHandler ProgressChange)
    {
      List<CWAccountData> _tasksList = new List<CWAccountData>();
      using (Entities entities = new Entities(WebUrl))
      {
        SADDocumentType sad = Element.GetAtIndex<SADDocumentType>(entities.SADDocument, sadDocumentTypeId);
        foreach (SADGood _sgx in sad.SADGood)
        {
          switch (_sgx.Procedure.RequestedProcedure())
          {
            case CustomsProcedureCodes.FreeCirculation:
              if (messageType == CustomsDocument.DocumentType.PZC)
                ClearThroughCustoms(entities, _sgx);
              else
                comments = "Document added";
              break;
            case CustomsProcedureCodes.InwardProcessing:
              {
                Clearence _newClearance = Clearence.CreataClearence(entities, "InwardProcessing", ClearenceProcedure._5171, _sgx);
                if (messageType == CustomsDocument.DocumentType.PZC)
                  CreateIPRAccount(entities, _newClearance, CustomsDocument.DocumentType.PZC, out comments, ProgressChange);
                else
                  comments = "Document added";
                break;
              }
            case CustomsProcedureCodes.CustomsWarehousingProcedure:
              Clearence _newWarehousinClearance = Clearence.CreataClearence(entities, "CustomsWarehousingProcedure", ClearenceProcedure._7100, _sgx);
              if (messageType == CustomsDocument.DocumentType.PZC)
              {
                comments = "CW account creation error";
                CWAccountData _accountData = new CWAccountData();
                _accountData.GetAccountData(entities, _newWarehousinClearance, ImportXMLCommon.Convert2MessageType(CustomsDocument.DocumentType.SAD), ProgressChange);
                _tasksList.Add(_accountData);
              }
              else
                comments = "Document added";
              break;
            case CustomsProcedureCodes.NoProcedure:
            case CustomsProcedureCodes.ReExport:
            default:
              throw new IPRDataConsistencyException("Clearence.Associate", string.Format("Unexpected procedure code for the {0} message", messageType), null, _wrongProcedure);
          }//switch ( _sgx.Procedure.RequestedProcedure() )
        }//foreach ( SADGood _sgx in sad.SADGood )
        entities.SubmitChanges();
      } //using ( Entities entities
      foreach (CWAccountData _accountData in _tasksList)
        CreateCWAccount(_accountData, WebUrl, out comments);
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
      IPRAccountData _iprdata = new IPRAccountData();
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
    private static void ClearThroughCustoms(Entities entities, SADGood good)
    {
      bool _ifAny = false;
      foreach (SADRequiredDocuments _rdx in good.SADRequiredDocuments)
      {
        if (_rdx.Code != XMLResources.RequiredDocumentConsignmentCode)
          continue;
        int? _cleranceInt = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber(_rdx.Number, Settings.GetParameter(entities, SettingsEntry.RequiredDocumentFinishedGoodExportConsignmentPattern));
        if (!_cleranceInt.HasValue)
          continue;
        Clearence _clearance = Element.GetAtIndex<Clearence>(entities.Clearence, _cleranceInt.Value);
        _clearance.FinishClearingThroughCustoms(entities, good);
        _ifAny = true;
      }// foreach 
      if (!_ifAny)
      {
        string _template = "Cannot find required document code={0} for customs document = {1}/ref={2}";
        throw new InputDataValidationException(
          String.Format(_template, XMLResources.RequiredDocumentConsignmentCode, good.SADDocumentIndex.DocumentNumber, good.SADDocumentIndex.ReferenceNumber),
          "SAD Required Documents",
          "clear through castoms fatal error", true);
      }
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
