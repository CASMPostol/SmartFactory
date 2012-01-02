<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransportResourcesUserControl.ascx.cs" Inherits="CAS.SmartFactory.Shepherd.Dashboards.CarrierDashboard.TransportResources.TransportResourcesUserControl" %>
<SharePoint:CssRegistration runat="server" ID="cssreg" Name="/_layouts/ShepherdDashboards/CAS_ShepherdDashboards.css" />
<asp:Panel ID="m_Panel" runat="server">
    <asp:Table ID="TransportResourcesWebPart" runat="server" CssClass="Table">
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label1" runat="server" Text="Shipping" CssClass="Label" Enabled=false></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="m_ShippingTextBox" runat="server" CssClass="TextBox" Enabled=false></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:ListBox ID="m_DriversListBox" runat="server" CssClass="ListBox" Enabled=false></asp:ListBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Button ID="m_AddDriverButton" runat="server" Text="==>" CssClass="Button" Enabled=false/>
                <br />
                <asp:Button ID="m_RemoveDriverButton" runat="server" Text="<==" CssClass="Button" Enabled=false/>
            </asp:TableCell>
            <asp:TableCell>
                <asp:ListBox ID="m_DriversTeamListBox" runat="server" CssClass="ListBox" Enabled=false></asp:ListBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label2" runat="server" Text="Truck" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_TruckDropDown" runat="server" CssClass="DropDown" Enabled=false>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                <asp:Label ID="Label3" runat="server" Text="Trailer" CssClass="Label"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:DropDownList ID="m_TrailerDropDown" runat="server" CssClass="DropDown" Enabled=false>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Panel>