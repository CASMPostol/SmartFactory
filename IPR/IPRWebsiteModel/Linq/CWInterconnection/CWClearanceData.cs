//<summary>
//  Title   : Name of Application
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Customs.Account;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.CWInterconnection
{
  /// <summary>
  /// class CWClearanceData 
  /// </summary>
  public class CWClearanceData : CommonClearanceData
  {

    /// <summary>
    /// Calls the service.
    /// </summary>
    /// <param name="requestUrl">The request URL.</param>
    /// <param name="warnningList">The warnning list.</param>
    public void CallService(string requestUrl, List<Customs.Warnning> warnningList)
    {
      IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
      ICWAccountFactory _cwFactory = serviceLocator.GetInstance<ICWAccountFactory>();
      _cwFactory.ClearThroughCustoms(this, warnningList, requestUrl);
    }

  }
}