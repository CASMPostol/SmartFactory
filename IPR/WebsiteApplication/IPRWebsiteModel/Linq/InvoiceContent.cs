﻿//<summary>
//  Title   : InvoiceContent
//  System  : Microsoft VisulaStudio 2013 / C#
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

using CAS.SharePoint.Logging;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq
{
  /// <summary>
  /// InvoiceContent entity
  /// </summary>
  public partial class InvoiceContent
  {
    /// <summary>
    /// Checks if export is possible.
    /// </summary>
    /// <param name="edc">The edc.</param>
    /// <param name="quantity">The quantity to export.</param>
    /// <returns>
    /// Not empty string if there is a warning.
    /// </returns>
    public string ExportIsPossible(Entities edc, double? quantity)
    {
      if (!quantity.HasValue)
        return "Valid quantity value must be provided";
      Batch _batch = Batch.FindLookup(edc, InvoiceContent2BatchIndex.Batch0);
      double _availableQuantity = _batch.FGQuantityAvailable.Value;
      if (_availableQuantity < quantity.Value)
        return String.Format(m_quantityIsUnavailable, _availableQuantity);
      return String.Empty;
    }
    internal void UpdateExportedDisposals(Entities edc, NamedTraceLogger.TraceAction trace)
    {
      trace("Entering InvoiceContent.UpdateExportedDisposals", 48, TraceSeverity.Verbose);
      IEnumerable<IGrouping<int, Disposal>> _dspslsGroups = from _dsx in this.Disposal(edc)
                                                            let _midx = _dsx.Disposal2MaterialIndex.Id.Value
                                                            group _dsx by _midx;
      foreach (IGrouping<int, Disposal> _gx in _dspslsGroups)
      {
        Disposal _dsp = _gx.FirstOrDefault<Disposal>();
        if (_dsp == null)
          continue;
        Material _mtrl = _dsp.Disposal2MaterialIndex;
        decimal _2Add = _mtrl.CalculatedQuantity(this) - _gx.Sum<Disposal>(v => v.SettledQuantityDec);
        //TODO it could cause that the closed IPR accounts will have account balance != 0 
        //TODO if current settled quantity < previous value it could also cause that old account will have holes that makes them difficult to be closed.
        IEnumerable<Disposal> _sorted = from _dx in _gx
                                        orderby _dx.SettledQuantityDec ascending
                                        select _dx;
        foreach (Disposal _dx in _sorted)
        {
          _dx.Adjust(edc, ref _2Add, trace);
          if (_2Add <= 0)
            break;
        }
        if (_2Add <= 0)
          continue;
        _mtrl.AddNewDisposals(edc, DisposalEnum.TobaccoInCigaretess, ref _2Add, this, trace);
      }
    }

    //private
    private IEnumerable<Disposal> Disposal(Entities edc)
    {
      if (!this.Id.HasValue)
        return null;
      if (m_Disposal == null)
        m_Disposal = from _dspslx in edc.Disposal let _id = _dspslx.Disposal2InvoiceContentIndex.Id.Value where this.Id.Value == _id select _dspslx;
      return m_Disposal;
    }
    private IEnumerable<Disposal> m_Disposal = null;
    private const string m_quantityIsUnavailable = "The requested quantity is unavailable. There is only {0} on the stock.";

  }
}
