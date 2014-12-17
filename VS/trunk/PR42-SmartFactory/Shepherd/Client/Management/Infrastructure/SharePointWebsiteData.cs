//<summary>
//  Title   : SharePointWebsiteData
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

namespace CAS.SmartFactory.Shepherd.Client.Management.Infrastructure
{
  /// <summary>
  /// Class SharePointWebsiteData.
  /// </summary>
  internal class SharePointWebsiteData : ISharePointWebsiteData
  {

    #region creator
    /// <summary>
    /// Initializes a new instance of the <see cref="SharePointWebsiteData"/> class.
    /// </summary>
    /// <param name="uRL">The URL of the website.</param>
    /// <param name="currentContentVersion">The current content version of the website.</param>
    public SharePointWebsiteData(string uRL, Version currentContentVersion)
    {
      URL = uRL;
      CurrentContentVersion = currentContentVersion;
    }
    #endregion

    #region ISharePointWebsiteData
    /// <summary>
    /// Gets the URL of the SharePoint website.
    /// </summary>
    /// <value>The URL.</value>
    public string URL
    {
      get;
      private set;
    }
    /// <summary>
    /// Gets the current content version of the SharePoint website.
    /// </summary>
    /// <value>The current content version.</value>
    public Version CurrentContentVersion
    {
      get;
      private set;
    }
    #endregion

  }
}
