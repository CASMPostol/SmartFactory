using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.Dashboards.Entities;
using System;

namespace CAS.SmartFactory.Shepherd.Dashboards.TimeSlotWebPart
{
  [ToolboxItemAttribute(false)]
  public class TimeSlotWebPart : WebPart
  {
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/TimeSlotWebPart/TimeSlotWebPartUserControl.ascx";

    protected override void CreateChildControls()
    {
      string _phase = "Befor Starting";
      try
      {
        Control _ctrl = Page.LoadControl(_ascxPath);
        _phase = "After Page.LoadControl";
        m_control = (TimeSlotWebPartUserControl)_ctrl;
        _phase = "After Casting";
         m_control.SimpleTimeSlotList = SimpleTimeSlotList;
        _phase = "After selection SimpleTimeSlotList";
        Controls.Add(m_control);
        _phase = "After Controls.Add";
      }
      catch (Exception ex)
      {
        string _frmt = "Cannot lod the user control at: {0} because : {1}";
        Controls.Add(new LiteralControl(String.Format(_frmt, _phase, ex.Message)));
      }
    }
    TimeSlotWebPartUserControl m_control = null;
    private InterconnectionDataTable<TimeSlotTimeSlot> m_SelectedTimeSlot = null;
    public TimeSlotWebPart()
    {
      SimpleTimeSlotList = true;
    }
    [WebBrowsable(true)]
    [Personalizable(PersonalizationScope.Shared)]
    [WebDisplayName("Simple list of available slots")]
    [WebDescription("Just a list instead of full day calendar page with rows")]
    [Microsoft.SharePoint.WebPartPages.SPWebCategoryName("CAS Custom Properties")]
    public bool SimpleTimeSlotList { get; set; }
    /// <summary>
    /// Gets the connection interface allowing to get selected entry of <see cref="TimeSlotTimeSlot"/>.
    /// </summary>
    /// <returns>Returns an instance of the <see cref="IWebPartRow"/> representing <see cref="TimeSlotTimeSlot"/>.</returns>
    [ConnectionProvider("Selected TimeSlot entry", "SelectedTimeSlotProviderPoint", AllowsMultipleConnections = true)]
    public IWebPartRow GetConnectionInterface()
    {
      if (m_control == null)
        return new InterconnectionDataTable<TimeSlotTimeSlot>();
      if (m_SelectedTimeSlot == null)
        m_SelectedTimeSlot = m_control.GetSelectedTimeSlotInterconnectionData();
      return m_SelectedTimeSlot;
    }
  }
}
