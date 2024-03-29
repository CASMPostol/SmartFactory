﻿//<summary>
//  Assembly: CAS.ShepherdManagement
//  Title   : LogPanelViewModel.cs
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
using Prism.Events;
using Prism.Logging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;

/// <summary>
/// The Controls namespace.
/// </summary>
namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Class LogPanelViewModel.
  /// </summary>
  [Export]
  [PartCreationPolicy(CreationPolicy.Shared)]
  public class LogPanelViewModel : PropertyChangedBase
  {

    #region public
    /// <summary>
    /// Gets or sets the progress list.
    /// </summary>
    /// <value>The progress list.</value>
    public ObservableCollection<string> ProgressList
    {
      get
      {
        return b_ProgressList;
      }
      set
      {
        RaiseHandler<System.Collections.ObjectModel.ObservableCollection<string>>(value, ref b_ProgressList, "ProgressList", this);
      }
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="LogPanelViewModel"/> class.
    /// </summary>
    /// <param name="eventAggregator">The event aggregator.</param>
    [ImportingConstructor]
    public LogPanelViewModel(IEventAggregator eventAggregator, ILoggerFacade logger)
    {
      m_EventAggregator = eventAggregator;
      m_ILoggerFacade = logger;
      this.m_EventAggregator.GetEvent<Infrastructure.ProgressChangeEvent>().Subscribe(this.ProgressUpdatedHandler, ThreadOption.UIThread);
    }
    #endregion

    #region private
    private void ProgressUpdatedHandler(ProgressChangedEventArgs progress)
    {
      if (progress.UserState == null)
        return;
      if (progress.UserState is string)
        ProgressList.Add(String.Format("{0:T}: {1}", DateTime.Now, (String)progress.UserState));
      m_ILoggerFacade.Log(progress.UserState.ToString(), Category.Info, Priority.Low);
    }
    private IEventAggregator m_EventAggregator;
    private ILoggerFacade m_ILoggerFacade;
    private ObservableCollection<string> b_ProgressList = new ObservableCollection<string>();
    #endregion

  }
}
