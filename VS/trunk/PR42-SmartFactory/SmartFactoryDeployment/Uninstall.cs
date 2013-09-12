﻿using System;
using System.Linq;
using System.Windows.Forms;
using CAS.SmartFactory.Deployment.Controls;
using CAS.SmartFactory.Deployment.Package;
using CAS.SmartFactory.Deployment.Properties;

namespace CAS.SmartFactory.Deployment
{
  /// <summary>
  /// Uninstall
  /// </summary>
  public partial class Uninstall : UserControl
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Uninstall"/> class.
    /// </summary>
    public Uninstall()
    {
      InitializeComponent();
    }
    internal void Uninstallation(InstallationStateData m_ApplicationState)
    {
      const string _src = "Uninstall";
      m_StatePropertyGrid.SelectedObject = m_ApplicationState.Wrapper;
      try
      {
        m_ProgresslListBox.SelectedIndexChanged += new EventHandler(m_UninstallListBox_SelectedIndexChanged);
        Tracing.TraceEvent.TraceVerbose(33, _src, "Uninstall starting.");
        FarmHelpers.GetFarm();
        string _msg = string.Empty;
        if (FarmHelpers.Farm != null)
        {
          _msg = String.Format(Resources.FarmGotAccess, FarmHelpers.Farm.Name, FarmHelpers.Farm.DisplayName, FarmHelpers.Farm.Status);
          m_ProgresslListBox.AddMessage(_msg);
        }
        else
          throw new ApplicationException(Resources.GettingAccess2LocalFarm);
        FarmHelpers.GetWebApplication(m_ApplicationState.WebApplicationUri);
        if (FarmHelpers.WebApplication != null)
        {
          _msg = String.Format(Resources.ApplicationFound, m_ApplicationState.WebApplicationURL, FarmHelpers.WebApplication.Name, FarmHelpers.WebApplication.DisplayName);
          m_ProgresslListBox.AddMessage(_msg);
        }
        else
          throw new ApplicationException(String.Format(Resources.GettingAccess2ApplicationFailed, m_ApplicationState.WebApplicationURL));
        m_ProgresslListBox.AddMessage(String.Format("Trying to get access to the site collection at the Url = {0}", m_ApplicationState.SiteCollectionURL));
        m_SiteCollectionHelper = new SiteCollectionHelper(FarmHelpers.WebApplication, m_ApplicationState.SiteCollectionURL);
        m_ProgresslListBox.AddMessage(String.Format("The site collection at the Url={0} has been opened.", m_SiteCollectionHelper.SiteCollection.Url));
        foreach (Solution _solution in from _sidx in m_ApplicationState.Solutions orderby _sidx.Priority descending select (_sidx))
        {
          if (_solution.Activated)
            try
            {
              foreach (var _fix in _solution.Fetures)
              {
                m_ProgresslListBox.AddMessage(String.Format("Deactivating the feature {0}.", _fix.DisplayName));
                m_SiteCollectionHelper.DeactivateFeature(_fix.FetureGuid, _solution.SPFeatureDefinitionScope);
              }
            }
            catch (Exception ex)
            {
              m_ProgresslListBox.AddMessage(String.Format(Resources.LastOperationFailedWithError, ex.Message));
            }
            finally
            {
              _solution.Activated = false;
            }
          else
          {
            m_ProgresslListBox.AddMessage(String.Format("Skiped the featurs dectivation step, because the solution {0} is not active.", _solution.SolutionID));
          }
          try
          {
            switch (_solution.FeatureDefinitionScope)
            {
              case FeatureDefinitionScope.Farm:
                if (_solution.Deployed)
                {
                  m_ProgresslListBox.AddMessage(String.Format("Retracing the solution {0}.", _solution.SolutionGuid));
                  FarmHelpers.RetrackSolution(_solution.SolutionGuid);
                }
                else
                  m_ProgresslListBox.AddMessage(String.Format("Retracing is skiped, the solution {0} is not deployed.", _solution.SolutionGuid));
                break;
              case FeatureDefinitionScope.Site:
                if (_solution.Deployed)
                {
                  m_ProgresslListBox.AddMessage(String.Format("Retracking the solution {0}.", _solution.SolutionGuid));
                  m_SiteCollectionHelper.RetracSolution(_solution.SolutionGuid);
                }
                else
                  m_ProgresslListBox.AddMessage(String.Format("Retracing is skiped, the solution {0} is not deployed.", _solution.SolutionGuid));
                break;
              case FeatureDefinitionScope.None:
              default:
                break;
            }
          }
          catch (Exception ex)
          {
            m_ProgresslListBox.AddMessage(String.Format(Resources.LastOperationFailedWithError, ex.Message));
          }
          finally
          {
            _solution.Deployed = false;
          }
        }
        if (m_ApplicationState.SiteCollectionCreated)
          try
          {
            m_ProgresslListBox.AddMessage(String.Format("Deleting the site collection at path = {0}.", m_ApplicationState.SiteCollectionURL));
            SiteCollectionHelper.DeleteSiteCollection(FarmHelpers.WebApplication, m_ApplicationState.SiteCollectionURL);
          }
          catch (Exception ex)
          {
            m_ProgresslListBox.AddMessage(String.Format(Resources.LastOperationFailedWithError, ex.Message));
          }
          finally
          {
            m_ApplicationState.SiteCollectionCreated = false;
          }
        m_ProgresslListBox.AddMessage("Uninstall finished.");
        m_ApplicationState.Save();
      }
      catch (Exception ex)
      {
        try
        {
          m_ApplicationState.Save();
        }
        catch (Exception _SaveEx)
        {
          m_ProgresslListBox.AddMessage(_SaveEx.Message);
        }
        string _msg = String.Format(Resources.InstalationAbortRollback, ex.Message);
        MessageBox.Show(_msg, Resources.UninstallErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Tracing.TraceEvent.TraceError(37, _src, String.Format("Uninstallation aborted with the error: {0}.", ex.Message));
        throw new ApplicationException(String.Format(Resources.LastOperationFailedWithError, ex.Message, ex));
      }
      finally
      {
        m_ProgresslListBox.SelectedIndexChanged -= new EventHandler(m_UninstallListBox_SelectedIndexChanged);
        m_StatePropertyGrid.Refresh();
      }
    }
    private SiteCollectionHelper m_SiteCollectionHelper;
    private void m_UninstallListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListBox _lb = (ListBox)sender;
      Tracing.TraceEvent.TraceInformation(521, "Uninstall", _lb.Items[_lb.Items.Count - 1].ToString());
    }
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (m_SiteCollectionHelper != null)
          m_SiteCollectionHelper.Dispose();
        if (components != null)
          components.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}