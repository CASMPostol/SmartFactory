﻿using System;
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
    public void FinishClearingThroughCustoms( Entities entities )
    {
      SADDocumentType sadDocument = Clearence2SadGoodID.SADDocumentIndex;
      DocumentNo = sadDocument.DocumentNumber;
      ReferenceNumber = sadDocument.ReferenceNumber;
      Status = true;
      foreach ( Disposal _disposal in Disposal )
        _disposal.FinishClearingThroughCustoms( entities, Clearence2SadGoodID );
      UpdateTitle( entities );
    }
    /// <summary>
    /// Clears through customs.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="sadGood">The sad good.</param>
    public void FinishClearingThroughCustoms( Entities entities, SADGood sadGood )
    {
      Clearence2SadGoodID = sadGood;
      FinishClearingThroughCustoms( entities );
    }
    /// <summary>
    /// Gets the customs debt date.
    /// </summary>
    /// <value>
    /// The customs debt date.
    /// </value>
    public DateTime CustomsDebtDate { get { return Clearence2SadGoodID == null ? Extensions.DateTimeNull : Clearence2SadGoodID.SADDocumentIndex.CustomsDebtDate.Value; } }
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
      _newClearence.UpdateTitle( entities );
      entities.SubmitChanges();
      _newClearence.UpdateTitle( entities );
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
      _newClearence.UpdateTitle( entities );
      entities.Clearence.InsertOnSubmit( _newClearence );
      entities.SubmitChanges();
      _newClearence.UpdateTitle( entities );
      return _newClearence;
    }
    /// <summary>
    /// Clears the through custom.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="sadConsignment">The _sad consignment.</param>
    public void ClearThroughCustom( Entities entities, SADConsignment sadConsignment )
    {
      SADConsignmentLibraryIndex = sadConsignment;
      foreach ( Disposal _dspsl in Disposal )
        _dspsl.ClearThroughCustom( this.ClearenceProcedure.Value );
      UpdateTitle( entities );
    }
    /// <summary>
    /// Updates the clerance.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedureDescription">The procedure description.</param>
    /// <param name="clearenceProcedure">The clearence procedure.</param>
    public void UpdateClerance( Entities entities, string procedureDescription, ClearenceProcedure clearenceProcedure )
    {
      ProcedureCode = procedureDescription;
      ClearenceProcedure = clearenceProcedure;
      UpdateTitle( entities );
    }
    public void UpdateTitle( Entities entities )
    {
      string _quantity = String.Empty;
      if ( this.Disposal.Any() )
        _quantity = this.Disposal.Sum<Disposal>( x => x.SettledQuantity.Value ).ToString( "F2" );
      else
        _quantity = " --- ";
      string _ClearanceTitleFormat = Settings.GetParameter( entities, SettingsEntry.ClearanceTitleFormat );
      Title = String.Format( _ClearanceTitleFormat, this.ProcedureCode, Entities.ToString( ClearenceProcedure.GetValueOrDefault( Linq.ClearenceProcedure.Invalid ) ),
                             ReferenceNumber.NotAvailable(), _quantity, Id.GetValueOrDefault( -999 ) );
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
    #endregion

  }
}
