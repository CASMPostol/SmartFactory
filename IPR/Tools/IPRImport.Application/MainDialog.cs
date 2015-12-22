using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using CAS.SmartFactory.IPR.ListsEventsHandlers.Reports;
using CAS.SmartFactory.xml.Dictionaries;

namespace CAS.SmartFactory.Management
{
  /// <summary>
  /// Main user interafe to manage website content.
  /// </summary>
  public partial class MainDialog: Form
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="MainDialog"/> class that is
    /// main user interface.
    /// </summary>
    public MainDialog()
    {
      InitializeComponent();
    }
    private void UpdateToolStrip( object obj, ProgressChangedEventArgs progres )
    {
      m_ToolStripStatusLabel.Text = (string)progres.UserState;
      if ( m_ToolStripProgressBar.Value + progres.ProgressPercentage > m_ToolStripProgressBar.Maximum )
        m_ToolStripProgressBar.Value = 0;
      else
        m_ToolStripProgressBar.Value += progres.ProgressPercentage;
      if ( m_fastRefresh )
        this.Refresh();
      if ( m_ToolStripProgressBar.Value >= m_ToolStripProgressBar.Maximum )
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
      UpdateToolStrip( this, new ProgressChangedEventArgs( 1, "Openning the file" ) );
      if ( m_OpenFileDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK )
        return null;
      return m_OpenFileDialog.OpenFile();
    }
    private bool m_fastRefresh = true;
    private void m_ImportButton_Click( object sender, EventArgs e )
    {
      m_fastRefresh = true;
      Stream strm = OpenFile();
      if ( strm == null )
      {
        m_ToolStripStatusLabel.Text = String.Empty;
        return;
      }
      try
      {
        UpdateToolStrip( this, new ProgressChangedEventArgs( 1, "Reading Data" ) );
        Configuration cnfg = Configuration.ImportDocument( strm );
        UpdateToolStrip( this, new ProgressChangedEventArgs( 10, "Importing Data" ) );
        DictionaryImport.ImportData( cnfg, m_URLTextBox.Text.Trim(), UpdateToolStrip );
        SetDone();
      }
      catch ( Exception ex )
      {
        MessageBox.Show( ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error );
      }
      finally
      {
        if ( strm != null )
          strm.Dispose();
      }
    }
    private void m_SKUReadButton_Click( object sender, EventArgs e )
    {
      //m_fastRefresh = true;
      //Stream strm = OpenFile();
      //if ( strm == null )
      //{
      //  m_ToolStripStatusLabel.Text = String.Empty;
      //  return;
      //}
      //try
      //{
      //  m_ToolStripStatusLabel.Text = "Reading Data";
      //  m_ToolStripProgressBar.Value = 0;
      //  SKUEventHandlers.SKUEvetReceiher( strm, m_URLTextBox.Text.Trim(), 0, m_OpenFileDialog.FileName, UpdateToolStrip );
      //  SetDone();
      //}
      //catch ( Exception ex )
      //{
      //  MessageBox.Show( ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error );
      //}
      //finally
      //{
      //  if ( strm != null )
      //    strm.Dispose();
      //}
    }
    private void m_GetStockButton_Click( object sender, EventArgs e )
    {
      m_fastRefresh = true;
      Stream strm = OpenFile();
      if ( strm == null )
      {
        m_ToolStripStatusLabel.Text = "Aborted";
        m_ToolStripProgressBar.Value = 0;
        return;
      }
      try
      {
        m_ToolStripStatusLabel.Text = "Reading Data";
        m_ToolStripProgressBar.Value = 0;
        StockLibraryEventReceiver.IportStockFromXML( strm, m_URLTextBox.Text.Trim(), 0, m_OpenFileDialog.FileName, UpdateToolStrip );
        SetDone();
      }
      catch ( Exception ex )
      {
        MessageBox.Show( ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error );
      }
      finally
      {
        if ( strm != null )
          strm.Dispose();
      }
    }
    private void m_InvoiceReadButton_Click( object sender, EventArgs e )
    {
      m_fastRefresh = true;
      Stream strm = OpenFile();
      if ( strm == null )
      {
        m_ToolStripStatusLabel.Text = "Aborted";
        m_ToolStripProgressBar.Value = 0;
        return;
      }
      try
      {
        m_ToolStripStatusLabel.Text = "Reading Data";
        m_ToolStripProgressBar.Value = 0;
        this.Refresh();
        CAS.SmartFactory.IPR.ListsEventsHandlers.InvoiceEventReceiver.IportInvoiceFromXml( strm, m_URLTextBox.Text.Trim(), 0, m_OpenFileDialog.FileName, UpdateToolStrip );
        SetDone();
      }
      catch ( Exception ex )
      {
        MessageBox.Show( ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error );
      }
      finally
      {
        if ( strm != null )
          strm.Dispose();
      }
    }
  }
}
