//<summary>
//  Title   : class Clearence
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using CAS.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// Clearance
  /// </summary>
  public partial class Clearence
  {
    #region public
    /// <summary>
    /// Gets the clearance.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="referenceNumber">The reference number.</param>
    /// <returns></returns>
    public static IQueryable<Clearence> GetClearence(Entities entities, string referenceNumber)
    {
      return from _cx in entities.Clearence where referenceNumber.Contains(_cx.ReferenceNumber) select _cx;
    }
    /// <summary>
    /// Clears through customs.
    /// </summary>
    /// <param name="entities">The entities.</param>
    public void FinishClearingThroughCustoms(Entities entities)
    {
      SADDocumentType sadDocument = Clearence2SadGoodID.SADDocumentIndex;
      DocumentNo = sadDocument.DocumentNumber;
      ReferenceNumber = sadDocument.ReferenceNumber;
      SPStatus = true;
      foreach (Disposal _disposal in this.Disposal(entities))
        _disposal.FinishClearingThroughCustoms(entities, Clearence2SadGoodID);
      UpdateTitle(entities);
    }
    /// <summary>
    /// Clears through customs.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="sadGood">The sad good.</param>
    public void FinishClearingThroughCustoms(Entities entities, SADGood sadGood)
    {
      Clearence2SadGoodID = sadGood;
      FinishClearingThroughCustoms(entities);
    }
    /// <summary>
    /// Finishes the clearing through customs procedure.
    /// </summary>
    /// <param name="good">The good.</param>
    public void FinishClearingThroughCustoms(SADGood good)
    {
      Clearence2SadGoodID = good;
      SADDocumentType sadDocument = good.SADDocumentIndex;
      DocumentNo = sadDocument.DocumentNumber;
      ReferenceNumber = sadDocument.ReferenceNumber;
      SPStatus = true;
    }
    /// <summary>
    /// Gets the customs debt date.
    /// </summary>
    /// <value>
    /// The customs debt date.
    /// </value>
    public DateTime CustomsDebtDate { get { return Clearence2SadGoodID == null ? Extensions.SPMinimum : Clearence2SadGoodID.SADDocumentIndex.CustomsDebtDate.Value; } }
    /// <summary>
    /// Starts the clearance and creates an object of <see cref="Clearence"/>.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="procedureCode">The procedure code.</param>
    /// <returns></returns>
    public static Clearence CreataClearence(Entities entities, string procedure, ClearenceProcedure procedureCode)
    {
      Clearence _newClearence = CreateClearance(procedure, procedureCode);
      entities.Clearence.InsertOnSubmit(_newClearence);
      _newClearence.UpdateTitle(entities);
      entities.SubmitChanges();
      _newClearence.UpdateTitle(entities);
      return _newClearence;
    }
    /// <summary>
    /// Creates the clearance.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="procedureCode">The procedure code.</param>
    /// <param name="good">The good.</param>
    /// <returns></returns>
    public static Clearence CreataClearence(Entities entities, string procedure, ClearenceProcedure procedureCode, SADGood good)
    {
      Clearence _newClearence = CreateClearance(procedure, procedureCode);
      _newClearence.Clearence2SadGoodID = good;
      _newClearence.DocumentNo = good.SADDocumentIndex.DocumentNumber;
      _newClearence.ReferenceNumber = good.SADDocumentIndex.ReferenceNumber;
      _newClearence.UpdateTitle(entities);
      entities.Clearence.InsertOnSubmit(_newClearence);
      entities.SubmitChanges();
      _newClearence.UpdateTitle(entities);
      return _newClearence;
    }
    /// <summary>
    /// Clears the through custom.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="sadConsignment">The _sad consignment.</param>
    public void ClearThroughCustom(Entities entities, SADConsignment sadConsignment)
    {
      SADConsignmentLibraryIndex = sadConsignment;
      foreach (Disposal _dspsl in this.Disposal(entities))
        _dspsl.ClearThroughCustom(entities, this.ClearenceProcedure.Value, this.SADDocumentNumber, _disposal => _disposal.Disposal2IPRIndex.RecalculateLastStarted(entities, _disposal));
      UpdateTitle(entities);
    }
    /// <summary>
    /// Updates the clearance.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedureDescription">The procedure description.</param>
    /// <param name="clearenceProcedure">The clearance procedure.</param>
    public void UpdateClerance(Entities entities, string procedureDescription, ClearenceProcedure clearenceProcedure)
    {
      ProcedureCode = procedureDescription;
      ClearenceProcedure = clearenceProcedure;
      UpdateTitle(entities);
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    /// <param name="entities">The auto-generated <see cref="Microsoft.SharePoint.Linq.DataContext"/> object.</param>
    public void UpdateTitle(Entities entities)
    {
      string _quantity = String.Empty;
      IEnumerable<Disposal> _dspsls = this.Disposal(entities);
      if (_dspsls.Count() > 0)
        _quantity = _dspsls.Sum<Disposal>(x => x.SettledQuantity.Value).ToString("F2");
      else
        _quantity = " --- ";
      string _ClearanceTitleFormat = Settings.GetParameter(entities, SettingsEntry.ClearanceTitleFormat);
      Title = String.Format(_ClearanceTitleFormat, this.ProcedureCode, Entities.ToString(ClearenceProcedure.GetValueOrDefault(Linq.ClearenceProcedure.Invalid)),
                             ReferenceNumber.NotAvailable(), _quantity, Id.GetValueOrDefault(-999));
    }
    /// <summary>
    /// gets the name of finished good export form file.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns></returns>
    public string FinishedGoodsExportFormFileName(Entities entities)
    {
      return Settings.FinishedGoodsExportFormFileName(entities, this.Id.Value);
    }
    /// <summary>
    /// Gets the sad document number.
    /// </summary>
    /// <value>
    /// The sad document number.
    /// </value>
    public int SADDocumentNumber { get { return this.Id.Value; } }
    /// <summary>
    /// Reverse lookup for disposals.
    /// </summary>
    /// <param name="entities">The <see cref="Entities"/> instance.</param>
    /// <returns></returns>
    public IEnumerable<Disposal> Disposal(Entities entities)
    {
      if (m_Disposal == null)
        m_Disposal = from _dspslx in entities.Disposal let _id = _dspslx.Disposal2ClearenceIndex.Id.Value where this.Id.Value == _id select _dspslx;
      return m_Disposal;
    }
    #endregion

    #region private
    IEnumerable<Disposal> m_Disposal = null;
    private static Clearence CreateClearance(string code, ClearenceProcedure procedure)
    {
      Clearence _newClearence = new Clearence()
      {
        DocumentNo = String.Empty.NotAvailable(),
        ProcedureCode = code,
        ReferenceNumber = String.Empty.NotAvailable(),
        SPStatus = false,
        ClearenceProcedure = procedure
      };
      return _newClearence;
    }
    #endregion

  }
}
