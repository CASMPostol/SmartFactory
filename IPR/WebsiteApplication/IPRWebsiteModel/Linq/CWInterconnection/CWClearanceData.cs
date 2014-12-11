//<summary>
//  Title   : Name of Application
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

using System.Collections.Generic;
using CAS.SharePoint.Common.ServiceLocation;
using CAS.SmartFactory.Customs.Account;
using Microsoft.Practices.ServiceLocation;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.CWInterconnection
{
  /// <summary>
  /// class CWClearanceData 
  /// </summary>
  public class CWClearanceData : CommonClearanceData
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="CWClearanceData"/> class.
    /// </summary>
    /// <param name="clearenceLookup">The clearence lookup.</param>
    public CWClearanceData(int clearenceLookup)
      : base(clearenceLookup)
    { }
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