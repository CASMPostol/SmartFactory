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
      this.m_ImportButton = new System.Windows.Forms.Button();
      this.m_Status = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // m_OpenFileDialog
      // 
      this.m_OpenFileDialog.DefaultExt = "xml";
      // 
      // m_ImportButton
      // 
      this.m_ImportButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.m_ImportButton.Location = new System.Drawing.Point(12, 12);
      this.m_ImportButton.Name = "m_ImportButton";
      this.m_ImportButton.Size = new System.Drawing.Size(347, 43);
      this.m_ImportButton.TabIndex = 0;
      this.m_ImportButton.Text = "Read Configuration";
      this.m_ImportButton.UseVisualStyleBackColor = true;
      this.m_ImportButton.Click += new System.EventHandler(this.m_ImportButton_Click);
      // 
      // m_Status
      // 
      this.m_Status.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.m_Status.Location = new System.Drawing.Point(19, 72);
      this.m_Status.Name = "m_Status";
      this.m_Status.Size = new System.Drawing.Size(339, 40);
      this.m_Status.TabIndex = 1;
      // 
      // MainDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(384, 161);
      this.Controls.Add(this.m_Status);
      this.Controls.Add(this.m_ImportButton);
      this.MinimumSize = new System.Drawing.Size(400, 100);
      this.Name = "MainDialog";
      this.Text = "Smart Factory Management";
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog m_OpenFileDialog;
    private System.Windows.Forms.Button m_ImportButton;
    private System.Windows.Forms.Label m_Status;
  }
}

