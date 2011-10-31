namespace CAS.SmartFactory.xml.Test
{
  partial class MainForm
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
      this.m_OpenXmlFile = new System.Windows.Forms.OpenFileDialog();
      this.m_CustomsOpenButton = new System.Windows.Forms.Button();
      this.m_DocumentContentLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // m_OpenXmlFile
      // 
      this.m_OpenXmlFile.DefaultExt = "xml";
      this.m_OpenXmlFile.FileName = "OpenXmlFile";
      this.m_OpenXmlFile.Title = "Customs XML file";
      // 
      // m_CustomsOpenButton
      // 
      this.m_CustomsOpenButton.Location = new System.Drawing.Point(12, 30);
      this.m_CustomsOpenButton.Name = "m_CustomsOpenButton";
      this.m_CustomsOpenButton.Size = new System.Drawing.Size(220, 23);
      this.m_CustomsOpenButton.TabIndex = 0;
      this.m_CustomsOpenButton.Text = "Open Customs XML";
      this.m_CustomsOpenButton.UseVisualStyleBackColor = true;
      this.m_CustomsOpenButton.Click += new System.EventHandler(this.m_CustomsOpenButton_Click);
      // 
      // m_DocumentContentLabel
      // 
      this.m_DocumentContentLabel.AutoSize = true;
      this.m_DocumentContentLabel.Location = new System.Drawing.Point(22, 86);
      this.m_DocumentContentLabel.Name = "m_DocumentContentLabel";
      this.m_DocumentContentLabel.Size = new System.Drawing.Size(0, 13);
      this.m_DocumentContentLabel.TabIndex = 1;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.m_DocumentContentLabel);
      this.Controls.Add(this.m_CustomsOpenButton);
      this.Name = "MainForm";
      this.Text = "Form1";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.OpenFileDialog m_OpenXmlFile;
    private System.Windows.Forms.Button m_CustomsOpenButton;
    private System.Windows.Forms.Label m_DocumentContentLabel;
  }
}

