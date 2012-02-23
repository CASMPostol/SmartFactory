<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
  Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OperatorWebPartUserControl.ascx.cs"
  Inherits="CAS.SmartFactory.Shepherd.Dashboards.OperatorWebPart.OperatorWebPartUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" />
<asp:Panel ID="m_OperatorPanel" runat="server">
    <asp:Table ID="OperatorTable" CssClass="TableInside" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label Text="Shipping" ID="Label1" runat="server" CssClass="Label" Enabled="false"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="m_ShippingLabel" runat="server" CssClass="Label"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Truck" Text="Truck No." CssClass="Label" runat="server"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="m_TruckNumberLabel" CssClass="Label" runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Trailer" Text="Trailer No." CssClass="Label" runat="server"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="m_TrailerNumberLabel" CssClass="Label" runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label Text="Dock number" ID="DocNumber" runat="server" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="m_DockNumberTextBox" CssClass="TextBox" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label Text="Trailer condition" ID="TrailerCondition" runat="server" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_TrailerConditionDropdown" runat="server" CssClass="DropDown"></asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label Text="Comments" ID="Comments" runat="server" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="m_ConditionComments" TextMode="Multiline" CssClass="TextBox" runat="server"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <asp:Button ID="m_OperatorEditButton" Text="Edit" CssClass="Button" runat="server" />
    <asp:Button ID="m_OperatorSaveButton" Text="Save" CssClass="Button" runat="server" />
    <asp:Button ID="m_OperatorCancelButton" Text="Cancel" CssClass="Button" runat="server" />
</asp:Panel>