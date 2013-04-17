<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GuardWebPartUserControl.ascx.cs"
  Inherits="CAS.SmartFactory.Shepherd.Dashboards.GuardWebPart.GuardWebPartUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" />
<asp:Panel ID="m_Panel" runat="server">
  <asp:Table ID="_GuardTable" runat="server" CssClass="Table">
    <asp:TableRow>
      <asp:TableCell>
        <asp:Label ID="m_Label4" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Shipping%>" CssClass="Label" Enabled="false"></asp:Label>
      </asp:TableCell>
      <asp:TableCell>
        <asp:Label ID="m_ShippingLabel" runat="server" CssClass="Label"></asp:Label>
      </asp:TableCell>
    </asp:TableRow>
  </asp:Table>
  <asp:Button ID="m_EnteredButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Enters%>" CssClass="Button" />
  <asp:Button ID="m_LeftButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Leaves%>" CssClass="Button" />
  <asp:Button ID="m_ArrivedButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Arrived%>" CssClass="Button" />
  <asp:Button ID="m_UnDoButton" runat="server" Text="<%$Resources:CASSmartFactoryShepherd,Undo%>" CssClass="Button" />
</asp:Panel>
