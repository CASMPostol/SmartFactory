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
  public static class Activate
  {

    /// <summary>
    /// Goes the specified site URL.
    /// </summary>
    /// <param name="siteURL">The site URL.</param>
    /// <param name="progress">The progress.</param>
    public static void Go(string siteURL, Func<object, EntitiesChangedEventArgs, bool> progress)
    {
      using (Entities edc = new Entities(siteURL))
      {
        UpdateDisposals(edc, progress);
        IPRRecalculateClearedRecords(edc, progress);
        ResetArchival(edc, progress);
      }
    }

    #region private
    private static void SubmitChanges(Func<object, EntitiesChangedEventArgs, bool> progress, Entities edc)
    {
      progress(null, new EntitiesChangedEventArgs(1, "SubmitChanges", edc));
      edc.SubmitChanges();
    }
    private static void UpdateDisposals(Entities entities, Func<object, EntitiesChangedEventArgs, bool> progress)
    {
      progress(null, new EntitiesChangedEventArgs(1, "Starting Activate.UpdateDisposals", entities));
      foreach (Disposal _dspx in entities.Disposal)
      {
        _dspx.Archival = false;
        if (_dspx.JSOXCustomsSummaryIndex != null)
          _dspx.JSOXReportID = _dspx.JSOXCustomsSummaryIndex.JSOXCustomsSummary2JSOXIndex.Id.Value;
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
      SubmitChanges(progress, entities);
      progress(null, new EntitiesChangedEventArgs(1, "Finished Activate.UpdateDisposals", entities));
    }
    private static void IPRRecalculateClearedRecords(Entities entities, Func<object, EntitiesChangedEventArgs, bool> progress)
    {
      progress(null, new EntitiesChangedEventArgs(1, "Starting Activate.IPRRecalculateClearedRecords", entities));
      foreach (IPR.WebsiteModel.Linq.IPR _iprX in entities.IPR)
      {
        _iprX.Archival = false;
        _iprX.RecalculateClearedRecords(entities, progress);
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
      SubmitChanges(progress, entities);
      progress(null, new EntitiesChangedEventArgs(1, "Finished Activate.IPRRecalculateClearedRecords", entities));
    }
    private static void ResetArchival(Entities entities, Func<object, EntitiesChangedEventArgs, bool> progress)
    {
      progress(null, new EntitiesChangedEventArgs(1, "Starting Activate.ResetArchival", entities));
      progress(null, new EntitiesChangedEventArgs(1, "Batch archive resetiny", entities));
      foreach (Batch _batchItem in entities.Batch)
      {
        _batchItem.Archival = false;
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
      SubmitChanges(progress, entities);
      progress(null, new EntitiesChangedEventArgs(1, "Material archive resetiny", entities));
      foreach (Material _materialItem in entities.Material)
      {
        _materialItem.Archival = false;
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
      SubmitChanges(progress, entities);
      progress(null, new EntitiesChangedEventArgs(1, "Finished Activate.ResetArchival", entities));
    }
    #endregion

  }
}
