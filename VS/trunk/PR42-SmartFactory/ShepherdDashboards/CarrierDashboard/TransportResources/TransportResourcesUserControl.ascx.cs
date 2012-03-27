﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TransportResources
{
  public partial class TransportResourcesUserControl : UserControl
  {
    #region public
    internal void GetData(Dictionary<InboundInterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      foreach (var item in _ProvidesDictionary)
        switch (item.Key)
        {
          case InboundInterconnectionData.ConnectionSelector.ShippingInterconnection:
            new ShippingInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
            break;
          default:
            break;
        }
    }
    internal TransportResources.RolesSet Role { get; set; }
    #endregion

    #region private

    #region state management
    [Serializable]
    private class MyControlState
    {
      public string ShippingIdx = String.Empty;
    }
    private MyControlState m_ControlState = new MyControlState();
    protected override void OnInit(EventArgs e)
    {
      Page.RegisterRequiresControlState(this);
      base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
      if (Role == TransportResources.RolesSet.SecurityEscort)
      {
        m_TrailerDropDown.Visible = false;
        m_TrailerHeaderLabel.Visible = false;
        m_TruckHeaderLabel.Text = "Car";
      }
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
      if (e.ID == m_ControlState.ShippingIdx)
        return;
      m_ControlState.ShippingIdx = e.ID;
      try
      {
        using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
          UpdateUserInterface(edc);
      }
      catch (Exception ex)
      {
        string _frmt = "User interface update error for shipping {0} with message: {1}";
        this.Controls.Add(new LiteralControl(String.Format(_frmt, m_ControlState.ShippingIdx, ex.Message)));
      }
    }
    #endregion

    #region User Interface Actions
    /// <summary>
    /// Clears the user interface.
    /// </summary>
    private void UpdateUserInterface(EntitiesDataContext edc)
    {
      ClearUserInterface();
      if (m_ControlState.ShippingIdx.IsNullOrEmpty())
        return;
      Shipping _Shipping = Element.GetAtIndex<Shipping>(edc.Shipping, m_ControlState.ShippingIdx);
      Partner _prtn = Role == TransportResources.RolesSet.Carrier ? _Shipping.VendorName : _Shipping.SecurityEscortProvider;
      if (_prtn == null)
        return; ;
      Dictionary<int, Driver> _drivers = _prtn.Driver.ToDictionary(x => x.Identyfikator.Value);
      foreach (ShippingDriversTeam item in from idx in _Shipping.ShippingDriversTeam select idx)
      {
        Driver _driver = item.Driver;
        if ((Role == TransportResources.RolesSet.SecurityEscort && _driver.VendorName.ServiceType.Value != ServiceType.SecurityEscortProvider) ||
           (Role == TransportResources.RolesSet.Carrier && _driver.VendorName.ServiceType.Value == ServiceType.SecurityEscortProvider))
          continue;
        m_DriversTeamListBox.Items.Add(new ListItem(_driver.Tytuł, item.Identyfikator.Value.ToString()));
        _drivers.Remove(_driver.Identyfikator.Value);
      }
      foreach (var item in _drivers)
        m_DriversListBox.Items.Add(new ListItem(item.Value.Tytuł, item.Key.ToString()));
      m_ShippingTextBox.Text = _Shipping.Tytuł;
      m_TruckDropDown.Items.Add(new ListItem());
      foreach (Truck _item in _prtn.Truck)
      {
        if ((Role == TransportResources.RolesSet.SecurityEscort && _item.VendorName.ServiceType.Value != ServiceType.SecurityEscortProvider) ||
          (Role == TransportResources.RolesSet.Carrier && _item.VendorName.ServiceType.Value == ServiceType.SecurityEscortProvider))
          continue;
        ListItem _li = new ListItem(_item.Tytuł, _item.Identyfikator.Value.ToString());
        m_TruckDropDown.Items.Add(_li);
        if (_Shipping.TruckCarRegistrationNumber == _item)
          _li.Selected = true;
      }
      m_TrailerDropDown.Items.Add(new ListItem());
      foreach (Trailer item in _prtn.Trailer)
      {
        ListItem _li = new ListItem(item.Tytuł, item.Identyfikator.Value.ToString());
        m_TrailerDropDown.Items.Add(_li);
        if (_Shipping.TrailerRegistrationNumber == item)
          _li.Selected = true;
      }
      SetButtons(true);
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
          ShippingDriversTeam _cd = Element.GetAtIndex<ShippingDriversTeam>(edc.DriversTeam, _sel.Value);
          Shipping _sh = _cd.ShippingIndex;
          _cd.Driver = null;
          _cd.ShippingIndex = null;
          edc.DriversTeam.DeleteOnSubmit(_cd);
          edc.SubmitChanges();
          _sh.CalculateState();
          edc.SubmitChanges();
          UpdateUserInterface(edc);
        }
      }
      catch (Exception ex)
      {
        SignalException("m_RemoveDriverButton_Click", "Remove driver error: {0}", ex);
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
              Driver = Element.GetAtIndex<Driver>(edc.Driver, _sel.Value),
              ShippingIndex = Element.GetAtIndex(edc.Shipping, m_ControlState.ShippingIdx)
            };
          edc.DriversTeam.InsertOnSubmit(_cd);
          edc.SubmitChanges();
          _cd.ShippingIndex.CalculateState();
          edc.SubmitChanges();
          UpdateUserInterface(edc);
        }
      }
      catch (Exception ex)
      {
        SignalException("m_AddDriverButton_Click", "Add driver error: {0}", ex);
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
          Shipping _sh = Element.GetAtIndex<Shipping>(edc.Shipping, m_ControlState.ShippingIdx);
          if (String.IsNullOrEmpty(_li.Value))
            if (Role == TransportResources.RolesSet.Carrier)
              _sh.TruckCarRegistrationNumber = null;
            else
              _sh.SecurityEscortCarRegistrationNumber = null;
          else
          {
            Truck _ct = Element.GetAtIndex<Truck>(edc.Truck, _li.Value);
            if (Role == TransportResources.RolesSet.Carrier)
              _sh.TruckCarRegistrationNumber = _ct;
            else
              _sh.SecurityEscortCarRegistrationNumber = _ct;
          }
          _sh.CalculateState();
          edc.SubmitChanges();
        }
        catch (Exception _ex)
        {
          SignalException("m_TruckDropDown_SelectedIndexChanged", "Truck selection error: {0}", _ex);
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
          Shipping _sh = Element.GetAtIndex<Shipping>(edc.Shipping, m_ControlState.ShippingIdx);
          if (String.IsNullOrEmpty(_li.Value))
            _sh.TrailerRegistrationNumber = null;
          else
            _sh.TrailerRegistrationNumber = Element.GetAtIndex<Trailer>(edc.Trailer, _li.Value);
          _sh.CalculateState();
          edc.SubmitChanges();
        }
        catch (Exception _ex)
        {
          SignalException("m_TrailerDropDown_SelectedIndexChanged", "Truck selection error: {0}", _ex);
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
    private void SignalException(string _source, string _format, Exception _ex)
    {
      string _msg = String.Format(_format, _ex.Message);
      this.Controls.Add(new LiteralControl(_msg ));
      using (EntitiesDataContext edc = new EntitiesDataContext(SPContext.Current.Web.Url))
      {
        Anons.WriteEntry(edc, _source, _msg);
      }
    }
    #endregion

    #endregion
  }
}
