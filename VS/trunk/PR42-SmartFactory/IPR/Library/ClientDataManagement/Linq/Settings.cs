//<summary>
//  Title   : public partial class Settings
//  System  : Microsoft VisulaStudio 2013 / C#
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CAS.SmartFactory.IPR.Client.DataManagement.Linq
{
  public partial class Settings
  {
    /// <summary>
    /// Gets the current content version.
    /// </summary>
    /// <param name="requestUrl">The request URL.</param>
    /// <param name="version">The version to be returned.</param>
    /// <param name="ReportProgress">The report of the progress.</param>
    public static void GetCurrentContentVersion(string requestUrl, ref Version version, ProgressChangedEventHandler ReportProgress)
    {
      using (Entities _edc = new Entities(requestUrl))
      {
        ReportProgress(null, new ProgressChangedEventArgs(1, "Trying to recognize SharePoint content version."));
        string _setting = _edc.Settings.ToList<Settings>().Where<Settings>(x => x.KeyValue.Contains(m_CurrentContentVersionLabel)).Select<Settings, string>(y => y.Title).FirstOrDefault<string>();
        if (String.IsNullOrEmpty(_setting))
        {
          ReportProgress(null, new ProgressChangedEventArgs(1, "There is no information about SharePoint content version on the Setting list."));
          return;
        }
        try
        {
          version = new Version(_setting);
        }
        catch (Exception _ex)
        {
          ReportProgress(null, new ProgressChangedEventArgs(1, String.Format("Cannot parse the version settings, because {0}", _ex.Message)));
        }
      }
    }
    public static void SaveCurrentContentVersion(string requestUrl, ref Version version, ProgressChangedEventHandler ReportProgress)
    {
      using (Entities _edc = new Entities(requestUrl))
      {
        ReportProgress(null, new ProgressChangedEventArgs(1, "Trying to update SharePoint content version."));
        Settings _version = _edc.Settings.ToList<Settings>().Where<Settings>(x => x.Title.Contains(m_CurrentContentVersionLabel)).FirstOrDefault<Settings>();
        if (_version == null)
        {
          ReportProgress(null, new ProgressChangedEventArgs(1, "There is no information about SharePoint content version on the Setting list."));
          _version = new Settings()
          {
            KeyValue = m_CurrentContentVersionLabel,
          };
          _edc.Settings.InsertOnSubmit(_version);
        }
        _version.Title = version.ToString();
        _edc.SubmitChanges();
      }
    }
    private const string m_CurrentContentVersionLabel = "Current Content Version";
  }
}
