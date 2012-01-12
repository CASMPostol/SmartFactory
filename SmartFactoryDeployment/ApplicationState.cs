using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CAS.SmartFactory.Deployment.Properties;

namespace CAS.SmartFactory.Deployment
{
  /// <summary>
  /// Application State
  /// </summary>
  [Serializable]
  public class ApplicationState
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationState"/> class.
    /// </summary>
    public ApplicationState()
    {
      SiteCollectionCreated = false;
      SiteCollectionSolutionsDeployed = false;
      SiteCollectionFeturesActivated = false;
      FarmSolutionsDeployed = false;
      FarmFeaturesActivated = false;
    }
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
    /// <summary>
    /// Gets the URI.
    /// </summary>
    /// <param name="_url">The _url.</param>
    internal void GetUri(string _url)
    {
      string _errorMessage = String.Empty;
      Uri _uri;
      if (ValidateUrl(_url, out _uri, out _errorMessage))
      {
        WebApplicationURL = _uri;
      }
      else
        throw new ApplicationException(_errorMessage);
    }
    internal void GetSiteCollectionURL(string _text)
    {
      //TODO add validation
      SiteCollectionURL = _text;
    }
    internal void GetOwnerLogin(string _text)
    {
      //TODO Add validation
      OwnerLogin = _text;
    }
    internal void GetOwnerEmail(string _text)
    {
      //TODO Add validation
      OwnerEmail = _text;
    }
    /// <summary>
    /// Gets or sets the web application URL.
    /// </summary>
    /// <value>
    /// A String that contains the URL for the site collection, for example, Site_Name or sites/Site_Name. 
    /// It may either be server-relative or absolute for typical sites.
    /// </value>
    public Uri WebApplicationURL { get; set; }
    /// <summary>
    /// Gets or sets the web application URL.
    /// </summary>
    /// <value>
    /// A String that contains the URL for the site collection, for example, Site_Name or sites/Site_Name. 
    /// It may either be server-relative or absolute for typical sites.
    /// </value>
    public string SiteCollectionURL { get; set; }
    /// <summary>
    /// Gets or sets the owner login.
    /// </summary>
    /// <value>
    /// A String that contains the user name of the owner of the site collection (for example, Domain\User). In Active 
    /// Directory Domain Services account creation mode, the ownerLogin parameter must contain a value even if 
    /// the value does not correspond to an actual user name.
    /// </value>
    public string OwnerLogin { get; set; }
    /// <summary>
    /// Gets or sets the owner email.
    /// </summary>
    /// <value>
    /// A String that contains the e-mail address of the owner of the site collection.
    /// </value>
    public string OwnerEmail { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether site collection has been created.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [site collection created]; otherwise, <c>false</c>.
    /// </value>
    public bool SiteCollectionCreated { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether site collection solutions have been deployed.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if site collection solutions deployed; otherwise, <c>false</c>.
    /// </value>
    public bool SiteCollectionSolutionsDeployed { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether site collection fetures have been activated.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if site collection fetures activated; otherwise, <c>false</c>.
    /// </value>
    public bool SiteCollectionFeturesActivated { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether farm solutions have been deployed.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if farm solutions deployed; otherwise, <c>false</c>.
    /// </value>
    public bool FarmSolutionsDeployed { get; set; }
    /// <summary>
    /// Gets or sets a value indicating whether farm features have been activated.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if farm features activated; otherwise, <c>false</c>.
    /// </value>
    public bool FarmFeaturesActivated { get; set; }
    /// <summary>
    /// Gets or sets the name of the site collection feture.
    /// </summary>
    /// <value>
    /// The name of the site collection feture.
    /// </value>
    public Guid SiteCollectionFetureId {get; set; }
    /// <summary>
    /// Gets or sets the name of the farm collection feture.
    /// </summary>
    /// <value>
    /// The name of the farm collection feture.
    /// </value>
    public Guid FarmFetureId { get; set; }

    /// <summary>
    /// Gets or sets the solution ID.
    /// </summary>
    /// <value>
    /// The solution ID.
    /// </value>
    /// <remarks>Use the Id property to return the GUID for the solution.</remarks>
    public Guid SolutionID { get; set; }
  }
}
