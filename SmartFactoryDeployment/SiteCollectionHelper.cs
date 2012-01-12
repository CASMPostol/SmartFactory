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
    internal static void DeploySolution(SPSite _site, FileInfo _featuteFile)
    {
      try
      {
        SPDocumentLibrary solutionGallery = (SPDocumentLibrary)_site.GetCatalog(SPListTemplateType.SolutionCatalog);
        SPFile file = solutionGallery.RootFolder.Files.Add(_featuteFile.Name, _featuteFile.OpenRead(), true);
        SPUserSolution solution = _site.Solutions.Add(file.Item.ID);
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
    internal static void ActivateFeature(SPSite _site, Guid _feature, SPFeatureDefinitionScope _scope)
    {
      try
      {
        _site.Features.Add(_feature, false, _scope);
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.FeatureActivationFailed, _feature, _site.Url, _scope, ex.Message);
        throw new ApplicationException(_msg); ;
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
  }
}
