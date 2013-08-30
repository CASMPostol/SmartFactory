//<summary>
//  Title   : Archive class contain collection of function supporting archival data management
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

using CAS.SmartFactory.IPR.WebsiteModel.Linq;
using System;

namespace CAS.SmartFactory.IPR.Client.FeatureActivation.Archival
{
  /// <summary>
  /// Archive class contain collection of function supporting archival data management
  /// </summary>
  internal static class Archive
  {

    internal static void Go(Entities edc, Action<object, EntitiesChangedEventArgs> ProgressChanged)
    {
      //ProgressChanged(null, new EntitiesChangedEventArgs(1, "Starting 
      foreach (IPR.WebsiteModel.Linq.IPR _iprX in edc.IPR)
        if (_iprX.AccountClosed.Value == true && _iprX.ClosingDate.Value + TimeSpan.FromDays(Properties.Settings.Default.IPRAccountArchivalDelay) > DateTime.Today)
        {
          _iprX.Archival = true;
          foreach (Disposal _dspx in _iprX.Disposal)
            _dspx.Archival = true;
        }
        else
          _iprX.Archival = false;
      foreach (Batch _batchX in edc.Batch)
      {
        if (_batchX.FGQuantityAvailable > 0)
        {
          _batchX.Archival = false;
          continue;
        }
        bool _2archive = true;
        foreach (Material _material in _batchX.Material)
        {
          foreach (Disposal _disposalX in _material.Disposal)
          {
            if (_disposalX.CustomsStatus.Value != CustomsStatus.Finished || _disposalX.SADDate.Value + TimeSpan.FromDays(Properties.Settings.Default.BatchArchivalDelay) > DateTime.Today)
            {
              _2archive = false;
              break;
            }
          }
          if (!_2archive)
            break;
        }
        if (_2archive)
        {
          _batchX.Archival = true;
          foreach (Material _material in _batchX.Material)
            _material.Archival = true;
        }
        else
        {
          _batchX.Archival = true;
          foreach (Material _material in _batchX.Material)
            _material.Archival = true;
        }
      }
    }
  }
}
