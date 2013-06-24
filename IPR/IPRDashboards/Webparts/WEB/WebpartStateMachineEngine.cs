using CAS.SharePoint.Web;
using CAS.SmartFactory.IPR.WebsiteModel.Linq;

namespace CAS.SmartFactory.IPR.Dashboards.Webparts.WEB
{
  internal abstract class WebpartStateMachineEngine: GenericStateMachineEngine
  {
    protected override void ShowActionResult( ActionResult _rslt )
    {
      switch ( _rslt.LastActionResult )
      {
        case ActionResult.Result.Success:
        case ActionResult.Result.NotValidated:
          break;
        case ActionResult.Result.Exception:
          ActivityLogCT.WriteEntry( _rslt.ActionException.Source, _rslt.ActionException.Message );
          break;
      }
    }
  }
}
