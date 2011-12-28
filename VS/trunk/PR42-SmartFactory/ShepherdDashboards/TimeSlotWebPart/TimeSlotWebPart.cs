using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CAS.SmartFactory.Shepherd.Dashboards.TimeSlotWebPart
{
  [ToolboxItemAttribute(false)]
  public class TimeSlotWebPart : WebPart
  {
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/TimeSlotWebPart/TimeSlotWebPartUserControl.ascx";

    protected override void CreateChildControls()
    {
      TimeSlotWebPartUserControl control = Page.LoadControl(_ascxPath) as TimeSlotWebPartUserControl;
      Controls.Add(control);
      control.SimpleTimeSlotList = SimpleTimeSlotList;
    }
    public TimeSlotWebPart()
    {
      SimpleTimeSlotList = true;
    }
    //TODO Add it to the editor as personalizable property
    public bool SimpleTimeSlotList { get; set; }
  }
}
