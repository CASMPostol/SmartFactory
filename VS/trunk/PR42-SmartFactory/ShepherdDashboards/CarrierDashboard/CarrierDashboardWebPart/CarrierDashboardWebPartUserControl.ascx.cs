using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Collections.Generic;
using System.Data;
using CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart
{
  public partial class CarrierDashboardWebPartUserControl : UserControl
  {
    internal void GetData(Dictionary<InterconnectionDataBase.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      foreach (var item in _ProvidesDictionary)
      {
        switch (item.Key)
        {
          case InterconnectionDataBase.ConnectionSelector.TrailerInterconnection:
            m_TrailerData.GetRowData(_ProvidesDictionary[InterconnectionDataBase.ConnectionSelector.TrailerInterconnection], NewDataEventHandler);
            break;
          case InterconnectionDataBase.ConnectionSelector.TruckInterconnection:
            m_TruckData.GetRowData(_ProvidesDictionary[InterconnectionDataBase.ConnectionSelector.TruckInterconnection], NewDataEventHandler);
            break;
          case InterconnectionDataBase.ConnectionSelector.ShippingInterconnection:
            m_ShippingData.GetRowData(_ProvidesDictionary[InterconnectionDataBase.ConnectionSelector.ShippingInterconnection], NewDataEventHandler);
            break;
          case InterconnectionDataBase.ConnectionSelector.TimeSlotInterconnection:
            m_TimeSlotData.GetRowData(_ProvidesDictionary[InterconnectionDataBase.ConnectionSelector.TimeSlotInterconnection], NewDataEventHandler);
            break;
          case InterconnectionDataBase.ConnectionSelector.WarehouseInterconnection:
            m_WarehouseData.GetRowData(_ProvidesDictionary[InterconnectionDataBase.ConnectionSelector.WarehouseInterconnection], NewDataEventHandler);
            break;
        }
      }
    }
    private void NewDataEventHandler(object sender, WarehouseInterconnectionData e)
    {
      m_WarehouseTextBox.Text = e.Title;
      m_WarehouseHiddenField.Value = e.ID;
    }
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
    }
    private void NewDataEventHandler(object sender, TimeSlotInterconnectionData e)
    {
      m_TimeSlotHiddenField.Value = e.ID;
      switch (m_State)
      {
        case ControlState.ViewState:
          break;
        case ControlState.NewState:
        case ControlState.EditState:
          m_TimeSlotTextBox.Text = e.GetDate;
          break;
        default:
          break;
      }
    }
    private void NewDataEventHandler(object sender, ShippingInterconnectionData e)
    {
      switch (m_State)
      {
        case ControlState.ViewState:
          m_TimeSlotHiddenField.Value = e.ID;
          m_TimeSlotTextBox.Text = e.TimeSlot;
          m_TruckRegistrationNumberTextBox.Text = e.TruckCarRegistrationNumber;
          m_TrailerRegistrationNumberTextBox.Text = e.TrailerRegistrationNumber;
          m_WarehouseTextBox.Text = e.Warehouse;
          break;
        case ControlState.EditState:
          break;
        case ControlState.NewState:
          break;
        default:
          break;
      }
    }
    private void NewDataEventHandler(object sender, TruckInterconnectionData e)
    {
      m_TruckRegistrationNumberTextBox.Text = e.Title;
      m_TruckRegistrationHiddenField.Value = e.ID;
    }
    private void NewDataEventHandler(object sender, TrailerInterconnectionData e)
    {
      m_TrailerRegistrationNumberTextBox.Text = e.Title;
      m_TrailerHiddenField.Value = e.ID;
    }
    #region private
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
        m_State = ControlState.ViewState;
      m_SaveButton.Click += new EventHandler(m_SaveButton_Click);
      m_NewShippingButton.Click += new EventHandler(m_NewShippingButton_Click);
      m_CancelButton.Click += new EventHandler(m_CancelButton_Click);
      m_EditButton.Click += new EventHandler(m_EditButton_Click);

    }
    private enum ControlState { ViewState, EditState, NewState }
    private ControlState m_State
    {
      set
      {
        m_StateLiteral.Text = value.ToString();
      }
      get
      {
        try
        {
          return (ControlState)Enum.Parse(typeof(ControlState), m_StateLiteral.Text);
        }
        catch (Exception)
        {
          return ControlState.ViewState;
        }
      }
    }
    private void RestoreState()
    {
      return;
    }
    private void ShowData()
    {
      m_TimeSlotTextBox.Text = m_NoConnectionMessage;
      m_TrailerRegistrationNumberTextBox.Text = m_NoConnectionMessage;
      m_TruckRegistrationNumberTextBox.Text = m_NoConnectionMessage;
      m_WarehouseTextBox.Text = m_NoConnectionMessage;
    }
    private string m_NoConnectionMessage = "No Connection";
    private TrailerInterconnectionData m_TrailerData = new TrailerInterconnectionData();
    private TruckInterconnectionData m_TruckData = new TruckInterconnectionData();
    private ShippingInterconnectionData m_ShippingData = new ShippingInterconnectionData();
    private TimeSlotInterconnectionData m_TimeSlotData = new TimeSlotInterconnectionData();
    private WarehouseInterconnectionData m_WarehouseData = new WarehouseInterconnectionData();
    #region Event Handlers
    private void m_NewShippingButton_Click(object sender, EventArgs e)
    {
      m_State = ControlState.NewState;
    }
    private void m_SaveButton_Click(object sender, EventArgs e)
    {
      m_State = ControlState.ViewState;
    }
    void m_CancelButton_Click(object sender, EventArgs e)
    {
      m_State = ControlState.ViewState;
    }
    void m_EditButton_Click(object sender, EventArgs e)
    {
      m_State = ControlState.EditState;
    }
    #endregion

    #endregion
  }
}
