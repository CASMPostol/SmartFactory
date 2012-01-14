using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using System.Security.Principal;
using Microsoft.SharePoint;
using System.Windows.Forms;
using CAS.SmartFactory.Deployment.Properties;
using System.IO;
using System.Threading;

namespace CAS.SmartFactory.Deployment
{
  internal static class SiteCollectionHelper
  {
    internal static bool DeleteIfExist { get; set; }
    internal static SPSite SiteCollection { get; set; }
    internal static void CreateSPSite(SPWebApplication _wapplication, string _uri, string _owner, string _email)
    {
      try
      {
        if (_wapplication.Sites.Names.Contains(_uri))
          if (DeleteIfExist)
            _wapplication.Sites.Delete(_uri);
          else
          {
            SiteCollection = _wapplication.Sites[_uri];
            return;
          }
        SiteCollection = _wapplication.Sites.Add(_uri, _owner, _email);
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
    internal static SPUserSolution DeploySolution(SPSite _site, FileInfo _featuteFile)
    {
      try
      {
        SPDocumentLibrary solutionGallery = (SPDocumentLibrary)_site.GetCatalog(SPListTemplateType.SolutionCatalog);
        SPFile file = solutionGallery.RootFolder.Files.Add(_featuteFile.Name, _featuteFile.OpenRead(), true);
        return _site.Solutions.Add(file.Item.ID);
      }
      catch (Exception ex)
      {
        string _msg = String.Format(
          Resources.DeploySolutionFailed,
          _site != null ? _site.Url : Resources.NullReference,
          _featuteFile.Name,
           ex.Message);
        throw new ApplicationException(_msg);
      }
    }
    internal static SPFeature ActivateFeature(SPSite _site, Guid _feature, SPFeatureDefinitionScope _scope)
    {
      try
      {
        int _try = 0;
        do
        {
          try
          {
            SPFeatureDefinition _def = _site.FeatureDefinitions.First(_fd => { return _feature == _fd.Id; });
            string _tmsg = String.Format("Found the definition for the feature Id={0} at the site Url={1} DisplayName={2}.", _feature, _site.Url, _def.DisplayName);
            SetUpData.TraceEvent.TraceVerbose(74, "SiteCollectionHelper", _tmsg);
            break;
          }
          catch (Exception) { }
          string _msg = String.Format("I cannot find definition for the feature Id = {0} at the site Url = {1} attempt {2} form 5.", _feature, _site.Url, _try++);
          SetUpData.TraceEvent.TraceVerbose(74, "SiteCollectionHelper", _msg);
          Thread.Sleep(1000);
        } while (_try < 5);
        return _site.Features.Add(_feature, false, _scope);
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.FeatureActivationFailed, _feature, _site.Url, _scope, ex.Message);
        throw new ApplicationException(_msg); ;
      }
    }
    internal static void DeactivateFeature(SPSite _site, Guid _feature)
    {
      try
      {
        _site.Features.Remove(_feature, false);
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.FeatureDeactivationFailed, _feature, _site.Url, ex.Message);
        throw new ApplicationException(_msg); ;
      }
    }
  }
}
