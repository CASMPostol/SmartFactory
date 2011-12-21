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
<asp:Panel ID="m_Panel" runat="server" Width="443px">
  <asp:Literal ID="m_StateLiteral" runat="server" Text="---" />
  <br />
  <asp:Label ID="Label1" runat="server" Text="Truck: " AssociatedControlID="m_TruckRegistrationNumberTextBox"></asp:Label>
  <asp:Label ID="m_TruckRegistrationNumberTextBox" runat="server" Text=" " />
  <br />
  <asp:Label ID="Label2" runat="server" AssociatedControlID="m_TrailerRegistrationNumberTextBox"
    Text="Trailer: "></asp:Label>
  <asp:TextBox ID="m_TrailerRegistrationNumberTextBox" runat="server" />
  <br />
  <asp:Label ID="Label3" runat="server" Text="Warehouse"></asp:Label>
  <asp:TextBox ID="m_WarehouseTextBox" runat="server"></asp:TextBox>
  <br />
  <asp:Label ID="Label4" runat="server" Text="Time Slot" AssociatedControlID="m_TimeSlotTextBox"></asp:Label>
  <asp:TextBox ID="m_TimeSlotTextBox" runat="server"></asp:TextBox>
  <br />
  <asp:Button ID="m_NewShippingButton" runat="server" Text="New Shipping" />
  <asp:Button ID="m_EditButton" runat="server" Text="Edit" />
  <asp:Button ID="m_SaveButton" runat="server" Text="Save" />
  <asp:Button ID="m_CancelButton" runat="server" Text="Cancel" />
</asp:Panel>
<asp:Panel ID="m_HiddenPanel" runat="server">
  <asp:HiddenField ID="m_TruckRegistrationHiddenField" runat="server" />
  <asp:HiddenField ID="m_TrailerHiddenField" runat="server" />
  <asp:HiddenField ID="m_DriverHiddenField" runat="server" />
  <asp:HiddenField ID="m_TimeSlotHiddenField" runat="server" />
  <asp:HiddenField ID="m_ShippingHiddenField" runat="server" />
  <asp:HiddenField ID="m_WarehouseHiddenField" runat="server" />
</asp:Panel>
