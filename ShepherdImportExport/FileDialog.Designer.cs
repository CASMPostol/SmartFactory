namespace CAS.SmartFactory.Shepherd.ImportExport
{
  partial class FileDialog
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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.m_OpenFileDialog = new System.Windows.Forms.OpenFileDialog();
      // 
      // m_OpenFileDialog
      // 
      this.m_OpenFileDialog.DefaultExt = "xml";
      this.m_OpenFileDialog.FileName = "ShepherdData";
      this.m_OpenFileDialog.Filter = "Xml files (*.xml)|*.xml|All files (*.*)|*.*";
      this.m_OpenFileDialog.SupportMultiDottedExtensions = true;
      this.m_OpenFileDialog.Title = "Preliminary Data File";

    }

    #endregion

    public System.Windows.Forms.OpenFileDialog m_OpenFileDialog;

  }
}
