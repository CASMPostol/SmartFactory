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
      State = CurrentStae.ApplicationSetupDataDialog;
      Manual = true;
    }
    internal bool Manual { get; set; }
    /// <summary>
    /// Gets the web application URL.
    /// </summary>
    #endregion

    #region private
    /// <summary>
    /// Current state of the installation.
    /// </summary>
    private enum CurrentStae
    {
      ApplicationSetupDataDialog,
      ManualSelection,
      InstalationDataConfirmation,
      ApplicationInstalation,
      Finisched
    }
    /// <summary>
    /// Gets or sets the state of the installation.
    /// </summary>
    /// <value>
    /// The state of the installation.
    /// </value>
    private CurrentStae State
    {
      get
      {
        return m_State;
      }
      set
      {
        m_State = value;
        m_ApplicationSetupDataDialogPanel.Visible = value == CurrentStae.ApplicationSetupDataDialog;
        m_ManualSelectionPanel.Visible = value == CurrentStae.ManualSelection;
        m_InstalationDataConfirmationPanel.Visible = value == CurrentStae.InstalationDataConfirmation;
        m_ApplicationInstalationPanel.Visible = value == CurrentStae.ApplicationInstalation;
        m_FinischedPanel.Visible = value == CurrentStae.Finisched;
        this.Refresh();
        StateMachine(new StateEvenArgs(LocalEvent.EnterState));
      }
    }
    CurrentStae m_State;
    private enum LocalEvent
    {
      Previous, Next, Cancel, Exception, EnterState
    }
    private class StateEvenArgs : EventArgs
    {
      internal LocalEvent Event { get; private set; }
      public StateEvenArgs(LocalEvent _event)
      {
        Event = _event;
      }
    }
    private class ExceptionEventArgs : StateEvenArgs
    {
      internal Exception Exception { get; private set; }
      public ExceptionEventArgs(Exception _exception)
        : base(LocalEvent.Exception)
      {
        Exception = _exception;
      }
    }
    private void StateMachine(StateEvenArgs _event)
    {
      switch (State)
      {
        case CurrentStae.ManualSelection:
          #region ManualSelection
          switch (_event.Event)
          {
            case LocalEvent.Previous:
              StateError();
              break;
            case LocalEvent.Next:
              State = CurrentStae.ApplicationSetupDataDialog;
              break;
            case LocalEvent.Cancel:
              ExitlInstallation();
              break;
            case LocalEvent.Exception:
              Exception _eea = ((ExceptionEventArgs)_event).Exception;
              if (MessageBox.Show(
                String.Format(Resources.LastOperationFailed, _eea),
                Resources.CaptionOperationFailure,
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
              {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
              }
              break;
            case LocalEvent.EnterState:
              m_PreviousButton.Visible = false;
              m_NextButton.Enabled = false;
              m_CancelButton.Enabled = true;
              m_CancelButton.Text = Resources.CancelButtonTextEXIT;
              break;
            default:
              break;
          }
          break;
          #endregion
        case CurrentStae.ApplicationSetupDataDialog:
          #region ApplicationSetupDataDialog
          switch (_event.Event)
          {
            case LocalEvent.Previous:
              break;
            case LocalEvent.Next:
              if (Manual)
                State = CurrentStae.ManualSelection;
              else
                State = CurrentStae.InstalationDataConfirmation;
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
        case CurrentStae.InstalationDataConfirmation:
          #region InstalationDataConfirmation
          switch (_event.Event)
          {
            case LocalEvent.Previous:
              State = CurrentStae.ApplicationSetupDataDialog;
              break;
            case LocalEvent.Next:
              State = CurrentStae.ApplicationInstalation;
              break;
            case LocalEvent.Cancel:
              CancelInstallation();
              break;
            case LocalEvent.Exception:
              break;
            case LocalEvent.EnterState:
              m_PreviousButton.Visible = true;
              m_NextButton.Enabled = false;
              m_CancelButton.Visible = true;
              if (!CheckPrerequisites())
                MessageBox.Show(Resources.CheckIinProcessFfailed, Resources.CheckIinProcessFfailedCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
              else
                m_NextButton.Enabled = true;
              break;
            default:
              break;
          }
          break;
          #endregion
        case CurrentStae.ApplicationInstalation:
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
        case CurrentStae.Finisched:
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
        m_InstalationDataConfirmationListBox.Items.Add(Resources.ValidationProcessStarting);
        this.Refresh();
        FarmHelpers.GetUri(m_ApplicationURLTextBox.Text);
        FarmHelpers.GetFarm();
        string _msg = string.Empty;
        if (FarmHelpers.Farm != null)
        {
          _msg = String.Format(Resources.GotAccess2Farm, FarmHelpers.Farm.Name, FarmHelpers.Farm.DisplayName, FarmHelpers.Farm.Status);
          LogValidationMessage(_msg);
        }
        else
          throw new ApplicationException(Resources.GettingAccess2LocalFarm);
        FarmHelpers.GetWebApplication(FarmHelpers.WebApplicationURL);
        if (FarmHelpers.WebApplication != null)
        {
          _msg = String.Format(Resources.ApplicationFound, FarmHelpers.WebApplicationURL, FarmHelpers.WebApplication.Name, FarmHelpers.WebApplication.DisplayName);
          LogValidationMessage(_msg);
        }
        else
          throw new ApplicationException(String.Format(Resources.GettingAccess2ApplicationFailed, FarmHelpers.WebApplicationURL));
        if (FarmHelpers.WebApplication.Sites.Names.Contains(m_SiteUrlTextBox.Text))
        {
          string _ms = String.Format(Resources.SiteCollectionExist, m_SiteUrlTextBox.Text);
          SiteCollectionHelper.DeleteIfExist = MessageBox.Show(_ms, "Site creation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
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
      base.OnClosing(e);
    }
    #endregion

    #region Event handlers
    private void m_ApplicationURLTextBox_Validating(object sender, CancelEventArgs e)
    {
      Uri _auri = null;
      string _errorMessage = String.Empty;
      if (FarmHelpers.ValidateUrl(m_ApplicationURLTextBox.Text, out _auri, out _errorMessage))
      {
        m_WebApplicationURLErrorProvider.Clear();
        FarmHelpers.WebApplicationURL = _auri;
      }
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
    private bool ValidEmailAddress(string _emailAddress, out string _errorMessage)
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
    private void m_OwnerEmailTextBox_Validated(object sender, EventArgs e)
    {
    }
    private void m_CancelButton_Click(object sender, EventArgs e)
    {
      StateMachine(new StateEvenArgs(LocalEvent.Cancel));
    }
    private void m_PreviousButton_Click(object sender, EventArgs e)
    {
      StateMachine(new StateEvenArgs(LocalEvent.Previous));
    }
    private void m_NextButton_Click(object sender, EventArgs e)
    {
      StateMachine(new StateEvenArgs(LocalEvent.Next));
    }
    #endregion

    #endregion
  }
}
