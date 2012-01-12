using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Administration;
using CAS.SmartFactory.Deployment.Properties;
using Microsoft.SharePoint;
using System.Threading;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CAS.SmartFactory.Deployment
{
  internal static class FarmHelpers
  {
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
    internal static SPSolution DeploySolution(System.IO.FileInfo _fi, SPWebApplication _wa, out Guid _solutionId)
    {
      try
      {
        SPSolution _sol = Farm.Solutions.Add(_fi.FullName);
        _solutionId = _sol.Id;
        Collection<SPWebApplication> _collection = new Collection<SPWebApplication>();
        _collection.Add(_wa);
        _sol.Deploy(DateTime.Now, true, _collection, false);
        int _round = 0;
        do
        {
          Thread.Sleep(200);
          if (_round++ > 200)
            throw new ApplicationException(Resources.DeplymentTimeout);
        } while (! _sol.Deployed );
        return _sol;
        //Debug.Assert(_sol.JobStatus == SPRunningJobStatus.Succeeded, "Job status error");
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.DeployFarmSolutionFailed, _fi.Name, ex.Message);
        throw new ApplicationException(_msg);
      }
    }
    internal static void DeactivateFeature(Guid _feature, SPSite _site)
    {
      try
      {
        _site.Features.Remove(_feature);
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.DeactivateFeatureFailed, ex.Message);
        throw new ApplicationException(_msg);
      }
    }
    internal static Guid FindSolution(string _FileName)
    {
      return (from _idx in Farm.Solutions where _idx.SolutionFile.Name == _FileName select _idx).First().Id;
    }
    internal static void RetrackSolution(Guid _solutionId)
    {
      string _name = "<no solution>";
      string _dislayName = "<no solution>";
      string _fileName = "<no solution>";
      try
      {
        SPSolution _sol = Farm.Solutions[_solutionId];
        if (_sol == null)
          throw new ApplicationException(Resources.CannotFindTheSolution);
        _name = _sol.Name;
        _dislayName = _sol.DisplayName;
        _fileName = _sol.SolutionFile.DisplayName;
        Farm.Solutions.Remove(_solutionId);
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.RetracDeploySolutionFailed, _name, _dislayName, _fileName, ex.Message);
        throw new ApplicationException(_msg);
      }
    }
  }
}
