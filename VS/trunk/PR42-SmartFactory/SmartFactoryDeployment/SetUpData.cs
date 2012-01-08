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
      State = CurrentStae.ApplicationInstalation;
      m_ApplicationURLTextBox.Text = Uri.UriSchemeHttp + Uri.SchemeDelimiter + SPServer.Local.Address;
      WindowsIdentity _id = WindowsIdentity.GetCurrent();
      m_OwnerLoginTextBox.Text = _id.Name;
    }
    /// <summary>
    /// Current state of the installation.
    /// </summary>
    internal enum CurrentStae
    {
      ManualSelection,
      ApplicationSetupDataDialog,
      InstalationDataConfirmation,
      ApplicationInstalation
    }
    /// <summary>
    /// Gets or sets the state og the installation.
    /// </summary>
    /// <value>
    /// The state of the installation.
    /// </value>
    internal CurrentStae State
    {
      get
      {
        return m_State;
      }
      set
      {
        m_State = value;
        StateMavine(new StateEvenArgs(LocalEvent.EnterState));
        m_ApplicationSetupDataDialogPanel.Visible = value == CurrentStae.ApplicationSetupDataDialog;
        m_ManualSelectionPanel.Visible = value == CurrentStae.ManualSelection;
        m_ApplicationInstalationPane.Visible = value == CurrentStae.ApplicationInstalation;
        m_NextButton.Visible = value > CurrentStae.ApplicationSetupDataDialog;
        m_PreviousButton.Visible = value > CurrentStae.InstalationDataConfirmation;
      }
    }
    /// <summary>
    /// Gets the web application URL.
    /// </summary>
    public Uri WebApplicationURL { get; private set; }
    /// <summary>
    /// Gets the web application.
    /// </summary>
    public SPWebApplication WebApplication { get; private set; }
    /// <summary>
    /// Gets or sets the farm.
    /// </summary>
    /// <value>
    /// The farm.
    /// </value>
    public SPFarm Farm { get; set; }
    #endregion

    #region private
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

    private void StateMavine(StateEvenArgs _event)
    {
      switch (State)
      {
        case CurrentStae.ManualSelection:
          switch (_event.Event)
          {
            case LocalEvent.Previous:

              break;
            case LocalEvent.Next:
              State = CurrentStae.ApplicationSetupDataDialog;
              break;
            case LocalEvent.Cancel:
              this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
              this.Close();
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
              m_NextButton.Visible = false;
              m_CancelButton.Enabled = true;
              m_CancelButton.Text = "EXIT";
              m_ApplicationSetupDataDialogPanel.Visible = true;
              m_ApplicationInstalationPane.Visible = false;
              break;
            default:
              break;
          }
          break;
        case CurrentStae.ApplicationSetupDataDialog:
          break;
        case CurrentStae.InstalationDataConfirmation:
          break;
        case CurrentStae.ApplicationInstalation:
          break;
        default:
          break;
      }
    }
    #region base override
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
      if (!Uri.TryCreate(m_ApplicationURLTextBox.Text, UriKind.Absolute, out _auri))
      {
        string _errorMessage = "The application address must be valid http address format. For example 'http://computer.domain:Port'";
        m_WebApplicationURLErrorProvider.SetError(m_ApplicationURLTextBox, _errorMessage);
        e.Cancel = true;
        return;
      }
      if (_auri.Scheme != Uri.UriSchemeHttp)
      {
        string _errorMessage = "Url must be Http protocol.";
        m_WebApplicationURLErrorProvider.SetError(m_ApplicationURLTextBox, _errorMessage);
        e.Cancel = true;
      }
      WebApplicationURL = _auri;
      WebApplication = SPWebApplication.Lookup(_auri);
      if (WebApplication != null)
        return;
      e.Cancel = true;
      string _em = "The address is wrong. I cannot get access to the web application";
      m_WebApplicationURLErrorProvider.SetError(m_ApplicationURLTextBox, _em);
    }
    private void m_ApplicationURLTextBox_Validated(object sender, EventArgs e)
    {
      m_WebApplicationURLErrorProvider.Clear();
    }
    private void m_OwnerEmailTextBox_Validating(object sender, CancelEventArgs e)
    {
      string _errorMsg;
      if (ValidEmailAddress(m_OwnerEmailTextBox.Text, out _errorMsg))
        return;
      // Cancel the event and select the text to be corrected by the user.
      e.Cancel = true;
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
      this.m_OwnerEmailErrorProvider.SetError(m_OwnerEmailTextBox, "");
    }
    private void m_CancelButton_Click(object sender, EventArgs e)
    {
      m_OwnerEmailErrorProvider.Clear();
    }
    #endregion
    #endregion
  }
}
