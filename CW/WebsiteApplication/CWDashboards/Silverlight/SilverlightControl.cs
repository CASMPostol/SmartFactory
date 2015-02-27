//<summary>
//  Title   : Silverlight Hosting WebControl
//  System  : Microsoft Visual C# .NET 2012
//  $LastChangedDate$
//  $Rev$
//  $LastChangedBy$
//  $URL$
//  $Id$
//
//  Copyright (C) 2013, CAS LODZ POLAND.
//  TEL: +48 (42) 686 25 47
//  mailto://techsupp@cas.eu
//  http://www.cas.eu
//</summary>

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Microsoft.SharePoint;
using System.Collections.Generic;
using CAS.SharePoint.Web;

namespace CAS.SmartFactory.CW.Dashboards.Silverlight
{
  /// <summary>
  /// Silverlight Hosting WebControl
  /// </summary>
  [
  DefaultProperty( "Source" ),
  ToolboxData( "<{0}:SilverlightPlugin runat=\"server\" />" )
  ]
  public class SilverlightWebControl: WebControl
  {
    #region public properties
    /// <summary>
    /// Gets or sets the URL of Silverlight .xap file".
    /// </summary>
    /// <value>
    /// The URL of Silverlight .xap file.
    /// </value>
    [Category( "CAS Silverlight" ), Bindable( false ), Localizable( false ), DefaultValue( "" ), Description( "URL of Silverlight .xap file" )]
    public string Source { get; set; }
    /// <summary>
    /// Gets or sets the comma separated list of name=value pairs initialize parameters.
    /// </summary>
    /// <value>
    /// The initialize parameters.
    /// </value>
    [Category( "CAS Silverlight" ), Bindable( false ), Localizable( false ), DefaultValue( "" ), Description( "Comma separated list of name=value pairs" )]
    public string InitParameter { get; set; }
    #endregion

    internal void AddInitParams( InitParam initParam )
    {
      m_InitParamList.Add( initParam );
    }

    #region private
    /// <summary>
    /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
    /// </summary>
    protected override void CreateChildControls()
    {
      base.CreateChildControls();
      try
      {
        if ( String.IsNullOrEmpty( Source ) )
          return;
        // Ensure we have set the height and width
        string width = ( this.Width == Unit.Empty ) ? "95%" : this.Width.ToString();
        string height = (this.Height == Unit.Empty) ? "95%" : this.Height.ToString();
        SilverlightExceptionScript _exceptionScript = new SilverlightExceptionScript()
        {
          ClientID = this.ClientID
        };
        //Render error handling script
        this.Controls.Add( new LiteralControl( _exceptionScript.TransformText() ) );
        HTMLHostinCode _host = new HTMLHostinCode()
        {
          ErrorScript = this.ClientID,
          m_Source = GetWebPartPath(),
          Height = height,
          Width = width,
        };
        foreach ( InitParam _imx in m_InitParamList )
          _host.AddInitParams( _imx );
        _host.AddInitParams( InitParameter );
        //Reender Silverlight WebPart hosting html
        this.Controls.Add( new LiteralControl( _host.TransformText() ) );
      }
      catch ( Exception ex )
      {
        this.Controls.Add( new ExceptionMessage( ex ) );
      }
    }
    private string GetWebPartPath()
    {
      SPSite currentSite = SPContext.Current.Site;
      if ( currentSite == null )
        throw new ApplicationException( this.GetType().Name + " cannot be used outsite the SP Contex." );
      return ( currentSite.ServerRelativeUrl == "/" ? "/" : currentSite.ServerRelativeUrl + "/" ) + Source;
    }
    private List<InitParam> m_InitParamList = new List<InitParam>();
    #endregion

  }
}
