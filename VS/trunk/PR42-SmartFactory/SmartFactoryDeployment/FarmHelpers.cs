using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using CAS.SmartFactory.Deployment.Properties;

namespace CAS.SmartFactory.Deployment
{
  internal static class FarmHelpers
  {
    internal static bool ValidateUrl(string _url, out Uri _auri, out string _errorMessage)
    {
      _auri = null;
      _errorMessage = string.Empty;
      if (!Uri.TryCreate(_url, UriKind.Absolute, out _auri))
      {
        _errorMessage = Resources.HTTPNotValid;
        return false;
      }
      if (_auri.Scheme != Uri.UriSchemeHttp)
      {
        _errorMessage = Resources.UrlMustBeHttp;
        return false;
      }
      return true;
    }
    internal static void GetUri(string _url)
    {
      string _errorMessage = String.Empty;
      Uri _uri;
      if (ValidateUrl(_url, out _uri, out _errorMessage))
      {
        FarmHelpers.WebApplicationURL = _uri;
      }
      else
        throw new ApplicationException(_errorMessage);
    }
    internal static void GetFarm()
    {
      try
      {
        Farm = SPFarm.Local;
      }
      catch (Exception ex)
      {
        string _msg = Resources.GettingAccess2LocalFarmException + ex.Message;
        throw new ApplicationException(_msg); ;
      }
    }
    internal static void GetWebApplication(Uri _auri)
    {
      try
      {
        WebApplication = SPWebApplication.Lookup(_auri);
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format(Properties.Resources.GetWebApplicationFfailed, ex.Message));
      }
    }
    /// <summary>
    /// Gets the web application.
    /// </summary>
    internal static SPWebApplication WebApplication { get; private set; }
    /// <summary>
    /// Gets or sets the farm.
    /// </summary>
    /// <value>
    /// The farm.
    /// </value>
    internal static SPFarm Farm { get; set; }
    public static Uri WebApplicationURL { get; set; }
  }
}
