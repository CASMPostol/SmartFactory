using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using CAS.SmartFactory.xml.Dictionaries;
using CAS.SmartFactory.IPR.Entities;

namespace CAS.SmartFactory.Management
{
  /// <summary>
  /// Main user interafe to manage website content.
  /// </summary>
  public partial class MainDialog : Form
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MainDialog"/> class that is
    /// main user interface.
    /// </summary>
    public MainDialog()
    {
      InitializeComponent();
    }

    private void m_ImportButton_Click(object sender, EventArgs e)
    {
      m_Status.Text = "Openning the file";
      Stream strm = null;
      using (OpenFileDialog od = new OpenFileDialog())
      {
        if (od.ShowDialog() != System.Windows.Forms.DialogResult.OK)
          return;
        strm = od.OpenFile();
      }
      try
      {
        m_Status.Text = "Reading Data";
        Configuration cnfg = Configuration.ImportDocument(strm);
        EntitiesDataContext.ImportData(cnfg, @"http://casmp/sites/IPR");
        m_Status.Text = "Done";
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
      finally
      {
        if (strm != null)
          strm.Dispose();
      }
    }
  }
}
