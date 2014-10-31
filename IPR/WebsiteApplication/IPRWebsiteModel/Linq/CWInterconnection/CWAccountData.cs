//<summary>
//  Title   : Customs Warehouse Account Record Data
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

using CAS.SharePoint.Common.ServiceLocation;
using CAS.SmartFactory.Customs;
using CAS.SmartFactory.Customs.Account;
using Microsoft.Practices.ServiceLocation;
using System.Collections.Generic;

namespace CAS.SmartFactory.IPR.WebsiteModel.Linq.CWInterconnection
{
  /// <summary>
  /// Customs Warehouse Account Record Data
  /// </summary>
  public class CWAccountData : AccountData
  {

    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="CWAccountData"/> class.
    /// </summary>
    /// <param name="clearenceLookup">The clearence lookup.</param>
    public CWAccountData(int clearenceLookup)
      : base(clearenceLookup)
    { }
    #endregion

    #region public
    /// <summary>
    /// Calls the remote service.
    /// </summary>
    /// <param name="requestUrl">The The URL of a Windows SharePoint Services "14" Web site.</param>
    /// <param name="warnningList">The warnning list.</param>
    public override void CallService(string requestUrl, List<Warnning> warnningList)
    {
      IServiceLocator serviceLocator = SharePointServiceLocator.GetCurrent();
      ICWAccountFactory _cwFactory = serviceLocator.GetInstance<ICWAccountFactory>();
      _cwFactory.CreateCWAccount(this, warnningList, requestUrl);
    }
    #endregion

    #region private
    /// <summary>
    /// Gets the net mass.
    /// </summary>
    /// <param name="edc">The entity data context.</param>
    /// <param name="good">The good.</param>
    protected internal override void GetNetMass(Entities edc, SADGood good)
    {
      NetMass = good.NetMass.GetValueOrDefault(0);
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
    #endregion

  }
}
