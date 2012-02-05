<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CarrierDashboardWebPartUserControl.ascx.cs"
  Inherits="CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.CarrierDashboardWebPart.CarrierDashboardWebPartUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" />
<asp:Panel ID="m_Panel" runat="server">
  <asp:Table ID="m_CarrierDashboardWebPart" runat="server" CssClass="Table">
    <asp:TableRow>
      <asp:TableCell>
        <asp:Literal ID="m_StateLiteral" runat="server" />
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_WarehousehHeaderLabel" runat="server" Text="Warehouse" CssClass="Label"
          ToolTip="Select a Warehouse and next a Time Slot from the calendar to schedule the shipping.">
        </asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_WarehouseLabel" runat="server" CssClass="TextBox" ></asp:Label>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_TimeSlotLabel" runat="server" Text="Time Slot" CssClass="Label"></asp:Label>
      </asp:TableCell>
      <asp:TableCell>
        <asp:Label ID="m_TimeSlotTextBox" runat="server" ToolTip="Select a Time Slot from the calendar to schedule the shipping."
          CssClass="TextBox" Enabled="false"></asp:Label>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_DocumentLabel" runat="server" Text="PO Number" CssClass="Label" />
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_DocumentTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_CommentsLabel" runat="server" Text="Comments" CssClass="Label" />
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_CommentsTextBox" runat="server" TextMode="Multiline" CssClass="TextBox"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_EstimateDeliveryTimeLabel" runat="server" Text="ETA" CssClass="Label" ToolTip="Estimated Time of Arrival"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <SharePoint:DateTimeControl ID="m_EstimateDeliveryTimeDateTimeControl" runat="server" ToolTip="The estimated time of arrival or ETA is a measure of when a ship, vehicle, aircraft, cargo or emergency service is expected to arrive at a certain place. ETA Usually means - estimated/expected time of arrival/to achieve/remaining." />
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_RouteHeaderLabel" runat="server" Text="Route" CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_RouteLabel" runat="server" Text="" CssClass="Label"></asp:Label>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_CityHeaderLabel" runat="server" Text="Destination"></asp:Label>
      </asp:TableCell>
      <asp:TableCell>
        <asp:Label ID="m_CityLabel" runat="server" Text=" --  Select a city from city table -- "></asp:Label>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_SecurityEscortHeaderLabel" runat="server" Text="Security Escort" CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_SecurityEscortLabel" runat="server" Text="" CssClass="Label"></asp:Label>
      </asp:TableCell>
      </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_TransportUnitTypeLabel" runat="server" Text="Transport Unit Type" />
      </asp:TableCell><asp:TableCell>
        <asp:DropDownList ID="m_TransportUnitTypeDropDownList" runat="server" />
      </asp:TableCell>
      </asp:TableRow>
  </asp:Table>
  <asp:Button ID="m_NewShippingButton" runat="server" Text="New Shipping" CssClass="Button" />
  <asp:Button ID="m_EditButton" runat="server" Text="Change" CssClass="Button" />
  <asp:Button ID="m_AbortButton" runat="server" Text="Abort" CssClass="Button" />
  <asp:Button ID="m_SaveButton" runat="server" Text="Save" CssClass="Button" />
  <asp:Button ID="m_CancelButton" runat="server" Text="Cancel" CssClass="Button" />
  <asp:Button ID="m_AcceptButton" runat="server" Text="Accept" CssClass="Button" />
</asp:Panel>
