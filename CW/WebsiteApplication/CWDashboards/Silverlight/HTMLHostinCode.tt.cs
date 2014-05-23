using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint;

namespace CAS.SmartFactory.CW.Dashboards.Silverlight
{
  public partial class HTMLHostinCode
  {
    public HTMLHostinCode()
    {
      // Set the SiteUrl here.  Can’t do this earlier since SPContext may be null during instantiation
      //AddInitParams( new InitParam( "SiteUrl", SPContext.Current.Site.Url ) );
      //AddInitParams( new InitParam( "DisplayWebUrl", SPContext.Current.Site.Url ) );
    }
    internal void AddInitParams( InitParam initParam )
    {
      AddInitParams( initParam.ToString() );
    }
    internal void AddInitParams( string initParam )
    {
      if ( String.IsNullOrEmpty( initParam ) )
        return;
      string _prefix = String.IsNullOrEmpty( m_Initparams ) ? String.Empty : ",";
      m_Initparams += _prefix + initParam;
    }
    public string Width = "95%";
    public string Height = "95%";
    public string ErrorScript = String.Empty;
    public int TimeOut = 1800;
    public bool ForceEnableHTMLAccess = false;
    public string m_Source = String.Empty;

    private string m_Initparams = String.Empty;
    private string m_url = SPContext.Current.Site.Url;
  }
}
