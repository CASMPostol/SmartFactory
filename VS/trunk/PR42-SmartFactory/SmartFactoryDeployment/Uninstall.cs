using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CAS.Lib.RTLib.Processes;
using System.IO;
using CAS.SmartFactory.Deployment.Properties;

namespace CAS.SmartFactory.Deployment
{
  /// <summary>
  /// Uninstall
  /// </summary>
  public partial class Uninstall : Form
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="Uninstall"/> class.
    /// </summary>
    public Uninstall()
    {
      const string _src = "Uninstall";
      try
      {
        InitializeComponent();
        m_UninstallListBox.SelectedIndexChanged += new EventHandler(m_UninstallListBox_SelectedIndexChanged);
        m_TraceEvent.TraceVerbose(33, _src, "Uninstall starting.");
        FileInfo _file = GetFileInfo();
        m_UninstallListBox.AddMessage(String.Format("Loading the application installation state from the file at: {0}", _file.FullName));
        m_InstallationStateData = InstallationStateData.Read(_file);
        FarmHelpers.GetFarm();
        string _msg = string.Empty;
        if (FarmHelpers.Farm != null)
        {
          _msg = String.Format(Resources.GotAccess2Farm, FarmHelpers.Farm.Name, FarmHelpers.Farm.DisplayName, FarmHelpers.Farm.Status);
          m_UninstallListBox.AddMessage(_msg);
        }
        else
          throw new ApplicationException(Resources.GettingAccess2LocalFarm);
        FarmHelpers.GetWebApplication(m_InstallationStateData.WebApplicationURL);
        if (FarmHelpers.WebApplication != null)
        {
          _msg = String.Format(Resources.ApplicationFound, m_InstallationStateData.WebApplicationURL, FarmHelpers.WebApplication.Name, FarmHelpers.WebApplication.DisplayName);
          m_UninstallListBox.AddMessage(_msg);
        }
        else
          throw new ApplicationException(String.Format(Resources.GettingAccess2ApplicationFailed, m_InstallationStateData.WebApplicationURL));
        m_UninstallListBox.AddMessage(String.Format("Trying to get access to the site collection at the Url = {0}", m_InstallationStateData.SiteCollectionURL));
        if (!FarmHelpers.WebApplication.Sites.Names.Contains(m_InstallationStateData.SiteCollectionURL))
        {
          string _frmt = "The web application Name={0} does not contains the site collection at Url = {1}";
          m_UninstallListBox.AddMessage(String.Format(_frmt, FarmHelpers.WebApplication.Name, m_InstallationStateData.SiteCollectionURL));
        }
        else
        {
          m_SiteCollectionHelper = new SiteCollectionHelper(FarmHelpers.WebApplication, m_InstallationStateData.SiteCollectionURL);
          m_UninstallListBox.AddMessage(String.Format("The site collection at the Url={0} has been opened.", m_SiteCollectionHelper.SiteCollection.Url));
        }
        if (m_InstallationStateData.FarmFeaturesActivated)
        {
          try
          {
            m_UninstallListBox.AddMessage(String.Format("Deactivating the feature {0}.", m_InstallationStateData.FarmFetureId));
            m_SiteCollectionHelper.DeactivateFeature(m_InstallationStateData.FarmFetureId);
          }
          catch (Exception ex)
          {
            m_UninstallListBox.AddMessage(String.Format(Resources.LastOperationFailedWithError, ex.Message));
          }
        }
        if (m_InstallationStateData.FarmSolutionsDeployed)
        {
          try
          {
            m_UninstallListBox.AddMessage(String.Format("Retracing the solution {0}.", m_InstallationStateData.SolutionID));
            FarmHelpers.RetrackSolution(m_InstallationStateData.SolutionID);
          }
          catch (Exception ex)
          {
            m_UninstallListBox.AddMessage(String.Format(Resources.LastOperationFailedWithError, ex.Message));
          }
        }
        if (m_InstallationStateData.SiteCollectionFeturesActivated)
        {
          m_UninstallListBox.AddMessage(String.Format("Deactivating the feature {0}.", m_InstallationStateData.SiteCollectionFetureId));
          m_SiteCollectionHelper.DeactivateFeature(m_InstallationStateData.SiteCollectionFetureId);
        }
        if (m_InstallationStateData.SiteCollectionSolutionsDeployed)
          m_UninstallListBox.AddMessage(String.Format("Retracking the solution {0}.", m_InstallationStateData.SiteCollectionFetureId));
        if (m_InstallationStateData.SiteCollectionCreated)
        {
          m_UninstallListBox.AddMessage(String.Format("Deleting the site collection at Url = {0}.", m_InstallationStateData.SiteCollectionURL));
          try
          {
            SiteCollectionHelper.DeleteSiteCollection(FarmHelpers.WebApplication, m_InstallationStateData.SiteCollectionURL);
          }
          catch (Exception ex)
          {
            m_UninstallListBox.AddMessage(String.Format(Resources.LastOperationFailedWithError, ex.Message));
          }
        }
        m_UninstallListBox.AddMessage("Uninstall finished.");
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.InstalationAbortRollback, ex.Message);
        MessageBox.Show(_msg, Resources.UninstallErrorCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        m_TraceEvent.TraceError(37, _src, String.Format("Uninstallation aborted with the error: {0}.", ex.Message));
        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      }
      finally
      {
        m_UninstallListBox.SelectedIndexChanged -= new EventHandler(m_UninstallListBox_SelectedIndexChanged);
      }
    }
    private SiteCollectionHelper m_SiteCollectionHelper;
    private void m_UninstallListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListBox _lb = (ListBox)sender;
      m_TraceEvent.TraceInformation(521, "Uninstall", _lb.Items[_lb.Items.Count - 1].ToString());
    }
    private FileInfo GetFileInfo()
    {
      string path = Path.Combine(Application.StartupPath, Properties.Settings.Default.InstallationStateFileName);
      return new FileInfo(path);
    }
    private TraceEvent m_TraceEvent = new TraceEvent("SharePoint.Deployment");
    private InstallationStateData m_InstallationStateData = null;
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
    private void m_CloseButton_Click(object sender, EventArgs e)
    {
      this.Close();
    }
  }
}
