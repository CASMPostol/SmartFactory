using System;
using System.Linq;
using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR;
using CAS.SmartFactory.xml;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.Linq.IPR
{
  public partial class SADDocumentType
  {

    #region public
    /// <summary>
    /// Res the export of goods.
    /// </summary>
    /// <param name="_edc">The _edc.</param>
    /// <exception cref="GenericStateMachineEngine.ActionResult"> if operation cannot be complited.</exception>
    /// <param name="_messageType">Type of the _message.</param>
    internal void ReExportOfGoods( Entities _edc, xml.Customs.CustomsDocument.DocumentType _messageType )
    {
      this.SADDocument2Clearence = FimdClearence( _edc );
      //TODO Define and use the reverse lookup field. 
      foreach ( var _disposal in from _dspx in _edc.Disposal where _dspx.Disposal2ClearenceIndex.Identyfikator == this.SADDocument2Clearence.Identyfikator select _dspx )
        //TODO not sure about this.CustomsDebtDate.Value, but it is the ony one date. 
        _disposal.Export( _edc, this.DocumentNumber, this.SADDocument2Clearence, this.CustomsDebtDate.Value );
      this.SADDocument2Clearence.DocumentNo = this.DocumentNumber;
      this.SADDocument2Clearence.ReferenceNumber = this.ReferenceNumber;
      this.SADDocument2Clearence.Status = true;
      this.SADDocument2Clearence.CreateTitle( _messageType.ToString() );
    }
    internal void ReleaseForFreeCirculation( Entities _edc, out string _comments )
    {
      //TODO NotImplementedException
      _comments = "NotImplementedException";
      throw new NotImplementedException() { Source = "ReleaseForFreeCirculation" };
    }
    #endregion

    #region private
    private Clearence FimdClearence( Entities _edc )
    {
      Clearence _clearance = null;
      foreach ( SADGood _sg in SADGood )
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
      throw GenericStateMachineEngine.ActionResult.NotValidated( String.Format( _template, this.DocumentNumber, this.ReferenceNumber ) );
    } //private Clearence FimdClearence(

    #endregion
  }
}
