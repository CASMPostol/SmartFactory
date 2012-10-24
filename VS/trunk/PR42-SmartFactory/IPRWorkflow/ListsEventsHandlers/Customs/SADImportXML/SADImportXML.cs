using System;
using System.Collections.Generic;
using System.Diagnostics;
using CAS.SharePoint;
using CAS.SmartFactory.Linq.IPR;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;
using Microsoft.SharePoint;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.IPR.Customs
{
  /// <summary>
  /// List Item Events
  /// </summary>
  public class SADImportXML: SPItemEventReceiver
  {
    /// <summary>
    /// An item is being added
    /// </summary>
    public override void ItemAdding( SPItemEventProperties properties )
    {
      base.ItemAdding( properties );
    }
    /// <summary>
    /// An item was added
    /// </summary>
    /// <param name="properties"> Contains properties for asynchronous list item event handlers, and serves as a base class for 
    /// event handlers.
    /// </param>
    public override void ItemAdded( SPItemEventProperties properties )
    {
      if ( properties.List.Title.Contains( CommonDefinition.SADDocumentLibrary ) )
      {
        string _at = "beginning";
        try
        {
          this.EventFiringEnabled = false;
          using ( Entities edc = new Entities( properties.WebUrl ) )
          {
            if ( properties.ListItem.File == null )
            {
              base.ItemAdded( properties );
              return;
              //TODO  [pr4-3435] Item add event - selective handling mechanism. http://itrserver/Bugs/BugDetail.aspx?bid=3435
              //throw new IPRDataConsistencyException("ItemAdded", "Import of a SAD declaration message failed because the file is empty.", null, "There is no file");
            }
            Anons mess = new Anons()
            {
              Title = m_Title,
              Treść = String.Format( "Import of the SAD declaration {0} starting.", properties.ListItem.File.ToString() )
            };
            edc.ActivityLog.InsertOnSubmit( mess );
            edc.SubmitChanges();
            _at = "GetAtIndex";
            SADDocumentLib entry = Element.GetAtIndex<SADDocumentLib>( edc.SADDocumentLibrary, properties.ListItem.ID );
            entry.SADDocumentLibraryOK = false;
            entry.SADDocumentLibraryComments = "Item adding error";
            _at = "ImportDocument";
            edc.SubmitChanges();
            CustomsDocument _message = CustomsDocument.ImportDocument( properties.ListItem.File.OpenBinaryStream() );
            _at = "GetSADDocument";
            SADDocumentType _sad = GetSADDocument( _message, edc, entry );
            _at = "SubmitChanges #1";
            edc.SubmitChanges();
            _at = "Clearence.Associate";
            string _comments = String.Empty;
            Clearence _clrnc = null;
            try
            {
              _clrnc = ClearenceExtension.Associate( edc, _message.MessageRootName(), _sad, out _comments, entry );
            }
            catch ( Exception ex )
            {
              throw ex;
            }
            finally
            {
              entry.SADDocumentLibraryComments = _comments;
              edc.SubmitChanges();
            }
            _sad.SADDocument2Clearence = _clrnc;
            entry.SADDocumentLibraryOK = true;
            entry.SADDocumentLibraryComments = _comments;
            _at = "SubmitChanges #2";
            edc.SubmitChanges();
          }
        }
        catch ( Exception ex )
        {
          SPWeb web = properties.Web; //TODO [pr4-3412] SPWeb and SPSite correct use; http://itrserver/Bugs/BugDetail.aspx?bid=3412
          SPList log = web.Lists.TryGetList( "Activity Log" );
          if ( log == null )
          {
            EventLog.WriteEntry( "CAS.SmartFActory", "Cannot open \"Activity Log\" list", EventLogEntryType.Error, 114 );
            return;
          }
          SPListItem item = log.AddItem();
          string _pattern = "XML import error at {0}.";
          if ( ex is CustomsDataException )
          {
            _pattern = "XML import error at {0}.";
            _at = ( (CustomsDataException)ex ).Source;
          }
          else if ( ex is IPRDataConsistencyException )
          {
            IPRDataConsistencyException _iprex = ex as IPRDataConsistencyException;
            _pattern = "SAD analyses error at {0}.";
            _at = _iprex.Source;
          }
          else if ( ex is GenericStateMachineEngine.ActionResult )
          {
            GenericStateMachineEngine.ActionResult _ar = ex as GenericStateMachineEngine.ActionResult;
            if ( _ar.LastActionResult == GenericStateMachineEngine.ActionResult.Result.NotValidated )
              _pattern = "SAD content validation error at {0}.";
            else
              _pattern = "SAD analyses internal error at {0}.";
            _at = _ar.Source;
          }
          else
          {
            _pattern = "ItemAdded error at {0}.";
          }
          item[ "Title" ] = String.Format( _pattern, _at );
          string _innerMsg = String.Empty;
          if ( ex.InnerException != null )
            _innerMsg = String.Format( " as the result of {0}.", ex.InnerException.Message );
          item[ "Body" ] = String.Format( "Message= {0}{1}", ex.Message, _innerMsg );
          item.UpdateOverwriteVersion();
        }
        finally
        {
          this.EventFiringEnabled = true;
        }
      }
      base.ItemAdded( properties );
    }
    private static SADDocumentType GetSADDocument( CustomsDocument document, Entities edc, SADDocumentLib lookup )
    {
      try
      {
        SADDocumentType newRow = new SADDocumentType()
          {
            SADDocumenLibrarytIndex = lookup,
            Title = String.Format( "{0}: {1} / {2}", document.MessageRootName(), document.GetDocumentNumber(), document.GetReferenceNumber() ),
            Currency = document.GetCurrency(),
            CustomsDebtDate = document.GetCustomsDebtDate(),
            DocumentNumber = document.GetDocumentNumber(),
            ExchangeRate = document.GetExchangeRate(),
            GrossMass = document.GetGrossMass(),
            ReferenceNumber = document.GetReferenceNumber()
          };
        edc.SADDocument.InsertOnSubmit( newRow );
        GetSADGood( document.GetSADGood(), edc, newRow );
        return newRow;
      }
      catch ( IPRDataConsistencyException _ex )
      {
        throw _ex;
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( "SADDocumentType", ex.Message, ex, "SAD main part analysis problem" );
      }
    }
    private static void GetSADGood( GoodDescription[] document, Entities edc, SADDocumentType lookup )
    {
      if ( document.NullOrEmpty<GoodDescription>() )
        return;
      try
      {
        foreach ( GoodDescription _doc in document )
        {
          string _description = _doc.GetDescription().SPValidSubstring();
          SADGood newRow = new SADGood()
          {
            SADDocumentIndex = lookup,
            Title = String.Format( "{0}: {1}", _doc.GetDescription().SPValidSubstring(), _doc.GetPCNTariffCode() ),
            GoodsDescription = _description,
            PCNTariffCode = _doc.GetPCNTariffCode(),
            GrossMass = _doc.GetGrossMass(),
            Procedure = _doc.GetProcedure(),
            TotalAmountInvoiced = _doc.GetTotalAmountInvoiced(),
            ItemNo = _doc.GetItemNo()
          };
          edc.SADGood.InsertOnSubmit( newRow );
          edc.SubmitChanges();
          GetSADDuties( _doc.GetSADDuties(), edc, newRow );
          GetSADPackage( _doc.GetSADPackage(), edc, newRow );
          GetSADQuantity( _doc.GetSADQuantity(), edc, newRow );
          GetSADRequiredDocuments( _doc.GetSADRequiredDocuments(), edc, newRow );
        }
      }
      catch ( IPRDataConsistencyException _ex )
      {
        throw _ex;
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( "GetSADGood", ex.Message, ex, "Goods analysis problem" );
      }
    }
    private static void GetSADDuties( DutiesDescription[] document, Entities edc, SADGood lookup )
    {
      if ( document.NullOrEmpty<DutiesDescription>() )
        return;
      List<SADDuties> rows = new List<SADDuties>();
      try
      {
        foreach ( DutiesDescription duty in document )
        {
          SADDuties newRow = new SADDuties()
          {
            SADDuties2SADGoodID = lookup,
            Title = String.Format( "{0}: {1}", duty.GetDutyType(), duty.GetAmount() ),
            Amount = duty.GetAmount(),
            DutyType = duty.GetDutyType()
          };
          rows.Add( newRow );
        }
        if ( rows.Count == 0 )
          return;
        edc.SADDuties.InsertAllOnSubmit( rows );
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( "GetSADDuties", ex.Message, ex, "Duties analysis problem" );
      }
    }
    private static void GetSADPackage( PackageDescription[] document, Entities edc, SADGood entry )
    {
      try
      {
        if ( document.NullOrEmpty<PackageDescription>() )
          return;
        List<SADPackage> rows = new List<SADPackage>();
        foreach ( PackageDescription package in document )
        {
          SADPackage newRow = new SADPackage()
          {
            SADPackage2SADGoodID = entry,
            Title = String.Format( "{0}: {1}", package.GetItemNo(), package.GetPackage() ),
            ItemNo = package.GetItemNo(),
            Package = package.GetPackage()
          };
          rows.Add( newRow );
        }
        if ( rows.Count == 0 )
          return;
        edc.SADPackage.InsertAllOnSubmit( rows );
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( "GetSADPackage", ex.Message, ex, "Packages analysis problem" );
      }
    }
    private static void GetSADQuantity( QuantityDescription[] document, Entities edc, SADGood entry )
    {
      if ( document.NullOrEmpty<QuantityDescription>() )
        return;
      List<SADQuantity> rows = new List<SADQuantity>();
      try
      {
        foreach ( QuantityDescription quantity in document )
        {
          SADQuantity newRow = new SADQuantity()
          {
            SADQuantity2SADGoodID = entry,
            Title = String.Format( "{0}: {1}", quantity.GetNetMass(), quantity.GetUnits() ),
            ItemNo = quantity.GetItemNo(),
            NetMass = quantity.GetNetMass(),
            Units = quantity.GetUnits()
          };
          rows.Add( newRow );
        }
        if ( rows.Count == 0 )
          return;
        edc.SADQuantity.InsertAllOnSubmit( rows );
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( "GetSADQuantity", ex.Message, ex, "Quantity analysis problem" );
      }
    }
    private static void GetSADRequiredDocuments( RequiredDocumentsDescription[] document, Entities edc, SADGood entry )
    {
      try
      {
        if ( document.NullOrEmpty<RequiredDocumentsDescription>() )
          return;
        List<SADRequiredDocuments> rows = new List<SADRequiredDocuments>();
        foreach ( RequiredDocumentsDescription requiredDocument in document )
        {
          SADRequiredDocuments newRow = new SADRequiredDocuments()
          {
            SADRequiredDoc2SADGoodID = entry,
            Title = String.Format( "{0}: {1}", requiredDocument.GetCode(), requiredDocument.GetNumber() ),
            Code = requiredDocument.GetCode(),
            Number = requiredDocument.GetNumber()
          };
          rows.Add( newRow );
        }
        if ( rows.Count == 0 )
          return;
        edc.SADRequiredDocuments.InsertAllOnSubmit( rows );
        edc.SubmitChanges();
      }
      catch ( Exception ex )
      {
        throw new IPRDataConsistencyException( "GetSADRequiredDocuments", ex.Message, ex, "Required documents analysis problem" );
      }
    }
    private const string m_Title = "SAD Document Import";
  }
}
