using System;
using System.IO;
using System.Windows.Forms;
using CAS.SmartFactory.xml.Customs;

namespace CAS.SmartFactory.xml.Test
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }
    private CustomsDocument m_CustomsDocument;

    private void m_CustomsOpenButton_Click(object sender, EventArgs e)
    {
      if (m_OpenXmlFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        try
        {
          using (Stream firlStrim = new FileStream(m_OpenXmlFile.FileName, FileMode.Open))
          {
            m_CustomsDocument = CustomsDocument.ImportDocument(firlStrim);
            m_DocumentContentLabel.Text = m_CustomsDocument.GetType().ToString();
          }
        }
        catch (Exception ex)
        {
          MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
          m_DocumentContentLabel.Text = ex.Message;
        }
      }
    }
  }
}
