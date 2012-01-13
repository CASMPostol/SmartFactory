﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
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
      State = ProcessState.ApplicationSetupDataDialog;
      Manual = Properties.Settings.Default.ManualMode;
      m_ApplicationURLTextBox.Text = Properties.Settings.Default.SiteCollectionURL;
    }
    internal bool Manual { get; set; }
    #endregion

    #region private
    /// <summary>
    /// Current state of the installation.
    /// </summary>
    private enum ProcessState
    {
      ApplicationSetupDataDialog,
      ManualSelection,
      InstalationDataConfirmation,
      ApplicationInstalation,
      Finisched
    }
    private ProcessState m_State;
    private InstallationStateData m_ApplicationState;
    private enum LocalEvent
    {
      Previous, Next, Cancel, Exception, EnterState
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

        foreach (TabPage _page in m_ContentTabControl.TabPages)
          m_ContentTabControl.TabPages.Remove(_page);
        switch (value)
        {
          case ProcessState.ApplicationSetupDataDialog:
            m_ContentTabControl.TabPages.Add(m_ApplicationSetupDataDialogPanel);
            break;
          case ProcessState.ManualSelection:
            m_ContentTabControl.TabPages.Add(m_ManualSelectionPanel);
            break;
          case ProcessState.InstalationDataConfirmation:
            m_ContentTabControl.TabPages.Add(m_ValidationPanel);
            break;
          case ProcessState.ApplicationInstalation:
            m_ContentTabControl.TabPages.Add(m_ApplicationInstalationPanel);
            break;
          case ProcessState.Finisched:
            m_ContentTabControl.TabPages.Add(m_FinischedPanel);
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
      switch (State)
      {
        case ProcessState.ManualSelection:
          #region ManualSelection
          switch (_event.Event)
          {
            case LocalEvent.Previous:
              State = ProcessState.ApplicationSetupDataDialog;
              break;
            case LocalEvent.Next:
              StateError();
              break;
            case LocalEvent.Cancel:
              ExitlInstallation();
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
              m_NextButton.Enabled = false;
              m_CancelButton.Enabled = true;
              m_CancelButton.Text = Resources.CancelButtonTextEXIT;
              m_ValidationPropertyGrid.SelectedObject = m_ApplicationState;
              break;
            default:
              break;
          }
          break;
          #endregion
        case ProcessState.ApplicationSetupDataDialog:
          #region ApplicationSetupDataDialog
          switch (_event.Event)
          {
            case LocalEvent.Previous:
              break;
            case LocalEvent.Next:
              State = ProcessState.InstalationDataConfirmation;
              break;
            case LocalEvent.Cancel:
              CancelInstallation();
              break;
            case LocalEvent.Exception:
              break;
            case LocalEvent.EnterState:
              m_PreviousButton.Visible = false;
              m_NextButton.Enabled = true;
              m_CancelButton.Visible = true;
              break;
            default:
              break;
          }
          break;
          #endregion
        case ProcessState.InstalationDataConfirmation:
          #region InstalationDataConfirmation
          switch (_event.Event)
          {
            case LocalEvent.Previous:
              State = ProcessState.ApplicationSetupDataDialog;
              break;
            case LocalEvent.Next:
              if (Manual)
                State = ProcessState.ManualSelection;
              else
                State = ProcessState.ApplicationInstalation;
              break;
            case LocalEvent.Cancel:
              CancelInstallation();
              break;
            case LocalEvent.Exception:
              break;
            case LocalEvent.EnterState:
              m_ApplicationState = new InstallationStateData();
              m_ValidationPropertyGrid.SelectedObject = m_ApplicationState;
              m_ValidationPropertyGrid.Text = Resources.InstallationProperties;
              m_PreviousButton.Visible = true;
              m_NextButton.Enabled = false;
              m_NextButton.Text = Resources.InstallButtonText;
              m_CancelButton.Visible = true;
              if (!CheckPrerequisites())
              {
                m_ApplicationState = null;
                MessageBox.Show(Resources.CheckIinProcessFfailed, Resources.CheckIinProcessFfailedCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
              }
              else
                m_NextButton.Enabled = true;
              break;
            default:
              break;
          }
          break;
          #endregion
        case ProcessState.ApplicationInstalation:
          #region ApplicationInstalation
          switch (_event.Event)
          {
            case LocalEvent.Previous:
              StateError();
              break;
            case LocalEvent.Next:
              ExitlInstallation();
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
              CancelInstallation();
              break;
            case LocalEvent.EnterState:
              m_PreviousButton.Visible = false;
              m_NextButton.Enabled = true;
              m_NextButton.Enabled = false;
              m_NextButton.Text = Resources.FinishButtonText;
              m_CancelButton.Visible = false;
              this.Refresh();
              Install();
              m_NextButton.Enabled = true;
              m_InstallationProgressBar.Value = m_InstallationProgressBar.Maximum;
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
              break;
            case LocalEvent.Next:
              break;
            case LocalEvent.Cancel:
              break;
            case LocalEvent.Exception:
              break;
            case LocalEvent.EnterState:
              m_PreviousButton.Visible = false;
              m_NextButton.Enabled = true;
              m_NextButton.Text = Resources.FinishButtonText;
              m_CancelButton.Visible = false;
              break;
            default:
              break;
          }
          break;
          #endregion
        default:
          break;
      }
    }
    private bool CheckPrerequisites()
    {
      try
      {
        LogValidationMessage(Resources.ValidationProcessStarting);
        //TODO add validation to:
        m_ApplicationState.GetUri(m_ApplicationURLTextBox.Text);
        m_ApplicationState.GetSiteCollectionURL(m_SiteUrlTextBox.Text);
        m_ApplicationState.GetOwnerLogin(m_OwnerLoginTextBox.Text);
        m_ApplicationState.GetOwnerEmail(m_OwnerEmailTextBox.Text);
        m_ApplicationState.FarmFetureId = new Guid(Settings.Default.FarmFeatureGuid);
        m_ApplicationState.SiteCollectionFetureId = new Guid(Settings.Default.SiteCollectionFeatureGuid);
        m_ValidationPropertyGrid.Refresh();
        FarmHelpers.GetFarm();
        string _msg = string.Empty;
        if (FarmHelpers.Farm != null)
        {
          _msg = String.Format(Resources.GotAccess2Farm, FarmHelpers.Farm.Name, FarmHelpers.Farm.DisplayName, FarmHelpers.Farm.Status);
          LogValidationMessage(_msg);
        }
        else
          throw new ApplicationException(Resources.GettingAccess2LocalFarm);
        FarmHelpers.GetWebApplication(m_ApplicationState.WebApplicationURL);
        if (FarmHelpers.WebApplication != null)
        {
          _msg = String.Format(Resources.ApplicationFound, m_ApplicationState.WebApplicationURL, FarmHelpers.WebApplication.Name, FarmHelpers.WebApplication.DisplayName);
          LogValidationMessage(_msg);
        }
        else
          throw new ApplicationException(String.Format(Resources.GettingAccess2ApplicationFailed, m_ApplicationState.WebApplicationURL));
        if (FarmHelpers.WebApplication.Sites.Names.Contains(m_ApplicationState.SiteCollectionURL))
        {
          SiteCollectionHelper.SiteCollection = FarmHelpers.WebApplication.Sites[m_ApplicationState.SiteCollectionURL];
          string _ms = String.Format(Resources.SiteCollectionExist, m_ApplicationState.SiteCollectionURL);
          SiteCollectionHelper.DeleteIfExist = MessageBox.Show(
            _ms,
            Resources.SiteCreation,
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question) != DialogResult.Yes;
          if (SiteCollectionHelper.DeleteIfExist)
            LogValidationMessage(Resources.SiteExistAndDelete);
          else
            LogValidationMessage(Resources.SiteExistAndReuse);
          this.Refresh();
        }
        m_ValidationPropertyGrid.Refresh();
        return true;
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.ValidationProcessFailed, ex.Message);
        m_ValidationListBox.AddMessage(_msg);
        return false;
      }
    }
    private void Install()
    {
      this.UseWaitCursor = true;
      try
      {
        m_InstallationProgressBar.Minimum = 0;
        m_InstallationProgressBar.Maximum = 15;
        m_InstallationProgresListBox.SelectedValueChanged += new EventHandler(m_InstallationProgresListBox_TextChanged);
        m_InstallationProgresListBox.AddMessage("Creating SPSite ... ");
        SiteCollectionHelper.CreateSPSite(
          FarmHelpers.WebApplication,
          m_ApplicationState.SiteCollectionURL,
          m_ApplicationState.OwnerLogin,
          m_ApplicationState.OwnerEmail);
        m_ApplicationState.SiteCollectionCreated = true;
        m_InstallationProgresListBox.AddMessage("Site collection created");
        m_ApplicationState.SiteCollectionCreated = true;
        FileInfo _fi = GetFile(Settings.Default.SiteCollectionSolutionFileName);
        m_InstallationProgresListBox.AddMessage(String.Format("Deploying solution: {0}", _fi.Name));
        SPUserSolution _solution = SiteCollectionHelper.DeploySolution(SiteCollectionHelper.SiteCollection, _fi);
        m_InstallationProgresListBox.AddMessage(String.Format("Solution deployed: {0}", _solution.Name));
        m_ApplicationState.SiteCollectionSolutionsDeployed = true;
        m_InstallationProgresListBox.AddMessage(String.Format("Activating Feature: {0} at: {1}", m_ApplicationState.SiteCollectionFetureId, SiteCollectionHelper.SiteCollection.Url));
        SPFeature _feature = SiteCollectionHelper.ActivateFeature(SiteCollectionHelper.SiteCollection, m_ApplicationState.SiteCollectionFetureId, Microsoft.SharePoint.SPFeatureDefinitionScope.Site);
        m_InstallationProgresListBox.AddMessage(String.Format("Feature activated : {0}", _feature.Definition.DisplayName));
        m_ApplicationState.SiteCollectionFeturesActivated = true;
        _fi = GetFile(Settings.Default.FarmSolutionFileName);
        m_InstallationProgresListBox.AddMessage(String.Format("Deploying Solution : {0}", _fi.Name));
        if (SiteCollectionHelper.SiteCollection == null)
          throw new ApplicationException(Resources.SiteCollectionNotExist);
        Guid _solutionID;
        m_InstallationProgresListBox.AddMessage("Waiting for completion .... ");
        SPSolution _sol = FarmHelpers.DeploySolution(_fi, FarmHelpers.WebApplication, out _solutionID);
        m_InstallationProgresListBox.AddMessage(String.Format("Solution deployed Name={0}, Deployed={1}, DeploymentState={2}, DisplayName={3} Status={4}", _sol.Name, _sol.Deployed, _sol.DeploymentState, _sol.DisplayName, _sol.Status));
        m_ApplicationState.FarmSolutionsDeployed = true;
        m_ApplicationState.SolutionID = _solutionID;
        m_InstallationProgresListBox.AddMessage(String.Format("Activating Feature: {0} at: {1}", m_ApplicationState.FarmFetureId, SiteCollectionHelper.SiteCollection.Url));
        _feature = SiteCollectionHelper.ActivateFeature(SiteCollectionHelper.SiteCollection, m_ApplicationState.FarmFetureId, Microsoft.SharePoint.SPFeatureDefinitionScope.Farm);
        m_InstallationProgresListBox.AddMessage(String.Format("Feature activated : {0}", _feature.Definition.DisplayName));
        m_ApplicationState.FarmFeaturesActivated = true;
        SaveInstallationState();
      }
      catch (Exception ex)
      {

        m_InstallationProgresListBox.AddMessage(ex.Message);
        try
        {
          SaveInstallationState();
        }
        catch (Exception _SaveEx)
        {
          m_InstallationProgresListBox.AddMessage(_SaveEx.Message);
        }
        this.StateMachine(new StateMachineExceptionEventArgs(ex));
      }
      finally
      {
        this.UseWaitCursor = false;
        m_InstallationProgresListBox.SelectedValueChanged += new EventHandler(m_InstallationProgresListBox_TextChanged);
      }
    }
    private void SaveInstallationState()
    {
      FileInfo _file = new FileInfo(Settings.Default.InstallationStateFileName);
      m_InstallationProgresListBox.AddMessage(String.Format("Saving installation details to the file {0}.", _file.FullName));
      m_ApplicationState.Save(_file);
      m_InstallationProgresListBox.AddMessage("Installation successfully completed");
    }
    private void m_InstallationProgresListBox_TextChanged(object sender, EventArgs e)
    {
      m_InstallationProgressBar.Value++;
      m_InstallationProgressBar.Refresh();
    }
    private void LogValidationMessage(string _msg)
    {
      m_ValidationListBox.AddMessage(_msg);
    }
    private void ExitlInstallation()
    {
      this.DialogResult = System.Windows.Forms.DialogResult.OK;
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
    private static FileInfo GetFile(string _fileName)
    {
      FileInfo _fi = new FileInfo(_fileName);
      if (!_fi.Exists)
        throw new FileNotFoundException(_fi.ToString());
      return _fi;
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
    /// Raises the <see cref="E:System.Windows.Forms.Form.Load"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
    protected override void OnLoad(EventArgs e)
    {
      try
      {
        m_ApplicationURLTextBox.Text = Uri.UriSchemeHttp + Uri.SchemeDelimiter + SPServer.Local.Address;
      }
      catch (Exception)
      {
        m_ApplicationURLTextBox.Text = "http://server.domain " + Resources.CannotGetAccessToLocalServer;
      }
      try
      {
        WindowsIdentity _id = WindowsIdentity.GetCurrent();
        m_OwnerLoginTextBox.Text = _id.Name;
      }
      catch (Exception)
      {
        m_OwnerLoginTextBox.Text = @"domain\user";
      }
      base.OnLoad(e);
    }
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
    #endregion

    #region Event handlers
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
        FileInfo _fi = GetFile(Settings.Default.SiteCollectionSolutionFileName);
        SiteCollectionHelper.DeploySolution(SiteCollectionHelper.SiteCollection, _fi);
        SiteCollectionHelper.ActivateFeature(SiteCollectionHelper.SiteCollection, m_ApplicationState.SiteCollectionFetureId, Microsoft.SharePoint.SPFeatureDefinitionScope.Site);
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
        FileInfo _fi = GetFile(Settings.Default.FarmSolutionFileName);
        if (SiteCollectionHelper.SiteCollection == null)
          throw new ApplicationException(Resources.SiteCollectionNotExist);
        Guid _solutionID;
        FarmHelpers.DeploySolution(_fi, FarmHelpers.WebApplication, out _solutionID);
        m_ApplicationState.SolutionID = _solutionID;
        SiteCollectionHelper.ActivateFeature(SiteCollectionHelper.SiteCollection, m_ApplicationState.FarmFetureId, Microsoft.SharePoint.SPFeatureDefinitionScope.Farm);
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
        FarmHelpers.DeactivateFeature(m_ApplicationState.FarmFetureId, SiteCollectionHelper.SiteCollection);
        FarmHelpers.RetrackSolution(m_ApplicationState.SolutionID);
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
