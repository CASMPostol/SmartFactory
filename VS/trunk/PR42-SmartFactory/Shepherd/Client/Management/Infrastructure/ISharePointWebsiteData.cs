//<summary>
//  Title   : ISharePointWebsiteData
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

/// <summary>
/// The Infrastructure namespace.
/// </summary>
namespace CAS.SmartFactory.Shepherd.Client.Management.Infrastructure
{
  /// <summary>
  /// Interface ISharePointWebsiteData - representation of the basic meta information about the SharePoint website
  /// </summary>
  public interface ISharePointWebsiteData
  {

    /// <summary>
    /// Gets the URL of the SharePoint website.
    /// </summary>
    /// <value>The URL.</value>
    string URL { get; }
    /// <summary>
    /// Gets the current content version of the SharePoint website.
    /// </summary>
    /// <value>The current content version.</value>
    Version CurrentContentVersion { get; }

  }
}
