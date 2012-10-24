using System;
using System.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.Linq.IPR
{
  public static class SADDocumentTypeExtension
  {

    #region public
    /// <summary>
    /// Res the export of goods.
    /// </summary>
    /// <param name="_edc">The _edc.</param>
    /// <exception cref="GenericStateMachineEngine.ActionResult"> if operation cannot be complited.</exception>
    /// <param name="_messageType">Type of the _message.</param>
    internal static void ReExportOfGoods( this SADDocumentType _this, Entities _edc, xml.Customs.CustomsDocument.DocumentType _messageType )
    {
      _this.SADDocument2Clearence = FimdClearence( _edc, _this );
      //TODO Define and use the reverse lookup field. 
      foreach ( var _disposal in from _dspx in _edc.Disposal where _dspx.Disposal2ClearenceIndex.Identyfikator == _this.SADDocument2Clearence.Identyfikator select _dspx )
        //TODO not sure about this.CustomsDebtDate.Value, but it is the ony one date. 
        _disposal.Export( _edc, _this.DocumentNumber, _this.SADDocument2Clearence, _this.CustomsDebtDate.Value );
      _this.SADDocument2Clearence.DocumentNo = _this.DocumentNumber;
      _this.SADDocument2Clearence.ReferenceNumber = _this.ReferenceNumber;
      _this.SADDocument2Clearence.Status = true;
      _this.SADDocument2Clearence.CreateTitle( _messageType.ToString() );
    }
    #endregion
    #region private
    private static Clearence FimdClearence( Entities _edc, SADDocumentType _this )
    {
      Clearence _clearance = null;
      foreach ( SADGood _sg in _this.SADGood )
      {
        if ( _sg.Procedure.RequestedProcedure() != CustomsProcedureCodes.ReExport )
          throw new IPRDataConsistencyException( "Clearence.Create", String.Format( "IE529 contains invalid customs procedure {0}", _sg.Title ), null, "Wrong customs procedure." );
        //TODO [pr4-3707] Export: Association of the SAD documents - SAD handling procedure modification http://itrserver/Bugs/BugDetail.aspx?bid=3707
        foreach ( SADRequiredDocuments _rdx in _sg.SADRequiredDocuments )
        {
          if ( _rdx.Code != XMLResources.RequiredDocumentFinishedGoodExportConsignmentCode )
          {
            int? _cleranceInt = XMLResources.GetRequiredDocumentFinishedGoodExportConsignmentNumber( _rdx.Number );
            if ( _cleranceInt.HasValue )
            {
              _clearance = Element.GetAtIndex<Clearence>( _edc.Clearence, _cleranceInt.Value );
              break;
            }
          }
        } // foreach ( SADRequiredDocuments _rdx in _sg.SADRequiredDocuments )
      }//foreach ( SADGood _sg in SADGood )
      if ( _clearance != null )
        return _clearance;
      string _template = "Cannot find required document code ={0} for customs document = {1}/ref={2}";
      throw GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _template, _this.DocumentNumber, _this.ReferenceNumber ) );
    } //private Clearence FimdClearence(

    #endregion
  }
}
