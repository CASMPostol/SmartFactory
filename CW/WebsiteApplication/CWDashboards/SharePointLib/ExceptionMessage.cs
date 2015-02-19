//<summary>
//  Title   : Exception Message WebControl
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
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
      
namespace CAS.SmartFactory.CW.Dashboards.SharePointLib
{
  /// <summary>
  /// Exception Message <see cref="WebControl"/>
  /// </summary>
  [DefaultProperty( "Message" )]
  [ToolboxData( "<{0}:ExceptionMEssage runat=server></{0}:ExceptionMEssage>" )]
  public class ExceptionMessage: WebControl
  {

    #region public
    /// <summary>
    /// Gets or sets a message that describes the current exception..
    /// </summary>
    /// <value>
    /// The error message that explains the reason for the exception, or an empty string("").
    /// </value>
    [Category( "CAS Silverlight" ), Bindable( false ), Localizable( false ), DefaultValue( "NA" ), Description( "The error message that explains the reason for the exception" )]
    public string Message { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionMessage"/> class.
    /// </summary>
    public ExceptionMessage()
    {
      Message = "NA";
    }
    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionMessage"/> class.
    /// </summary>
    /// <param name="exception">The exception to be rendered as the <see cref="LiteralControl"/>.</param>
    public ExceptionMessage( Exception exception )
    {
      Message = Convert2ErrorMessageFormat( exception );
    }
    #endregion

    #region private
    protected override void CreateChildControls()
    {
      base.CreateChildControls();
      this.Controls.Add( new LiteralControl( Message ) );
    }
    private const string ErrorMessageFormat = "<font color=red>The operation {1} has been interrupted by an error: {0}</font><br/>";
    private static string Convert2ErrorMessageFormat( Exception exception )
    {
      return String.Format( ErrorMessageFormat, exception.Message, exception.Source );
    }
    #endregion

  }
}
