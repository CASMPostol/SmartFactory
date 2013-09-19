using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace TestDisposalRequest.SilverLight.DisposalRequest
{
  [ToolboxItemAttribute( false )]
  public class DisposalRequest: WebPart
  {
    protected override void CreateChildControls()
    {
      SilverlightWebControl _slwc = new SilverlightWebControl()
      {
        Source = "SiteAssets/TestDisposalRequest/DisposalRequestWebPart/DisposalRequestWebPart.xap",
      };
      Controls.Add( _slwc );
    }
  }
}
