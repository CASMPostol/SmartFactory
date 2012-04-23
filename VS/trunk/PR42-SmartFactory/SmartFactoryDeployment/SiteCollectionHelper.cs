using System;
using System.IO;
using System.Linq;
using System.Threading;
using CAS.SmartFactory.Deployment.Controls;
using CAS.SmartFactory.Deployment.Properties;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace CAS.SmartFactory.Deployment
{
  internal class SiteCollectionHelper : IDisposable
  {
    private SiteCollectionHelper(SPSite _siteCollection)
    {
      SiteCollection = _siteCollection;
    }
    public SiteCollectionHelper(SPWebApplication _wapplication, string _Url)
    {
      try
      {
        if (!FarmHelpers.WebApplication.Sites.Names.Contains(_Url))
        {
          string _frmt = "The web application Name={0} does not contains the site collection at Url = {1}";
          throw new ApplicationException(String.Format(_frmt, FarmHelpers.WebApplication.Name, _Url));
        }
        SiteCollection = _wapplication.Sites[_Url];
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format(Properties.Resources.LastOperationFailed, ex.Message), ex);
      }
    }
    internal static bool DeleteIfExist { get; set; }
    internal SPSite SiteCollection { get; private set; }
    /// <summary>
    /// Creates an <see cref="Microsoft.SharePoint.SPSite"/> object in the collection based on the specified URL, title, description, locale identifier (LCID), and site
    /// definition or site template, as well as on the user name, user display name, and e-mail address of the owner of the site collection.
    /// </summary>
    /// <param name="_wapplication">The _wapplication.</param>
    /// <param name="siteUrl">A string that contains the server-relative URL for the site object (for example, Site_Name or sites/Site_Name).</param>
    /// <param name="title">A string that contains the title of the site object.</param>
    /// <param name="description">A string that contains the description for the site object.</param>
    /// <param name="nLCID">An unsigned 32-bit integer that specifies the LCID for the site object.</param>
    /// <param name="webTemplate">A string that specifies the site definition or site template for the site object. Specify null to create a site 
    /// without applying a template to it. For a list of default site definitions.</param>
    /// <param name="ownerLogin">A string that contains the user name of the owner of the site object (for example, Domain\User). In Active Directory Domain 
    /// Services account creation mode, the strOwnerLogin parameter must contain a value even if the value does not correspond to an actual user name.</param>
    /// <param name="ownerName">A string that contains the display name of the owner of the site object.</param>
    /// <param name="ownerEmail">A string that contains the e-mail address of the owner of the site object.</param>
    /// <returns>An <see cref="SiteCollectionHelper "/> </returns>
    internal static SiteCollectionHelper CreateSPSite
      (SPWebApplication _wapplication, string siteUrl, string title, string description, uint nLCID, string webTemplate, string ownerLogin, string ownerName, string ownerEmail)
    {
      try
      {
        if (_wapplication.Sites.Names.Contains(siteUrl))
          if (DeleteIfExist)
            _wapplication.Sites.Delete(siteUrl);
          else
          {
            return new SiteCollectionHelper(_wapplication.Sites[siteUrl]);
          }
        //TODO http://itrserver/Bugs/BugDetail.aspx?bid=3260
        return new SiteCollectionHelper(_wapplication.Sites.Add( siteUrl,  title,  description,  nLCID,  webTemplate,  ownerLogin,  ownerName,  ownerEmail));
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format(Resources.CreateSiteCollectionFailed, ex.Message));
      }
    }
    internal static void DeleteSiteCollection(SPWebApplication _wa, string _uri)
    {
      try
      {
        if (!_wa.Sites.Names.Contains(_uri))
          throw new ApplicationException(Resources.CannotDelete);
        _wa.Sites.Delete(_uri);
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format(Resources.CreateSiteCollectionFailed, ex.Message));
      }
    }
    internal SPUserSolution DeploySolution(FileInfo _featuteFile)
    {
      try
      {
        if (SiteCollection == null)
          throw new ApplicationException(Resources.SiteCollectionNotExist);
        SPDocumentLibrary solutionGallery = (SPDocumentLibrary)SiteCollection.GetCatalog(SPListTemplateType.SolutionCatalog);
        SPFile file = solutionGallery.RootFolder.Files.Add(_featuteFile.Name, _featuteFile.OpenRead(), true);
        return SiteCollection.Solutions.Add(file.Item.ID);
      }
      catch (Exception ex)
      {
        string _msg = String.Format(
          Resources.DeploySolutionFailed,
          SiteCollection != null ? SiteCollection.Url : Resources.NullReference,
          _featuteFile.Name,
           ex.Message);
        throw new ApplicationException(_msg);
      }
    }
    internal void RetracSolution(Guid _usGuid)
    {
      try
      {
        Tracing.TraceEvent.TraceVerbose(90, "RetracSolution", String.Format("The solution {0} will be deleted from the SolutionCatalog", _usGuid));
        SPUserSolution _us = SiteCollection.Solutions[_usGuid];
        SiteCollection.Solutions.Remove(_us);
        SPDocumentLibrary solutionGallery = (SPDocumentLibrary)SiteCollection.GetCatalog(SPListTemplateType.SolutionCatalog);
        foreach (SPListItem _li in solutionGallery.Items)
        {
          if (_li.File.Name == _us.Name)
          {
            solutionGallery.Items.DeleteItemById(_li.ID);
            Tracing.TraceEvent.TraceInformation(90, "RetracSolution", String.Format("The solution {0} has been deleted from the SolutionCatalog", _us.Name));
            return;
          }
        }
        Tracing.TraceEvent.TraceWarning(90, "RetracSolution", String.Format("The solution {0} has not been found in the SolutionCatalog", _us.Name));
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format(Resources.LastOperationFailed, ex.Message));
      }
    }
    internal SPFeature ActivateFeature(Guid _feature, SPFeatureDefinitionScope _scope)
    {
      try
      {
        int _try = 0;
        do
        {
          try
          {
            SPFeatureDefinition _def;
            string _tmsg = String.Empty;
            if (_scope == SPFeatureDefinitionScope.Site)
            {
              _def = SiteCollection.FeatureDefinitions.First(_fd => { return _feature == _fd.Id; });
              _tmsg = String.Format("Found the definition of the feature Id={0} at the site Url={1} DisplayName={2}.", _feature, SiteCollection.Url, _def.DisplayName);
            }
            else
            {
              _def = FarmHelpers.Farm.FeatureDefinitions.First(_fd => { return _feature == _fd.Id; });
              _tmsg = String.Format("Found the definition of the feature Id={0} at the Farm DisplayName={1}.", _feature, FarmHelpers.Farm.DisplayName);
            }
            Tracing.TraceEvent.TraceVerbose(90, "SiteCollectionHelper", _tmsg);
            break;
          }
          catch (Exception) { }
          string _msg = String.Format("I cannot find definition for the feature Id = {0} at the site Url = {1} attempt {2} form 5.", _feature, SiteCollection.Url, _try++);
          Tracing.TraceEvent.TraceVerbose(95, "SiteCollectionHelper", _msg);
          Thread.Sleep(1000);
        } while (_try < 5);
        return SiteCollection.Features.Add(_feature, false, _scope);
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.FeatureActivationFailed, _feature, SiteCollection.Url, _scope, ex.Message);
        throw new ApplicationException(_msg); ;
      }
    }
    internal void DeactivateFeature(Guid _feature, SPFeatureDefinitionScope _scope)
    {
      try
      {
        try
        {
          SiteCollection.Features.Remove(_feature, false);
          string _tmsg = String.Empty;
          if (_scope == SPFeatureDefinitionScope.Site)
          {
            SiteCollection.FeatureDefinitions.Remove(_feature, true);
            _tmsg = String.Format("Found the definition of the feature Id={0} at the site Url={1} DisplayName={2}.", _feature, SiteCollection.Url, _def.DisplayName);
          }
          else
          {
            FarmHelpers.Farm.FeatureDefinitions.Remove(_feature, true);
            _tmsg = String.Format("Removed the definition of the feature Id={0} at the Farm DisplayName={1}.", _feature, FarmHelpers.Farm.DisplayName);
          }
          Tracing.TraceEvent.TraceVerbose(90, "SiteCollectionHelper", _tmsg);
        }
        catch (Exception _ex) 
        {
          string _msg = String.Format("I cannot remove definition for the feature Id = {0} at the site Url = {1} because {1}.", _feature, SiteCollection.Url, _ex);
          Tracing.TraceEvent.TraceVerbose(95, "SiteCollectionHelper", _msg);
        }
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.FeatureDeactivationFailed, _feature, SiteCollection.Url, ex.Message);
        throw new ApplicationException(_msg); ;
      }
    }
    public void Dispose()
    {
      if (SiteCollection != null)
        SiteCollection.Dispose();
    }
  }
}
