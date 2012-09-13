namespace CAS.SmartFactory.Management
{
  partial class MainDialog
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
      this.m_OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
      this.m_URLLabel = new System.Windows.Forms.Label();
      this.m_TableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
      this.m_ImportButton = new System.Windows.Forms.Button();
      this.m_SKUReadButton = new System.Windows.Forms.Button();
      this.m_URLTextBox = new System.Windows.Forms.TextBox();
      this.m_GetStockButton = new System.Windows.Forms.Button();
      this.m_InvoiceReadButton = new System.Windows.Forms.Button();
      this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
      this.statusStrip1 = new System.Windows.Forms.StatusStrip();
      this.m_ToolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
      this.m_ToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
      this.m_TableLayoutPanel.SuspendLayout();
      this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
      this.toolStripContainer1.ContentPanel.SuspendLayout();
      this.toolStripContainer1.SuspendLayout();
      this.statusStrip1.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_OpenFileDialog
      // 
      this.m_OpenFileDialog.DefaultExt = "xml";
      // 
      // m_URLLabel
      // 
      this.m_URLLabel.AutoSize = true;
      this.m_URLLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_URLLabel.Location = new System.Drawing.Point(3, 0);
      this.m_URLLabel.Name = "m_URLLabel";
      this.m_URLLabel.Size = new System.Drawing.Size(29, 26);
      this.m_URLLabel.TabIndex = 1;
      this.m_URLLabel.Text = "URL";
      this.m_URLLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_TableLayoutPanel
      // 
      this.m_TableLayoutPanel.ColumnCount = 2;
      this.m_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.m_TableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.m_TableLayoutPanel.Controls.Add(this.m_URLLabel, 0, 0);
      this.m_TableLayoutPanel.Controls.Add(this.m_ImportButton, 0, 1);
      this.m_TableLayoutPanel.Controls.Add(this.m_SKUReadButton, 0, 2);
      this.m_TableLayoutPanel.Controls.Add(this.m_URLTextBox, 1, 0);
      this.m_TableLayoutPanel.Controls.Add(this.m_GetStockButton, 0, 3);
      this.m_TableLayoutPanel.Controls.Add(this.m_InvoiceReadButton, 0, 4);
      this.m_TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_TableLayoutPanel.Location = new System.Drawing.Point(0, 0);
      this.m_TableLayoutPanel.Name = "m_TableLayoutPanel";
      this.m_TableLayoutPanel.RowCount = 5;
      this.m_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.m_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.m_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.m_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.m_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.m_TableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.m_TableLayoutPanel.Size = new System.Drawing.Size(384, 202);
      this.m_TableLayoutPanel.TabIndex = 2;
      // 
      // m_ImportButton
      // 
      this.m_TableLayoutPanel.SetColumnSpan(this.m_ImportButton, 2);
      this.m_ImportButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_ImportButton.Location = new System.Drawing.Point(3, 29);
      this.m_ImportButton.Name = "m_ImportButton";
      this.m_ImportButton.Size = new System.Drawing.Size(378, 40);
      this.m_ImportButton.TabIndex = 0;
      this.m_ImportButton.Text = "Read Configuration";
      this.m_ImportButton.UseVisualStyleBackColor = true;
      this.m_ImportButton.Click += new System.EventHandler(this.m_ImportButton_Click);
      // 
      // m_SKUReadButton
      // 
      this.m_TableLayoutPanel.SetColumnSpan(this.m_SKUReadButton, 2);
      this.m_SKUReadButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_SKUReadButton.Location = new System.Drawing.Point(3, 75);
      this.m_SKUReadButton.Name = "m_SKUReadButton";
      this.m_SKUReadButton.Size = new System.Drawing.Size(378, 40);
      this.m_SKUReadButton.TabIndex = 2;
      this.m_SKUReadButton.Text = "Read SKU XML Document";
      this.m_SKUReadButton.UseVisualStyleBackColor = true;
      this.m_SKUReadButton.Click += new System.EventHandler(this.m_SKUReadButton_Click);
      // 
      // m_URLTextBox
      // 
      this.m_URLTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.m_URLTextBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
      this.m_URLTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllSystemSources;
      this.m_URLTextBox.Location = new System.Drawing.Point(38, 3);
      this.m_URLTextBox.Name = "m_URLTextBox";
      this.m_URLTextBox.Size = new System.Drawing.Size(343, 20);
      this.m_URLTextBox.TabIndex = 3;
      this.m_URLTextBox.Text = "http://casmp/sites/ipr";
      // 
      // m_GetStockButton
      // 
      this.m_TableLayoutPanel.SetColumnSpan(this.m_GetStockButton, 2);
      this.m_GetStockButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_GetStockButton.Location = new System.Drawing.Point(3, 121);
      this.m_GetStockButton.Name = "m_GetStockButton";
      this.m_GetStockButton.Size = new System.Drawing.Size(378, 40);
      this.m_GetStockButton.TabIndex = 4;
      this.m_GetStockButton.Text = "Read Stock xml document";
      this.m_GetStockButton.UseVisualStyleBackColor = true;
      this.m_GetStockButton.Click += new System.EventHandler(this.m_GetStockButton_Click);
      // 
      // m_InvoiceReadButton
      // 
      this.m_TableLayoutPanel.SetColumnSpan(this.m_InvoiceReadButton, 2);
      this.m_InvoiceReadButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_InvoiceReadButton.Location = new System.Drawing.Point(3, 167);
      this.m_InvoiceReadButton.Name = "m_InvoiceReadButton";
      this.m_InvoiceReadButton.Size = new System.Drawing.Size(378, 32);
      this.m_InvoiceReadButton.TabIndex = 6;
      this.m_InvoiceReadButton.Text = "Read Invoice xml Document";
      this.m_InvoiceReadButton.UseVisualStyleBackColor = true;
      this.m_InvoiceReadButton.Click += new System.EventHandler(this.m_InvoiceReadButton_Click);
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
      this.toolStripContainer1.ContentPanel.Controls.Add(this.m_TableLayoutPanel);
      this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(384, 202);
      this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
      this.toolStripContainer1.Name = "toolStripContainer1";
      this.toolStripContainer1.Size = new System.Drawing.Size(384, 249);
      this.toolStripContainer1.TabIndex = 3;
      this.toolStripContainer1.Text = "toolStripContainer1";
      // 
      // statusStrip1
      // 
      this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
      this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_ToolStripStatusLabel,
            this.m_ToolStripProgressBar});
      this.statusStrip1.Location = new System.Drawing.Point(0, 0);
      this.statusStrip1.Name = "statusStrip1";
      this.statusStrip1.Size = new System.Drawing.Size(384, 22);
      this.statusStrip1.TabIndex = 0;
      // 
      // m_ToolStripStatusLabel
      // 
      this.m_ToolStripStatusLabel.AutoSize = false;
      this.m_ToolStripStatusLabel.Name = "m_ToolStripStatusLabel";
      this.m_ToolStripStatusLabel.Size = new System.Drawing.Size(200, 17);
      // 
      // m_ToolStripProgressBar
      // 
      this.m_ToolStripProgressBar.Name = "m_ToolStripProgressBar";
      this.m_ToolStripProgressBar.Size = new System.Drawing.Size(100, 16);
      // 
      // MainDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(384, 249);
      this.Controls.Add(this.toolStripContainer1);
      this.MinimumSize = new System.Drawing.Size(400, 100);
      this.Name = "MainDialog";
      this.Text = "Smart Factory Management";
      this.m_TableLayoutPanel.ResumeLayout(false);
      this.m_TableLayoutPanel.PerformLayout();
      this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
      this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
      this.toolStripContainer1.ContentPanel.ResumeLayout(false);
      this.toolStripContainer1.ResumeLayout(false);
      this.toolStripContainer1.PerformLayout();
      this.statusStrip1.ResumeLayout(false);
      this.statusStrip1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog m_OpenFileDialog;
    private System.Windows.Forms.Label m_URLLabel;
    private System.Windows.Forms.TableLayoutPanel m_TableLayoutPanel;
    private System.Windows.Forms.Button m_SKUReadButton;
    private System.Windows.Forms.ToolStripContainer toolStripContainer1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel m_ToolStripStatusLabel;
    private System.Windows.Forms.ToolStripProgressBar m_ToolStripProgressBar;
    private System.Windows.Forms.Button m_ImportButton;
    private System.Windows.Forms.TextBox m_URLTextBox;
    private System.Windows.Forms.Button m_GetStockButton;
    private System.Windows.Forms.Button m_InvoiceReadButton;
  }
}

