using System;
using System.Linq;
using CAS.SharePoint;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Clearence
  /// </summary>
  public partial class Clearence
  {
    /// <summary>
    /// Fimds the clearence.
    /// </summary>
    /// <param name="_edc">The _edc.</param>
    /// <param name="referenceNumber">The _reference number.</param>
    /// <returns>An object od <see cref=""/> that has <paramref name="referenceNumber"/></returns>
    //TODO rename to get
    public static IQueryable<Clearence> FimdClearence( Entities _edc, string referenceNumber )
    {
      return from _cx in _edc.Clearence where referenceNumber.Contains( _cx.ReferenceNumber ) select _cx;
    }
    /// <summary>
    /// Clears it through customs.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="messageType">Type of the message.</param>
    public void ClearThroughCustoms( Entities entities, string messageType )
    {
      SADDocumentType sadDocument = SADGoodID.SADDocumentIndex;
      DocumentNo = sadDocument.DocumentNumber;
      ReferenceNumber = sadDocument.ReferenceNumber;
      Status = true;
      CreateTitle( messageType );
      foreach ( Disposal _disposal in Disposal )
        _disposal.ClearThroughCustoms( entities, sadDocument.DocumentNumber, this, sadDocument.CustomsDebtDate.Value, SADGoodID.PCNTariffCode );
    }
    /// <summary>
    /// Clears the through customs.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="messageType">Type of the message.</param>
    /// <param name="sadGood">The sad good.</param>
    public void ClearThroughCustoms( Entities entities, string messageType, SADGood sadGood )
    {
      SADGoodID = sadGood;
      ClearThroughCustoms( entities, messageType );
    }
    /// <summary>
    /// Creatas the clearence.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="code">The code.</param>
    /// <param name="procedure">The procedure.</param>
    /// <returns></returns>
    public static Clearence CreataClearence( Entities edc, string code, ClearenceProcedure procedure )
    {
      Clearence _newClearence = new Clearence()
      {
        DocumentNo = String.Empty.NotAvailable(),
        ProcedureCode = code,
        ReferenceNumber = String.Empty.NotAvailable(),
        Status = false,
        Title = "Created",
        ClearenceProcedure = procedure
      };
      edc.Clearence.InsertOnSubmit( _newClearence );
      edc.SubmitChanges();
      return _newClearence;
    }
    /// <summary>
    /// Creates the title.
    /// </summary>
    /// <param name="messageType">Type of the message.</param>
    public void CreateTitle( string messageType )
    {
      //TODO common naming convention must be implemented.
      Title = String.Format( "{0} Ref: {1}", messageType.NotAvailable(), ReferenceNumber.NotAvailable() );
    }
  }
}
