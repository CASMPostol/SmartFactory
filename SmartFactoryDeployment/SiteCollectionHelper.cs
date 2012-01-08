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
    internal static SPSite Create(SPWebApplication _wa, string _uri, string _owner, string _email)
    {
      try
      {
        if (_wa.Sites.Names.Contains(_uri))
        {
          string _ms = String.Format(Resources.SiteCollectionExist, _uri);
          if (MessageBox.Show(_ms, "Site creation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            return _wa.Sites[_uri];
          else
            _wa.Sites.Delete(_uri);
        }
        return _wa.Sites.Add(_uri, _owner, _email);
      }
      catch (Exception ex)
      {
        throw new ApplicationException(String.Format(Resources.CreateSiteCollectionFailed, ex.Message));
      }
    }
    internal static void DeploySolution(SPSite _site, FileInfo _featuteFile, string _featureName)
    {
      try
      {
        SPDocumentLibrary solutionGallery = (SPDocumentLibrary)_site.GetCatalog(SPListTemplateType.SolutionCatalog);
        SPFile file = solutionGallery.RootFolder.Files.Add(_featuteFile.Name, _featuteFile.OpenRead(), true);
        SPUserSolution solution = _site.Solutions.Add(file.Item.ID);
        SPFeatureDefinition _fd = _site.FeatureDefinitions[_featureName];
        Guid _fg = _fd.Id;
        _site.Features.Add(_fd.Id, false, SPFeatureDefinitionScope.Site );
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.DeploySolutionFailed, _site.Url, _featuteFile.Name, _featureName, ex.Message);
        throw new ApplicationException(_msg);
      }
    }
  }
}
