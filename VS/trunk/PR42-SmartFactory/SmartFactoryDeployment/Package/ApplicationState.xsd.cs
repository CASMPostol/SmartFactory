using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;
using CAS.SmartFactory.Deployment.Controls;
using CAS.SmartFactory.Deployment.Properties;
using Microsoft.SharePoint;
using System.Reflection;

namespace CAS.SmartFactory.Deployment.Package

{
  /// <summary>
  /// Application State
  /// </summary>
  public partial class InstallationStateData
  {

    #region public
    internal void Save()
    {
      try
      {
        FileInfo _file = GetFileInfo();
        Tracing.TraceEvent.TraceInformation(25, "InstallationStateData.Save", String.Format("Saving installation details to the file {0}.", _file.FullName));
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
        throw new ApplicationException(string.Format(Resources.ConfigurationSaveInstallationStateDataFailure, ex.Message));
      }
    }
    internal static InstallationStateData Read()
    {
      try
      {
        FileInfo _file = GetFileInfo();
        Tracing.TraceEvent.TraceVerbose(49, "InstallationStateData.Read", String.Format("Loading the application installation state from the file at: {0}", _file.FullName));
        using (Stream _strm = _file.OpenRead())
        {
          XmlSerializer _srlzr = new XmlSerializer(typeof(InstallationStateData));
          return (InstallationStateData)_srlzr.Deserialize(_strm);
        }
      }
      catch (Exception ex)
      {
        string _msg = string.Format(Resources.ConfigurationReadInstallationStateDataFailure, ex.Message);
        Tracing.TraceEvent.TraceError(56, "InstallationStateData.Read", ex.Message);
        throw new ApplicationException(_msg, ex);
      }
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
    /// <summary>
    /// Gets or sets the web application URI.
    /// </summary>
    /// <value>
    /// A <see cref="Uri"/> that contains the URL for the site collection, for example, Site_Name or sites/Site_Name.
    /// It may either be server-relative or absolute for typical sites.
    /// </value>
    internal Uri WebApplicationUri
    {
      get { return new Uri(WebApplicationURL); }
      set { WebApplicationURL = value.ToString(); }
    }
    internal InstallationStateDataWrapper Wrapper { get { return new _stateData(this); } } 
    #endregion

    #region private
    private static FileInfo GetFileInfo()
    {
      string path = Path.Combine(
        Path.GetDirectoryName(Assembly.GetCallingAssembly().Location), Properties.Settings.Default.InstallationStateFileName);
      return new FileInfo(path);
    }
    private class _stateData : InstallationStateDataWrapper
    {
      public _stateData(InstallationStateData _parent)
        : base(_parent)
      { }
    } 
    #endregion

  }
  /// <summary>
  /// This class represents a SharePoint solution
  /// </summary>
  public partial class Solution
  {
    /// <summary>
    /// Gets the definition scope.
    /// </summary>
    public SPFeatureDefinitionScope SPFeatureDefinitionScope
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
    /// Gets the solution ID.
    /// </summary>
    /// <value>
    /// The solution ID.
    /// </value>
    /// <remarks>Use the SolutionGuid property to return the GUID for the solution.</remarks>
    internal Guid SolutionGuid
    {
      get { return SolutionID.Parse(); }
      set { SolutionID = value.ToString(); }
    }
    /// <summary>
    /// Get the Solution file info.
    /// </summary>
    /// <exception cref="FileNotFoundException">File not found.</exception>
    /// <returns>Object of <see cref="FileInfo"/></returns>
    internal FileInfo SolutionFileInfo()
    {
      FileInfo _fi = new FileInfo(this.FileName);
      if (!_fi.Exists)
        throw new FileNotFoundException(_fi.ToString());
      return _fi;
    }
  }
  /// <summary>
  ///  This class represents a SharePoint feature
  /// </summary>
  public partial class Feature
  {
    /// <summary>
    /// Gets the site collection feture id.
    /// </summary>
    public Guid FetureGuid
    {
      get { return this.DefinitionId.Parse(); }
    }
    /// <summary>
    /// Sets the <see cref="Microsoft.SharePoint.SPFeatureScope"/> scope in the configuration file.
    /// </summary>
    /// <value>
    /// The <see cref="Microsoft.SharePoint.SPFeatureScope"/> scope.
    /// </value>
    public Microsoft.SharePoint.SPFeatureScope SPScope
    {
      set
      {
        switch (value)
        {
          case SPFeatureScope.Farm:
            this.Scope = FeatureScope.Farm;
            break;
          case SPFeatureScope.ScopeInvalid:
            this.Scope = FeatureScope.ScopeInvalid;
            break;
          case SPFeatureScope.Site:
            this.Scope = FeatureScope.Site;
            break;
          case SPFeatureScope.Web:
            this.Scope = FeatureScope.Web;
            break;
          case SPFeatureScope.WebApplication:
            this.Scope = FeatureScope.WebApplication;
            break;
        }
      }
    }

  }

}
