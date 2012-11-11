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
    #region public
    /// <summary>
    /// Gets the clearence.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="referenceNumber">The reference number.</param>
    /// <returns></returns>
    public static IQueryable<Clearence> GetClearence( Entities entities, string referenceNumber )
    {
      return from _cx in entities.Clearence where referenceNumber.Contains( _cx.ReferenceNumber ) select _cx;
    }
    /// <summary>
    /// Clears through customs.
    /// </summary>
    /// <param name="entities">The entities.</param>
    public void ClearThroughCustoms( Entities entities )
    {
      SADDocumentType sadDocument = Clearence2SadGoodID.SADDocumentIndex;
      DocumentNo = sadDocument.DocumentNumber;
      ReferenceNumber = sadDocument.ReferenceNumber;
      Status = true;
      foreach ( Disposal _disposal in Disposal )
        _disposal.ClearThroughCustoms( entities, sadDocument.DocumentNumber, this, sadDocument.CustomsDebtDate.Value, Clearence2SadGoodID.PCNTariffCode );
    }
    /// <summary>
    /// Clears through customs.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="sadGood">The sad good.</param>
    public void ClearThroughCustoms( Entities entities, SADGood sadGood )
    {
      Clearence2SadGoodID = sadGood;
      ClearThroughCustoms( entities );
    }
    /// <summary>
    /// Creatas the clearence.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="procedureCode">The procedure code.</param>
    /// <returns></returns>
    public static Clearence CreataClearence( Entities entities, string procedure, ClearenceProcedure procedureCode )
    {
      Clearence _newClearence = CreateClearance( procedure, procedureCode );
      entities.Clearence.InsertOnSubmit( _newClearence );
      entities.SubmitChanges();
      return _newClearence;
    }
    /// <summary>
    /// Creatas the clearence.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="procedureCode">The procedure code.</param>
    /// <param name="good">The good.</param>
    /// <returns></returns>
    public static Clearence CreataClearence( Entities entities, string procedure, ClearenceProcedure procedureCode, SADGood good )
    {
      Clearence _newClearence = CreateClearance( procedure, procedureCode );
      _newClearence.Clearence2SadGoodID = good;
      _newClearence.DocumentNo = good.SADDocumentIndex.DocumentNumber;
      _newClearence.ReferenceNumber = good.SADDocumentIndex.ReferenceNumber;
      entities.Clearence.InsertOnSubmit( _newClearence );
      entities.SubmitChanges();
      return _newClearence;
    }
    #endregion

    #region private
    private static Clearence CreateClearance( string code, ClearenceProcedure procedure )
    {
      Clearence _newClearence = new Clearence()
      {
        DocumentNo = String.Empty.NotAvailable(),
        ProcedureCode = code,
        ReferenceNumber = String.Empty.NotAvailable(),
        Status = false,
        ClearenceProcedure = procedure
      };
      return _newClearence;
    }
    protected override void OnPropertyChanged( string propertyName )
    {
      Title = String.Format( "Procedure {0}/{1} Ref: {1}", this.ProcedureCode, Entities.ToString( ClearenceProcedure.GetValueOrDefault( Linq.ClearenceProcedure.Invalid ) ), ReferenceNumber.NotAvailable() );
      base.OnPropertyChanged( propertyName );
    }
    #endregion

  }
}
