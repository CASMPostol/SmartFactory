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
        <asp:Label ID="m_WarehouseLabel" runat="server" CssClass="Label"></asp:Label>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_TimeSlotLabel" runat="server" Text="Time Slot" CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_TimeSlotTextBox" runat="server" ToolTip="Select a Time Slot from the calendar to schedule the shipping."
          CssClass="Label" Enabled="false"></asp:Label>
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
        <asp:TextBox ID="m_CommentsTextBox" runat="server" TextMode="Multiline" Height="60" CssClass="TextBox"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_EstimateDeliveryTimeLabel" runat="server" Text="ETA" CssClass="Label"
          ToolTip="Estimated Time of Arrival"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <SharePoint:DateTimeControl ID="m_EstimateDeliveryTimeDateTimeControl" runat="server"
          ToolTip="The estimated time of arrival or ETA is a measure of when a ship, vehicle, aircraft, cargo or emergency service is expected to arrive at a certain place. ETA Usually means - estimated/expected time of arrival/to achieve/remaining." />
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_CityHeaderLabel" runat="server" Text="Destination" CssClass="Label">
        </asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_CityLabel" runat="server" Text=" --  Select a city from city table -- "
          CssClass="_Label"></asp:Label>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_PartnerHeaderLabel" runat="server" Text="Vendor" CssClass="Label">
        </asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:Label ID="m_PartnerLabel" runat="server" Text=" --  Select a vendor from vendors table -- "
          CssClass="_Label">
        </asp:Label>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell ColumnSpan="2">
        <asp:Panel runat="server" BorderColor="ActiveCaptionText" GroupingText="Route/Escort Editing"
          ToolTip="Select the check box to enable editing" ID="m_CoordinatorPanel">
          <asp:Table runat="server">
            <asp:TableRow>
              <asp:TableCell>
                <asp:Label ID="Edit" CssClass="Label" runat="server" Text="Edit Route / Escort"></asp:Label>
                <asp:CheckBox runat="server" TextAlign="Left" ToolTip="Select to start editing route and escort association." AutoPostBack="True" ID="m_CoordinatorEditCheckBox" />
              </asp:TableCell>
              <asp:TableCell>
                <asp:Label ID="SPAN" runat="server" CssClass="Label" Text="|"></asp:Label>
              </asp:TableCell>
              <asp:TableCell>
                <asp:Label ID="m_SecurityRequiredLabel" runat="server" Text="Security Required" CssClass="Label"></asp:Label>
                <asp:CheckBox ID="m_SecurityRequiredChecbox" AutoPostBack="true" runat="server" Checked="false"></asp:CheckBox>
              </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
          <asp:Table runat="server">
            <asp:TableRow>
              <asp:TableCell>
                <asp:Label ID="m_RouteHeaderLabel" runat="server" Text="Route" CssClass="Label"></asp:Label></asp:TableCell>
              <asp:TableCell>
                <asp:Label ID="m_RouteLabel" runat="server" Text="" CssClass="Label"></asp:Label></asp:TableCell>
            </asp:TableRow>            
            <asp:TableRow>
              <asp:TableCell>
                <asp:Label ID="m_SecurityEscortHeaderLabel" runat="server" Text="Security Escort" CssClass="Label"></asp:Label>
              </asp:TableCell>
              <asp:TableCell>
                <asp:Label ID="m_SecurityEscortLabel" runat="server" Text="" CssClass="Label"></asp:Label>
              </asp:TableCell>
            </asp:TableRow>
          </asp:Table>
        </asp:Panel>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_TransportUnitTypeLabel" runat="server" Text="Transport Unit" CssClass="Label" />
      </asp:TableCell><asp:TableCell>
        <asp:DropDownList ID="m_TransportUnitTypeDropDownList" runat="server" CssClass="DropDown" />
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label Text="Dock number" ID="m_DocNumberLabel" runat="server" CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_DockNumberTextBox" CssClass="TextBox" runat="server"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label Text="Trailer condition" ID="m_TrailerConditionDropdownLabel" runat="server"
          CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:DropDownList ID="m_TrailerConditionDropdown" runat="server" CssClass="DropDown">
        </asp:DropDownList>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label Text="Comments" ID="m_TrailerConditionCommentsLabel" runat="server" CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_TrailerConditionCommentsTextBox" TextMode="Multiline" Height="60" CssClass="TextBox"
          ToolTip="Add note about trailer condition." runat="server"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_ContainerNoLabel" runat="server" Text="Container No." CssClass="Label"></asp:Label>
      </asp:TableCell><asp:TableCell>
        <asp:TextBox ID="m_ContainerNoTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
      </asp:TableCell></asp:TableRow>
  </asp:Table>
  <asp:Button ID="m_NewShippingButton" runat="server" Text="Add new" CssClass="Button" />
  <asp:Button ID="m_EditButton" runat="server" Text="Edit" CssClass="Button" />
  <asp:Button ID="m_AbortButton" runat="server" Text="Abort" CssClass="Button" />
  <asp:Button ID="m_SaveButton" runat="server" Text="Save" CssClass="Button" />
  <asp:Button ID="m_CancelButton" runat="server" Text="Cancel" CssClass="Button" />
</asp:Panel>
