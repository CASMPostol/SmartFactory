﻿using System;
using System.Web;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace CAS.SmartFactory.Shepherd.Dashboards.Layouts.ShepherdDashboards
{
  public partial class DriverNewForm : LayoutsPageBase
  {
    #region Page override
    protected void Page_Load(object sender, EventArgs e)
    {
      if (m_PartnerTitle.Items.Count == 0)
      {
        Driver _drv = null;
        if (!Request.Params["ID"].IsNullOrEmpty())
        {
          _drv = Element.GetAtIndex<Driver>(EDC.Driver, Request.Params["ID"]);
          m_DriverIDNumber.Text = _drv.IdentityDocumentNumber;
          m_DriverMobileNo.Text = _drv.NumerTelefonuKomórkowego;
          m_DriverTitle.Text = _drv.Tytuł;
        }
        Partner _Partner = Partner.FindForUser(EDC, SPContext.Current.Web.CurrentUser);
        if (_Partner == null)
          m_PartnerTitle.AddPartner(true, _drv == null ? null : _drv.VendorName, EDC);
        else
          m_PartnerTitle.AddPartner(false, _Partner, EDC);
      }
      this.m_CancelButton.Click += new EventHandler(m_CancelButton_Click);
      this.m_SaveButton.Click += new EventHandler(m_SaveButton_Click);
    }
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

    #region Event handlers
    private void m_SaveButton_Click(object sender, EventArgs e)
    {
      try
      {
        Partner _prtn = Element.GetAtIndex<Partner>(EDC.Partner, m_PartnerTitle.SelectedValue);
        Driver _nd = new Entities.Driver()
         {
           IdentityDocumentNumber = this.m_DriverIDNumber.Text,
           NumerTelefonuKomórkowego = this.m_DriverMobileNo.Text,
           Tytuł = m_DriverTitle.Text,  
           VendorName = _prtn
         };
        EDC.Driver.InsertOnSubmit(_nd);
        EDC.SubmitChanges();
        SPUtility.Redirect("../WebPartPages/ManageDriversDashboard", SPRedirectFlags.UseSource, HttpContext.Current);
      }
      catch (Exception ex)
      {
        ReportException("m_SaveButton_Click", ex);
      }
    }
    private void m_CancelButton_Click(object sender, EventArgs e)
    {
      SPUtility.Redirect(this.ResolveClientUrl("../WebPartPages/ManageDriversDashboard"), SPRedirectFlags.RelativeToLayoutsPage, HttpContext.Current);
    }
    #endregion

    #region private
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
    private void ReportException(string _source, Exception ex)
    {
      string _tmplt = "The current operation has been interrupted by error {0}.";
      Entities.Anons _entry = new Anons(_source, String.Format(_tmplt, ex.Message));
      EDC.EventLogList.InsertOnSubmit(_entry);
      EDC.SubmitChanges();
    }
    #endregion
  }
}
