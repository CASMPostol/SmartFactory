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
      this.m_UninstallListBox = new System.Windows.Forms.ListBox();
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.m_InstalationStatePropertyGrid = new System.Windows.Forms.PropertyGrid();
      this.tableLayoutPanel1.SuspendLayout();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_UninstallListBox
      // 
      this.m_UninstallListBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_UninstallListBox.Enabled = false;
      this.m_UninstallListBox.FormattingEnabled = true;
      this.m_UninstallListBox.ItemHeight = 16;
      this.m_UninstallListBox.Location = new System.Drawing.Point(0, 0);
      this.m_UninstallListBox.Margin = new System.Windows.Forms.Padding(4);
      this.m_UninstallListBox.Name = "m_UninstallListBox";
      this.m_UninstallListBox.Size = new System.Drawing.Size(599, 216);
      this.m_UninstallListBox.TabIndex = 0;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 3;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.splitContainer1, 0, 1);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(605, 441);
      this.tableLayoutPanel1.TabIndex = 1;
      // 
      // splitContainer1
      // 
      this.tableLayoutPanel1.SetColumnSpan(this.splitContainer1, 3);
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.Location = new System.Drawing.Point(3, 3);
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
      this.splitContainer1.Size = new System.Drawing.Size(599, 435);
      this.splitContainer1.SplitterDistance = 215;
      this.splitContainer1.TabIndex = 4;
      // 
      // m_InstalationStatePropertyGrid
      // 
      this.m_InstalationStatePropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_InstalationStatePropertyGrid.Location = new System.Drawing.Point(0, 0);
      this.m_InstalationStatePropertyGrid.Name = "m_InstalationStatePropertyGrid";
      this.m_InstalationStatePropertyGrid.Size = new System.Drawing.Size(599, 215);
      this.m_InstalationStatePropertyGrid.TabIndex = 3;
      // 
      // Uninstall
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.AppWorkspace;
      this.Controls.Add(this.tableLayoutPanel1);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
      this.Margin = new System.Windows.Forms.Padding(4);
      this.Name = "Uninstall";
      this.Size = new System.Drawing.Size(605, 441);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox m_UninstallListBox;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.PropertyGrid m_InstalationStatePropertyGrid;

  }
}