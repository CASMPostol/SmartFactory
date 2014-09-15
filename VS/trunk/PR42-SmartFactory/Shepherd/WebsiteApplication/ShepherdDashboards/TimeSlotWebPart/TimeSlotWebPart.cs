using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CAS.SmartFactory.Shepherd.DataModel.Entities;

namespace CAS.SmartFactory.Shepherd.Dashboards.TimeSlotWebPart
{
  /// <summary>
  /// TimeSlot WebPart
  /// </summary>
  [ToolboxItemAttribute(false)]
  public class TimeSlotWebPart : WebPart
  {
    // Visual Studio might automatically update this path when you change the Visual Web Part project item.
    private const string _ascxPath = @"~/_CONTROLTEMPLATES/CAS.SmartFactory.Shepherd.Dashboards/TimeSlotWebPart/TimeSlotWebPartUserControl.ascx";

    /// <summary>
    /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
    /// </summary>
    protected override void CreateChildControls()
    {
      string _phase = "Befor Starting";
      try
      {
        Control _ctrl = Page.LoadControl(_ascxPath);
        _phase = "After Page.LoadControl";
        m_control = (TimeSlotWebPartUserControl)_ctrl;
        _phase = "After Casting";
        m_control.Role = Role;
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
    /// <summary>
    /// Initializes a new instance of the <see cref="TimeSlotWebPart"/> class.
    /// </summary>
    public TimeSlotWebPart()
    { }
    #region Personalization properties
    /// <summary>
    /// Gets or sets the role of the hosting dashboard.
    /// </summary>
    /// <value>
    /// The role.
    /// </value>
    [WebBrowsable(true)]
    [Personalizable(PersonalizationScope.Shared)]
    [WebDisplayName("The dashboard role")]
    [WebDescription("The role of the dashboard this web part is located on. Depending on the role the dashboard customizes" +
      " the functionality provided to the user.")]
    [Microsoft.SharePoint.WebPartPages.SPWebCategoryName("CAS Custom Properties")]
    public GlobalDefinitions.Roles Role { get; set; }
    #endregion

    #region interconnection
    /// <summary>
    /// Gets the connection interface allowing to get selected entry of <see cref="TimeSlot"/>.
    /// </summary>
    /// <returns>Returns an instance of the <see cref="IWebPartRow"/> representing <see cref="TimeSlot"/>.</returns>
    [ConnectionProvider("Selected TimeSlot entry", "SelectedTimeSlotProviderPoint", AllowsMultipleConnections = true)]
    [DefaultValue(false)]
    public IWebPartRow GetConnectionInterface()
    {
      if (m_control == null)
        return new InterconnectionDataTable<TimeSlot>();
      if (m_SelectedTimeSlot == null)
        m_SelectedTimeSlot = m_control.GetSelectedTimeSlotInterconnectionData();
      return m_SelectedTimeSlot;
    }
    #endregion
  }
}
