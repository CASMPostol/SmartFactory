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
      m_label1 = new System.Windows.Forms.Label();
      this.tableLayoutPanel1.SuspendLayout();
      this.SuspendLayout();
      // 
      // m_label1
      // 
      m_label1.AutoSize = true;
      m_label1.Location = new System.Drawing.Point(3, 0);
      m_label1.Name = "m_label1";
      m_label1.Size = new System.Drawing.Size(125, 13);
      m_label1.TabIndex = 1;
      m_label1.Text = "Uninstallation progress ...";
      m_label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // m_UninstallListBox
      // 
      this.tableLayoutPanel1.SetColumnSpan(this.m_UninstallListBox, 2);
      this.m_UninstallListBox.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_UninstallListBox.Enabled = false;
      this.m_UninstallListBox.FormattingEnabled = true;
      this.m_UninstallListBox.Location = new System.Drawing.Point(3, 16);
      this.m_UninstallListBox.Name = "m_UninstallListBox";
      this.m_UninstallListBox.Size = new System.Drawing.Size(448, 310);
      this.m_UninstallListBox.TabIndex = 0;
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.Controls.Add(this.m_UninstallListBox, 0, 1);
      this.tableLayoutPanel1.Controls.Add(m_label1, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.m_CloseButton, 0, 2);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 3;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(454, 358);
      this.tableLayoutPanel1.TabIndex = 1;
      // 
      // m_CloseButton
      // 
      this.m_CloseButton.Dock = System.Windows.Forms.DockStyle.Fill;
      this.m_CloseButton.Location = new System.Drawing.Point(3, 332);
      this.m_CloseButton.Name = "m_CloseButton";
      this.m_CloseButton.Size = new System.Drawing.Size(125, 23);
      this.m_CloseButton.TabIndex = 2;
      this.m_CloseButton.Text = "Close";
      this.m_CloseButton.UseVisualStyleBackColor = true;
      this.m_CloseButton.Click += new System.EventHandler(this.m_CloseButton_Click);
      // 
      // Uninstall
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(454, 358);
      this.Controls.Add(this.tableLayoutPanel1);
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.Name = "Uninstall";
      this.Text = "Smart Factory Uninstall";
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListBox m_UninstallListBox;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Button m_CloseButton;

  }
}