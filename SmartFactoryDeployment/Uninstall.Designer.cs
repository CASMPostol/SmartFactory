namespace CAS.SmartFactory.Deployment
{
  partial class Uninstall
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
      System.Windows.Forms.Label m_label1;
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Uninstall));
      this.m_UninstallListBox = new System.Windows.Forms.ListBox();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.m_CloseButton = new System.Windows.Forms.Button();
      this.m_InstalationStatePropertyGrid = new System.Windows.Forms.PropertyGrid();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.m_UninstallButton = new System.Windows.Forms.Button();
      m_label1 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_label1
      // 
      m_label1.AutoSize = true;
      this.tableLayoutPanel1.SetColumnSpan(m_label1, 2);
      m_label1.Location = new System.Drawing.Point(4, 0);
      m_label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      m_label1.Name = "m_label1";
      m_label1.Size = new System.Drawing.Size(157, 16);
      m_label1.TabIndex = 1;
      m_label1.Text = "Uninstallation progress ...";
      m_label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_UninstallListBox
      // 
      this.m_UninstallListBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_UninstallListBox.Enabled = false;
      this.m_UninstallListBox.FormattingEnabled = true;
      this.m_UninstallListBox.ItemHeight = 16;
      this.m_UninstallListBox.Location = new System.Drawing.Point(0, 0);
      this.m_UninstallListBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.m_UninstallListBox.Name = "m_UninstallListBox";
      this.m_UninstallListBox.Size = new System.Drawing.Size(599, 188);
      this.m_UninstallListBox.TabIndex = 0;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(m_label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.m_CloseButton, 0, 2);
      this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
      this.tableLayoutPanel1.Controls.Add(this.m_UninstallButton, 1, 2);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(605, 441);
      this.tableLayoutPanel1.TabIndex = 1;
      // 
      // m_CloseButton
      // 
      this.m_CloseButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_CloseButton.Location = new System.Drawing.Point(4, 409);
      this.m_CloseButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.m_CloseButton.Name = "m_CloseButton";
      this.m_CloseButton.Size = new System.Drawing.Size(92, 28);
      this.m_CloseButton.TabIndex = 2;
      this.m_CloseButton.Text = "Cancel";
      this.m_CloseButton.UseVisualStyleBackColor = true;
      this.m_CloseButton.Click += new System.EventHandler(this.m_CloseButton_Click);
      // 
      // m_InstalationStatePropertyGrid
      // 
      this.m_InstalationStatePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_InstalationStatePropertyGrid.Location = new System.Drawing.Point(0, 0);
      this.m_InstalationStatePropertyGrid.Name = "m_InstalationStatePropertyGrid";
      this.m_InstalationStatePropertyGrid.Size = new System.Drawing.Size(599, 191);
      this.m_InstalationStatePropertyGrid.TabIndex = 3;
      // 
      // splitContainer1
      // 
      this.tableLayoutPanel1.SetColumnSpan(this.splitContainer1, 3);
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(3, 19);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.Controls.Add(this.m_InstalationStatePropertyGrid);
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.Controls.Add(this.m_UninstallListBox);
      this.splitContainer1.Size = new System.Drawing.Size(599, 383);
      this.splitContainer1.SplitterDistance = 191;
      this.splitContainer1.TabIndex = 4;
      // 
      // m_UninstallButton
      // 
      this.m_UninstallButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_UninstallButton.Location = new System.Drawing.Point(103, 408);
      this.m_UninstallButton.Name = "m_UninstallButton";
      this.m_UninstallButton.Size = new System.Drawing.Size(94, 30);
      this.m_UninstallButton.TabIndex = 5;
      this.m_UninstallButton.Text = "Uninstall";
      this.m_UninstallButton.UseVisualStyleBackColor = true;
      this.m_UninstallButton.Click += new System.EventHandler(this.m_UninstallButton_Click);
      // 
      // Uninstall
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.AppWorkspace;
      this.ClientSize = new System.Drawing.Size(605, 441);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
      this.Name = "Uninstall";
      this.Text = "Smart Factory Uninstall";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox m_UninstallListBox;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Button m_CloseButton;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.PropertyGrid m_InstalationStatePropertyGrid;
    private System.Windows.Forms.Button m_UninstallButton;

  }
}