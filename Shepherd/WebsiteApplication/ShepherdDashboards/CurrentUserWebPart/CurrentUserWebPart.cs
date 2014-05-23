//<summary>
//  Title   : class CurrentUserWebPart
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
using System.ComponentModel;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebPartPages;

namespace CAS.SmartFactory.Shepherd.Dashboards.CurrentUserWebPart
{
  /// <summary>
  /// CurrentUserWebPart <see cref="WebPart"/>
  /// </summary>
  [ToolboxItemAttribute(false)]
  public class CurrentUserWebPart : WebPart
  {

    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/CurrentUserWebPart/CurrentUserWebPartUserControl.ascx";
    /// <summary>
    /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
    /// </summary>
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
    /// <summary>
    /// The event handler for the System.Web.UI.Control.PreRender event that occurs immediately before the Web Part is rendered to the Web Part Page it is contained on.
    /// </summary>
    /// <param name="e">A System.EventArgs that contains the event data.</param>
    protected override void OnPreRender(EventArgs e)
    {
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
      return new UserDescriptor(SPContext.Current.Web.CurrentUser);
    }
    #endregion

    #region public
    /// <summary>
    /// Gets the connection interface.
    /// </summary>
    /// <returns></returns>
    [System.Web.UI.WebControls.WebParts.ConnectionProvider("Current User", "CurrentUserProviderPoint", AllowsMultipleConnections = true)]
    public System.Web.UI.WebControls.WebParts.IWebPartRow GetConnectionInterface()
    {
      return GetUserDescriptor();
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="CurrentUserWebPart"/> class.
    /// </summary>
    public CurrentUserWebPart()
      : base()
    {
      //m_UserDescriptor = GetUserDescriptor();
    }
    internal static string CurrentUserProviderPoint = "CurrentUserProviderPoint";
    #endregion
  }
}
