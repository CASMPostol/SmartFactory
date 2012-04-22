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
      {
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
                  ExitlInstallation( DialogResult.OK  );
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
                  //TODO add validation to:
                  m_ApplicationState.GetUri(m_ApplicationURLTextBox.Text);
                  m_ApplicationState.GetSiteCollectionURL(m_SiteCollectionUrlTextBox.Text);
                  m_ApplicationState.GetOwnerLogin(m_OwnerLoginTextBox.Text);
                  m_ApplicationState.GetOwnerEmail(m_OwnerEmailTextBox.Text);
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
                  InitSetupData();
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
                  m_ValidationPropertyGrid.SelectedObject = m_ApplicationState;
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
                  StateError();
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
                  State = ProcessState.Validation;
                  break;
                case LocalEvent.EnterState:
                  m_InstallationProgresListBox.Items.Clear();
                  m_PreviousButton.Visible = false;
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
                  ExitlInstallation(DialogResult.Abort);
                  break;
                case LocalEvent.Cancel:
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
      } while (_stay);
    }
    private void InitSetupData()
    {
      m_ApplicationState = InstallationStateData.Read();
      m_SetupPropertyGrid.SelectedObject = m_ApplicationState;
      //string _msg = String.Format(Resources.ConfigurationReadInstallationStateDataFailure, ex.Message);
      //MessageBox.Show(_msg, Resources.RetrackCaption, MessageBoxButtons.OK, MessageBoxIcon.Question);
      try
      {
        SPSecurity.RunWithElevatedPrivileges(delegate()
          {
            m_ApplicationURLTextBox.Text = Uri.UriSchemeHttp + Uri.SchemeDelimiter + SPServer.Local.Address;
            m_SiteCollectionUrlTextBox.Text = Uri.UriSchemeHttp + Uri.SchemeDelimiter + m_ApplicationState.SiteCollectionURL + "." + SPServer.Local.Address;
          }
        );
      }
      catch (Exception)
      {
        m_ApplicationURLTextBox.Text = "http://server.domain " + Resources.CannotGetAccessToLocalServer;
        m_SiteCollectionUrlTextBox.Text = String.Format("http://{0}.server.domain", Settings.Default.SiteCollectionURL);
      }
      try
      {
        if (String.IsNullOrEmpty(m_ApplicationState.OwnerLogin))
        {
          WindowsIdentity _id = WindowsIdentity.GetCurrent();
          m_OwnerLoginTextBox.Text = _id.Name;
        }
        else
          m_OwnerLoginTextBox.Text = m_ApplicationState.OwnerLogin; 
        m_OwnerEmailLabel.Text = m_ApplicationState.OwnerEmail;
      }
      catch (Exception)
      {
        m_OwnerLoginTextBox.Text = @"domain\user";
      }
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
          FarmHelpers.GetWebApplication(m_ApplicationState.WebApplicationURL);
          if (FarmHelpers.WebApplication != null)
          {
            _msg = String.Format(Resources.ApplicationFound, m_ApplicationState.WebApplicationURL, FarmHelpers.WebApplication.Name, FarmHelpers.WebApplication.DisplayName);
            m_ValidationListBox.AddMessage(_msg);
          }
          else
            throw new ApplicationException(String.Format(Resources.GettingAccess2ApplicationFailed, m_ApplicationState.WebApplicationURL));
          if (FarmHelpers.WebApplication.Sites.Names.Contains(m_ApplicationState.SiteCollectionURL))
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
        SPSecurity.RunWithElevatedPrivileges(delegate()
        {
          m_InstallationProgressBar.Minimum = 0;
          m_InstallationProgressBar.Maximum = 18;
          m_InstallationProgresListBox.SelectedValueChanged += new EventHandler(m_ListBox_TextChanged);
          m_InstallationProgresListBox.AddMessage("Creating SPSite ... ");
          m_SiteCollectionHelper = SiteCollectionHelper.CreateSPSite(
            FarmHelpers.WebApplication,
            m_ApplicationState.SiteCollectionURL,
            m_ApplicationState.OwnerLogin,
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
                Guid _solutionID;
                m_InstallationProgresListBox.AddMessage("Waiting for completion .... ");
                SPSolution _sol = FarmHelpers.DeploySolution(_fi, FarmHelpers.WebApplication, out _solutionID);
                _sltn.SolutionGuid = _solutionID;
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
              m_InstallationProgresListBox.AddMessage(String.Format("Activating Feature: {0} at: {1}", _fix.FetureGuid, m_SiteCollectionHelper.SiteCollection.Url));
              SPFeature _ffeature = m_SiteCollectionHelper.ActivateFeature(_fix.FetureGuid, _sltn.SPFeatureDefinitionScope);
              m_InstallationProgresListBox.AddMessage(String.Format("Feature activated : {0}", _ffeature.Definition.DisplayName));
              _fix.DisplayName = _ffeature.Definition.DisplayName;
              _fix.Version = _ffeature.Version.ToString();
              _fix.SPScope = _ffeature.Definition.Scope;
            }
            _sltn.Activated = true;
          }
          SaveInstallationState();
          m_InstallationProgresListBox.AddMessage("Installation successfully completed");
        });
      }
      catch (Exception ex)
      {
        try
        {
          SaveInstallationState();
        }
        catch (Exception _SaveEx)
        {
          m_InstallationProgresListBox.AddMessage(_SaveEx.Message);
        }
        string _msg = String.Format(Resources.LastOperationFailedWithError, ex);
        m_InstallationProgresListBox.AddMessage(_msg);
        throw new ApplicationException(_msg, ex);
      }
      finally
      {
        this.UseWaitCursor = false;
        m_InstallationProgresListBox.SelectedValueChanged -= new EventHandler(m_ListBox_TextChanged);
      }
    }
    private void SaveInstallationState()
    {
      FileInfo _file = new FileInfo(Settings.Default.InstallationStateFileName);
      m_InstallationProgresListBox.AddMessage(String.Format("Saving installation details to the file {0}.", _file.FullName));
      m_ApplicationState.Save(_file);
    }
    private void ExitlInstallation(DialogResult _res)
    {
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
    private static bool ValidEmailAddress(string _emailAddress, out string _errorMessage)
    {
      // Confirm that the e-mail address string is not empty.
      if (_emailAddress.Length == 0)
      {
        _errorMessage = "e-mail address is required.";
        return false;
      }
      // Confirm that there is an "@" and a "." in the e-mail address, and in the correct order.
      if (_emailAddress.IndexOf("@") > -1)
      {
        if (_emailAddress.IndexOf(".", _emailAddress.IndexOf("@")) > _emailAddress.IndexOf("@"))
        {
          _errorMessage = "";
          return true;
        }
      }
      _errorMessage = "e-mail address must be valid e-mail address format.\n" +
         "For example 'someone@example.com' ";
      return false;
    }

    #region base override
    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Form.Closing"/> event.
    /// </summary>
    /// <param name="e">A <see cref="T:System.ComponentModel.CancelEventArgs"/> that contains the event data.</param>
    protected override void OnClosing(CancelEventArgs e)
    {
      if (m_ApplicationState != null)
        Settings.Default.SiteCollectionURL = m_ApplicationState.SiteCollectionURL;
      base.OnClosing(e);
    }
    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        m_trace.Trace.TraceEventClose();
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
      m_InstallationProgressBar.Value++;
      m_InstallationProgressBar.Refresh();
    }
    private void m_ApplicationURLTextBox_Validating(object sender, CancelEventArgs e)
    {
      Uri _auri = null;
      string _errorMessage = String.Empty;
      if (InstallationStateData.ValidateUrl(m_ApplicationURLTextBox.Text, out _auri, out _errorMessage))
        m_WebApplicationURLErrorProvider.Clear();
      else
        m_WebApplicationURLErrorProvider.SetError(m_ApplicationURLTextBox, _errorMessage);
    }
    private void m_ApplicationURLTextBox_Validated(object sender, EventArgs e)
    {
    }
    private void m_OwnerEmailTextBox_Validating(object sender, CancelEventArgs e)
    {
      string _errorMsg;
      if (ValidEmailAddress(m_OwnerEmailTextBox.Text, out _errorMsg))
      {
        this.m_OwnerEmailErrorProvider.SetError(m_OwnerEmailTextBox, "");
        return;
      }
      // Cancel the event and select the text to be corrected by the user.
      m_OwnerEmailTextBox.Select(0, m_OwnerLoginTextBox.Text.Length);
      // Set the ErrorProvider error with the text to display. 
      this.m_OwnerEmailErrorProvider.SetError(m_OwnerEmailTextBox, _errorMsg);
    }
    private void m_OwnerEmailTextBox_Validated(object sender, EventArgs e)
    {
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
          m_ApplicationState.OwnerLogin,
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
