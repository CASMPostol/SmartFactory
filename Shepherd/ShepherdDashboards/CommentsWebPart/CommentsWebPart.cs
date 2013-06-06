using System;
using System.ComponentModel;
using System.Web.UI;
//<summary>
//  Title   : CommentsWebPart
//  System  : Microsoft Visual C# .NET
//  $LastChangedDate:$
//  $Rev:$
//  $LastChangedBy:$
//  $URL:$
//  $Id:$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.Shepherd.Dashboards.CommentsWebPart
{
  [ToolboxItemAttribute( false )]
  public class CommentsWebPart: WebPart
  {
    #region private
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/CommentsWebPart/CommentsWebPartUserControl.ascx";
    private CommentsWebPartUserControl m_Control;
    IWebPartRow m_Provider = null;
    protected override void CreateChildControls()
    {
      object _Ctrl = Page.LoadControl( _ascxPath );
      m_Control = (CommentsWebPartUserControl)_Ctrl;
      Controls.Add( m_Control );
      m_Control.Role = Role;
      if ( m_Provider != null )
        m_Control.SetInterconnectionData( m_Provider );
    }
    protected override void OnPreRender( EventArgs e )
    {
      base.OnPreRender( e );
    }
    #endregion

    #region creator
    public CommentsWebPart()
    {
      Role = GlobalDefinitions.Roles.None;
    }
    #endregion

    #region Personalization properties
    /// <summary>
    /// Gets or sets the role of the hosting dashboard.
    /// </summary>
    /// <value>
    /// The role.
    /// </value>
    [WebBrowsable( true )]
    [Personalizable( PersonalizationScope.Shared )]
    [WebDisplayName( "The dashboard role" )]
    [WebDescription( "The role of the dashboard this web part is located on. Depending on the role the dashboard customizes" +
      " the functionality provided to the user." )]
    [Microsoft.SharePoint.WebPartPages.SPWebCategoryName( "CAS Custom Properties" )]
    public GlobalDefinitions.Roles Role { get; set; }
    #endregion

    #region Interconnections Providers
    /// <summary>
    /// Sets the BatchInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Shipping list interconnection", "ShippingInterconnectionData", AllowsMultipleConnections = false )]
    public void SetProjectProvider( IWebPartRow _provider )
    {
      if ( m_Control != null )
        m_Control.SetInterconnectionData( _provider );
      else
        m_Provider = _provider;
    }
    #endregion

  }
}
