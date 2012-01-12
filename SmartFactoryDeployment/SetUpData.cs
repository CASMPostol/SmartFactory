using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint.Administration;
using System.Security.Principal;
using CAS.SmartFactory.Deployment.Properties;
using System.Diagnostics;
using System.Threading;
using System.IO;

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
      Manual = true;
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
    private ApplicationState m_ApplicationState;
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
        m_ApplicationSetupDataDialogPanel.Visible = value == ProcessState.ApplicationSetupDataDialog;
        m_ManualSelectionPanel.Visible = value == ProcessState.ManualSelection;
        m_InstalationDataConfirmationPanel.Visible = value == ProcessState.InstalationDataConfirmation;
        m_ApplicationInstalationPanel.Visible = value == ProcessState.ApplicationInstalation;
        m_FinischedPanel.Visible = value == ProcessState.Finisched;
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
              m_PropertyGrid.SelectedObject = m_ApplicationState;
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
              m_ApplicationState = new ApplicationState();
              m_PreviousButton.Visible = true;
              m_NextButton.Enabled = false;
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
              break;
            case LocalEvent.EnterState:
              m_PreviousButton.Visible = false;
              m_NextButton.Enabled = true;
              m_NextButton.Enabled = false;
              m_NextButton.Text = Resources.FinishButtonText;
              m_CancelButton.Visible = false;
              m_InstallationProgresListBox.Items.Clear();
              m_InstallationProgresListBox.BeginUpdate();
              m_InstallationProgresListBox.Items.Add("Instalation started...");
              m_InstallationProgresListBox.EndUpdate();
              this.AutoSize = true;
              this.PerformLayout();
              this.Refresh();
              Thread.Sleep(3000);
              m_NextButton.Enabled = true;
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
        return true;
      }
      catch (Exception ex)
      {
        string _msg = String.Format(Resources.ValidationProcessFailed, ex.Message);
        m_InstalationDataConfirmationListBox.Items.Add(_msg);
        this.Refresh();
        return false;
      }
    }
    private void LogValidationMessage(string _msg)
    {
      m_InstalationDataConfirmationListBox.Items.Add(_msg);
      this.Refresh();
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
      if (ApplicationState.ValidateUrl(m_ApplicationURLTextBox.Text, out _auri, out _errorMessage))
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
