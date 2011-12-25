using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;

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
      m_Control = Page.LoadControl(_ascxPath) as CurrentUserWebPartUserControl;
      m_Control.DisplayUserName(m_UserDescriptor);
      Controls.Add(m_Control);
    }
    private CurrentUserWebPartUserControl m_Control;

    private UserDescriptor m_UserDescriptor;
    #endregion

    #region public
    [ConnectionProvider("Current User", "CurrentUserProviderPoint", AllowsMultipleConnections = true)]
    public IWebPartRow GetConnectionInterface()
    {
      return m_UserDescriptor;
    }
    public CurrentUserWebPart()
      : base()
    {
      SPWeb currentWeb = SPControl.GetContextWeb(this.Context);
      m_UserDescriptor = new UserDescriptor(currentWeb.CurrentUser);
    }
    #endregion
  }
}
