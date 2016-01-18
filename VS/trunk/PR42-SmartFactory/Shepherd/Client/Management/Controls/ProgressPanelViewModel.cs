//<summary>
//  Title   : ProgressPanelViewModel
//  System  : Microsoft VisualStudio 2013 / C#
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

using CAS.Common.ComponentModel;
using CAS.SmartFactory.Shepherd.Client.Management.Infrastructure;
using Prism.Events;
using Prism.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;

/// <summary>
/// The Controls namespace.
/// </summary>
namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{

  /// <summary>
  /// Class ProgressPanelViewModel.
  /// </summary>
  [Export]
  public class ProgressPanelViewModel : PropertyChangedBase
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="ProgressPanelViewModel"/> class providing a view model for <see cref="ProgressPanel"/> controlling content of the
    /// <see cref="ProgressBar"/>, and labels: "Website URL, Version, Phase
    /// </summary>
    /// <param name="eventAggregator">The event aggregator.</param>
    [ImportingConstructor]
    public ProgressPanelViewModel(IEventAggregator eventAggregator, ILoggerFacade loggingService)
    {
      m_EventAggregator = eventAggregator;
      m_loggingService = loggingService;
      this.m_EventAggregator.GetEvent<ProgressChangeEvent>().Subscribe(this.ProgressUpdatedHandler, ThreadOption.PublisherThread);
      this.m_EventAggregator.GetEvent<SharePointWebsiteEvent>().Subscribe(this.SharePointWebsiteDataHandler, ThreadOption.PublisherThread);
      this.m_EventAggregator.GetEvent<MachineStateNameEvent>().Subscribe(this.MachineStateNameEventHandler, ThreadOption.PublisherThread);
    }
    #endregion

    #region public UI API
    /// <summary>
    /// Gets or sets the progress.
    /// </summary>
    /// <value>
    /// The progress.
    /// </value>
    public int Progress
    {
      get
      {
        return b_Progress;
      }
      set
      {
        RaiseHandler<int>(value, ref b_Progress, "Progress", this);
      }
    }
    /// <summary>
    /// Gets or sets the progress bar maximum.
    /// </summary>
    /// <value>
    /// The progress bar maximum.
    /// </value>
    public int ProgressBarMaximum
    {
      get
      {
        return b_ProgressBarMaximum;
      }
      set
      {
        RaiseHandler<int>(value, ref b_ProgressBarMaximum, "ProgressBarMaximum", this);
      }
    }
    /// <summary>
    /// Gets or sets the URL of the SharePoint website.
    /// </summary>
    /// <value>The URL of the website.</value>
    public string URL
    {
      get
      {
        return b_URL;
      }
      set
      {
        RaiseHandler<string>(value, ref b_URL, "URL", this);
      }
    }
    /// <summary>
    /// Gets or sets the current content version of the SharePoint website.
    /// </summary>
    /// <value>The current content version.</value>
    public Version CurrentContentVersion
    {
      get
      {
        return b_CurrentContentVersion;
      }
      set
      {
        RaiseHandler<Version>(value, ref b_CurrentContentVersion, "CurrentContentVersion", this);
      }
    }
    /// <summary>
    /// Gets or sets the version visibility. By default it is Collapsed <see cref="Visibility"/>
    /// </summary>
    /// <value>The version visibility.</value>
    public Visibility VersionVisibility
    {
      get
      {
        return b_VersionVisibility;
      }
      set
      {
        RaiseHandler<System.Windows.Visibility>(value, ref b_VersionVisibility, "VersionVisibility", this);
      }
    }
    /// <summary>
    /// Gets or sets the state name of the machine.
    /// </summary>
    /// <value>The state of the machine.</value>
    public string MachineStateName
    {
      get
      {
        return b_MachineStateName;
      }
      set
      {
        RaiseHandler<string>(value, ref b_MachineStateName, "MachineStateName", this);
      }
    }
    #endregion

    #region private
    private void ProgressUpdatedHandler(ProgressChangedEventArgs progress)
    {
      UpdateProgressBar(progress.ProgressPercentage);
    }
    private void SharePointWebsiteDataHandler(ISharePointWebsiteData data)
    {
      URL = string.IsNullOrEmpty(data.URL) ? Properties.Resources.URLError : data.URL;
      CurrentContentVersion = data.CurrentContentVersion;
      if (data.CurrentContentVersion == null)
        VersionVisibility = Visibility.Collapsed;
      else
        VersionVisibility = Visibility.Visible;
      m_loggingService.Log
        (String.Format("Mew ISharePointWebsiteData: URL={0}, CurrentContentVersion={1}", URL, CurrentContentVersion == null ? "null" : data.CurrentContentVersion.ToString()), Category.Debug, Priority.Low);
    }
    private void MachineStateNameEventHandler(string machineStateName)
    {
      MachineStateName = machineStateName;
      m_loggingService.Log(String.Format("Mew MachineStateName {0}", MachineStateName), Category.Debug, Priority.Low);
      UpdateProgressBar(0);
    }
    private void UpdateProgressBar(int progress)
    {
      if (progress <= 0)
      {
        Progress = 0;
        ProgressBarMaximum = ProgressBarMaximumDefault;
        m_loggingService.Log(String.Format("Set the progress bar to the standby position Progress=0, ProgressBarMaximum={0}", ProgressBarMaximumDefault), Category.Debug, Priority.Low);
        return;
      }
      while (ProgressBarMaximum - Progress < progress)
      {
        ProgressBarMaximum *= 2;
        m_loggingService.Log(String.Format("Changed progress bar scale to ProgressBarMaximum={0}", ProgressBarMaximum), Category.Debug, Priority.Low);
      }
      Progress += progress;
    }
    private IEventAggregator m_EventAggregator;
    private const int ProgressBarMaximumDefault = 100;
    //backing fields
    private string b_URL = Properties.Resources.URLUnknown;
    private Version b_CurrentContentVersion = null;
    private Visibility b_VersionVisibility = Visibility.Collapsed;
    private int b_Progress;
    private int b_ProgressBarMaximum = ProgressBarMaximumDefault;
    private string b_MachineStateName = " --- ";
    private ILoggerFacade m_loggingService;
    #endregion

  }
}
