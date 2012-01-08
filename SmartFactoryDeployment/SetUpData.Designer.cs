namespace CAS.SmartFactory.Deployment
{
  partial class SetUpData
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.TableLayoutPanel m_ApplicationSetupTableLayoutPanel;
      System.Windows.Forms.Label m_ApplicationURL;
      System.Windows.Forms.Label m_OwnerEmailabel;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetUpData));
      System.Windows.Forms.Label m_SiteURLLabel;
      this.m_OwnerLoginTextBox = new System.Windows.Forms.TextBox();
      this.m_ApplicationURLTextBox = new System.Windows.Forms.TextBox();
      this.m_SiteUrlTextBox = new System.Windows.Forms.TextBox();
      this.m_OwnerEmailLabel = new System.Windows.Forms.Label();
      this.m_OwnerEmailTextBox = new System.Windows.Forms.TextBox();
      this.m_NextButton = new System.Windows.Forms.Button();
      this.m_CancelButton = new System.Windows.Forms.Button();
      this.m_TtoolTip = new System.Windows.Forms.ToolTip(this.components);
      this.m_OwnerEmailErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.m_WebApplicationURLErrorProvider = new System.Windows.Forms.ErrorProvider(this.components);
      this.m_ApplicationSetupDataDialogPanel = new System.Windows.Forms.Panel();
      this.m_ApplicationInstalationPane = new System.Windows.Forms.Panel();
      this.m_EnvironmentCheckedListBox = new System.Windows.Forms.CheckedListBox();
      this.m_PreviousButton = new System.Windows.Forms.Button();
      this.m_ManualSelectionPanel = new System.Windows.Forms.Panel();
      m_ApplicationSetupTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      m_ApplicationURL = new System.Windows.Forms.Label();
      m_OwnerEmailabel = new System.Windows.Forms.Label();
      m_SiteURLLabel = new System.Windows.Forms.Label();
      m_ApplicationSetupTableLayoutPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_OwnerEmailErrorProvider)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_WebApplicationURLErrorProvider)).BeginInit();
      this.m_ApplicationSetupDataDialogPanel.SuspendLayout();
      this.m_ApplicationInstalationPane.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_ApplicationSetupTableLayoutPanel
      // 
      m_ApplicationSetupTableLayoutPanel.AutoSize = true;
      m_ApplicationSetupTableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      m_ApplicationSetupTableLayoutPanel.ColumnCount = 3;
      m_ApplicationSetupTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      m_ApplicationSetupTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      m_ApplicationSetupTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      m_ApplicationSetupTableLayoutPanel.Controls.Add(m_ApplicationURL, 0, 0);
      m_ApplicationSetupTableLayoutPanel.Controls.Add(m_OwnerEmailabel, 0, 2);
      m_ApplicationSetupTableLayoutPanel.Controls.Add(this.m_OwnerLoginTextBox, 1, 2);
      m_ApplicationSetupTableLayoutPanel.Controls.Add(this.m_ApplicationURLTextBox, 1, 0);
      m_ApplicationSetupTableLayoutPanel.Controls.Add(this.m_SiteUrlTextBox, 1, 1);
      m_ApplicationSetupTableLayoutPanel.Controls.Add(m_SiteURLLabel, 0, 1);
      m_ApplicationSetupTableLayoutPanel.Controls.Add(this.m_OwnerEmailLabel, 0, 3);
      m_ApplicationSetupTableLayoutPanel.Controls.Add(this.m_OwnerEmailTextBox, 1, 3);
      m_ApplicationSetupTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      m_ApplicationSetupTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
      m_ApplicationSetupTableLayoutPanel.Name = "m_ApplicationSetupTableLayoutPanel";
      m_ApplicationSetupTableLayoutPanel.RowCount = 5;
      m_ApplicationSetupTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_ApplicationSetupTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_ApplicationSetupTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_ApplicationSetupTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      m_ApplicationSetupTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_ApplicationSetupTableLayoutPanel.Size = new System.Drawing.Size(484, 253);
      m_ApplicationSetupTableLayoutPanel.TabIndex = 0;
      // 
      // m_ApplicationURL
      // 
      m_ApplicationURL.AutoSize = true;
      m_ApplicationURL.Dock = System.Windows.Forms.DockStyle.Fill;
      m_ApplicationURL.Location = new System.Drawing.Point(3, 0);
      m_ApplicationURL.Name = "m_ApplicationURL";
      m_ApplicationURL.Size = new System.Drawing.Size(109, 26);
      m_ApplicationURL.TabIndex = 0;
      m_ApplicationURL.Text = "Web application URL";
      m_ApplicationURL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      this.m_TtoolTip.SetToolTip(m_ApplicationURL, "\r\n\r\n");
      // 
      // m_OwnerEmailabel
      // 
      m_OwnerEmailabel.AutoSize = true;
      m_OwnerEmailabel.Dock = System.Windows.Forms.DockStyle.Fill;
      m_OwnerEmailabel.Location = new System.Drawing.Point(3, 52);
      m_OwnerEmailabel.Name = "m_OwnerEmailabel";
      m_OwnerEmailabel.Size = new System.Drawing.Size(109, 26);
      m_OwnerEmailabel.TabIndex = 4;
      m_OwnerEmailabel.Text = "Site owner login";
      m_OwnerEmailabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_OwnerLoginTextBox
      // 
      this.m_OwnerLoginTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_OwnerLoginTextBox.Location = new System.Drawing.Point(118, 55);
      this.m_OwnerLoginTextBox.Name = "m_OwnerLoginTextBox";
      this.m_OwnerLoginTextBox.Size = new System.Drawing.Size(343, 20);
      this.m_OwnerLoginTextBox.TabIndex = 5;
      this.m_TtoolTip.SetToolTip(this.m_OwnerLoginTextBox, resources.GetString("m_OwnerLoginTextBox.ToolTip"));
      // 
      // m_ApplicationURLTextBox
      // 
      this.m_ApplicationURLTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_ApplicationURLTextBox.Location = new System.Drawing.Point(118, 3);
      this.m_ApplicationURLTextBox.Name = "m_ApplicationURLTextBox";
      this.m_ApplicationURLTextBox.Size = new System.Drawing.Size(343, 20);
      this.m_ApplicationURLTextBox.TabIndex = 1;
      this.m_ApplicationURLTextBox.Text = "http://computer.domain";
      this.m_TtoolTip.SetToolTip(this.m_ApplicationURLTextBox, "A string that specifies the URL of the Web application. \r\nFor example \'http://com" +
        "puter.domain:Port\'.\r\n");
      this.m_ApplicationURLTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.m_ApplicationURLTextBox_Validating);
      this.m_ApplicationURLTextBox.Validated += new System.EventHandler(this.m_ApplicationURLTextBox_Validated);
      // 
      // m_SiteUrlTextBox
      // 
      this.m_SiteUrlTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_SiteUrlTextBox.Location = new System.Drawing.Point(118, 29);
      this.m_SiteUrlTextBox.Name = "m_SiteUrlTextBox";
      this.m_SiteUrlTextBox.Size = new System.Drawing.Size(343, 20);
      this.m_SiteUrlTextBox.TabIndex = 6;
      this.m_SiteUrlTextBox.Text = "sites/sf";
      this.m_TtoolTip.SetToolTip(this.m_SiteUrlTextBox, resources.GetString("m_SiteUrlTextBox.ToolTip"));
      // 
      // m_SiteURLLabel
      // 
      m_SiteURLLabel.AutoSize = true;
      m_SiteURLLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      m_SiteURLLabel.Location = new System.Drawing.Point(3, 26);
      m_SiteURLLabel.Name = "m_SiteURLLabel";
      m_SiteURLLabel.Size = new System.Drawing.Size(109, 26);
      m_SiteURLLabel.TabIndex = 7;
      m_SiteURLLabel.Text = "Site Url";
      m_SiteURLLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_OwnerEmailLabel
      // 
      this.m_OwnerEmailLabel.AutoSize = true;
      this.m_OwnerEmailLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_OwnerEmailLabel.Location = new System.Drawing.Point(3, 78);
      this.m_OwnerEmailLabel.Name = "m_OwnerEmailLabel";
      this.m_OwnerEmailLabel.Size = new System.Drawing.Size(109, 20);
      this.m_OwnerEmailLabel.TabIndex = 8;
      this.m_OwnerEmailLabel.Text = "Site owner email";
      this.m_OwnerEmailLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_OwnerEmailTextBox
      // 
      this.m_OwnerEmailTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_OwnerEmailTextBox.Location = new System.Drawing.Point(118, 81);
      this.m_OwnerEmailTextBox.Name = "m_OwnerEmailTextBox";
      this.m_OwnerEmailTextBox.Size = new System.Drawing.Size(343, 20);
      this.m_OwnerEmailTextBox.TabIndex = 9;
      this.m_OwnerEmailTextBox.Text = "someone@example.com";
      this.m_TtoolTip.SetToolTip(this.m_OwnerEmailTextBox, "Smart Factory site collection owner email address.\r\nA string that contains the e-" +
        "mail address of the owner of the site collection.\r\nFor example someone@example.c" +
        "om\r\n");
      this.m_OwnerEmailTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.m_OwnerEmailTextBox_Validating);
      this.m_OwnerEmailTextBox.Validated += new System.EventHandler(this.m_OwnerEmailTextBox_Validated);
      // 
      // m_NextButton
      // 
      this.m_NextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.m_NextButton.Location = new System.Drawing.Point(114, 211);
      this.m_NextButton.MaximumSize = new System.Drawing.Size(0, 30);
      this.m_NextButton.MinimumSize = new System.Drawing.Size(100, 30);
      this.m_NextButton.Name = "m_NextButton";
      this.m_NextButton.Size = new System.Drawing.Size(100, 30);
      this.m_NextButton.TabIndex = 2;
      this.m_NextButton.Text = ">>";
      this.m_NextButton.UseVisualStyleBackColor = true;
      // 
      // m_CancelButton
      // 
      this.m_CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.m_CancelButton.CausesValidation = false;
      this.m_CancelButton.Location = new System.Drawing.Point(222, 211);
      this.m_CancelButton.MaximumSize = new System.Drawing.Size(0, 30);
      this.m_CancelButton.MinimumSize = new System.Drawing.Size(100, 30);
      this.m_CancelButton.Name = "m_CancelButton";
      this.m_CancelButton.Size = new System.Drawing.Size(100, 30);
      this.m_CancelButton.TabIndex = 3;
      this.m_CancelButton.Text = "CANCEL";
      this.m_CancelButton.UseVisualStyleBackColor = true;
      this.m_CancelButton.Click += new System.EventHandler(this.m_CancelButton_Click);
      // 
      // m_TtoolTip
      // 
      this.m_TtoolTip.AutoPopDelay = 20000;
      this.m_TtoolTip.InitialDelay = 500;
      this.m_TtoolTip.IsBalloon = true;
      this.m_TtoolTip.ReshowDelay = 100;
      // 
      // m_OwnerEmailErrorProvider
      // 
      this.m_OwnerEmailErrorProvider.ContainerControl = this;
      // 
      // m_WebApplicationURLErrorProvider
      // 
      this.m_WebApplicationURLErrorProvider.ContainerControl = this;
      // 
      // m_ApplicationSetupDataDialogPanel
      // 
      this.m_ApplicationSetupDataDialogPanel.Controls.Add(m_ApplicationSetupTableLayoutPanel);
      this.m_ApplicationSetupDataDialogPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_ApplicationSetupDataDialogPanel.Location = new System.Drawing.Point(0, 0);
      this.m_ApplicationSetupDataDialogPanel.Name = "m_ApplicationSetupDataDialogPanel";
      this.m_ApplicationSetupDataDialogPanel.Size = new System.Drawing.Size(484, 253);
      this.m_ApplicationSetupDataDialogPanel.TabIndex = 4;
      // 
      // m_ApplicationInstalationPane
      // 
      this.m_ApplicationInstalationPane.Controls.Add(this.m_EnvironmentCheckedListBox);
      this.m_ApplicationInstalationPane.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_ApplicationInstalationPane.Location = new System.Drawing.Point(0, 0);
      this.m_ApplicationInstalationPane.Name = "m_ApplicationInstalationPane";
      this.m_ApplicationInstalationPane.Size = new System.Drawing.Size(484, 253);
      this.m_ApplicationInstalationPane.TabIndex = 5;
      // 
      // m_EnvironmentCheckedListBox
      // 
      this.m_EnvironmentCheckedListBox.FormattingEnabled = true;
      this.m_EnvironmentCheckedListBox.Location = new System.Drawing.Point(6, 4);
      this.m_EnvironmentCheckedListBox.Name = "m_EnvironmentCheckedListBox";
      this.m_EnvironmentCheckedListBox.Size = new System.Drawing.Size(475, 184);
      this.m_EnvironmentCheckedListBox.TabIndex = 0;
      // 
      // m_PreviousButton
      // 
      this.m_PreviousButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.m_PreviousButton.CausesValidation = false;
      this.m_PreviousButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.m_PreviousButton.Location = new System.Drawing.Point(6, 211);
      this.m_PreviousButton.MaximumSize = new System.Drawing.Size(0, 30);
      this.m_PreviousButton.MinimumSize = new System.Drawing.Size(100, 30);
      this.m_PreviousButton.Name = "m_PreviousButton";
      this.m_PreviousButton.Size = new System.Drawing.Size(100, 30);
      this.m_PreviousButton.TabIndex = 6;
      this.m_PreviousButton.Text = "<<";
      this.m_PreviousButton.UseVisualStyleBackColor = true;
      // 
      // m_ManualSelectionPanel
      // 
      this.m_ManualSelectionPanel.Dock = System.Windows.Forms.DockStyle.Top;
      this.m_ManualSelectionPanel.Location = new System.Drawing.Point(0, 0);
      this.m_ManualSelectionPanel.Name = "m_ManualSelectionPanel";
      this.m_ManualSelectionPanel.Size = new System.Drawing.Size(484, 205);
      this.m_ManualSelectionPanel.TabIndex = 1;
      // 
      // SetUpData
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(484, 253);
      this.Controls.Add(this.m_CancelButton);
      this.Controls.Add(this.m_PreviousButton);
      this.Controls.Add(this.m_NextButton);
      this.Controls.Add(this.m_ManualSelectionPanel);
      this.Controls.Add(this.m_ApplicationInstalationPane);
      this.Controls.Add(this.m_ApplicationSetupDataDialogPanel);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(500, 200);
      this.Name = "SetUpData";
      this.Text = "Smart Factory setup data";
      m_ApplicationSetupTableLayoutPanel.ResumeLayout(false);
      m_ApplicationSetupTableLayoutPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.m_OwnerEmailErrorProvider)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.m_WebApplicationURLErrorProvider)).EndInit();
      this.m_ApplicationSetupDataDialogPanel.ResumeLayout(false);
      this.m_ApplicationSetupDataDialogPanel.PerformLayout();
      this.m_ApplicationInstalationPane.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TextBox m_ApplicationURLTextBox;
    private System.Windows.Forms.Button m_NextButton;
    private System.Windows.Forms.Button m_CancelButton;
    private System.Windows.Forms.ToolTip m_TtoolTip;
    private System.Windows.Forms.TextBox m_OwnerLoginTextBox;
    private System.Windows.Forms.TextBox m_SiteUrlTextBox;
    private System.Windows.Forms.Label m_OwnerEmailLabel;
    private System.Windows.Forms.ErrorProvider m_OwnerEmailErrorProvider;
    private System.Windows.Forms.TextBox m_OwnerEmailTextBox;
    private System.Windows.Forms.ErrorProvider m_WebApplicationURLErrorProvider;
    private System.Windows.Forms.Button m_PreviousButton;
    private System.Windows.Forms.Panel m_ApplicationInstalationPane;
    private System.Windows.Forms.Panel m_ApplicationSetupDataDialogPanel;
    private System.Windows.Forms.CheckedListBox m_EnvironmentCheckedListBox;
    private System.Windows.Forms.Panel m_ManualSelectionPanel;

  }
}