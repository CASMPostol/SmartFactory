//<summary>
//  Title   : Activate Rel. 1.80 fetures.
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

using System;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation.Activate180
{
  /// <summary>
  /// Activate helper functions
  /// </summary>
  internal static class Activate
  {
    internal static void UpdateDisposals(Entities entities, Action<object, EntitiesChangedEventArgs> progress)
    {
      progress(null, new EntitiesChangedEventArgs(1, "Starting Activate.UpdateDisposals", entities));
      foreach (Disposal _dspx in entities.Disposal)
      {
        _dspx.Archival = false;
        if (_dspx.JSOXCustomsSummaryIndex != null)
          _dspx.JSOXReportID = _dspx.JSOXCustomsSummaryIndex.JSOXCustomsSummary2JSOXIndex.Id.Value;
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
    }
    internal static void IPRRecalculateClearedRecords(Entities entities, Action<object, EntitiesChangedEventArgs> progress)
    {
      progress(null, new EntitiesChangedEventArgs(1, "Starting Activate.IPRRecalculateClearedRecords", entities));
      foreach (IPR.WebsiteModel.Linq.IPR _iprX in entities.IPR)
      {
        _iprX.Archival = false;
        _iprX.RecalculateClearedRecords(entities, progress);
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
    }
    internal static void ResetArchival(Entities entities, Action<object, EntitiesChangedEventArgs> progress)
    {
      progress(null, new EntitiesChangedEventArgs(1, "Starting Activate.ResetArchival", entities));
      foreach (Batch _batchItem in entities.Batch)
      {
        _batchItem.Archival = false;
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
      progress(null, new EntitiesChangedEventArgs(1, "Material archive resetiny", entities));
      foreach (Material _materialItem in entities.Material)
      {
        _materialItem.Archival = false;
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }

    }
  }
}
