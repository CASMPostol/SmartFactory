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
    public AddNew( DataContextAsync context )
      : this()
    {
      m_DataContext = context;
    }
    internal Exception LastExeption { get; private set; }
    internal double ToDispose = 0;
    internal List<CustomsWarehouse> Accounts { get; private set; }

    #region private
    private DataContextAsync m_DataContext;
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
          LastExeption = e.Error;
          x_TextBoxSelectedBatch.Text = "Not found";
          x_TextBoxTotalStock.Text = e.Error.Message;
        }
        if ( e.Cancelled )
        {
          LastExeption = null;
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
        x_TextBoxSelectedBatch.Text = Accounts.First<CustomsWarehouse>().Batch;
        x_TextBoxTotalStock.Text = Accounts.Sum( x => x.TobaccoNotAllocated.Value ).ToString();
      }
      catch ( Exception ex )
      {
        global::System.Windows.MessageBox.Show( ex.Message, "Serach exception", MessageBoxButton.OK );
      }
    }
    #endregion

  }
}

