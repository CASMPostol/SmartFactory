//<summary>
//  Title   : Customs Warehouse Account Record Data
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

using System.Collections.Generic;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Account;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.SharePoint.Common.ServiceLocation;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.Account
{
  /// <summary>
  /// Customs Warehouse Account Record Data
  /// </summary>
  public class CWAccountData: AccountData
  {
    /// <summary>
    /// Calls the remote service.
    /// </summary>
    /// <param name="requestUrl">The The URL of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="warnningList">The warnning list.</param>
    public override void CallService( string requestUrl, List<Warnning> warnningList )
    {
      IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
      ICWAccountFactory _cwFactory = serviceLocator.GetInstance<ICWAccountFactory>();
      _cwFactory.CreateCWAccount( this, warnningList, requestUrl );
    }
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <param name="good">The good.</param>
    protected internal override void GetNetMass( SADGood good )
    {
      NetMass = good.NetMass.GetValueOrDefault( 0 );
    }
    /// <summary>
    /// Gets the customs process.
    /// </summary>
    /// <value>
    /// The customs process.
    /// </value>
    protected override CustomsProcess Process
    {
      get { return CustomsProcess.cw; }
    }
  }
}
