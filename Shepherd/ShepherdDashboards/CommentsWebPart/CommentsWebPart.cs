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
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/CommentsWebPart/CommentsWebPartUserControl.ascx";
    private CommentsWebPartUserControl m_Control;
    protected override void CreateChildControls()
    {
      m_Control = Page.LoadControl( _ascxPath ) as CommentsWebPartUserControl;
      Controls.Add( m_Control );
    }
    protected override void OnPreRender( EventArgs e )
    {
      base.OnPreRender( e );
    }
    #region Interconnections Providers
    /// <summary>
    /// Sets the BatchInterconnection provider.
    /// </summary>
    /// <param name="_provider">The provider interface.</param>
    [ConnectionConsumer( "Shipping list interconnection", "ShippingInterconnectionData", AllowsMultipleConnections = false )]
    public void SetProjectProvider( IWebPartRow _provider )
    {
      m_Control.SetInterconnectionData( _provider );
    }
    #endregion
  }
}
