using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using CAS.SmartFactory.Deployment.Properties;
using Microsoft.SharePoint.Administration;
using CAS.SmartFactory.Deployment.Controls;

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
    internal static SPSolution DeploySolution(System.IO.FileInfo _fi, TimeSpan _timeout)
    {
      return DeploySolution(_fi, null, _timeout);
    }
    internal static SPSolution DeploySolution(System.IO.FileInfo _fi, SPWebApplication _wa, TimeSpan _timeout)
    {
      try
      {
        SPSolution _sol = Farm.Solutions.Add(_fi.FullName);
        if (_wa != null)
        {
          Collection<SPWebApplication> _collection = new Collection<SPWebApplication>();
          _collection.Add(_wa);
          _sol.Deploy(DateTime.Now, true, _collection, true);
        }
        else
          _sol.Deploy(DateTime.Now, true, false);
        Stopwatch _runTime = new Stopwatch();
        _runTime.Start();
        while (!_sol.Deployed)
        {
          Thread.Sleep(200);
          string _msg = String.Format(
           "Waiting for the solution to be deployed DeploymentState={0}, LastOperationResult={1}, Status={2}",
           _sol.DeploymentState,
           _sol.LastOperationResult,
           _sol.Status);
          if (_sol.JobExists)
          {
            _msg += String.Format(" JobStatus={0}", _sol.JobStatus);
          }
          Tracing.TraceEvent.TraceVerbose(80, "DeploySolution", _msg);
          if (_runTime.Elapsed > _timeout) 
          {
            string _tom = String.Format(Resources.DeplymentTimeout,
              _sol.DeploymentState,
              _sol.Status,
              _sol.LastOperationResult,
              _sol.LastOperationDetails);
            throw new ApplicationException(_tom);
          } //if
        }; //while (!_sol.Deployed)
        Tracing.TraceEvent.TraceVerbose(81, "DeploySolution", String.Format(Resources.DeploymentSuccess, _runTime.Elapsed));
        return _sol;
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.DeployFarmSolutionFailed, _fi.Name, ex.Message);
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
