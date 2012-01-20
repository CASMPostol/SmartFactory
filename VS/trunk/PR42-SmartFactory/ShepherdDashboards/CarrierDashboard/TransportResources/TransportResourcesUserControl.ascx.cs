using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;
using System.Linq;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TransportResources
{
  public partial class TransportResourcesUserControl : UserControl
  {
    #region public
    internal void GetData(System.Collections.Generic.Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      foreach (var item in _ProvidesDictionary)
        switch (item.Key)
        {
          case InboundInterconnectionData.ConnectionSelector.ShippingInterconnection:
            new ShippingInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
            break;
          case InboundInterconnectionData.ConnectionSelector.PartnerInterconnection:
            new PartnerInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
            break;
          default:
            break;
        }
    }
    #endregion

    #region private

    #region state management
    [Serializable]
    private class MyControlState
    {
      public int? PartnerIndex = null;
      public int? ShippingIdx = null;
    }
    private MyControlState m_ControlState = new MyControlState();
    protected override void OnInit(EventArgs e)
    {
      Page.RegisterRequiresControlState(this);
      base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      m_AddDriverButton.Click += new EventHandler(m_AddDriverButton_Click);
      m_RemoveDriverButton.Click += new EventHandler(m_RemoveDriverButton_Click);
      m_TrailerDropDown.SelectedIndexChanged += new EventHandler(m_TrailerDropDown_SelectedIndexChanged);
      m_TruckDropDown.SelectedIndexChanged += new EventHandler(m_TruckDropDown_SelectedIndexChanged);
    }
    protected override void LoadControlState(object _state)
    {
      if (_state != null)
        m_ControlState = (MyControlState)_state;
    }
    protected override object SaveControlState()
    {
      return m_ControlState;
    }
    #endregion

    #region Connectivity
    private void NewDataEventHandler(object sender, ShippingInterconnectionData e)
    {
      int? _ci = e.GetIndex();
      if (_ci == m_ControlState.ShippingIdx)
        return;
      m_ControlState.ShippingIdx = _ci;
      UpdateUserInterface();
    }
    private void NewDataEventHandler(object sender, PartnerInterconnectionData e)
    {
      int? _ci = e.GetIndex();
      if (_ci == m_ControlState.PartnerIndex)
        return;
      m_ControlState.ShippingIdx = null;
      m_ControlState.PartnerIndex = _ci;
      UpdateUserInterface();
    }
    #endregion

    #region User Interface Actions
    /// <summary>
    /// Clears the user interface.
    /// </summary>
    private void UpdateUserInterface()
    {
      ClearUserInterface();
      if (!m_ControlState.PartnerIndex.HasValue || !m_ControlState.ShippingIdx.HasValue)
        return;
      try
      {
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
        {
          Dictionary<int, Driver> _drivers = Driver.GetAllForUser(edc, m_ControlState.PartnerIndex.Value).
            ToDictionary(p => p.Identyfikator.Value);
          foreach (ShippingDriversTeam item in from idx in edc.DriversTeam
                                               where idx.ShippingIndex.Identyfikator == m_ControlState.ShippingIdx.Value
                                               select idx)
          {
            Driver _driver = item.Driver;
            m_DriversTeamListBox.Items.Add(new ListItem(_driver.Tytuł, item.Identyfikator.Value.ToString()));
            _drivers.Remove(_driver.Identyfikator.Value);
          }
          foreach (var item in _drivers)
            m_DriversListBox.Items.Add(new ListItem(item.Value.Tytuł, item.Key.ToString()));
          ShippingOperationInbound _Shipping = ShippingOperationInbound.GetAtIndex(edc, m_ControlState.ShippingIdx);
          m_ShippingTextBox.Text = _Shipping.Tytuł;
          m_TruckDropDown.Items.Add(new ListItem());
          foreach (Truck item in Truck.GetAllForUser(edc, m_ControlState.PartnerIndex.Value))
          {
            ListItem _li = new ListItem(item.Tytuł, item.Identyfikator.Value.ToString());
            m_TruckDropDown.Items.Add(_li);
            if (_Shipping.TruckCarRegistrationNumber == item)
              _li.Selected = true;
          }
          m_TrailerDropDown.Items.Add(new ListItem());
          foreach (Trailer item in Trailer.GetAllForUser(edc, m_ControlState.PartnerIndex.Value))
          {
            ListItem _li = new ListItem(item.Tytuł, item.Identyfikator.Value.ToString());
            m_TrailerDropDown.Items.Add(_li);
            if (_Shipping.TrailerRegistrationNumber == item)
              _li.Selected = true;
          }
        }
        SetButtons(true);
      }
      catch (Exception ex)
      {
        string _frmt = "User interface update error for user: {0}/shipping{1} with message: {2}";
        this.Controls.Add(new LiteralControl(String.Format(_frmt, m_ControlState.PartnerIndex, m_ControlState.ShippingIdx, ex.Message)));
      }
    }
    private void ClearUserInterface()
    {
      m_ShippingTextBox.Text = String.Empty;
      m_DriversListBox.Items.Clear();
      m_DriversTeamListBox.Items.Clear();
      m_TrailerDropDown.Items.Clear();
      m_TruckDropDown.Items.Clear();
      SetButtons(false);
    }
    private void SetButtons(bool _enabled)
    {
      m_AddDriverButton.Enabled = _enabled && m_DriversListBox.Items.Count > 0;
      m_RemoveDriverButton.Enabled = _enabled && m_DriversTeamListBox.Items.Count > 0;
      m_TrailerDropDown.Enabled = _enabled;
      m_TruckDropDown.Enabled = _enabled;
      m_DriversListBox.Enabled = _enabled;
      m_DriversTeamListBox.Enabled = _enabled;
    }
    #endregion

    #region Evnt Handlers
    private void m_RemoveDriverButton_Click(object sender, EventArgs e)
    {
      try
      {
        ListItem _sel = m_DriversTeamListBox.SelectedItem;
        if (_sel == null)
          return;
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
        {
          ShippingDriversTeam _cd = ShippingDriversTeam.GetAtIndex(edc, _sel.Value);
          edc.DriversTeam.DeleteOnSubmit(_cd);
          edc.SubmitChanges();
        }
        UpdateUserInterface();
      }
      catch (Exception ex)
      {
        SignalException("Remove driver error: {0}", ex);
      }
    }
    private void m_AddDriverButton_Click(object sender, EventArgs e)
    {
      try
      {
        ListItem _sel = m_DriversListBox.SelectedItem;
        if (_sel == null)
          return;
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
        {
          ShippingDriversTeam _cd = new ShippingDriversTeam()
            {
              Driver = Driver.GetAtIndex(edc, _sel.Value),
              ShippingIndex = ShippingOperationInbound.GetAtIndex(edc, m_ControlState.ShippingIdx)
            };
          edc.DriversTeam.InsertOnSubmit(_cd);
          edc.SubmitChanges();
        }
        UpdateUserInterface();
      }
      catch (Exception ex)
      {
        SignalException("Add driver error: {0}", ex);
      }
    }
    private void m_TruckDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListItem _li = m_TruckDropDown.SelectedItem;
      if (_li == null)
        return;
      using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
      {
        try
        {
          ShippingOperationInbound _sh = ShippingOperationInbound.GetAtIndex(edc, m_ControlState.ShippingIdx.Value);
          if (String.IsNullOrEmpty(_li.Value))
            _sh.TruckCarRegistrationNumber = null;
          else
            _sh.TruckCarRegistrationNumber = Truck.GetAtIndex(edc, _li.Value);
          edc.SubmitChanges();
        }
        catch (Exception _ex)
        {
          SignalException("Truck selection error: {0}", _ex);
        }
      }
    }
    private void m_TrailerDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      ListItem _li = m_TrailerDropDown.SelectedItem;
      if (_li == null)
        return;
      using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
      {
        try
        {
          ShippingOperationInbound _sh = ShippingOperationInbound.GetAtIndex(edc, m_ControlState.ShippingIdx.Value);
          if (String.IsNullOrEmpty(_li.Value))
            _sh.TrailerRegistrationNumber = null;
          else
            _sh.TrailerRegistrationNumber = Trailer.GetAtIndex(edc, _li.Value);
          edc.SubmitChanges();
        }
        catch (Exception _ex)
        {
          SignalException("Truck selection error: {0}", _ex);
        }
      }
    }
    private ListBox SortTextBox(ListBox _listBox)
    {
      var _ldSorted = from ListItem _rng in _listBox.Items orderby _rng.Value ascending select _rng;
      ListBox _ret = new ListBox();
      foreach (var item in _ldSorted)
        _ret.Items.Add(new ListItem(item.Text, item.Value));
      return _ret;
    }
    private void SignalException(string _format, Exception _ex)
    {
      this.Controls.Add(new LiteralControl(String.Format(_format, _ex.Message)));
    }
    #endregion

    #endregion
  }
}
