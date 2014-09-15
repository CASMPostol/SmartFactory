//<summary>
//  Title   : Guard Web Part User Control
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2014, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.DataModel.Entities;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.Shepherd.Dashboards.GuardWebPart
{
  /// <summary>
  /// Guard Web Part User Control
  /// </summary>
  public partial class GuardWebPartUserControl : UserControl
  {
    #region public
    internal void SetInterconnectionData(Dictionary<InterconnectionData.ConnectionSelector, IWebPartRow> _ProvidesDictionary)
    {
      foreach (var item in _ProvidesDictionary)
        try
        {
          switch (item.Key)
          {
            case InterconnectionData.ConnectionSelector.ShippingInterconnection:
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
      m_ShippingLabel.Text = _currentShipping.Title;
    }
    private void ClearUserInterface()
    {
      m_ShippingLabel.Text = String.Empty;
    }
    private void ReportException(string _source, Exception _ex)
    {
      string _tmplt = "ReportExceptionTemplate".GetShepherdLocalizedString();
      Anons _entry = Anons.CreateAnons(_source, String.Format(_tmplt, _ex.Message));
      EDC.EventLogList.InsertOnSubmit(_entry);
      EDC.SubmitChanges();
    }
    private void ReportAlert(Shipping _shipping, string _msg)
    {
      AlarmsAndEvents _ae = new AlarmsAndEvents()
      {
        AlarmsAndEventsList2Shipping = _shipping,
        AlarmsAndEventsList2PartnerTitle = _shipping.PartnerTitle,
        Title = _msg,
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
        switch (CurrentShipping.ShippingState.Value)
        {
          case ShippingState.Underway:
            TimeSlot _ts = CurrentShipping.OccupiedTimeSlots(EDC).FirstOrDefault();
            if (_ts == null)
              break;
            CurrentShipping.StartTime = _ts.StartTime;
            CurrentShipping.ShippingState = ShippingState.Confirmed;
            CurrentShipping.TruckAwaiting = false;
            CurrentShipping.CalculateState(EDC, x => { });
            EDC.SubmitChanges();
            break;
          case ShippingState.Creation:
          case ShippingState.Confirmed:
          case ShippingState.Delayed:
          case ShippingState.WaitingForCarrierData:
          case ShippingState.WaitingForConfirmation:
            if (CurrentShipping.TruckAwaiting.GetValueOrDefault(true))
            CurrentShipping.TruckAwaiting = false;
            CurrentShipping.CalculateState(EDC, x => { });
            EDC.SubmitChanges();
            break;          
          case ShippingState.None:
          case ShippingState.Invalid:
          case ShippingState.Canceled:
          case ShippingState.Cancelation:
          case ShippingState.Completed:
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
        switch (CurrentShipping.ShippingState.Value)
        {
          case ShippingState.Confirmed:
          case ShippingState.Creation:
          case ShippingState.Delayed:
          case ShippingState.WaitingForCarrierData:
          case ShippingState.WaitingForConfirmation:
            if (CurrentShipping.TruckAwaiting.GetValueOrDefault(false))
              return;
            CurrentShipping.TruckAwaiting = true;
            CurrentShipping.ArrivalTime = DateTime.Now;
            EDC.SubmitChanges();
            break;
          case ShippingState.Underway:
          case ShippingState.None:
          case ShippingState.Invalid:
          case ShippingState.Canceled:
          case ShippingState.Completed:
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
        switch (CurrentShipping.ShippingState.Value)
        {
          case ShippingState.Underway:
            CurrentShipping.EndTime = DateTime.Now;
            CurrentShipping.ShippingState = ShippingState.Completed;
            CurrentShipping.ShippingDuration = (CurrentShipping.EndTime.Value - CurrentShipping.StartTime.Value).TotalMinutes;
            EDC.SubmitChanges();
            break;
          case ShippingState.Confirmed:
          case ShippingState.Creation:
          case ShippingState.Delayed:
          case ShippingState.WaitingForCarrierData:
          case ShippingState.WaitingForConfirmation:
          case ShippingState.None:
          case ShippingState.Invalid:
          case ShippingState.Canceled:
          case ShippingState.Completed:
          case ShippingState.Cancelation:
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
        switch (CurrentShipping.ShippingState.Value)
        {
          case ShippingState.Confirmed:
          case ShippingState.Creation:
          case ShippingState.Delayed:
          case ShippingState.WaitingForCarrierData:
          case ShippingState.WaitingForConfirmation:
            if (!CurrentShipping.TruckAwaiting.GetValueOrDefault(false))
              CurrentShipping.ArrivalTime = DateTime.Now;
            CurrentShipping.StartTime = DateTime.Now;
            CurrentShipping.WarehouseStartTime = DateTime.Now;
            CurrentShipping.ShippingState = ShippingState.Underway;
            EDC.SubmitChanges();
            break;
          case ShippingState.Underway:
          case ShippingState.None:
          case ShippingState.Invalid:
          case ShippingState.Canceled:
          case ShippingState.Completed:
          case ShippingState.Cancelation:
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
