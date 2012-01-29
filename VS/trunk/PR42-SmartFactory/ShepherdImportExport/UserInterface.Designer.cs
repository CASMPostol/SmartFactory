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
      System.Windows.Forms.StatusStrip m_StatusStrip1;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInterface));
      this.m_TimeSlotsCreateButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.m_URLTextBox = new System.Windows.Forms.TextBox();
      this.button1 = new System.Windows.Forms.Button();
      this.m_ImportDictionaries = new System.Windows.Forms.Button();
      this.m_ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
      this.m_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.m_FileManagementComonent = new CAS.SmartFactory.Shepherd.ImportExport.FileDialog(this.components);
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.m_ToolTip = new System.Windows.Forms.ToolTip(this.components);
      this.m_FileNameStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      m_tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
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
      m_tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
      m_tableLayoutPanel1.Controls.Add(this.m_URLTextBox, 1, 0);
      m_tableLayoutPanel1.Controls.Add(this.button1, 0, 2);
      m_tableLayoutPanel1.Controls.Add(this.m_ImportDictionaries, 0, 3);
      m_tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      m_tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      m_tableLayoutPanel1.Name = "m_tableLayoutPanel1";
      m_tableLayoutPanel1.RowCount = 5;
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      m_tableLayoutPanel1.Size = new System.Drawing.Size(597, 265);
      m_tableLayoutPanel1.TabIndex = 1;
      // 
      // m_TimeSlotsCreateButton
      // 
      this.m_TimeSlotsCreateButton.Location = new System.Drawing.Point(3, 29);
      this.m_TimeSlotsCreateButton.Name = "m_TimeSlotsCreateButton";
      this.m_TimeSlotsCreateButton.Size = new System.Drawing.Size(116, 23);
      this.m_TimeSlotsCreateButton.TabIndex = 0;
      this.m_TimeSlotsCreateButton.Text = "Generate Time Slots";
      this.m_TimeSlotsCreateButton.UseVisualStyleBackColor = true;
      this.m_TimeSlotsCreateButton.Click += new System.EventHandler(this.m_TimeSlotsCreateButton_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Dock = System.Windows.Forms.DockStyle.Left;
      this.label1.Location = new System.Drawing.Point(3, 0);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(78, 26);
      this.label1.TabIndex = 1;
      this.label1.Text = "Collection URL";
      this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_URLTextBox
      // 
      this.m_URLTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_URLTextBox.Location = new System.Drawing.Point(125, 3);
      this.m_URLTextBox.Name = "m_URLTextBox";
      this.m_URLTextBox.Size = new System.Drawing.Size(469, 20);
      this.m_URLTextBox.TabIndex = 2;
      this.m_URLTextBox.Text = "http://casmp/";
      // 
      // button1
      // 
      this.button1.AutoSize = true;
      this.button1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.button1.Location = new System.Drawing.Point(3, 58);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(116, 23);
      this.button1.TabIndex = 3;
      this.button1.Text = "Add TimeSlots";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
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
      this.m_ImportDictionaries.Click += new System.EventHandler(this.m_ImportDictionaries_Click);
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
      m_StatusStrip1.Size = new System.Drawing.Size(597, 22);
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
      this.m_ToolStripStatusLabel.Size = new System.Drawing.Size(100, 17);
      this.m_ToolStripStatusLabel.Text = "----";
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
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(597, 265);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.LeftToolStripPanelVisible = false;
      this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.RightToolStripPanelVisible = false;
      this.toolStripContainer1.Size = new System.Drawing.Size(597, 312);
      this.toolStripContainer1.TabIndex = 1;
      this.toolStripContainer1.Text = "toolStripContainer1";
      // 
      // m_FileNameStatusLabel
      // 
      this.m_FileNameStatusLabel.Name = "m_FileNameStatusLabel";
      this.m_FileNameStatusLabel.Size = new System.Drawing.Size(0, 17);
      // 
      // UserInterface
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(597, 312);
      this.Controls.Add(this.toolStripContainer1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox m_URLTextBox;
    private System.Windows.Forms.ToolStripStatusLabel m_ToolStripStatusLabel;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button m_ImportDictionaries;
    private System.Windows.Forms.ToolTip m_ToolTip;
    private System.Windows.Forms.ToolStripStatusLabel m_FileNameStatusLabel;
  }
}

