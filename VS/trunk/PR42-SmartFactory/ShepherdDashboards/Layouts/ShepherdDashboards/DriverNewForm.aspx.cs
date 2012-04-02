﻿using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Linq;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using System.Web.UI.WebControls;
using System.Web.UI;
using Microsoft.SharePoint.Utilities;
using System.Web;

namespace CAS.SmartFactory.Shepherd.Dashboards.Layouts.ShepherdDashboards
{
  public partial class DriverNewForm : LayoutsPageBase
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsCallback)
      {
        Partner _Partner = Partner.FindForUser(EDC, SPContext.Current.Web.CurrentUser);
        if (_Partner == null)
        {
          m_PartnerTitle.DataSource = from Partner _prtnrs in EDC.Partner
                                      select new { Name = _prtnrs.Tytuł, ID = _prtnrs.Identyfikator };
          m_PartnerTitle.DataValueField = "ID";
          m_PartnerTitle.DataTextField = "Name";
          m_PartnerTitle.DataBind();
        }
        else
          m_PartnerTitle.Items.Add(new ListItem(_Partner.Tytuł, _Partner.Identyfikator.Value.ToString()));
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
    private void m_SaveButton_Click(object sender, EventArgs e)
    {
      try
      {
        Driver _nd = new Entities.Driver()
         {
           IdentityDocumentNumber = this.m_DriverIDNumber.Text,
           NumerTelefonuKomórkowego = this.m_DriverMobileNo.Text,
           Tytuł = m_DriverTitle.Text,
           VendorName = Element.GetAtIndex<Partner>(EDC.Partner, m_PartnerTitle.SelectedValue)
         };
        EDC.Driver.InsertOnSubmit(_nd);
        EDC.SubmitChanges();
        SPUtility.Redirect("WebPartPages/ManageDriversDashboard.aspx", SPRedirectFlags.UseSource, HttpContext.Current); 
      }
      catch (Exception ex)
      {
        string _frmt = "Cannot save the provided data because : {0}";
        Controls.Add(new LiteralControl(String.Format(_frmt, ex.Message)));
      }
    }
    private void m_CancelButton_Click(object sender, EventArgs e)
    {
      SPUtility.Redirect("WebPartPages/ManageDriversDashboard.aspx", SPRedirectFlags.UseSource, HttpContext.Current);
    }
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

    #region Reports
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
