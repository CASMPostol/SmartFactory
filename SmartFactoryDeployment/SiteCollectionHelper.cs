﻿using System;
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
  internal class SiteCollectionHelper : IDisposable
  {
    private SiteCollectionHelper(SPSite _siteCollection)
    {
      SiteCollection = _siteCollection;
    }
    public SiteCollectionHelper(SPWebApplication _wapplication, string _Url)
    {
      SiteCollection = _wapplication.Sites[_Url];
    }
    internal static bool DeleteIfExist { get; set; }
    internal SPSite SiteCollection { get; private set; }
    internal static SiteCollectionHelper CreateSPSite(SPWebApplication _wapplication, string _uri, string _owner, string _email)
    {
      try
      {
        if (_wapplication.Sites.Names.Contains(_uri))
          if (DeleteIfExist)
            _wapplication.Sites.Delete(_uri);
          else
          {
            return new SiteCollectionHelper(_wapplication.Sites[_uri]);
          }
        return new SiteCollectionHelper(_wapplication.Sites.Add(_uri, _owner, _email));
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
            SetUpData.TraceEvent.TraceVerbose(90, "SiteCollectionHelper", _tmsg);
            break;
          }
          catch (Exception) { }
          string _msg = String.Format("I cannot find definition for the feature Id = {0} at the site Url = {1} attempt {2} form 5.", _feature, SiteCollection.Url, _try++);
          SetUpData.TraceEvent.TraceVerbose(95, "SiteCollectionHelper", _msg);
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
    internal void DeactivateFeature(Guid _feature)
    {
      try
      {
        SiteCollection.Features.Remove(_feature, false);
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
