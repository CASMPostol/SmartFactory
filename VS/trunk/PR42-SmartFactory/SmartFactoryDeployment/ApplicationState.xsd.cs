using System;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using CAS.SmartFactory.Deployment.Properties;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;

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
      //TODO remove at cleanup.
      //SiteCollectionCreated = false;
      //SiteCollectionSolutionsDeployed = false;
      //SiteCollectionFeturesActivated = false;
      //FarmSolutionsDeployed = false;
      //FarmFeaturesActivated = false;
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
        throw new ApplicationException(string.Format(Resources.ReadInstallationStateDataFailure, ex.Message));
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
    internal SortedList<int, Solution> SolutionsToInstall
    {
      get
      {
        SortedList<int, Solution> _ret = new SortedList<int, Solution>();
        foreach (Solution _sl in Solutions)
          _ret.Add(_sl.Priority, _sl);
        return _ret;
      }
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

    //[Browsable(true)]
    //[ReadOnly(true)]
    //[Category("Solutions")]
    //[XmlIgnore()]
    //public Guid SiteCollectionFetureId
    //{
    //  get { return XmlSiteCollectionFetureId.Parse(); }
    //  set { XmlSiteCollectionFetureId = value.ToString(); }
    //}
    ///// <summary>
    ///// Gets or sets the name of the farm collection feture.
    ///// </summary>
    ///// <value>
    ///// The name of the farm collection feture.
    ///// </value>
    //[Browsable(true)]
    //[ReadOnly(true)]
    //[Category("Solutions")]
    //[XmlIgnore()]
    //public Guid FarmFetureId
    //{
    //  get { return XmlFarmFetureId.Parse(); }
    //  set { XmlFarmFetureId = value.ToString(); }
    //}

    //[Browsable(true)]
    //[ReadOnly(true)]
    //[Category("Installation")]
    //[XmlIgnore()]

    #endregion

  }
  public partial class Solution
  {
    /// <summary>
    /// Gets the definition scope.
    /// </summary>
    public SPFeatureDefinitionScope DefinitionScope
    {
      get
      {
        Microsoft.SharePoint.SPFeatureDefinitionScope _ret = Microsoft.SharePoint.SPFeatureDefinitionScope.None;
        switch (FeatureDefinitionScope)
        {
          case FeatureDefinitionScope.None:
            _ret = Microsoft.SharePoint.SPFeatureDefinitionScope.None;
            break;
          case FeatureDefinitionScope.Farm:
            _ret = Microsoft.SharePoint.SPFeatureDefinitionScope.Farm;
            break;
          case FeatureDefinitionScope.Site:
            _ret = Microsoft.SharePoint.SPFeatureDefinitionScope.Site;
            break;
          default:
            throw new ApplicationException("DefinitionScope-wrong FeatureDefinitionScope");
        }
        return _ret;
      }
    }
    /// <summary>
    /// Gets the site collection feture id.
    /// </summary>
    internal Guid FetureGuid
    {
      get { return FetureId.Parse(); }
    }
    /// <summary>
    /// Gets the solution ID.
    /// </summary>
    /// <value>
    /// The solution ID.
    /// </value>
    /// <remarks>Use the SolutionGuid property to return the GUID for the solution.</remarks>
    public Guid SolutionGuid
    {
      get { return SolutionID.Parse(); }
      set { SolutionID = value.ToString(); }
    }
  }
}
