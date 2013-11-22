//<summary>
//  Title   : class MainPageData
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;

namespace CAS.SmartFactory.CW.Dashboards.ExitSheetWebPart
{
  /// <summary>
  /// class MainPageData
  /// </summary>
  internal class MainPageData : INotifyPropertyChanged, IDisposable
  {

    #region INotifyPropertyChanged Members

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion

    #region IDisposable Members

    public void Dispose()
    {
      if (m_Context != null)
        m_Context.Dispose();
      m_Disposed = true;
    }

    #endregion

    #region private

    #region vars
    private bool m_Disposed = false;
    private DataContextAsync m_Context = new DataContextAsync();
    #endregion

    #endregion

  }
}
