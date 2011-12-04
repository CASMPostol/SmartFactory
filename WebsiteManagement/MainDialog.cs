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
using CAS.SmartFactory.IPR.ListsEventsHandlers.Dictionaries;

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
      m_fastRefresh = true;
      Stream strm = OpenFile();
      if (strm == null)
      {
        m_ToolStripStatusLabel.Text = String.Empty;
        return;
      }
      try
      {
        m_ToolStripStatusLabel.Text = "Reading Data";
        m_ToolStripProgressBar.Value = 0;
        Configuration cnfg = Configuration.ImportDocument(strm);
        m_ToolStripProgressBar.Value = 10;
        m_ToolStripStatusLabel.Text = "Importing Data";
        EntitiesDataContext.ImportData(cnfg, m_URLTextBox.Text.Trim(), UpdateToolStrip);
        SetDone();
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
    private void m_SKUReadButton_Click(object sender, EventArgs e)
    {
      m_fastRefresh = true;
      Stream strm = OpenFile();
      if (strm == null)
      {
        m_ToolStripStatusLabel.Text = String.Empty;
        return;
      }
      try
      {
        m_ToolStripStatusLabel.Text = "Reading Data";
        m_ToolStripProgressBar.Value = 0;
        SKUEventHandlers.SKUEvetReceiher(strm, m_URLTextBox.Text.Trim(), 0, UpdateToolStrip);
        SetDone();
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
    private void UpdateToolStrip(object obj, ProgressChangedEventArgs progres)
    {
      m_ToolStripStatusLabel.Text = (string)progres.UserState;
      m_ToolStripProgressBar.Value += progres.ProgressPercentage;
      if (m_fastRefresh)
        this.Refresh();
      if (m_ToolStripProgressBar.Value >= m_ToolStripProgressBar.Maximum)
      {
        m_ToolStripProgressBar.Value = m_ToolStripProgressBar.Minimum;
        this.Refresh();
        m_fastRefresh = false;
      }
    }
    private void SetDone()
    {
      m_ToolStripStatusLabel.Text = "Done";
      m_ToolStripProgressBar.Value = m_ToolStripProgressBar.Minimum;
    }
    private Stream OpenFile()
    {
      m_ToolStripStatusLabel.Text = "Openning the file";
      using (OpenFileDialog od = new OpenFileDialog())
      {
        if (od.ShowDialog() != System.Windows.Forms.DialogResult.OK)
          return null;
        return od.OpenFile();
      }
    }
    private bool m_fastRefresh = true;
  }
}
