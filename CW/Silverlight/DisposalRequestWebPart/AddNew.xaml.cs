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
  public partial class AddNew : ChildWindow
  {
    #region public
    public AddNew()
    {
      InitializeComponent();
      CustomsProcedure = string.Empty;
    }
    public AddNew(DataContextAsync context)
      : this()
    {
      m_DataContext = context;
    }
    internal double ToDispose { get; private set; }
    internal List<CustomsWarehouse> Accounts { get; private set; }
    internal string CustomsProcedure { get; private set; }
    #endregion

    #region private
    private const string c_DoubleFormat = "F2";
    private double m_Available = 0;
    private DataContextAsync m_DataContext;
    private void OKButton_Click(object sender, RoutedEventArgs e)
    {
      double _2dsps = 0;
      if (!Double.TryParse(x_TextBoxQtyToClear.Text, out _2dsps))
      {
        MessageBox.Show("Wrong qty to dispose", (string)this.Title, MessageBoxButton.OK);
        return;
      }
      if (Accounts.Count == 0)
      {
        MessageBox.Show("No account found. Are you sure to close the window.", (string)this.Title, MessageBoxButton.OK);
        return;
      }
      if (_2dsps > m_Available || _2dsps < 0)
      {
        x_TextBoxQtyToClear.Text = x_TextBoxTotalStock.Text;
        string _msg = String.Format("The entered qty is not available, {0} kg is available.", m_Available.ToString(c_DoubleFormat));
        MessageBox.Show(_msg, (string)this.Title, MessageBoxButton.OK);
        return;
      }
      CustomsProcedure = ((ComboBoxItem)x_ComboBoxProcedure.SelectedItem).Content as string;
      ToDispose = _2dsps;
      this.DialogResult = true;
    }
    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
      this.DialogResult = false;
    }
    private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
    {

    }
    private void x_ButtonBatchSearch_Click(object sender, RoutedEventArgs e)
    {
      if (m_DataContext == null)
      {
        this.DialogResult = false;
        return;
      }
      m_DataContext.GetListCompleted += m_DataContext_GetListCompleted;
      m_DataContext.GetListAsync<CustomsWarehouse>(CommonDefinition.CustomsWarehouseTitle,
                                                    CommonDefinition.GetCAMLSelectedID(x_TextBoxBatchSearch.Text, CommonDefinition.FieldBatch, CommonDefinition.CAMLTypeText)
                                                   );
    }
    private void m_DataContext_GetListCompleted(object siurce, GetListAsyncCompletedEventArgs e)
    {
      try
      {
        m_DataContext.GetListCompleted -= m_DataContext_GetListCompleted;
        if (e.Error != null)
        {
          x_TextBoxSelectedBatch.Text = "Not found";
          x_TextBoxTotalStock.Text = e.Error.Message;
        }
        if (e.Cancelled)
        {
          x_TextBoxSelectedBatch.Text = "Not found";
          x_TextBoxTotalStock.Text = "Cancelled";
        }
        Accounts = e.Result<CustomsWarehouse>();
        if (Accounts.Count == 0)
        {
          x_TextBoxSelectedBatch.Text = "Not found";
          x_TextBoxTotalStock.Text = "N/A";
          return;
        }
        Accounts.Sort(new Comparison<CustomsWarehouse>(CustomsWarehouse.CompareCustomsWarehouse));
        x_TextBoxSelectedBatch.Text = String.Format("{0}/{1} accounts", Accounts.First<CustomsWarehouse>().Batch, Accounts.Count);
        m_Available = Accounts.Sum(x => x.TobaccoNotAllocated.Value);
        x_TextBoxTotalStock.Text = m_Available.ToString(c_DoubleFormat);
        x_TextBoxQtyToClear.Text = 0.0.ToString(c_DoubleFormat);
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message, "Serach exception", MessageBoxButton.OK);
      }
    }
    #endregion

  }
}

