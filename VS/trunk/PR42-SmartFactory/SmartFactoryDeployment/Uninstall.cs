using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CAS.Lib.RTLib.Processes;
using CAS.SmartFactory.Deployment.Properties;
using CAS.SmartFactory.Deployment.Controls;

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
      if (UninstallUserControl != null)
        new ApplicationException("Uninstall is singleton, but new object is created");
      UninstallUserControl = this;
    }
    internal static Uninstall UninstallUserControl { get; private set; }

    internal void Uninstallation(InstallationStateData m_ApplicationState)
    {
      const string _src = "Uninstall";
      try
      {
        m_UninstallListBox.SelectedIndexChanged += new EventHandler(m_UninstallListBox_SelectedIndexChanged);
        Tracing.TraceEvent.TraceVerbose(33, _src, "Uninstall starting.");
        FarmHelpers.GetFarm();
        string _msg = string.Empty;
        if (FarmHelpers.Farm != null)
        {
          _msg = String.Format(Resources.GotAccess2Farm, FarmHelpers.Farm.Name, FarmHelpers.Farm.DisplayName, FarmHelpers.Farm.Status);
          m_UninstallListBox.AddMessage(_msg);
        }
        else
          throw new ApplicationException(Resources.GettingAccess2LocalFarm);
        FarmHelpers.GetWebApplication(m_ApplicationState.WebApplicationURL);
        if (FarmHelpers.WebApplication != null)
        {
          _msg = String.Format(Resources.ApplicationFound, m_ApplicationState.WebApplicationURL, FarmHelpers.WebApplication.Name, FarmHelpers.WebApplication.DisplayName);
          m_UninstallListBox.AddMessage(_msg);
        }
        else
          throw new ApplicationException(String.Format(Resources.GettingAccess2ApplicationFailed, m_ApplicationState.WebApplicationURL));
        m_UninstallListBox.AddMessage(String.Format("Trying to get access to the site collection at the Url = {0}", m_ApplicationState.SiteCollectionURL));
        m_SiteCollectionHelper = new SiteCollectionHelper(FarmHelpers.WebApplication, m_ApplicationState.SiteCollectionURL);
        m_UninstallListBox.AddMessage(String.Format("The site collection at the Url={0} has been opened.", m_SiteCollectionHelper.SiteCollection.Url));
        foreach (Solution _solution in from _sidx in m_ApplicationState.Solutions orderby _sidx.Priority descending select (_sidx))
        {
          if (_solution.Activated)
            try
            {
              foreach (var _fix in _solution.Fetures)
              {
                m_UninstallListBox.AddMessage(String.Format("Deactivating the feature {0}.", _fix.DisplayName));
                m_SiteCollectionHelper.DeactivateFeature(_fix.FetureGuid);
              }
            }
            catch (Exception ex)
            {
              m_UninstallListBox.AddMessage(String.Format(Resources.LastOperationFailedWithError, ex.Message));
            }
            finally
            {
              _solution.Activated = false;
            }
          else
          {
            m_UninstallListBox.AddMessage(String.Format("Skiped, the solution {0} is not active.", _solution.SolutionID));
          }
          try
          {
            switch (_solution.FeatureDefinitionScope)
            {
              case FeatureDefinitionScope.Farm:
                if (_solution.Deployed)
                {
                  m_UninstallListBox.AddMessage(String.Format("Retracing the solution {0}.", _solution.SolutionGuid));
                  FarmHelpers.RetrackSolution(_solution.SolutionGuid);
                }
                else
                  m_UninstallListBox.AddMessage(String.Format("Retracing is kiped, the solution {0} is not deployed.", _solution.SolutionGuid));
                break;
              case FeatureDefinitionScope.Site:
                if (_solution.Deployed)
                {
                  m_UninstallListBox.AddMessage(String.Format("Retracking the solution {0}.", _solution.SolutionGuid));
                  m_SiteCollectionHelper.RetracSolution(_solution.SolutionGuid);
                }
                else
                  m_UninstallListBox.AddMessage(String.Format("Retracing is skiped, the solution {0} is not deployed.", _solution.SolutionGuid));
                break;
              case FeatureDefinitionScope.None:
              default:
                break;
            }
          }
          catch (Exception ex)
          {
            m_UninstallListBox.AddMessage(String.Format(Resources.LastOperationFailedWithError, ex.Message));
          }
          finally
          {
            _solution.Deployed = false;
          }
        }
        if (m_ApplicationState.SiteCollectionCreated)
          try
          {
            m_UninstallListBox.AddMessage(String.Format("Deleting the site collection at Url = {0}.", m_ApplicationState.SiteCollectionURL));
            SiteCollectionHelper.DeleteSiteCollection(FarmHelpers.WebApplication, m_ApplicationState.SiteCollectionURL);
          }
          catch (Exception ex)
          {
            m_UninstallListBox.AddMessage(String.Format(Resources.LastOperationFailedWithError, ex.Message));
          }
          finally
          {
            m_ApplicationState.SiteCollectionCreated = false;
          }
        m_UninstallListBox.AddMessage("Uninstall finished.");
        m_ApplicationState.Save(Extenshions.GetFileInfo());
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.InstalationAbortRollback, ex.Message);
        MessageBox.Show(_msg, Resources.UninstallErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        Tracing.TraceEvent.TraceError(37, _src, String.Format("Uninstallation aborted with the error: {0}.", ex.Message));
        throw new ApplicationException(String.Format(Resources.LastOperationFailedWithError, ex.Message, ex));
      }
      finally
      {
        m_UninstallListBox.SelectedIndexChanged -= new EventHandler(m_UninstallListBox_SelectedIndexChanged);
        m_InstalationStatePropertyGrid.Refresh();
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
    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      try
      {
        //FileInfo _file = Extenshions.GetFileInfo();
        //m_UninstallListBox.AddMessage(String.Format("Loading the application installation state from the file at: {0}", _file.FullName));
        //m_InstallationStateData = InstallationStateData.Read(_file);
        //m_InstalationStatePropertyGrid.SelectedObject = m_InstallationStateData;
      }
      catch (Exception ex)
      {
        //m_UninstallListBox.AddMessage(Resources.SoftwareIsNotInstalled);
        //Tracing.TraceEvent.TraceVerbose(162, "OnLoad", ex.Message);
        //MessageBox.Show(Resources.SoftwareIsNotInstalled, Resources.RetrackCaption, MessageBoxButtons.OK, MessageBoxIcon.Question);
      }
      base.OnLoad(e);
    }

  }
}
