﻿<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
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
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_WarehouseLabel" runat="server" Text="Warehouse" CssClass="Label"></asp:Label>
      </asp:TableCell>
      <asp:TableCell>
        <asp:TextBox ID="m_WarehouseTextBox" runat="server" CssClass="TextBox" Enabled="false"></asp:TextBox>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_Label4" runat="server" Text="Time Slot" AssociatedControlID="m_TimeSlotTextBox"
          CssClass="Label"></asp:Label></asp:TableCell>
      <asp:TableCell>
        <asp:TextBox ID="m_TimeSlotTextBox" runat="server" CssClass="TextBox" Enabled="false"></asp:TextBox>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_Label5" runat="server" Text="PO Number" CssClass="Label" />
      </asp:TableCell>
      <asp:TableCell>
        <asp:TextBox ID="m_DocumentTextBox" runat="server" CssClass="TextBox"></asp:TextBox>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_Label7" runat="server" Text="Comments" CssClass="Label" />
      </asp:TableCell>
      <asp:TableCell>
        <asp:TextBox ID="m_CommentsTextBox" runat="server" TextMode="Multiline" CssClass="TextBox"></asp:TextBox>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="Label1" runat="server" Text="Delivery Time" CssClass="Label"></asp:Label>
      </asp:TableCell>
      <asp:TableCell>
        <asp:TextBox ID="m_EstimateDeliveryTime" CssClass="TextBox" runat="server"></asp:TextBox>
      </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
    <asp:TableCell>
        <asp:Label ID="Label2" runat="server" Text="Route" CssClass="Label"></asp:Label>
    </asp:TableCell>
        <asp:TableCell>
        <asp:DropDownList ID="m_Route" runat="server" CssClass="DropDown">
        </asp:DropDownList>
    </asp:TableCell>
    </asp:TableRow>
    <asp:TableRow>
        <asp:TableCell>
            <asp:Label ID="Label3" runat="server" Text="Security Escort" CssClass="Label"></asp:Label> 
        </asp:TableCell>
        <asp:TableCell>
        <asp:DropDownList ID="m_SecurityEscortCatalog" runat="server" CssClass="DropDown">
        </asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
  <asp:Button ID="m_NewShippingButton" runat="server" Text="New Shipping" CssClass="Button" />
  <asp:Button ID="m_EditButton" runat="server" Text="Edit" CssClass="Button" />
  <asp:Button ID="m_ConfirmButton" runat="server" Text="Confirm" CssClass="Button" />
  <asp:Button ID="m_Abort" runat="server" Text="Abort" CssClass="Button" />
  <asp:Button ID="m_SaveButton" runat="server" Text="Save" CssClass="Button" />
  <asp:Button ID="m_CancelButton" runat="server" Text="Cancel" CssClass="Button" />
</asp:Panel>
<asp:Panel ID="m_HiddenPanel" runat="server">
  <asp:HiddenField ID="m_TruckRegistrationHiddenField" runat="server" />
  <asp:HiddenField ID="m_DriverHiddenField" runat="server" />
  <asp:HiddenField ID="m_TimeSlotHiddenField" runat="server" />
  <asp:HiddenField ID="m_ShippingHiddenField" runat="server" />
  <asp:HiddenField ID="m_WarehouseHiddenField" runat="server" />
</asp:Panel>
