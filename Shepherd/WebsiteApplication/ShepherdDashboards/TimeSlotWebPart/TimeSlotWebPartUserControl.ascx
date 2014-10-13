<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TimeSlotWebPartUserControl.ascx.cs"
  Inherits="CAS.SmartFactory.Shepherd.Dashboards.TimeSlotWebPart.TimeSlotWebPartUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" />
<asp:Table ID="Table1" runat="server" CssClass="Table">
  <asp:TableRow runat="server">
    <asp:TableCell runat="server">
      <asp:DropDownList CssClass="DropDown" ID="m_WarehouseDropDownList" runat="server"
        Width="100%" DataTextField="Title" DataValueField="Id" AutoPostBack="true">
      </asp:DropDownList>
    </asp:TableCell>
  </asp:TableRow>
  <asp:TableRow>
    <asp:TableCell>
      <asp:Calendar CssClass="DashboardCalendar" runat="server" ID="m_Calendar" DayNameFormat="FirstLetter"
        SelectionMode="Day" />
    </asp:TableCell>
    <asp:TableCell runat="server">
      <asp:ListBox CssClass="DashboardTimeSlotList" ID="m_TimeSlotList" runat="server"
        AutoPostBack="true" ToolTip="<%$Resources:CASSmartFactoryShepherd,AvailableTimeSlotsToolTip%>" Rows="12"></asp:ListBox>
    </asp:TableCell>
  </asp:TableRow>
  <asp:TableRow>
    <asp:TableCell>
      <asp:Panel ID="m_DoubleTimeSlotsPanel" runat="server">
        <asp:CheckBox ID="m_ShowDoubleTimeSlots" runat="server" CssClass="CheckBox" AutoPostBack="true" />
        <asp:Label ID="Label1" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,ShowDoubleTimeslots%>" CssClass="Label"></asp:Label>
      </asp:Panel>
    </asp:TableCell>
  </asp:TableRow>
</asp:Table>
<asp:HiddenField runat="server" ID="m_UserLocalTime"></asp:HiddenField>
