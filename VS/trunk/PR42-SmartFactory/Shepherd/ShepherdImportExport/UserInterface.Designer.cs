namespace CAS.SmartFactory.Shepherd.ImportExport
{
  partial class UserInterface
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;



    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      System.Windows.Forms.TableLayoutPanel m_tableLayoutPanel1;
      System.Windows.Forms.Label m_CollectionURLlabel1;
      System.Windows.Forms.StatusStrip m_StatusStrip1;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInterface));
      this.m_TimeSlotsCreateButton = new System.Windows.Forms.Button();
      this.m_URLTextBox = new System.Windows.Forms.TextBox();
      this.m_AddTimeSlotsButton = new System.Windows.Forms.Button();
      this.m_ImportDictionaries = new System.Windows.Forms.Button();
      this.m_TestDataCheckBox = new System.Windows.Forms.CheckBox();
      this.m_ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
      this.m_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.m_FileNameStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.m_FileManagementComonent = new CAS.SmartFactory.Shepherd.ImportExport.FileDialog(this.components);
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.m_ToolTip = new System.Windows.Forms.ToolTip(this.components);
      m_tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      m_CollectionURLlabel1 = new System.Windows.Forms.Label();
      m_StatusStrip1 = new System.Windows.Forms.StatusStrip();
      m_tableLayoutPanel1.SuspendLayout();
      m_StatusStrip1.SuspendLayout();
      this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_tableLayoutPanel1
      // 
      m_tableLayoutPanel1.ColumnCount = 2;
      m_tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      m_tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      m_tableLayoutPanel1.Controls.Add(this.m_TimeSlotsCreateButton, 0, 1);
      m_tableLayoutPanel1.Controls.Add(m_CollectionURLlabel1, 0, 0);
      m_tableLayoutPanel1.Controls.Add(this.m_URLTextBox, 1, 0);
      m_tableLayoutPanel1.Controls.Add(this.m_AddTimeSlotsButton, 0, 2);
      m_tableLayoutPanel1.Controls.Add(this.m_ImportDictionaries, 0, 3);
      m_tableLayoutPanel1.Controls.Add(this.m_TestDataCheckBox, 1, 3);
      m_tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      m_tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      m_tableLayoutPanel1.Name = "m_tableLayoutPanel1";
      m_tableLayoutPanel1.RowCount = 5;
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      m_tableLayoutPanel1.Size = new System.Drawing.Size(884, 355);
      m_tableLayoutPanel1.TabIndex = 1;
      // 
      // m_TimeSlotsCreateButton
      // 
      this.m_TimeSlotsCreateButton.Location = new System.Drawing.Point(3, 29);
      this.m_TimeSlotsCreateButton.Name = "m_TimeSlotsCreateButton";
      this.m_TimeSlotsCreateButton.Size = new System.Drawing.Size(116, 23);
      this.m_TimeSlotsCreateButton.TabIndex = 0;
      this.m_TimeSlotsCreateButton.Text = "Create Dictionaries";
      this.m_TimeSlotsCreateButton.UseVisualStyleBackColor = true;
      this.m_TimeSlotsCreateButton.Click += new System.EventHandler(this.CreateDictionaries_Click);
      // 
      // m_CollectionURLlabel1
      // 
      m_CollectionURLlabel1.AutoSize = true;
      m_CollectionURLlabel1.Dock = System.Windows.Forms.DockStyle.Left;
      m_CollectionURLlabel1.Location = new System.Drawing.Point(3, 0);
      m_CollectionURLlabel1.Name = "m_CollectionURLlabel1";
      m_CollectionURLlabel1.Size = new System.Drawing.Size(78, 26);
      m_CollectionURLlabel1.TabIndex = 1;
      m_CollectionURLlabel1.Text = "Collection URL";
      m_CollectionURLlabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_URLTextBox
      // 
      this.m_URLTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_URLTextBox.Location = new System.Drawing.Point(125, 3);
      this.m_URLTextBox.Name = "m_URLTextBox";
      this.m_URLTextBox.Size = new System.Drawing.Size(756, 20);
      this.m_URLTextBox.TabIndex = 2;
      this.m_URLTextBox.Text = "http://casmp/";
      // 
      // m_AddTimeSlotsButton
      // 
      this.m_AddTimeSlotsButton.AutoSize = true;
      this.m_AddTimeSlotsButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_AddTimeSlotsButton.Location = new System.Drawing.Point(3, 58);
      this.m_AddTimeSlotsButton.Name = "m_AddTimeSlotsButton";
      this.m_AddTimeSlotsButton.Size = new System.Drawing.Size(116, 23);
      this.m_AddTimeSlotsButton.TabIndex = 3;
      this.m_AddTimeSlotsButton.Text = "Add TimeSlots";
      this.m_AddTimeSlotsButton.UseVisualStyleBackColor = true;
      this.m_AddTimeSlotsButton.Click += new System.EventHandler(this.AddTimeSlots_Click);
      // 
      // m_ImportDictionaries
      // 
      this.m_ImportDictionaries.AutoSize = true;
      this.m_ImportDictionaries.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_ImportDictionaries.Location = new System.Drawing.Point(3, 87);
      this.m_ImportDictionaries.Name = "m_ImportDictionaries";
      this.m_ImportDictionaries.Size = new System.Drawing.Size(116, 23);
      this.m_ImportDictionaries.TabIndex = 4;
      this.m_ImportDictionaries.Text = "Import Dictionaries";
      this.m_ImportDictionaries.UseVisualStyleBackColor = true;
      this.m_ImportDictionaries.Click += new System.EventHandler(this.ImportDictionaries_Click);
      // 
      // m_TestDataCheckBox
      // 
      this.m_TestDataCheckBox.AutoSize = true;
      this.m_TestDataCheckBox.Checked = true;
      this.m_TestDataCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
      this.m_TestDataCheckBox.Location = new System.Drawing.Point(125, 87);
      this.m_TestDataCheckBox.Name = "m_TestDataCheckBox";
      this.m_TestDataCheckBox.Size = new System.Drawing.Size(71, 17);
      this.m_TestDataCheckBox.TabIndex = 5;
      this.m_TestDataCheckBox.Text = "Test data";
      this.m_TestDataCheckBox.UseVisualStyleBackColor = true;
      // 
      // m_StatusStrip1
      // 
      m_StatusStrip1.Dock = System.Windows.Forms.DockStyle.None;
      m_StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_ToolStripProgressBar,
            this.m_ToolStripStatusLabel,
            this.m_FileNameStatusLabel});
      m_StatusStrip1.Location = new System.Drawing.Point(0, 0);
      m_StatusStrip1.Name = "m_StatusStrip1";
      m_StatusStrip1.Size = new System.Drawing.Size(884, 22);
      m_StatusStrip1.TabIndex = 0;
      // 
      // m_ToolStripProgressBar
      // 
      this.m_ToolStripProgressBar.AutoSize = false;
      this.m_ToolStripProgressBar.Name = "m_ToolStripProgressBar";
      this.m_ToolStripProgressBar.Size = new System.Drawing.Size(150, 16);
      // 
      // m_ToolStripStatusLabel
      // 
      this.m_ToolStripStatusLabel.AutoSize = false;
      this.m_ToolStripStatusLabel.Name = "m_ToolStripStatusLabel";
      this.m_ToolStripStatusLabel.Size = new System.Drawing.Size(400, 17);
      this.m_ToolStripStatusLabel.Text = "----";
      this.m_ToolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_FileNameStatusLabel
      // 
      this.m_FileNameStatusLabel.Name = "m_FileNameStatusLabel";
      this.m_FileNameStatusLabel.Size = new System.Drawing.Size(0, 17);
      // 
      // toolStripContainer1
      // 
      // 
      // toolStripContainer1.BottomToolStripPanel
      // 
      this.toolStripContainer1.BottomToolStripPanel.Controls.Add(m_StatusStrip1);
      // 
      // toolStripContainer1.ContentPanel
      // 
      this.toolStripContainer1.ContentPanel.Controls.Add(m_tableLayoutPanel1);
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(884, 355);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.LeftToolStripPanelVisible = false;
      this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.RightToolStripPanelVisible = false;
      this.toolStripContainer1.Size = new System.Drawing.Size(884, 402);
      this.toolStripContainer1.TabIndex = 1;
      this.toolStripContainer1.Text = "toolStripContainer1";
      // 
      // UserInterface
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(884, 402);
      this.Controls.Add(this.toolStripContainer1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MinimumSize = new System.Drawing.Size(900, 440);
      this.Name = "UserInterface";
      this.Text = "Import Data";
      m_tableLayoutPanel1.ResumeLayout(false);
      m_tableLayoutPanel1.PerformLayout();
      m_StatusStrip1.ResumeLayout(false);
      m_StatusStrip1.PerformLayout();
      this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
      this.toolStripContainer1.ContentPanel.ResumeLayout(false);
      this.toolStripContainer1.ResumeLayout(false);
      this.toolStripContainer1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private FileDialog m_FileManagementComonent;
    private System.Windows.Forms.Button m_TimeSlotsCreateButton;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.ToolStripProgressBar m_ToolStripProgressBar;
    private System.Windows.Forms.TextBox m_URLTextBox;
    private System.Windows.Forms.ToolStripStatusLabel m_ToolStripStatusLabel;
    private System.Windows.Forms.Button m_AddTimeSlotsButton;
    private System.Windows.Forms.Button m_ImportDictionaries;
    private System.Windows.Forms.ToolTip m_ToolTip;
    private System.Windows.Forms.ToolStripStatusLabel m_FileNameStatusLabel;
    private System.Windows.Forms.CheckBox m_TestDataCheckBox;
  }
}

