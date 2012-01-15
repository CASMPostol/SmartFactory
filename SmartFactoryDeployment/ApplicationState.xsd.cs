using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using CAS.SmartFactory.Deployment.Properties;

namespace CAS.SmartFactory.Deployment
{
  /// <summary>
  /// Application State
  /// </summary>
  public partial class InstallationStateData
  {
    #region ctor
    /// <summary>
    /// Initializes a new instance of the <see cref="InstallationStateData"/> class.
    /// </summary>
    public InstallationStateData()
    {
      SiteCollectionCreated = false;
      SiteCollectionSolutionsDeployed = false;
      SiteCollectionFeturesActivated = false;
      FarmSolutionsDeployed = false;
      FarmFeaturesActivated = false;
    }
    #endregion

    #region public
    internal void Save(System.IO.FileInfo _file)
    {
      try
      {
        if (_file.Exists)
        {
          _file.Attributes = 0;
          _file.Delete();
        }
        using (Stream _strm = _file.OpenWrite())
        {
          XmlSerializer _srlzr = new XmlSerializer(typeof(InstallationStateData));
          _srlzr.Serialize(_strm, this);
        }
        _file.Refresh();
        _file.Attributes = FileAttributes.ReadOnly | _file.Attributes;
      }
      catch (Exception ex)
      {
        throw new ApplicationException(string.Format(Resources.SaveInstallationStateDataFailure, ex.Message));
      }
    }
    internal static InstallationStateData Read(System.IO.FileInfo _file)
    {
      try
      {
        using (Stream _strm = _file.OpenRead())
        {
          XmlSerializer _srlzr = new XmlSerializer(typeof(InstallationStateData));
          return (InstallationStateData)_srlzr.Deserialize(_strm);
        }
      }
      catch (Exception ex)
      {
        throw new ApplicationException(string.Format(Resources.SaveInstallationStateDataFailure, ex.Message));
      }
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="InstallationStateData"/> class.
    /// </summary>
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
    #endregion

    #region Browsable public properties
    /// <summary>
    /// Gets or sets the web application URL.
    /// </summary>
    /// <value>
    /// A String that contains the URL for the site collection, for example, Site_Name or sites/Site_Name. 
    /// It may either be server-relative or absolute for typical sites.
    /// </value>
    [Browsable(true)]
    [ReadOnly(true)]
    [Category("User")]
    [XmlIgnore()]
    public Uri WebApplicationURL
    {
      get { return new Uri(XmlWebApplicationURL); }
      set { XmlWebApplicationURL = value.ToString(); }
    }
    /// <summary>
    /// Gets or sets the name of the site collection feture.
    /// </summary>
    /// <value>
    /// The name of the site collection feture.
    /// </value>
    [Browsable(true)]
    [ReadOnly(true)]
    [Category("Solutions")]
    [XmlIgnore()]
    public Guid SiteCollectionFetureId
    {
      get { return XmlSiteCollectionFetureId.Parse(); }
      set { XmlSiteCollectionFetureId = value.ToString(); }
    }
    /// <summary>
    /// Gets or sets the name of the farm collection feture.
    /// </summary>
    /// <value>
    /// The name of the farm collection feture.
    /// </value>
    [Browsable(true)]
    [ReadOnly(true)]
    [Category("Solutions")]
    [XmlIgnore()]
    public Guid FarmFetureId
    {
      get { return XmlFarmFetureId.Parse(); }
      set { XmlFarmFetureId = value.ToString(); }
    }
    /// <summary>
    /// Gets or sets the solution ID.
    /// </summary>
    /// <value>
    /// The solution ID.
    /// </value>
    /// <remarks>Use the Id property to return the GUID for the solution.</remarks>
    [Browsable(true)]
    [ReadOnly(true)]
    [Category("Installation")]
    [XmlIgnore()]
    public Guid SolutionID
    {
      get { return XmlSolutionID.Parse(); }
      set { XmlSolutionID = value.ToString(); }
    }
    #endregion
   
  }
}
