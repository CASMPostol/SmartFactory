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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserInterface));
      this.m_InputFileDialog = new CAS.SmartFactory.Shepherd.ImportExport.FileDialog(this.components);
      this.m_TimeSlotsCreateButton = new System.Windows.Forms.Button();
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.m_ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
      this.label1 = new System.Windows.Forms.Label();
      this.m_URLTextBox = new System.Windows.Forms.TextBox();
      m_tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      m_tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_TimeSlotsCreateButton
      // 
      this.m_TimeSlotsCreateButton.Location = new System.Drawing.Point(3, 29);
      this.m_TimeSlotsCreateButton.Name = "m_TimeSlotsCreateButton";
      this.m_TimeSlotsCreateButton.Size = new System.Drawing.Size(116, 23);
      this.m_TimeSlotsCreateButton.TabIndex = 0;
      this.m_TimeSlotsCreateButton.Text = "Create Time Slots";
      this.m_TimeSlotsCreateButton.UseVisualStyleBackColor = true;
      this.m_TimeSlotsCreateButton.Click += new System.EventHandler(this.m_TimeSlotsCreateButton_Click);
      // 
      // toolStripContainer1
      // 
      // 
      // toolStripContainer1.BottomToolStripPanel
      // 
      this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
      // 
      // toolStripContainer1.ContentPanel
      // 
      this.toolStripContainer1.ContentPanel.Controls.Add(m_tableLayoutPanel1);
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(284, 240);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.LeftToolStripPanelVisible = false;
      this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.RightToolStripPanelVisible = false;
      this.toolStripContainer1.Size = new System.Drawing.Size(284, 262);
      this.toolStripContainer1.TabIndex = 1;
      this.toolStripContainer1.Text = "toolStripContainer1";
      // 
      // statusStrip1
      // 
      this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_ToolStripProgressBar});
      this.statusStrip1.Location = new System.Drawing.Point(0, 0);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(284, 22);
      this.statusStrip1.TabIndex = 0;
      // 
      // m_ToolStripProgressBar
      // 
      this.m_ToolStripProgressBar.Name = "m_ToolStripProgressBar";
      this.m_ToolStripProgressBar.Size = new System.Drawing.Size(100, 16);
      // 
      // m_tableLayoutPanel1
      // 
      m_tableLayoutPanel1.ColumnCount = 2;
      m_tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      m_tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      m_tableLayoutPanel1.Controls.Add(this.m_TimeSlotsCreateButton, 0, 1);
      m_tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
      m_tableLayoutPanel1.Controls.Add(this.m_URLTextBox, 1, 0);
      m_tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      m_tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      m_tableLayoutPanel1.Name = "m_tableLayoutPanel1";
      m_tableLayoutPanel1.RowCount = 2;
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      m_tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      m_tableLayoutPanel1.Size = new System.Drawing.Size(284, 240);
      m_tableLayoutPanel1.TabIndex = 1;
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
      this.m_URLTextBox.Size = new System.Drawing.Size(156, 20);
      this.m_URLTextBox.TabIndex = 2;
      this.m_URLTextBox.Text = "http://casmp/";
      // 
      // UserInterface
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.toolStripContainer1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "UserInterface";
      this.Text = "Import Data";
      this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
      this.toolStripContainer1.ContentPanel.ResumeLayout(false);
      this.toolStripContainer1.ResumeLayout(false);
      this.toolStripContainer1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      m_tableLayoutPanel1.ResumeLayout(false);
      m_tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private FileDialog m_InputFileDialog;
    private System.Windows.Forms.Button m_TimeSlotsCreateButton;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripProgressBar m_ToolStripProgressBar;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox m_URLTextBox;
  }
}

