﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.GuardWebPart
{
  public partial class GuardWebPartUserControl : UserControl
  {
    #region public
    internal void SetInterconnectionData(Dictionary<CarrierDashboard.InboundInterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      foreach (var item in _ProvidesDictionary)
        try
        {
          switch (item.Key)
          {
            case InboundInterconnectionData.ConnectionSelector.ShippingInterconnection:
              new ShippingInterconnectionData().SetRowData(_ProvidesDictionary[item.Key], NewDataEventHandler);
              break;
            default:
              throw new ApplicationException("SetInterconnectionData - SetInterconnectionData");
          }
        }
        catch (Exception ex)
        {
          ReportException("SetInterconnectionData at: " + item.Key.ToString(), ex);
        }
    }
    #endregion

    #region UserControl override
    [Serializable]
    private class ControlState
    {
      #region state fields
      public string ShippingID = String.Empty;
      #endregion

      #region public
      public ControlState(ControlState _old)
      {
        if (_old == null)
          return;
      }
      #endregion
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnInit(EventArgs e)
    {
      Page.RegisterRequiresControlState(this);
      base.OnInit(e);
    }
    /// <summary>
    /// Handles the Load event of the Page control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    protected void Page_Load(object sender, EventArgs e)
    {
      m_EnteredButton.Click += new EventHandler(m_EnteredButton_Click);
      m_LeftButton.Click += new EventHandler(m_LeftButton_Click);
      m_ArrivedButton.Click += new EventHandler(m_ArrivedButton_Click);
      m_UnDoButton.Click += new EventHandler(m_UnDoButton_Click);
    }
    /// <summary>
    /// Loads the state of the control.
    /// </summary>
    /// <param name="state">The state.</param>
    protected override void LoadControlState(object state)
    {
      if (state != null)
        m_ControlState = (ControlState)state;
    }
    /// <summary>
    /// Saves any server control state changes that have occurred since the time the page was posted back to the server.
    /// </summary>
    /// <returns>
    /// Returns the server control's current state. If there is no state associated with the control, this method returns null.
    /// </returns>
    protected override object SaveControlState()
    {
      return m_ControlState;
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
      base.OnPreRender(e);
    }
    /// <summary>
    /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
    /// </summary>
    /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
    protected override void OnUnload(EventArgs e)
    {
      if (_EDC != null)
      {
        try
        {
          _EDC.SubmitChanges();
          _EDC.Dispose();
          _EDC = null;
        }
        catch (Exception ex)
        {
          ReportException("OnUnload", ex);
        }
      }
      base.OnUnload(e);
    }

    #endregion

    #region Interconnection
    internal void NewDataEventHandler(object sender, ShippingInterconnectionData _interconnectionData)
    {
      if (m_ControlState.ShippingID == _interconnectionData.ID)
        return;
      ClearUserInterface();
      m_ControlState.ShippingID = _interconnectionData.ID;
      ShowShipping(CurrentShipping);
    }
    #endregion

    #region private
    private void ShowShipping(Shipping _currentShipping)
    {
      m_ShippingLabel.Text = _currentShipping.Tytuł;
    }
    private void ClearUserInterface()
    {
      m_ShippingLabel.Text = String.Empty;
    }
    private void ReportException(string _source, Exception _ex)
    {
      string _tmplt = "The current operation has been interrupted by error {0}.";
      Entities.Anons _entry = new Anons(_source, String.Format(_tmplt, _ex.Message));
      EDC.EventLogList.InsertOnSubmit(_entry);
      EDC.SubmitChanges();
    }
    private void ReportAlert(Shipping _shipping, string _msg)
    {
      Entities.AlarmsAndEvents _ae = new Entities.AlarmsAndEvents()
      {
        ShippingIndex = _shipping,
        VendorName = _shipping.VendorName,
        Tytuł = _msg,
      };
      EDC.AlarmsAndEvents.InsertOnSubmit(_ae);
      EDC.SubmitChanges();
    }

    #region event handlers
    private void m_UnDoButton_Click(object sender, EventArgs e)
    {
      try
      {
        if (CurrentShipping == null)
          return;
        switch (CurrentShipping.State.Value)
        {
          case State.Underway:
            TimeSlot _ts = (from _tsidx in CurrentShipping.TimeSlot
                            where _tsidx.Occupied.Value == Occupied.Occupied0
                            orderby _tsidx.StartTime ascending
                            select _tsidx).FirstOrDefault();
            if (_ts == null)
              break;
            CurrentShipping.StartTime = _ts.StartTime;
            CurrentShipping.EndTime = _ts.EndTime;
            CurrentShipping.Duration = _ts.Duration();
            CurrentShipping.CalculateState();
            CurrentShipping.Awaiting = false;
            EDC.SubmitChanges();
            break;
          case State.Confirmed:
          case State.Creation:
          case State.Delayed:
          case State.WaitingForCarrierData:
          case State.WaitingForSecurityData:
          case State.None:
          case State.Invalid:
          case State.Canceled:
          case State.Cancelation:
          case State.Completed:
          default:
            break;
        }
      }
      catch (Exception ex)
      {
        ReportException("UnDoButton_Click", ex);
      }
    }
    private void m_ArrivedButton_Click(object sender, EventArgs e)
    {
      try
      {
        if (CurrentShipping == null)
          return;
        switch (CurrentShipping.State.Value)
        {
          case State.Confirmed:
          case State.Creation:
          case State.Delayed:
          case State.WaitingForCarrierData:
          case State.WaitingForSecurityData:
            if (CurrentShipping.Awaiting.GetValueOrDefault(false))
              return;
            CurrentShipping.Awaiting = true;
            EDC.SubmitChanges();
            break;
          case State.Underway:
          case State.None:
          case State.Invalid:
          case State.Canceled:
          case State.Completed:
          default:
            break;
        }
      }
      catch (Exception ex)
      {
        ReportException("m_ArrivedButton_Click", ex);
      }
    }
    private void m_LeftButton_Click(object sender, EventArgs e)
    {
      try
      {
        if (CurrentShipping == null)
          return;
        switch (CurrentShipping.State.Value)
        {
          case State.Underway:
            CurrentShipping.EndTime = DateTime.Now;
            CurrentShipping.State = State.Completed;
            CurrentShipping.Duration = (CurrentShipping.EndTime.Value - CurrentShipping.StartTime.Value).TotalMinutes;
            EDC.SubmitChanges();
            break;
          case State.Confirmed:
          case State.Creation:
          case State.Delayed:
          case State.WaitingForCarrierData:
          case State.WaitingForSecurityData:
          case State.None:
          case State.Invalid:
          case State.Canceled:
          case State.Completed:
          case State.Cancelation:
          default:
            break;
        }
      }
      catch (Exception ex)
      {
        ReportException("LeftButton_Click", ex);
      }
    }
    private void m_EnteredButton_Click(object sender, EventArgs e)
    {
      try
      {
        if (CurrentShipping == null)
          return;
        switch (CurrentShipping.State.Value)
        {
          case State.Confirmed:
          case State.Creation:
          case State.Delayed:
          case State.WaitingForCarrierData:
          case State.WaitingForSecurityData:
            CurrentShipping.StartTime = DateTime.Now;
            CurrentShipping.State = State.Underway;
            CurrentShipping.Awaiting = true;
            EDC.SubmitChanges();
            break;
          case State.Underway:
          case State.None:
          case State.Invalid:
          case State.Canceled:
          case State.Completed:
          case State.Cancelation:
          default:
            break;
        }
      }
      catch (Exception ex)
      {
        ReportException("EnteredButton_Click", ex);
      }
    }
    #endregion

    #region variables
    private ControlState m_ControlState = new ControlState(null);
    private EntitiesDataContext _EDC = null;
    private EntitiesDataContext EDC
    {
      get
      {
        if (_EDC != null)
          return _EDC;
        _EDC = new EntitiesDataContext(SPContext.Current.Web.Url);
        return _EDC;
      }
    }
    private Shipping m_CurrentShipping_Shipping;
    private Shipping CurrentShipping
    {
      get
      {
        if (m_CurrentShipping_Shipping != null)
          return m_CurrentShipping_Shipping;
        if (m_ControlState.ShippingID.IsNullOrEmpty())
        {
          return null;
        }
        m_CurrentShipping_Shipping = Element.GetAtIndex<Shipping>(EDC.Shipping, m_ControlState.ShippingID);
        return m_CurrentShipping_Shipping;
      }
    }
    #endregion

    #endregion
  }
}
