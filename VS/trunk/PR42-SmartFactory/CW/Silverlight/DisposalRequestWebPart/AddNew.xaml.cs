using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Data;
using CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart.Linq;

namespace CAS.SmartFactory.CW.Dashboards.DisposalRequestWebPart
{
  public partial class AddNew: ChildWindow
  {
    public AddNew()
    {
      InitializeComponent();
      LastExeption = null;
    }
    public AddNew( Entities context )
      : this()
    {
      m_DataContext = context;
    }
    internal Exception LastExeption { get; private set; }
    internal double ToDispose = 0;
    internal List<CustomsWarehouse> Accounts { get; private set; }
    private Entities m_DataContext;
    private BackgroundWorker m_Worker = new BackgroundWorker();
    private void OKButton_Click( object sender, RoutedEventArgs e )
    {
      this.DialogResult = true;
    }
    private void CancelButton_Click( object sender, RoutedEventArgs e )
    {
      this.DialogResult = false;
    }
    private void ChildWindow_Loaded( object sender, RoutedEventArgs e )
    {

    }
    private void x_ButtonBatchSearch_Click( object sender, RoutedEventArgs e )
    {
      m_Worker.DoWork += Worker_DoWork;
      m_Worker.ProgressChanged += Worker_ProgressChanged;
      m_Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
      m_Worker.WorkerReportsProgress = false;
      m_Worker.WorkerSupportsCancellation = false;
      m_Worker.RunWorkerAsync();
    }
    private void Worker_RunWorkerCompleted( object sender, RunWorkerCompletedEventArgs e )
    {
      if ( e.Error != null )
      {
        LastExeption = e.Error;
        DialogResult = false;
        return;
      }
      if ( e.Cancelled )
      {
        LastExeption = null;
        DialogResult = false;
        return;
      }
      Accounts = (List<CustomsWarehouse>)e.Result;
      if ( Accounts.Count == 0 )
      {
        x_TextBoxSelectedBatch.Text = "Not found";
        x_TextBoxTotalStock.Text = "N/A";
        return;
      }
      x_TextBoxSelectedBatch.Text = Accounts.First<CustomsWarehouse>().Batch;
      x_TextBoxTotalStock.Text = Accounts.Sum( x => x.TobaccoNotAllocated.Value ).ToString();
    }
    private void Worker_ProgressChanged( object sender, ProgressChangedEventArgs e )
    {
      throw new NotImplementedException();
    }
    private void Worker_DoWork( object sender, DoWorkEventArgs e )
    {
      BackgroundWorker _mq = (BackgroundWorker)sender;
      e.Result = m_DataContext.CustomsWarehouse.Filter( CommonDefinition.GetCAMLSelectedID( x_TextBoxBatchSearch.Text, CommonDefinition.FieldCWDisposal2DisposalRequestLibraryID, CommonDefinition.CAMLTypeNumber ) ).ToList();
    }
  }
}

