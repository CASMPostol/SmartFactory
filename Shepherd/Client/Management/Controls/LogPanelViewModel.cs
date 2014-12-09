// ***********************************************************************
// Assembly         : CAS.ShepherdManagement
// Author           : mariusz postol
// Created          : 12-09-2014
//
// Last Modified By : mariusz postol
// Last Modified On : 12-09-2014
// ***********************************************************************
// <copyright file="LogPanelViewModel.cs" company="CAS">
//     Copyright (c) CAS. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The Controls namespace.
/// </summary>
namespace CAS.SmartFactory.Shepherd.Client.Management.Controls
{
  /// <summary>
  /// Class LogPanelViewModel.
  /// </summary>
  [Export]
  public class LogPanelViewModel : CAS.Common.ComponentModel.PropertyChangedBase
  {
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

    private ObservableCollection<string> b_ProgressList = new ObservableCollection<string>();

  }
}
