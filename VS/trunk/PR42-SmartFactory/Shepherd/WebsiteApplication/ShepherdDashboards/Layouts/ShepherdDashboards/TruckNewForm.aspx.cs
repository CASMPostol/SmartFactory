using System;
using System.Web;
using System.Web.UI.WebControls;
using CAS.SmartFactory.Shepherd.Entities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;

namespace CAS.SmartFactory.Shepherd.Dashboards.Layouts.ShepherdDashboards
{
  public partial class TruckNewForm : LayoutsPageBase
  {
    #region Page override
    protected void Page_Load(object sender, EventArgs e)
    {
      try
      {
        if (m_PartnerTitle.Items.Count == 0)
        {
          m_VehicleType.Items.Add(new ListItem(VehicleType.SecurityEscortCar.ToString(), ((int)VehicleType.SecurityEscortCar).ToString()));
          m_VehicleType.Items.Add(new ListItem(VehicleType.Truck.ToString(), ((int)VehicleType.Truck).ToString()));
          m_VehicleType.Items.Add(new ListItem(VehicleType.Van.ToString(), ((int)VehicleType.Van).ToString()));
          Truck _drv = null;
          m_ItemID.Value = Request.Params["ID"];
          if (!m_ItemID.Value.IsNullOrEmpty())
          {
            _drv = Element.GetAtIndex<Truck>(EDC.Truck, m_ItemID.Value);
            m_Comments.Text = _drv.AdditionalComments;
            m_TruckTitle.Text = _drv.Tytuł;
            m_VehicleType.Select((int)_drv.VehicleType.GetValueOrDefault(0));
          }
          Partner _Partner = Partner.FindForUser(EDC, SPContext.Current.Web.CurrentUser);
          if (_Partner == null)
            m_PartnerTitle.AddPartner(true, _drv == null ? null : _drv.Truck2PartnerTitle, EDC);
          else
            m_PartnerTitle.AddPartner(false, _Partner, EDC);
        }
        this.m_CancelButton.Click += new EventHandler(m_CancelButton_Click);
        this.m_CancelButton.UseSubmitBehavior = false;
        this.m_SaveButton.Click += new EventHandler(m_SaveButton_Click);
        this.m_SaveButton.UseSubmitBehavior = false;
      }
      catch (Exception ex)
      {
        ReportException("Page_Load", ex);
        throw;
      }
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
        Truck _nd = null;
        if (m_ItemID.Value.IsNullOrEmpty())
        {
          _nd = new Truck();
          EDC.Truck.InsertOnSubmit(_nd);
        }
        else
          _nd = Element.GetAtIndex<Truck>(EDC.Truck, m_ItemID.Value);
        _nd.AdditionalComments = this.m_Comments.Text;
        _nd.Tytuł = this.m_TruckTitle.Text;
        _nd.VehicleType = (VehicleType)m_VehicleType.SelectedValue.String2Int().Value;
        _nd.Truck2PartnerTitle = _prtn;
        EDC.SubmitChanges();
        SPUtility.Redirect(Request.Params["Source"], SPRedirectFlags.Default, HttpContext.Current);
      }
      catch (Exception ex)
      {
        ReportException("m_SaveButton_Click", ex);
      }
    }
    private void m_CancelButton_Click(object sender, EventArgs e)
    {
      SPUtility.Redirect(Request.Params["Source"], SPRedirectFlags.Default, HttpContext.Current);
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
      Anons _entry = new Anons(_source, String.Format(_tmplt, ex.Message));
      EDC.EventLogList.InsertOnSubmit(_entry);
      EDC.SubmitChanges();
    }
    #endregion
  }
}
