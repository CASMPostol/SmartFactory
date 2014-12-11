//<summary>
//  Assembly         : CAS.ShepherdManagement
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
