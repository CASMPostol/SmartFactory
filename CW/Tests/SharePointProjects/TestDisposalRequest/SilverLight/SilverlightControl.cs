using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Microsoft.SharePoint;

namespace TestDisposalRequest.SilverLight
{
  [
  DefaultProperty( "Text" ),
  ToolboxData( "<{0}:SilverlightPlugin runat=\"server\" />" )
  ]
  public class SilverlightWebControl: WebControl
  {
    /// <summary>
    /// Gets or sets the URL of Silverlight .xap file".
    /// </summary>
    /// <value>
    /// The URL.
    /// </value>
    [Category( "Silverlight" ), Bindable( false ), Localizable( false ), DefaultValue( "" ), Description( "URL of Silverlight .xap file" )]
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets the comma separated list of name=value pairs initialize parameters.
    /// </summary>
    /// <value>
    /// The initialize parameters.
    /// </value>
    [Category( "Silverlight" ), Bindable( false ), Localizable( false ), DefaultValue( "" ), Description( "Comma separated list of name=value pairs" )]
    public string InitParameters { get; set; }

    //    #region Script
    //    private const string SILVERLIGHT_EXCEPTION_SCRIPT_BLOCK = @"
    //                    <script type=""text/javascript"">
    //                        function {0}Error (sender, args) {{
    //        
    //                            var appSource = """";
    //                            if (sender != null && sender != 0) {{
    //                              appSource = sender.getHost().Source;
    //                            }}
    //                    
    //                            var errorType = args.ErrorType;
    //                            var iErrorCode = args.ErrorCode;
    //        
    //                            if (errorType == ""ImageError"" || errorType == ""MediaError"") {{
    //                              return;
    //                            }}
    //        
    //                            var errMsg = ""Unhandled Error in Silverlight Application "" +  appSource + ""\n"" ;
    //        
    //                            errMsg += ""Code: ""+ iErrorCode + ""    \n"";
    //                            errMsg += ""Category: "" + errorType + ""       \n"";
    //                            errMsg += ""Message: "" + args.ErrorMessage + ""     \n"";
    //        
    //                            if (errorType == ""ParserError"") {{
    //                                errMsg += ""File: "" + args.xamlFile + ""     \n"";
    //                                errMsg += ""Line: "" + args.lineNumber + ""     \n"";
    //                                errMsg += ""Position: "" + args.charPosition + ""     \n"";
    //                            }}
    //                            else if (errorType == ""RuntimeError"") {{           
    //                                if (args.lineNumber != 0) {{
    //                                    errMsg += ""Line: "" + args.lineNumber + ""     \n"";
    //                                    errMsg += ""Position: "" +  args.charPosition + ""     \n"";
    //                                }}
    //                                errMsg += ""MethodName: "" + args.methodName + ""     \n"";
    //                            }}
    //        
    //                            throw new Error(errMsg);
    //                        }}
    //                    </script>
    //                ";

    //    #endregion

    //    #region Object Tag

    //    // {0} - Width
    //    // {1} - Height
    //    // {2} - Source
    //    // {3} - Error Script
    //    // {4} - Initparams
    //    private const string SILVERLIGHT_OBJECT_TAG = @"
    //                <div style=""overflow-x: hidden; position:relative; width:{0}; height:{1};"">
    //                    <object data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" width=""{0}"" height=""{1}"">
    //        		      <param name=""source"" value=""{2}""/>
    //        		      <param name=""onError"" value=""{3}Error"" />
    //        		      <param name=""background"" value=""white"" />
    //        		      <param name=""minRuntimeVersion"" value=""4.0.50401.0"" />
    //                      <param name=""initparams"" value=""{4}"" />
    //        		      <param name=""autoUpgrade"" value=""true"" />
    //        		      <a href=""http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50401.0"" style=""text-decoration:none"">
    //         			      <img src=""http://go.microsoft.com/fwlink/?LinkId=161376"" alt=""Get Microsoft Silverlight"" style=""border-style:none""/>
    //        		      </a>
    //        	        </object><iframe id=""_sl_historyFrame"" style=""visibility:hidden;height:0px;width:0px;border:0px""></iframe></div>";
    //    #endregion

    protected override void CreateChildControls()
    {
      base.CreateChildControls();
      if ( String.IsNullOrEmpty( Source ) )
        return;
      // Ensure we have set the height and width
      string width = ( this.Width == Unit.Empty ) ? "100%" : this.Width.ToString();
      string height = ( this.Height == Unit.Empty ) ? "100%" : this.Height.ToString();
      SilverlightExceptionScript _exceptionScript = new SilverlightExceptionScript()
      {
        ClientID = this.ClientID
      };
      //Render error handling script
      this.Controls.Add( new LiteralControl( _exceptionScript.TransformText() ) );
      //String.Format( SILVERLIGHT_EXCEPTION_SCRIPT_BLOCK, this.ClientID ) ) );
      HTMLHostinCode _host = new HTMLHostinCode()
      {
        ErrorScript = this.ClientID,
        Height = height,
        Initparams = InitParameters == null ? String.Empty : InitParameters,
        Source = WebPartPath(),
        Width = width,
      };
      this.Controls.Add( new LiteralControl( _host.TransformText() ) );
      //String.Format( SILVERLIGHT_OBJECT_TAG, width, height, this.Source, this.ClientID, this.InitParameters ) ) );
    }
    private string WebPartPath()
    {
      SPSite currentSite = SPContext.Current.Site;
      if ( currentSite == null )
        throw new ApplicationException( this.GetType().Name + " cannot be used outsite the SP Contex." );
      return ( currentSite.ServerRelativeUrl == "/" ? "/" : currentSite.ServerRelativeUrl + "/" ) + Source;
    }
  }
}
