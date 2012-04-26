using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using CAS.SmartFactory.Deployment.Controls;
using CAS.SmartFactory.Deployment.Package;
using CAS.SmartFactory.Deployment.Properties;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace CAS.SmartFactory.Deployment
{
  /// <summary>
  /// Dialog collecting setup informations
  /// </summary>
  public partial class SetUpData : Form
  {
    #region public
    /// <summary>
    /// Initializes a new instance of the <see cref="SetUpData"/> class.
    /// </summary>
    public SetUpData()
    {
      InitializeComponent();
      m_trace.Trace.TraceVerbose(26, "SetUpData", "Starting the application");
      Manual = Properties.Settings.Default.ManualMode;
      State = ProcessState.SetupDataDialog;
    }
    internal bool Manual { get; set; }
    #endregion

    #region private
    /// <summary>
    /// Current state of the installation.
    /// </summary>
    private enum ProcessState
    {
      SetupDataDialog,
      ManualSelection,
      Validation,
      Installation,
      Finisched,
      Uninstall
    }
    private ProcessState m_State;
    private SiteCollectionHelper m_SiteCollectionHelper = null;
    private InstallationStateData m_ApplicationState;
    private enum LocalEvent
    {
      Previous, Next, Cancel, Exception, EnterState, Uninstall
    }
    private class StateMachineEvenArgs : EventArgs
    {
      internal LocalEvent Event { get; private set; }
      public StateMachineEvenArgs(LocalEvent _event)
      {
        Event = _event;
      }
    }
    private class StateMachineExceptionEventArgs : StateMachineEvenArgs
    {
      internal Exception Exception { get; private set; }
      public StateMachineExceptionEventArgs(Exception _exception)
        : base(LocalEvent.Exception)
      {
        Exception = _exception;
        Tracing.TraceEvent.TraceError(68, "StateMachineExceptionEventArgs", String.Format("The following error envountered: {0}.", _exception.Message));
      }
    }
    /// <summary>
    /// Gets or sets the state of the installation.
    /// </summary>
    /// <value>
    /// The state of the installation.
    /// </value>
    private ProcessState State
    {
      get
      {
        return m_State;
      }
      set
      {
        m_State = value;
        m_trace.Trace.TraceVerbose(86, "State", String.Format("Entered the state: {0}.", value));
        foreach (TabPage _page in m_ContentTabControl.TabPages)
          m_ContentTabControl.TabPages.Remove(_page);
        switch (value)
        {
          case ProcessState.SetupDataDialog:
            m_ContentTabControl.TabPages.Add(m_ApplicationSetupDataDialogPanel);
            break;
          case ProcessState.ManualSelection:
            m_ContentTabControl.TabPages.Add(m_ManualSelectionPanel);
            break;
          case ProcessState.Validation:
            m_ContentTabControl.TabPages.Add(m_ValidationPanel);
            break;
          case ProcessState.Installation:
            m_ContentTabControl.TabPages.Add(m_ApplicationInstalationPanel);
            break;
          case ProcessState.Finisched:
            m_ContentTabControl.TabPages.Add(m_FinischedPanel);
            break;
          case ProcessState.Uninstall:
            m_ContentTabControl.TabPages.Add(m_UninstallPanel);
            break;
          default:

            break;
        }
        //this.AutoSize = true;
        this.PerformLayout();
        this.Refresh();
        StateMachine(new StateMachineEvenArgs(LocalEvent.EnterState));
      }
    }
    private void StateMachine(StateMachineEvenArgs _event)
    {
      bool _stay = false;
      do
      #region do
      {
        _stay = false;
        try
        {
          switch (State)
          {
            case ProcessState.ManualSelection:
              #region ManualSelection
              //TODO to be removed ot updated http://itrserver/Bugs/BugDetail.aspx?bid=3302
              switch (_event.Event)
              {
                case LocalEvent.Previous:
                  State = ProcessState.SetupDataDialog;
                  break;
                case LocalEvent.Next:
                  ExitlInstallation(DialogResult.OK);
                  break;
                case LocalEvent.Uninstall:
                  StateError();
                  break;
                case LocalEvent.Cancel:
                  StateError();
                  break;
                case LocalEvent.Exception:
                  Exception _eea = ((StateMachineExceptionEventArgs)_event).Exception;
                  if (MessageBox.Show(
                    String.Format(Resources.LastOperationFailed, _eea.Message),
                    Resources.CaptionOperationFailure,
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                    CancelInstallation();
                  break;
                case LocalEvent.EnterState:
                  m_PreviousButton.Visible = true;
                  m_NextButton.Visible = true;
                  m_NextButton.Text = Resources.NextButtonTextEXIT;
                  m_CancelButton.Visible = false;
                  break;
                default:
                  break;
              }
              break;
              #endregion
            case ProcessState.SetupDataDialog:
              #region SetupDataDialog
              switch (_event.Event)
              {
                case LocalEvent.Previous:
                  StateError();
                  break;
                case LocalEvent.Next:
                  m_ApplicationState.Save();
                  State = ProcessState.Validation;
                  break;
                case LocalEvent.Uninstall:
                  StateError();
                  break;
                case LocalEvent.Cancel:
                  CancelInstallation();
                  break;
                case LocalEvent.Exception:
                  Exception _eea = ((StateMachineExceptionEventArgs)_event).Exception;
                  MessageBox.Show(
                    String.Format(Resources.InstalationAbortRollback, _eea.Message),
                    Resources.CaptionOperationFailure,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                  ExitlInstallation(DialogResult.Abort);
                  break;
                case LocalEvent.EnterState:
                  m_UninstallButton.Visible = false;
                  m_PreviousButton.Visible = false;
                  m_NextButton.Enabled = true;
                  m_NextButton.Text = Resources.NextButtonTextNext;
                  m_CancelButton.Visible = true;
                  m_ApplicationState = InstallationStateData.Read();
                  m_SetupPropertyGrid.SelectedObject = m_ApplicationState.Wrapper;
                  break;
                default:
                  break;
              }
              break;
              #endregion
            case ProcessState.Validation:
              #region Validation
              switch (_event.Event)
              {
                case LocalEvent.Previous:
                  State = ProcessState.SetupDataDialog;
                  break;
                case LocalEvent.Next:
                  m_ApplicationState.Save();
                  if (Manual)
                    State = ProcessState.ManualSelection;
                  else
                  {
                    State = ProcessState.Installation;
                  }
                  break;
                case LocalEvent.Uninstall:
                  State = ProcessState.Uninstall;
                  break;
                case LocalEvent.Cancel:
                  CancelInstallation();
                  break;
                case LocalEvent.Exception:
                  Exception _eea = ((StateMachineExceptionEventArgs)_event).Exception;
                  MessageBox.Show(
                    String.Format(Resources.InstalationAbortRollback, _eea.Message),
                    Resources.CaptionOperationFailure,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                  ExitlInstallation(DialogResult.Abort);
                  break;
                case LocalEvent.EnterState:
                  m_ValidationListBox.Items.Clear();
                  m_ValidationPropertyGrid.SelectedObject = m_ApplicationState.Wrapper;
                  m_ValidationPropertyGrid.Text = Resources.InstallationProperties;
                  m_PreviousButton.Visible = true;
                  m_NextButton.Visible = true;
                  m_NextButton.Text = Resources.NextButtonTextNext;
                  m_CancelButton.Visible = true;
                  if (!VerifyPrerequisites())
                  {
                    MessageBox.Show(Resources.CheckIinProcessFfailed, Resources.CheckIinProcessFfailedCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    m_NextButton.Enabled = false;
                  }
                  else
                    m_NextButton.Text = Resources.NextButtonTextInstall;
                  m_NextButton.Visible = true;
                  m_NextButton.Enabled = true;

                  if (m_ApplicationState.SiteCollectionCreated)
                    m_UninstallButton.Visible = true;
                  else
                    m_UninstallButton.Visible = false;
                  break;
                default:
                  break;
              }
              break;
              #endregion
            case ProcessState.Installation:
              #region Installation
              switch (_event.Event)
              {
                case LocalEvent.Previous:
                  State = ProcessState.Validation;
                  break;
                case LocalEvent.Next:
                  ExitlInstallation(DialogResult.Abort);
                  break;
                case LocalEvent.Cancel:
                  StateError();
                  break;
                case LocalEvent.Exception:
                  Exception _eea = ((StateMachineExceptionEventArgs)_event).Exception;
                  MessageBox.Show(
                    String.Format(Resources.InstalationAbortRollback, _eea.Message),
                    Resources.CaptionOperationFailure,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                  break;
                case LocalEvent.EnterState:
                  m_InstallationProgresListBox.Items.Clear();
                  m_PreviousButton.Visible = true;
                  m_NextButton.Enabled = true;
                  m_NextButton.Enabled = false;
                  m_NextButton.Text = Resources.FinishButtonText;
                  m_UninstallButton.Visible = false;
                  m_CancelButton.Visible = false;
                  this.Refresh();
                  Install();
                  m_NextButton.Enabled = true;
                  m_InstallationProgressBar.Value = m_InstallationProgressBar.Maximum;
                  m_InstallationProgresListBox.Enabled = true;
                  break;
                default:
                  break;
              }
              break;
              #endregion
            case ProcessState.Finisched:
              #region Finisched
              switch (_event.Event)
              {
                case LocalEvent.Previous:
                case LocalEvent.Next:
                case LocalEvent.Cancel:
                case LocalEvent.Exception:
                  Exception _eea = ((StateMachineExceptionEventArgs)_event).Exception;
                  MessageBox.Show(
                    String.Format(Resources.InstalationAbortRollback, _eea.Message),
                    Resources.CaptionOperationFailure,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                  ExitlInstallation(DialogResult.Abort);
                  break;
                case LocalEvent.EnterState:
                  m_PreviousButton.Visible = false;
                  m_NextButton.Enabled = true;
                  m_NextButton.Text = Resources.FinishButtonText;
                  m_UninstallButton.Visible = true;
                  m_CancelButton.Visible = false;
                  break;
                default:
                  break;
              }
              break;
              #endregion
            case ProcessState.Uninstall:
              #region Uninstall
              switch (_event.Event)
              {
                case LocalEvent.Previous:
                  StateError();
                  break;
                case LocalEvent.Next:
                  ExitlInstallation(DialogResult.OK);
                  break;
                case LocalEvent.Cancel:
                  StateError();
                  break;
                case LocalEvent.Exception:
                  Exception _eea = ((StateMachineExceptionEventArgs)_event).Exception;
                  MessageBox.Show(
                    String.Format(Resources.InstalationAbortRollback, _eea.Message),
                    Resources.CaptionOperationFailure,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                  State = ProcessState.Validation;
                  break;
                case LocalEvent.EnterState:
                  m_PreviousButton.Visible = false;
                  m_NextButton.Enabled = true;
                  m_UninstallUserControl.Uninstallation(m_ApplicationState);
                  m_NextButton.Text = Resources.FinishButtonText;
                  m_UninstallButton.Visible = false;
                  m_CancelButton.Visible = false;
                  break;
                default:
                  break;
              }
              break;
              #endregion
            default:
              throw new ApplicationException("StateMachine - wrong state");
          }
        }
        catch (Exception ex)
        {
          _stay = true;
          _event = new StateMachineExceptionEventArgs(ex);
        }
      }
      #endregion
      while (_stay);
    }
    private bool VerifyPrerequisites()
    {
      try
      {
        SPSecurity.RunWithElevatedPrivileges(delegate()
        {
          m_ValidationListBox.SelectedValueChanged += new EventHandler(this.m_ListBox_TextChanged);
          m_ValidationListBox.AddMessage(Resources.ValidationProcessStarting);
          m_ValidationPropertyGrid.Refresh();
          FarmHelpers.GetFarm();
          string _msg = string.Empty;
          if (FarmHelpers.Farm != null)
          {
            _msg = String.Format(Resources.FarmGotAccess, FarmHelpers.Farm.Name, FarmHelpers.Farm.DisplayName, FarmHelpers.Farm.Status);
            m_ValidationListBox.AddMessage(_msg);
          }
          else
            throw new ApplicationException(Resources.GettingAccess2LocalFarm);
          FarmHelpers.GetWebApplication(m_ApplicationState.WebApplicationUri);
          if (FarmHelpers.WebApplication != null)
          {
            _msg = String.Format(Resources.ApplicationFound, m_ApplicationState.WebApplicationURL, FarmHelpers.WebApplication.Name, FarmHelpers.WebApplication.DisplayName);
            m_ValidationListBox.AddMessage(_msg);
          }
          else
            throw new ApplicationException(String.Format(Resources.GettingAccess2ApplicationFailed, m_ApplicationState.WebApplicationURL));
          bool _spsiteExist = FarmHelpers.WebApplication.Sites.Names.Contains(m_ApplicationState.SiteCollectionURL);
          if (m_ApplicationState.SiteCollectionCreated)
          {
            if (_spsiteExist)
              m_ValidationListBox.AddMessage(Resources.SiteExistAndReuse);
            else
            {
              UriBuilder _spuri = new UriBuilder(m_ApplicationState.WebApplicationUri) { Path = m_ApplicationState.SiteCollectionURL };
              string _siteNotExist = String.Format("Cannot get access to the installed site collection at URL = {0}", _spuri);
              throw new ApplicationException(_siteNotExist);
            }
          }
          else
            if (_spsiteExist)
            {
              //SiteCollectionHelper.SiteCollection = FarmHelpers.WebApplication.Sites[m_ApplicationState.SiteCollectionURL];
              string _ms = String.Format(Resources.SiteCollectionExist, m_ApplicationState.SiteCollectionURL);
              SiteCollectionHelper.DeleteIfExist = MessageBox.Show(
                _ms,
                Resources.SiteCreation,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) != DialogResult.Yes;
              if (SiteCollectionHelper.DeleteIfExist)
                m_ValidationListBox.AddMessage(Resources.SiteExistAndDelete);
              else
                m_ValidationListBox.AddMessage(Resources.SiteExistAndReuse);
              this.Refresh();
            }
          m_ValidationListBox.AddMessage(Resources.ValidationProcessSuccessfullyFinished);
          m_ValidationPropertyGrid.Refresh();
        });
        return true;
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.ValidationProcessFailed, ex.Message);
        m_ValidationListBox.AddMessage(_msg);
        return false;
      }
      finally
      {
        m_ValidationListBox.SelectedValueChanged -= new EventHandler(this.m_ListBox_TextChanged);
      }
    }
    private void Install()
    {
      this.UseWaitCursor = true;
      try
      {
        m_InstallationProgressBar.Minimum = 0;
        m_InstallationProgressBar.Maximum = 10;
        m_InstallationProgresListBox.SelectedValueChanged += new EventHandler(m_ListBox_TextChanged);
        m_InstallationProgresListBox.AddMessage("Creating SPSite ... ");
        m_SiteCollectionHelper = SiteCollectionHelper.CreateSPSite(
          FarmHelpers.WebApplication,
          m_ApplicationState.SiteCollectionURL,
          m_ApplicationState.Title,
          m_ApplicationState.Description,
          m_ApplicationState.LCID,
          m_ApplicationState.SiteTemplate,
          m_ApplicationState.OwnerLogin,
          m_ApplicationState.OwnerName,
          m_ApplicationState.OwnerEmail);
        m_ApplicationState.SiteCollectionCreated = true;
        m_InstallationProgresListBox.AddMessage("Site collection created");
        foreach (Solution _sltn in m_ApplicationState.SolutionsToInstall.Values)
        {
          FileInfo _fi = _sltn.SolutionFileInfo();
          m_InstallationProgresListBox.AddMessage(String.Format("Deploying solution: {0}", _fi.Name));
          switch (_sltn.FeatureDefinitionScope)
          {
            case FeatureDefinitionScope.Farm:
              TimeSpan _timeout = new TimeSpan(0, 0, Settings.Default.SolutionDeploymentTimeOut);
              string _waitingForCompletion = String.Format("Waiting for completion .... It could take up to {0} s. ", _timeout);
              m_InstallationProgresListBox.AddMessage(_waitingForCompletion);
              SPSolution _sol = null;
              if (_sltn.Global)
                _sol = FarmHelpers.DeploySolution(_fi, _timeout);
              else
                _sol = FarmHelpers.DeploySolution(_fi, FarmHelpers.WebApplication, _timeout);
              _sltn.SolutionGuid = _sol.Id;
              m_InstallationProgresListBox.AddMessage(String.Format("Solution deployed Name={0}, Deployed={1}, DeploymentState={2}, DisplayName={3} Status={4}", _sol.Name, _sol.Deployed, _sol.DeploymentState, _sol.DisplayName, _sol.Status));
              break;
            case FeatureDefinitionScope.Site:
              SPUserSolution _solution = null;
              _solution = m_SiteCollectionHelper.DeploySolution(_fi);
              _sltn.SolutionGuid = _solution.SolutionId;
              m_InstallationProgresListBox.AddMessage(String.Format("Solution deployed: {0}", _solution.Name));
              break;
            case FeatureDefinitionScope.None:
            default:
              throw new ApplicationException("Wrong FeatureDefinitionScope in the configuration file");
          }
          _sltn.Deployed = true;
          foreach (Feature _fix in _sltn.Fetures)
          {
            bool _repeat;
            do
            {
              _repeat = false;
              try
              {
                m_InstallationProgresListBox.AddMessage(String.Format("Activating Feature: {0} at: {1}", _fix.FetureGuid, m_SiteCollectionHelper.SiteCollection.Url));
                SPFeature _ffeature = m_SiteCollectionHelper.ActivateFeature(_fix.FetureGuid, _sltn.SPFeatureDefinitionScope);
                m_InstallationProgresListBox.AddMessage(String.Format("Feature activated : {0}", _ffeature.Definition.DisplayName));
                _fix.DisplayName = _ffeature.Definition.DisplayName;
                _fix.Version = _ffeature.Version.ToString();
                _fix.SPScope = _ffeature.Definition.Scope;
              }
              catch (Exception ex)
              {
                //TODO add message about exception http://itrserver/Bugs/BugDetail.aspx?bid=3321
                switch (MessageBox.Show(Resources.FeatureActivationFailureMBox, "Install ActivateFeature", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2))
                {
                  case DialogResult.Abort:
                    throw ex;
                  case DialogResult.Retry:
                    _repeat = true;
                    break;
                  case DialogResult.Ignore:
                  case DialogResult.No:
                  case DialogResult.None:
                  case DialogResult.OK:
                  case DialogResult.Cancel:
                  case DialogResult.Yes:
                  default:
                    break;
                }
              }
            } while (_repeat);
          }//foreach (Feature _fix in _sltn.Fetures)
          _sltn.Activated = true;
        } //foreach (Solution _sltn 
        m_ApplicationState.Save();
        m_InstallationProgresListBox.AddMessage("Product installation successfully completed");
      }
      catch (Exception ex)
      {
        try
        {
          m_ApplicationState.Save();
        }
        catch (Exception _SaveEx)
        {
          m_InstallationProgresListBox.AddMessage(_SaveEx.Message);
        }
        string _msg = String.Format(Resources.LastOperationFailedWithError, ex.Message);
        m_InstallationProgresListBox.AddMessage(_msg);
        throw new ApplicationException(_msg, ex);
      }
      finally
      {
        this.UseWaitCursor = false;
        m_InstallationProgresListBox.SelectedValueChanged -= new EventHandler(m_ListBox_TextChanged);
      }
    }
    private void ExitlInstallation(DialogResult _res)
    {
      Tracing.TraceEvent.TraceVerbose(532, "ExitlInstallation", string.Format("Closing the application with the result {0}", _res));
      this.DialogResult = _res;
      this.Close();
    }
    private void CancelInstallation()
    {
      if (this.DialogResult != System.Windows.Forms.DialogResult.OK)
        if (
          MessageBox.Show(Resources.AreYouSure2Cancel,
          Resources.CancelInstallationCaption,
          MessageBoxButtons.OKCancel,
          MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.OK)
          return;
      this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.Close();
    }
    private void StateError()
    {
      Debug.Assert(false, "State error");
    }
    #region base override
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
    #endregion

    #region Event handlers
    private void m_ListBox_TextChanged(object sender, EventArgs e)
    {
      ListBox _lb = (ListBox)sender;
      m_trace.Trace.TraceInformation(521, "SetUpData", _lb.Items[_lb.Items.Count - 1].ToString());
      if (m_InstallationProgressBar.Value == m_InstallationProgressBar.Maximum)
        m_InstallationProgressBar.Maximum *= 2;
      m_InstallationProgressBar.Value++;
      m_InstallationProgressBar.Refresh();
    }
    private void m_CancelButton_Click(object sender, EventArgs e)
    {
      StateMachine(new StateMachineEvenArgs(LocalEvent.Cancel));
    }
    private void m_PreviousButton_Click(object sender, EventArgs e)
    {
      StateMachine(new StateMachineEvenArgs(LocalEvent.Previous));
    }
    private void m_NextButton_Click(object sender, EventArgs e)
    {
      StateMachine(new StateMachineEvenArgs(LocalEvent.Next));
    }
    private void m_UninstallButton_Click(object sender, EventArgs e)
    {
      StateMachine(new StateMachineEvenArgs(LocalEvent.Uninstall));
    }
    private void m_CreateWebCollectionButton_Click(object sender, EventArgs e)
    {
      try
      {
        SiteCollectionHelper.CreateSPSite(
          FarmHelpers.WebApplication,
            m_ApplicationState.SiteCollectionURL,
            m_ApplicationState.Title,
            m_ApplicationState.Description,
            m_ApplicationState.LCID,
            m_ApplicationState.SiteTemplate,
            m_ApplicationState.OwnerLogin,
            m_ApplicationState.OwnerName,
            m_ApplicationState.OwnerEmail);
        m_ApplicationState.SiteCollectionCreated = true;
      }
      catch (Exception ex)
      {
        this.StateMachine(new StateMachineExceptionEventArgs(ex));
      }
    }
    private void m_DeployActiwateWebsiteButton_Click(object sender, EventArgs e)
    {
      try
      {
        //TODO must be clenup 
        //FileInfo _fi = GetFile(Settings.Default.SiteCollectionSolutionFileName);
        //m_SiteCollectionHelper.DeploySolution(_fi);
        //m_SiteCollectionHelper.ActivateFeature(m_ApplicationState.SiteCollectionFetureId, Microsoft.SharePoint.SPFeatureDefinitionScope.Site);
      }
      catch (Exception ex)
      {
        this.StateMachine(new StateMachineExceptionEventArgs(ex));
      }
    }
    private void m_DeployDaschboardsButton_Click(object sender, EventArgs e)
    {
      this.UseWaitCursor = true;
      try
      {
        //TODO must be clean up 
        //FileInfo _fi = GetFile(Settings.Default.FarmSolutionFileName);
        //if (m_SiteCollectionHelper == null)
        //  throw new ApplicationException(Resources.SiteCollectionNotExist);
        //Guid _solutionID;
        //FarmHelpers.DeploySolution(_fi, FarmHelpers.WebApplication, out _solutionID);
        //m_ApplicationState.SolutionID = _solutionID;
        //m_SiteCollectionHelper.ActivateFeature(m_ApplicationState.FarmFetureId, Microsoft.SharePoint.SPFeatureDefinitionScope.Farm);
      }
      catch (Exception ex)
      {
        this.StateMachine(new StateMachineExceptionEventArgs(ex));
      }
      finally { this.UseWaitCursor = false; }
    }
    private void m_DeleteWebsite_Click(object sender, EventArgs e)
    {
      try
      {
        SiteCollectionHelper.DeleteSiteCollection(FarmHelpers.WebApplication, m_ApplicationState.SiteCollectionURL);
      }
      catch (Exception ex)
      {
        this.StateMachine(new StateMachineExceptionEventArgs(ex));
      }
    }
    private void m_RetrackDaschboard_Click(object sender, EventArgs e)
    {
      try
      {
        //TODO must be clean up
        //m_SiteCollectionHelper.DeactivateFeature(m_ApplicationState.FarmFetureId);
        //FarmHelpers.RetrackSolution(m_ApplicationState.SolutionID);
      }
      catch (Exception ex)
      {
        this.StateMachine(new StateMachineExceptionEventArgs(ex));
      }
    }
    #endregion

    #endregion

  }
}
