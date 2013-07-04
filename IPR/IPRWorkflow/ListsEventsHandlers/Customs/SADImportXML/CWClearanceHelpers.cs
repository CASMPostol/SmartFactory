using System;
using System.Collections.Generic;
using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR.WebsiteModel;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using CAS.SmartFactory.IPR.WebsiteModel.Linq.Account;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.IPR.ListsEventsHandlers.Customs.SADImportXML
{
  /// <summary>
  /// CWClearanceHelpers implementation of the <see cref="ICWClearanceHelpers"/> interface
  /// </summary>
  public class CWClearanceHelpers: ICWClearanceHelpers
  {
    private CWClearanceHelpers()
    {

    }
    //public CWClearanceHelpers 
    /// <summary>
    /// Creates the CW account.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="clearence">The clearence.</param>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="comments">The comments.</param>
    /// <param name="warnings">The warnings.</param>
    /// <exception cref="IPRDataConsistencyException">IPR account creation error</exception>
    public void CreateCWAccount( Entities entities, Clearence clearence, CustomsDocument.DocumentType messageType, out string comments, List<InputDataValidationException> warnings )
    {
      string _at = "started";
      comments = "IPR account creation error";
      string _referenceNumber = String.Empty;
      try
      {
        SADDocumentType declaration = clearence.Clearence2SadGoodID.SADDocumentIndex;
        _referenceNumber = declaration.ReferenceNumber;
        if ( WebsiteModel.Linq.IPR.RecordExist( entities, clearence.DocumentNo ) )
        {
          string _msg = "IPR record with the same SAD document number: {0} exist";
          throw GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _msg, clearence.DocumentNo ) );
        }
        _at = "newIPRData";
        comments = "Inconsistent or incomplete data to create IPR account";
        CWAccountData _accountData = new CWAccountData( entities, clearence.Clearence2SadGoodID, ImportXMLCommon.Convert2MessageType( messageType ) );
        ErrorsList _ar = new ErrorsList();
        if ( !_accountData.Validate( _ar ) )
          warnings.Add( new InputDataValidationException( "Inconsistent or incomplete data to create IPR account", "Create IPR Account", _ar ) );
        comments = "Consent lookup filed";
        _at = "new IPRClass";
        CW _cw = new CW( entities, _accountData, clearence, declaration );
        _at = "new InsertOnSubmit";
        entities.CW.InsertOnSubmit( _cw );
        clearence.Status = true;
        _at = "new SubmitChanges #1";
        entities.SubmitChanges();
        _cw.UpdateTitle();
        _at = "new SubmitChanges #2";
        entities.SubmitChanges();
      }
      catch ( InputDataValidationException )
      {
        throw;
      }
      catch ( GenericStateMachineEngine.ActionResult _ex )
      {
        _ex.Message.Insert( 0, String.Format( "Message={0}, Reference={1}; ", messageType, _referenceNumber ) );
        throw;
      }
      catch ( Exception _ex )
      {
        string _src = String.Format( "CreateCWAccount method error at {0}", _at );
        throw new IPRDataConsistencyException( _src, _ex.Message, _ex, "IPR account creation error" );
      }
      comments = "IPR account created";
    }
    internal static ICWClearanceHelpers GetICWClearanceHelpers()
    {
      throw new NotImplementedException();
    }
  }
}
