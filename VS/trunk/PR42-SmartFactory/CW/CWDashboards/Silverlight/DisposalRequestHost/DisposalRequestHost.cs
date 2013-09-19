//<summary>
//  Title   : Disposal Request SilverLight Host
//  System  : Microsoft Visual C# .NET 2012
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
      
using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.CW.Dashboards.SharePointLib;

namespace CAS.SmartFactory.CW.Dashboards.Silverlight.DisposalRequestHost
{
  /// <summary>
  /// Disposal Request SilverLight <see cref="WebPart"/> Host
  /// </summary>
  [ToolboxItemAttribute( false )]
  public class DisposalRequestHost: WebPart
  {
    /// <summary>
    /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
    /// </summary>
    protected override void CreateChildControls()
    {
      try
      {
        SilverlightWebControl _slwc = new SilverlightWebControl()
                                          {
                                            Source = "SiteAssets/TestDisposalRequest/DisposalRequestWebPart/DisposalRequestWebPart.xap",
                                          };
        Controls.Add( _slwc );
      }
      catch ( Exception ex )
      {
        this.Controls.Add( new ExceptionMessage( ex ) );
      }
    }
  }
}
