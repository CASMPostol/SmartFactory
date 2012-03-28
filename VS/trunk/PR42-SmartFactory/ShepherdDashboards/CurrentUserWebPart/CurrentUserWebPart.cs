﻿using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using Microsoft.SharePoint.WebPartPages;

namespace CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart
{
  [ToolboxItemAttribute(false)]
  public class CurrentUserWebPart : WebPart
  {

    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/CurrentUserWebPart/CurrentUserWebPartUserControl.ascx";
    protected override void CreateChildControls()
    {
      string _phase = "Starting";
      try
      {
        Control _ctrl = Page.LoadControl(_ascxPath);
        _phase = "After Page.Load";
        m_Control = (CurrentUserWebPartUserControl)_ctrl;
        _phase = "After cast";
        _phase = "After DisplayUserName";
        Controls.Add(m_Control);
        _phase = "Finishing";
      }
      catch (Exception ex)
      {
        string _frmt = "Cannot lod the user control at: {0} because : {1}";
        Controls.Add(new LiteralControl(String.Format(_frmt, _phase, ex.Message)));
      }
    }
    protected override void OnPreRender(EventArgs e)
    {
      //if (m_UserDescriptor == null)
      try
      {
        m_Control.DisplayUserName(GetUserDescriptor());
      }
      catch (Exception ex)
      {
        string _frmt = "Cannot execute OnPreRender because: {0}";
        Controls.Add(new LiteralControl(String.Format(_frmt, ex.Message)));
      } 
      base.OnLoad(e);
    }
    private CurrentUserWebPartUserControl m_Control;
    private UserDescriptor GetUserDescriptor()
    {
      if (this.Context == null)
        return null;
      using (SPWeb currentWeb = SPControl.GetContextWeb(this.Context))
        return new UserDescriptor(currentWeb.CurrentUser);
    }
    #endregion

    #region public
    [System.Web.UI.WebControls.WebParts.ConnectionProvider("Current User", "CurrentUserProviderPoint", AllowsMultipleConnections = true)]
    public System.Web.UI.WebControls.WebParts.IWebPartRow GetConnectionInterface()
    {
      return GetUserDescriptor();
    }
    public CurrentUserWebPart()
      : base()
    {
      //m_UserDescriptor = GetUserDescriptor();
    }
    internal static string CurrentUserProviderPoint = "CurrentUserProviderPoint";
    #endregion
  }
}
