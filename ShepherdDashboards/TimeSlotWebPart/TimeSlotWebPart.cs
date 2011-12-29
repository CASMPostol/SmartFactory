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
      m_control = Page.LoadControl(_ascxPath) as TimeSlotWebPartUserControl;
      m_control.SimpleTimeSlotList = SimpleTimeSlotList;
      Controls.Add(m_control);
    }
    TimeSlotWebPartUserControl m_control = null;
    private InterconnectionDataTable<Entities.TimeSlotTimeSlot> m_SelectedTimeSlot = null;
    public TimeSlotWebPart()
    {
      SimpleTimeSlotList = true;
    }
    //TODO Add it to the editor as personalizable property
    public bool SimpleTimeSlotList { get; set; }
    [ConnectionProvider("Selected TimeSlot", "SelectedTimeSlotProviderPoint", AllowsMultipleConnections = true)]
    public IWebPartRow GetConnectionInterface()
    {
      if (m_control == null)
        return new InterconnectionDataTable<Entities.TimeSlotTimeSlot>();
      if (m_SelectedTimeSlot == null)
        m_SelectedTimeSlot = m_control.GetSelectedTimeSlotInterconnectionData();
      return m_SelectedTimeSlot;
    }
  }
}
