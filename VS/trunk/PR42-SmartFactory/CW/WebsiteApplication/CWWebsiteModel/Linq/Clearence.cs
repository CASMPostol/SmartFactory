//<summary>
//  Title   : partial class Clearence
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

using CAS.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.CW.WebsiteModel.Linq
{
  /// <summary>
  /// partial class Clearence
  /// </summary>
  public partial class Clearence
  {

    /// <summary>
    /// Creates the clearance.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <param name="procedure">The procedure.</param>
    /// <param name="procedureCode">The procedure code.</param>
    /// <returns></returns>
    public static Clearence CreataClearence(Entities entities, string procedure, ClearenceProcedure procedureCode, WebsiteModelExtensions.TraceAction traceEvent)
    {
      Clearence _newClearence = CreateClearance(procedure, procedureCode);
      entities.Clearence.InsertOnSubmit(_newClearence);
      _newClearence.UpdateTitle(entities, traceEvent);
      entities.SubmitChanges();
      _newClearence.UpdateTitle(entities, traceEvent);
      return _newClearence;
    }
    /// <summary>
    /// Updates the title.
    /// </summary>
    /// <param name="entities">The auto-generated <see cref="Microsoft.SharePoint.Linq.DataContext"/> object.</param>
    public void UpdateTitle(Entities entities, WebsiteModelExtensions.TraceAction traceEvent)
    {
      string _quantity = String.Empty;
      //IQueryable<CustomsWarehouseDisposal> _Dspsls = from _Dspx in entities.CustomsWarehouseDisposal where _Dspx == this.Id select {ssss = _d.s }
      //if ( this.Disposal.Any() )
      //  _quantity = this.Disposal.Sum<Disposal>( x => x.SettledQuantity.Value ).ToString( "F2" );
      //else
      //  _quantity = " --- ";
      traceEvent("Starting Clearence.UpdateTitle", 57, TraceSeverity.Verbose);
      string _ClearanceTitleFormat = Settings.GetParameter(entities, SettingsEntry.ClearanceTitleFormatCW);
      Title = String.Format(_ClearanceTitleFormat,
                             this.ProcedureCode, //0
                             ClearenceProcedure.GetValueOrDefault(Linq.ClearenceProcedure.Invalid).Convert2String(), //1
                             ReferenceNumber.NotAvailable(), //2
                             Id.GetValueOrDefault(-999)); //3
      traceEvent("Finished Clearence.UpdateTitle; new Title: " + Title, 57, TraceSeverity.Verbose);
    }
    public string SADTemplateDocumentNameFileName(Entities entities)
    {
      return Settings.SADTemplateDocumentNameFileName(entities, this.Id.Value);
    }
    internal void FinishClearThroughCustoms(Entities entities, WebsiteModelExtensions.TraceAction traceEvent)
    {
      traceEvent("Starting Clearence.FinishClearThroughCustoms", 73, TraceSeverity.Verbose);
      SADDocumentType _sadDocument = Clearence2SadGoodID.SADDocumentIndex;
      DocumentNo = _sadDocument.DocumentNumber;
      ReferenceNumber = _sadDocument.ReferenceNumber;
      SPStatus = true;
      foreach (CustomsWarehouseDisposal _cwdx in this.CustomsWarehouseDisposal(entities, false))
        _cwdx.FinishClearThroughCustoms(entities, Clearence2SadGoodID, traceEvent);
      UpdateTitle(entities, traceEvent);
      traceEvent("Finished Clearence.FinishClearThroughCustoms", 73, TraceSeverity.Verbose);
    }

    #region private
    private static Clearence CreateClearance(string code, ClearenceProcedure procedure)
    {
      Clearence _newClearence = new Clearence()
      {
        Archival = false,
        DocumentNo = String.Empty.NotAvailable(),
        ProcedureCode = code,
        ReferenceNumber = String.Empty.NotAvailable(),
        SPStatus = false,
        ClearenceProcedure = procedure
      };
      return _newClearence;
    }
    private IEnumerable<CustomsWarehouseDisposal> m_CustomsWarehouseDisposal = null;
    private IEnumerable<CustomsWarehouseDisposal> CustomsWarehouseDisposal(Entities edc, bool emptyListIfNew)
    {
      if (!this.Id.HasValue)
        return emptyListIfNew ? new CustomsWarehouseDisposal[] { } : null;
      if (m_CustomsWarehouseDisposal == null)
        m_CustomsWarehouseDisposal = from _cwdx in edc.CustomsWarehouseDisposal let _id = _cwdx.CWL_CWDisposal2CustomsWarehouseID.Id.Value where _id == this.Id.Value select _cwdx;
      return m_CustomsWarehouseDisposal;
    }
    #endregion

  }
}
