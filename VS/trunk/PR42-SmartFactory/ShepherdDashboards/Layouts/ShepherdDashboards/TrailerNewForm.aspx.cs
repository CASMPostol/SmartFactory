using System;
using System.Web;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace CAS.SmartFactory.Shepherd.Dashboards.Layouts.ShepherdDashboards
{
  public partial class TrailerNewForm : LayoutsPageBase
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (m_PartnerTitle.Items.Count == 0)
      {
        Trailer _trl = null;
        if (!Request.Params["ID"].IsNullOrEmpty())
        {
          _trl = Element.GetAtIndex<Trailer>(EDC.Trailer, Request.Params["ID"]);
          m_Comments.Text = _trl.Comments;
          m_TruckTitle.Text = _trl.Tytuł;
        }
        Partner _Partner = Partner.FindForUser(EDC, SPContext.Current.Web.CurrentUser);
        if (_Partner == null)
          m_PartnerTitle.AddPartner(true, _trl == null ? null : _trl.VendorName, EDC);
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
    #region Event handlers
    private void m_SaveButton_Click(object sender, EventArgs e)
    {
      try
      {
        Partner _prtn = Element.GetAtIndex<Partner>(EDC.Partner, m_PartnerTitle.SelectedValue);
        Trailer _nd = new Entities.Trailer()
        {
           Comments = this.m_Comments.Text,
           Tytuł = this.m_TruckTitle.Text,
          VendorName = _prtn
        };
        EDC.Trailer.InsertOnSubmit(_nd);
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
