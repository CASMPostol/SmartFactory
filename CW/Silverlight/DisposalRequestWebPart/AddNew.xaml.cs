//<summary>
//  Title   : Name of Application
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
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
    }
    public AddNew( DataContextAsync context )
      : this()
    {
      m_DataContext = context;
    }
    internal double ToDispose { get; private set; }
    internal List<CustomsWarehouse> Accounts { get; private set; }

    #region private
    private DataContextAsync m_DataContext;
    private void OKButton_Click( object sender, RoutedEventArgs e )
    {
      double _2dsps = 0;
      if ( !Double.TryParse( x_TextBoxQtyToClear.Text, out _2dsps ) )
        MessageBox.Show( (string)this.Title, "Wrong qty to dispose", MessageBoxButton.OK );
      ToDispose = _2dsps;
      if ( Accounts.Count == 0 )
      {
        if ( MessageBox.Show( (string)this.Title, "No account found. Are you sure to close the window.", MessageBoxButton.OKCancel ) != MessageBoxResult.OK )
          this.DialogResult = false;
          return;
      }
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
      if ( m_DataContext == null )
      {
        this.DialogResult = false;
        return;
      }
      m_DataContext.GetListCompleted += m_DataContext_GetListCompleted;
      m_DataContext.GetListAsync<CustomsWarehouse>( CommonDefinition.CustomsWarehouseTitle,
                                                    CommonDefinition.GetCAMLSelectedID( x_TextBoxBatchSearch.Text, CommonDefinition.FieldBatch, CommonDefinition.CAMLTypeText )
                                                   );
    }
    private void m_DataContext_GetListCompleted( object siurce, GetListAsyncCompletedEventArgs e )
    {
      try
      {
        m_DataContext.GetListCompleted -= m_DataContext_GetListCompleted;
        if ( e.Error != null )
        {
          x_TextBoxSelectedBatch.Text = "Not found";
          x_TextBoxTotalStock.Text = e.Error.Message;
        }
        if ( e.Cancelled )
        {
          x_TextBoxSelectedBatch.Text = "Not found";
          x_TextBoxTotalStock.Text = "Cancelled";
        }
        Accounts = e.Result<CustomsWarehouse>();
        if ( Accounts.Count == 0 )
        {
          x_TextBoxSelectedBatch.Text = "Not found";
          x_TextBoxTotalStock.Text = "N/A";
          return;
        }
        x_TextBoxSelectedBatch.Text = String.Format( "{0}/{1} accounts", Accounts.First<CustomsWarehouse>().Batch, Accounts.Count );
        x_TextBoxTotalStock.Text = Accounts.Sum( x => x.TobaccoNotAllocated.Value ).ToString();
      }
      catch ( Exception ex )
      {
        MessageBox.Show( ex.Message, "Serach exception", MessageBoxButton.OK );
      }
    }
    #endregion

  }
}

