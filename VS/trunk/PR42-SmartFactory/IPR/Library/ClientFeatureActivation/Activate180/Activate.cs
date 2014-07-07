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
    /// <param name="doActivate1800">if set to <c>true</c> [document activate1800].</param>
    /// <param name="progress">The progress.</param>
    public static void Go(string siteURL, bool doActivate1800, Func<object, EntitiesChangedEventArgs, bool> progress)
    {
      using (EntitiesDataContext edc = new EntitiesDataContext(siteURL))
      {
        if (!doActivate1800)
        {
          progress(null, new EntitiesChangedEventArgs(1, "Activation of Rel 1.81 skipped", edc));
          return;
        }
        UpdateDisposals(edc, progress);
        IPRRecalculateClearedRecords(edc, progress);
        ResetArchival(edc, progress);
      }
    }

    #region private
    private static void SubmitChanges(Func<object, EntitiesChangedEventArgs, bool> progress, EntitiesDataContext edc)
    {
      progress(null, new EntitiesChangedEventArgs(1, "SubmitChanges", edc));
      edc.SubmitChanges();
    }
    private static void UpdateDisposals(EntitiesDataContext entities, Func<object, EntitiesChangedEventArgs, bool> progress)
    {
      progress(null, new EntitiesChangedEventArgs(1, "Starting Activate.UpdateDisposals", entities));
      int _cl = 0;
      foreach (Disposal _dspx in entities.Disposal)
      {
        _dspx.Archival = false;
        if (!_dspx.JSOXReportID.HasValue && _dspx.JSOXCustomsSummaryIndex != null)
          _dspx.JSOXReportID = _dspx.JSOXCustomsSummaryIndex.JSOXCustomsSummary2JSOXIndex.Id.Value;
        if (String.IsNullOrEmpty(_dspx.SadConsignmentNo))
          switch (_dspx.CustomsStatus.Value)
          {
            case CustomsStatus.NotStarted:
              break;
            case CustomsStatus.Started:
            case CustomsStatus.Finished:
              if (_dspx.Disposal2ClearenceIndex == null)
              {
                progress(null, new EntitiesChangedEventArgs(1, "Wrong Disposal to ClearenceIndex lookup", entities));
                break;
              }
              _dspx.SadConsignmentNo = String.Format("{0:D7}", _dspx.Disposal2ClearenceIndex.Id.Value);
              break;
            default:
              progress(null, new EntitiesChangedEventArgs(1, "Wrong Disposal customs status", entities));
              break;
          }
        _cl++;
        if (_cl % 100 == 0)
          SubmitChanges(progress, entities);
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
      SubmitChanges(progress, entities);
      progress(null, new EntitiesChangedEventArgs(1, "Finished Activate.UpdateDisposals", entities));
    }
    private static void IPRRecalculateClearedRecords(EntitiesDataContext entities, Func<object, EntitiesChangedEventArgs, bool> progress)
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
    private static void ResetArchival(EntitiesDataContext entities, Func<object, EntitiesChangedEventArgs, bool> progress)
    {
      progress(null, new EntitiesChangedEventArgs(1, "Starting Activate.ResetArchival", entities));
      progress(null, new EntitiesChangedEventArgs(1, "Batch archive reseting", entities));
      foreach (Batch _batchItem in entities.Batch)
      {
        _batchItem.Archival = false;
        progress(null, new EntitiesChangedEventArgs(1, null, entities));
      }
      SubmitChanges(progress, entities);
      progress(null, new EntitiesChangedEventArgs(1, "Material archive reseting", entities));
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
